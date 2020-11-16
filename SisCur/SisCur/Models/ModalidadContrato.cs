namespace SisCur.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("ModalidadContrato")]
    public partial class ModalidadContrato
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public ModalidadContrato()
        {
            Docente = new HashSet<Docente>();
        }

        [Key]
        public int IIDMODALIDADCONTRATO { get; set; }

        [StringLength(100)]
        public string NOMBRE { get; set; }

        [StringLength(200)]
        public string DESCRIPCION { get; set; }

        public int? BHABILITADO { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Docente> Docente { get; set; }
    }
}
