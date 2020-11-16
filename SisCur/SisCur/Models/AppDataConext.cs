namespace SisCur.Models
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class AppDataConext : DbContext
    {
        public AppDataConext()
            : base("name=DataConext")
        {
        }

        public virtual DbSet<Alumno> Alumno { get; set; }
        public virtual DbSet<Aula> Aula { get; set; }
        public virtual DbSet<Curso> Curso { get; set; }
        public virtual DbSet<DetalleMatricula> DetalleMatricula { get; set; }
        public virtual DbSet<Docente> Docente { get; set; }
        public virtual DbSet<DocentePeriodo> DocentePeriodo { get; set; }
        public virtual DbSet<Grado> Grado { get; set; }
        public virtual DbSet<GradoSeccion> GradoSeccion { get; set; }
        public virtual DbSet<GradoSeccionAula> GradoSeccionAula { get; set; }
        public virtual DbSet<Matricula> Matricula { get; set; }
        public virtual DbSet<ModalidadContrato> ModalidadContrato { get; set; }
        public virtual DbSet<Pagina> Pagina { get; set; }
        public virtual DbSet<Periodo> Periodo { get; set; }
        public virtual DbSet<PeriodoGradoCurso> PeriodoGradoCurso { get; set; }
        public virtual DbSet<Rol> Rol { get; set; }
        public virtual DbSet<RolPagina> RolPagina { get; set; }
        public virtual DbSet<Seccion> Seccion { get; set; }
        public virtual DbSet<Sexo> Sexo { get; set; }
        public virtual DbSet<sysdiagrams> sysdiagrams { get; set; }
        public virtual DbSet<TIPOUSUARIO> TIPOUSUARIO { get; set; }
        public virtual DbSet<Usuario> Usuario { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Alumno>()
                .Property(e => e.NOMBRE)
                .IsUnicode(false);

            modelBuilder.Entity<Alumno>()
                .Property(e => e.APPATERNO)
                .IsUnicode(false);

            modelBuilder.Entity<Alumno>()
                .Property(e => e.APMATERNO)
                .IsUnicode(false);

            modelBuilder.Entity<Alumno>()
                .Property(e => e.TELEFONOPADRE)
                .IsUnicode(false);

            modelBuilder.Entity<Alumno>()
                .Property(e => e.TELEFONOMADRE)
                .IsUnicode(false);

            modelBuilder.Entity<Alumno>()
                .Property(e => e.IIDTIPOUSUARIO)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<Aula>()
                .Property(e => e.NOMBRE)
                .IsUnicode(false);

            modelBuilder.Entity<Aula>()
                .Property(e => e.BHABILITADO)
                .IsRequired();

            modelBuilder.Entity<Curso>()
                .Property(e => e.NOMBRE)
                .IsUnicode(false);

            modelBuilder.Entity<Curso>()
                .Property(e => e.DESCRIPCION)
                .IsUnicode(false);

            modelBuilder.Entity<Curso>()
                .HasMany(e => e.DetalleMatricula)
                .WithRequired(e => e.Curso)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Docente>()
                .Property(e => e.NOMBRE)
                .IsUnicode(false);

            modelBuilder.Entity<Docente>()
                .Property(e => e.APPATERNO)
                .IsUnicode(false);

            modelBuilder.Entity<Docente>()
                .Property(e => e.APMATERNO)
                .IsUnicode(false);

            modelBuilder.Entity<Docente>()
                .Property(e => e.DIRECCION)
                .IsUnicode(false);

            modelBuilder.Entity<Docente>()
                .Property(e => e.TELEFONOCELULAR)
                .IsUnicode(false);

            modelBuilder.Entity<Docente>()
                .Property(e => e.TELEFONOFIJO)
                .IsUnicode(false);

            modelBuilder.Entity<Docente>()
                .Property(e => e.EMAIL)
                .IsUnicode(false);

            modelBuilder.Entity<Docente>()
                .Property(e => e.IIDTIPOUSUARIO)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<Docente>()
                .HasMany(e => e.DocentePeriodo)
                .WithRequired(e => e.Docente)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Grado>()
                .Property(e => e.NOMBRE)
                .IsUnicode(false);

            modelBuilder.Entity<Matricula>()
                .HasMany(e => e.DetalleMatricula)
                .WithRequired(e => e.Matricula)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<ModalidadContrato>()
                .Property(e => e.NOMBRE)
                .IsUnicode(false);

            modelBuilder.Entity<ModalidadContrato>()
                .Property(e => e.DESCRIPCION)
                .IsUnicode(false);

            modelBuilder.Entity<Pagina>()
                .Property(e => e.MENSAJE)
                .IsUnicode(false);

            modelBuilder.Entity<Pagina>()
                .Property(e => e.ACCION)
                .IsUnicode(false);

            modelBuilder.Entity<Pagina>()
                .Property(e => e.CONTROLADOR)
                .IsUnicode(false);

            modelBuilder.Entity<Pagina>()
                .HasMany(e => e.RolPagina)
                .WithRequired(e => e.Pagina)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Periodo>()
                .Property(e => e.NOMBRE)
                .IsUnicode(false);

            modelBuilder.Entity<Periodo>()
                .HasMany(e => e.DocentePeriodo)
                .WithRequired(e => e.Periodo)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Rol>()
                .Property(e => e.NOMBRE)
                .IsUnicode(false);

            modelBuilder.Entity<Rol>()
                .Property(e => e.DESCRIPCION)
                .IsUnicode(false);

            modelBuilder.Entity<Rol>()
                .HasMany(e => e.RolPagina)
                .WithRequired(e => e.Rol)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Seccion>()
                .Property(e => e.NOMBRE)
                .IsUnicode(false);

            modelBuilder.Entity<Sexo>()
                .Property(e => e.NOMBRE)
                .IsUnicode(false);

            modelBuilder.Entity<TIPOUSUARIO>()
                .Property(e => e.IIDTIPOUSUARIO)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<TIPOUSUARIO>()
                .Property(e => e.NOMBRE)
                .IsUnicode(false);

            modelBuilder.Entity<TIPOUSUARIO>()
                .HasMany(e => e.Usuario)
                .WithOptional(e => e.TIPOUSUARIO1)
                .HasForeignKey(e => e.TIPOUSUARIO);

            modelBuilder.Entity<Usuario>()
                .Property(e => e.NOMBREUSUARIO)
                .IsUnicode(false);

            modelBuilder.Entity<Usuario>()
                .Property(e => e.CONTRA)
                .IsUnicode(false);

            modelBuilder.Entity<Usuario>()
                .Property(e => e.TIPOUSUARIO)
                .IsFixedLength()
                .IsUnicode(false);
        }
    }
}
