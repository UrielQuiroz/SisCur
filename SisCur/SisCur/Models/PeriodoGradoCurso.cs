namespace SisCur.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("PeriodoGradoCurso")]
    public partial class PeriodoGradoCurso
    {
        [Key]
        public int IID { get; set; }

        public int? IIDPERIODO { get; set; }

        public int? IIDGRADO { get; set; }

        public int? IIDCURSO { get; set; }

        public int? BHABILITADO { get; set; }

        public virtual Curso Curso { get; set; }

        public virtual Grado Grado { get; set; }

        public virtual Periodo Periodo { get; set; }
    }
}
