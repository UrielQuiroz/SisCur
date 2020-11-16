namespace SisCur.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Usuario")]
    public partial class Usuario
    {
        [Key]
        public int IIDUSUARIO { get; set; }

        [StringLength(100)]
        public string NOMBREUSUARIO { get; set; }

        [StringLength(64)]
        public string CONTRA { get; set; }

        [StringLength(1)]
        public string TIPOUSUARIO { get; set; }

        public int? IID { get; set; }

        public int? IIDROL { get; set; }

        public int? BHABILITADO { get; set; }

        [NotMapped]
        public virtual Rol Rol { get; set; }

        [NotMapped]
        public virtual TIPOUSUARIO TIPOUSUARIO1 { get; set; }
    }
}
