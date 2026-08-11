using System.Data.Entity;
using TurnosApiMvcEf6.Models;

namespace TurnosApiMvcEf6.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext()
            : base("name=TurnosDb")
        {
        }

        public DbSet<Paciente> Pacientes { get; set; }
        public DbSet<Profesional> Profesionales { get; set; }
        public DbSet<Turno> Turnos { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Turno>()
                .HasRequired(turno => turno.Paciente)
                .WithMany(paciente => paciente.Turnos)
                .HasForeignKey(turno => turno.PacienteId);

            modelBuilder.Entity<Turno>()
                .HasRequired(turno => turno.Profesional)
                .WithMany(profesional => profesional.Turnos)
                .HasForeignKey(turno => turno.ProfesionalId);

            base.OnModelCreating(modelBuilder);
        }
    }
}
