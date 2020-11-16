namespace SisCur.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("GradoSeccionAula")]
    public partial class GradoSeccionAula
    {
        [Key]
        public int IID { get; set; }

        public int? IIDPERIODO { get; set; }

        public int? IIDGRADOSECCION { get; set; }

        public int? IIDAULA { get; set; }

        public int? BHABILITADO { get; set; }

        public int? IIDDOCENTE { get; set; }

        public int? IIDCURSO { get; set; }

        public virtual Aula Aula { get; set; }

        public virtual Docente Docente { get; set; }

        public virtual Periodo Periodo { get; set; }
    }
}
