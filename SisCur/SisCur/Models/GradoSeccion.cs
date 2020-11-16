namespace SisCur.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("GradoSeccion")]
    public partial class GradoSeccion
    {
        [Key]
        public int IID { get; set; }

        public int? IIDGRADO { get; set; }

        public int? IIDSECCION { get; set; }

        public int? BHABILITADO { get; set; }

        public virtual Grado Grado { get; set; }

        public virtual Seccion Seccion { get; set; }
    }
}
