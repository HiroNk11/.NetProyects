using MesaAyuda.Web.Models;
using MesaAyuda.Web.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
namespace MesaAyuda.Web.Controllers;
[Authorize]
public class TicketsController(TicketService service, UserManager<AppUser> users) : Controller
{
    public async Task<IActionResult> Index(TicketFilter filter)
    {
        if (!ModelState.IsValid) return BadRequest("Los filtros no son válidos.");
        var visible = service.VisibleTo(User).AsNoTracking();
        var open = await visible.CountAsync(t => t.Status == TicketStatus.Nuevo);
        var progress = await visible.CountAsync(t => t.Status == TicketStatus.EnCurso);
        var resolved = await visible.CountAsync(t => t.Status == TicketStatus.Resuelto);
        var urgent = await visible.CountAsync(t => t.Priority == TicketPriority.Urgente && t.Status != TicketStatus.Cerrado);
        var query = visible;
        if (!string.IsNullOrWhiteSpace(filter.Query))
        {
            var term = filter.Query.Trim().ToLower();
            query = query.Where(t => t.Title.ToLower().Contains(term) || t.Description.ToLower().Contains(term));
        }
        if (filter.Status.HasValue) query = query.Where(t => t.Status == filter.Status);
        if (filter.Priority.HasValue) query = query.Where(t => t.Priority == filter.Priority);
        var total = await query.CountAsync();
        var pages = Math.Max(1, (total + 9) / 10);
        filter.Page = Math.Min(filter.Page, pages);
        var tickets = await query.Include(t => t.Requester).Include(t => t.Technician)
            .OrderByDescending(t => t.UpdatedAt).ThenByDescending(t => t.Id)
            .Skip((filter.Page - 1) * 10).Take(10).ToListAsync();
        return View(new TicketListView(tickets, filter, total, pages, open, progress, resolved, urgent));
    }
    public IActionResult Create() => View(new CreateTicketInput());
    [HttpPost]
    public async Task<IActionResult> Create(CreateTicketInput input)
    {
        if (!ModelState.IsValid) return View(input);
        try
        {
            var id = await service.CreateAsync(input, User);
            TempData["Success"] = "Ticket creado. Podés seguir su evolución desde esta página.";
            return RedirectToAction(nameof(Details), new { id });
        }
        catch (ArgumentException ex)
        {
            ModelState.AddModelError("", ex.Message);
            return View(input);
        }
    }
    public async Task<IActionResult> Details(int id)
    {
        var ticket = await service.VisibleTo(User).AsNoTracking()
            .Include(t => t.Requester).Include(t => t.Technician)
            .Include(t => t.Comments).ThenInclude(c => c.Author)
            .Include(t => t.Events).ThenInclude(e => e.Actor)
            .AsSplitQuery().SingleOrDefaultAsync(t => t.Id == id);
        if (ticket is null) return NotFound();
        var technicians = User.IsInRole(Roles.Admin)
            ? (await users.GetUsersInRoleAsync(Roles.Technician)).OrderBy(u => u.DisplayName).ToList() : [];
        return View(new TicketDetailView(ticket, technicians, service.CanManage(ticket, User), User.IsInRole(Roles.Admin)));
    }
    [HttpPost]
    public async Task<IActionResult> Comment(int id, CommentInput input) =>
        !ModelState.IsValid ? BadRequest("El comentario no es válido.") : Result(await service.CommentAsync(id, input, User), id);
    [HttpPost, Authorize(Roles = Roles.Admin)]
    public async Task<IActionResult> Assign(int id, AssignInput input) =>
        !ModelState.IsValid ? BadRequest("La asignación no es válida.") : Result(await service.AssignAsync(id, input, User), id);
    [HttpPost]
    public async Task<IActionResult> Status(int id, ChangeStatusInput input) =>
        !ModelState.IsValid ? BadRequest("El estado no es válido.") : Result(await service.ChangeStatusAsync(id, input, User), id);
    private IActionResult Result(ChangeResult result, int id)
    {
        if (result == ChangeResult.NotFound) return NotFound();
        if (result == ChangeResult.Forbidden) return Forbid();
        if (result == ChangeResult.Conflict)
            return StatusCode(409, "Otra persona actualizó el ticket. Volvé al detalle y recargá antes de intentarlo de nuevo.");
        if (result == ChangeResult.Invalid)
            return BadRequest("La operación no es válida para el estado actual. Para iniciar un ticket, asigná un técnico primero.");
        TempData["Success"] = "Los cambios se guardaron correctamente.";
        return RedirectToAction(nameof(Details), new { id });
    }
}
