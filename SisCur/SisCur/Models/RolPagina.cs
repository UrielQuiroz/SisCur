namespace SisCur.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("RolPagina")]
    public partial class RolPagina
    {
        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int IIDROL { get; set; }

        [Key]
        [Column(Order = 1)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int IIDPAGINA { get; set; }

        public int? BHABILITADO { get; set; }

        public virtual Pagina Pagina { get; set; }

        public virtual Rol Rol { get; set; }
    }
}
