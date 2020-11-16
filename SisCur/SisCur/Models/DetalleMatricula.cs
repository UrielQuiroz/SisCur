namespace SisCur.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("DetalleMatricula")]
    public partial class DetalleMatricula
    {
        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int IIDMATRICULA { get; set; }

        [Key]
        [Column(Order = 1)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int IIDCURSO { get; set; }

        public decimal? NOTA1 { get; set; }

        public decimal? NOTA2 { get; set; }

        public decimal? NOTA3 { get; set; }

        public decimal? NOTA4 { get; set; }

        public decimal? PROMEDIO { get; set; }

        public int? bhabilitado { get; set; }

        public virtual Curso Curso { get; set; }

        public virtual Matricula Matricula { get; set; }
    }
}
