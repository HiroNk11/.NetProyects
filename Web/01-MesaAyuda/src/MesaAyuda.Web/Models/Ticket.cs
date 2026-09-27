using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;
namespace MesaAyuda.Web.Models;

public static class Roles
{
    public const string Requester = "Solicitante";
    public const string Technician = "Tecnico";
    public const string Admin = "Administrador";
}
public class AppUser : IdentityUser
{
    [MaxLength(100)] public string DisplayName { get; set; } = "";
}
public enum TicketStatus { Nuevo, EnCurso, Resuelto, Cerrado }
public enum TicketPriority { Baja, Media, Alta, Urgente }
public class Ticket
{
    public int Id { get; set; }
    [MaxLength(120)] public string Title { get; set; } = "";
    [MaxLength(4000)] public string Description { get; set; } = "";
    public TicketStatus Status { get; set; }
    public TicketPriority Priority { get; set; }
    public string RequesterId { get; set; } = "";
    public AppUser Requester { get; set; } = null!;
    public string? TechnicianId { get; set; }
    public AppUser? Technician { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    [ConcurrencyCheck] public int Version { get; set; } = 1;
    public List<TicketComment> Comments { get; set; } = [];
    public List<TicketEvent> Events { get; set; } = [];
}
public class TicketComment
{
    public int Id { get; set; }
    public int TicketId { get; set; }
    public string AuthorId { get; set; } = "";
    public AppUser Author { get; set; } = null!;
    [MaxLength(2000)] public string Body { get; set; } = "";
    public DateTime CreatedAt { get; set; }
}
public class TicketEvent
{
    public int Id { get; set; }
    public int TicketId { get; set; }
    public string ActorId { get; set; } = "";
    public AppUser Actor { get; set; } = null!;
    [MaxLength(300)] public string Description { get; set; } = "";
    public DateTime CreatedAt { get; set; }
}
