namespace SisCur.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Matricula")]
    public partial class Matricula
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Matricula()
        {
            DetalleMatricula = new HashSet<DetalleMatricula>();
        }

        [Key]
        public int IIDMATRICULA { get; set; }

        public int? IIDPERIODO { get; set; }

        public int? IIDGRADO { get; set; }

        public int? IIDSECCION { get; set; }

        public int? IIDALUMNO { get; set; }

        public DateTime? FECHA { get; set; }

        public int? BHABILITADO { get; set; }

        public virtual Alumno Alumno { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<DetalleMatricula> DetalleMatricula { get; set; }

        public virtual Grado Grado { get; set; }

        public virtual Periodo Periodo { get; set; }

        public virtual Seccion Seccion { get; set; }
    }
}
