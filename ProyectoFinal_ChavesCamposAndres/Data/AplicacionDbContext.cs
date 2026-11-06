using ProyectoFinal_ChavesCamposAndres.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
namespace ProyectoFinal_ChavesCamposAndres.Data;

public class AplicacionDbContext : IdentityDbContext<Usuario>
{
    public AplicacionDbContext(DbContextOptions<AplicacionDbContext> options)
        : base(options) { }
    public DbSet<Estudiante> Estudiantes => Set<Estudiante>();
    public DbSet<Carrera> Carreras => Set<Carrera>();
    public DbSet<Curso> Cursos => Set<Curso>();
    public DbSet<Docente> Docentes => Set<Docente>();
    public DbSet<Matricula> Matriculas => Set<Matricula>();
    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        builder.Entity<Estudiante>().ToTable("Estudiantes");
        builder.Entity<Carrera>().ToTable("Carreras");
        builder.Entity<Curso>().ToTable("Cursos");
        builder.Entity<Docente>().ToTable("Docentes");
        builder.Entity<Matricula>().ToTable("Matriculas");
        builder.Entity<Carrera>().HasIndex(x => x.Codigo).IsUnique();
        builder.Entity<Curso>().HasIndex(x => x.Codigo).IsUnique();
        builder.Entity<Docente>().HasIndex(x => x.Identificacion).IsUnique();
        builder.Entity<Estudiante>().HasIndex(x => x.Identificacion).IsUnique();
        builder.Entity<Estudiante>().HasIndex(x => x.UsuarioId).IsUnique();
        builder.Entity<Matricula>()
            .HasIndex(x => new { x.EstudianteId, x.CursoId }).IsUnique();
        builder.Entity<Estudiante>().HasOne(x => x.Usuario).WithMany()
            .HasForeignKey(x => x.UsuarioId).OnDelete(DeleteBehavior.Restrict);
        builder.Entity<Estudiante>().HasOne(x => x.Carrera).WithMany()
            .HasForeignKey(x => x.CarreraId).OnDelete(DeleteBehavior.Restrict);
        builder.Entity<Curso>().HasOne(x => x.Carrera).WithMany()
            .HasForeignKey(x => x.CarreraId).OnDelete(DeleteBehavior.Restrict);
        builder.Entity<Curso>().HasOne(x => x.Docente).WithMany()
            .HasForeignKey(x => x.DocenteId).OnDelete(DeleteBehavior.Restrict);
        builder.Entity<Docente>().HasOne(x => x.Carrera).WithMany()
            .HasForeignKey(x => x.CarreraId).OnDelete(DeleteBehavior.Restrict);
        builder.Entity<Matricula>().HasOne(x => x.Estudiante).WithMany()
            .HasForeignKey(x => x.EstudianteId).OnDelete(DeleteBehavior.Restrict);
        builder.Entity<Matricula>().HasOne(x => x.Curso).WithMany()
            .HasForeignKey(x => x.CursoId).OnDelete(DeleteBehavior.Restrict);
        builder.Entity<Carrera>().HasData(
            new Carrera
            {
                Id = 1,
                Codigo = "SIS",
                Nombre = "Ingeniería en Sistemas de Computación"
            },
            new Carrera
            {
                Id = 2,
                Codigo = "ADM",
                Nombre = "Administración de Negocios"
            },
            new Carrera
            {
                Id = 3,
                Codigo = "TUR",
                Nombre = "Gestión Turística"
            });
        builder.Entity<Docente>().HasData(
            new Docente
            {
                Id = 1,
                NombreCompleto = "Sofía Umaña Portugués",
                Identificacion = "DOC021",
                Correo = "sofia.umana@ufidelitas.ac.cr",
                CarreraId = 1
            },
            new Docente
            {
                Id = 2,
                NombreCompleto = "Rafael Guzmán López",
                Identificacion = "DOC035",
                Correo = "rafael.guzman@ufidelitas.ac.cr",
                CarreraId = 1
            },
            new Docente
            {
                Id = 3,
                NombreCompleto = "Laura Méndez Campos",
                Identificacion = "DOC011",
                Correo = "laura.mendez@ufidelitas.ac.cr",
                CarreraId = 1
            },
            new Docente
            {
                Id = 4,
                NombreCompleto = "Diego Navarro Jiménez",
                Identificacion = "DOC083",
                Correo = "diego.navarro@ufidelitas.ac.cr"
            });
        builder.Entity<Curso>().HasData(
            new Curso
            {
                Id = 1,
                Codigo = "SIS101",
                Nombre = "Introducción a la Programación",
                Creditos = 4,
                CarreraId = 1,
                DocenteId = 1
            },
            new Curso
            {
                Id = 2,
                Codigo = "SIS102",
                Nombre = "Matemática Discreta",
                Creditos = 3,
                CarreraId = 1,
                DocenteId = 2
            },
            new Curso
            {
                Id = 3,
                Codigo = "SIS103",
                Nombre = "Fundamentos de Computación",
                Creditos = 3,
                CarreraId = 1,
                DocenteId = 3
            });
    }
}