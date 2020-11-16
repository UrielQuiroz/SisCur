namespace SisCur.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("DocentePeriodo")]
    public partial class DocentePeriodo
    {
        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int IIDDOCENTE { get; set; }

        [Key]
        [Column(Order = 1)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int IIDPERIODO { get; set; }

        public int? BHABILITADO { get; set; }

        public virtual Docente Docente { get; set; }

        public virtual Periodo Periodo { get; set; }
    }
}
