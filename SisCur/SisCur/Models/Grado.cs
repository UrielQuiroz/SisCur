namespace SisCur.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Grado")]
    public partial class Grado
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Grado()
        {
            GradoSeccion = new HashSet<GradoSeccion>();
            Matricula = new HashSet<Matricula>();
            PeriodoGradoCurso = new HashSet<PeriodoGradoCurso>();
        }

        [Key]
        public int IIDGRADO { get; set; }

        [StringLength(100)]
        public string NOMBRE { get; set; }

        public int? BHABILITADO { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<GradoSeccion> GradoSeccion { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Matricula> Matricula { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PeriodoGradoCurso> PeriodoGradoCurso { get; set; }
    }
}
