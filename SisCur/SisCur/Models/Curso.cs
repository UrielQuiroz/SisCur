namespace SisCur.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Curso")]
    public partial class Curso
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Curso()
        {
            DetalleMatricula = new HashSet<DetalleMatricula>();
            PeriodoGradoCurso = new HashSet<PeriodoGradoCurso>();
        }

        [Key]
        public int IIDCURSO { get; set; }

        [StringLength(100)]
        public string NOMBRE { get; set; }

        [StringLength(200)]
        public string DESCRIPCION { get; set; }

        public int? BHABILITADO { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<DetalleMatricula> DetalleMatricula { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PeriodoGradoCurso> PeriodoGradoCurso { get; set; }
    }
}
