namespace SisCur.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Pagina")]
    public partial class Pagina
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Pagina()
        {
            RolPagina = new HashSet<RolPagina>();
        }

        [Key]
        public int IIDPAGINA { get; set; }

        [StringLength(100)]
        public string MENSAJE { get; set; }

        [StringLength(50)]
        public string ACCION { get; set; }

        [StringLength(50)]
        public string CONTROLADOR { get; set; }

        public int? BHABILITADO { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<RolPagina> RolPagina { get; set; }
    }
}
