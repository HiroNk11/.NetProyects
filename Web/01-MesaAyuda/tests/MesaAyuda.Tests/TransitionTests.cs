using MesaAyuda.Web.Models;
using MesaAyuda.Web.Services;
namespace MesaAyuda.Tests;

public class TransitionTests
{
    [Theory]
    [InlineData(TicketStatus.Nuevo, TicketStatus.EnCurso, true)]
    [InlineData(TicketStatus.Nuevo, TicketStatus.Resuelto, false)]
    [InlineData(TicketStatus.Nuevo, TicketStatus.Cerrado, false)]
    [InlineData(TicketStatus.EnCurso, TicketStatus.Resuelto, true)]
    [InlineData(TicketStatus.EnCurso, TicketStatus.Nuevo, false)]
    [InlineData(TicketStatus.Resuelto, TicketStatus.EnCurso, true)]
    [InlineData(TicketStatus.Resuelto, TicketStatus.Cerrado, true)]
    [InlineData(TicketStatus.Cerrado, TicketStatus.EnCurso, false)]
    [InlineData(TicketStatus.EnCurso, TicketStatus.EnCurso, false)]
    public void Workflow_only_allows_supported_transitions(TicketStatus from, TicketStatus to, bool allowed)
        => Assert.Equal(allowed, TicketService.CanTransition(from, to));
}
