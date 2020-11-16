namespace SisCur.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Periodo")]
    public partial class Periodo
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Periodo()
        {
            DocentePeriodo = new HashSet<DocentePeriodo>();
            GradoSeccionAula = new HashSet<GradoSeccionAula>();
            Matricula = new HashSet<Matricula>();
            PeriodoGradoCurso = new HashSet<PeriodoGradoCurso>();
        }

        [Key]
        public int IIDPERIODO { get; set; }

        [StringLength(100)]
        public string NOMBRE { get; set; }

        public DateTime? FECHAINICIO { get; set; }

        public DateTime? FECHAFIN { get; set; }

        public int? BHABILITADO { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<DocentePeriodo> DocentePeriodo { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<GradoSeccionAula> GradoSeccionAula { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Matricula> Matricula { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PeriodoGradoCurso> PeriodoGradoCurso { get; set; }
    }
}
