namespace SisCur.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Alumno")]
    public partial class Alumno
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Alumno()
        {
            Matricula = new HashSet<Matricula>();
        }

        [Key]
        public int IIDALUMNO { get; set; }

        [StringLength(100)]
        public string NOMBRE { get; set; }

        [StringLength(150)]
        public string APPATERNO { get; set; }

        [StringLength(150)]
        public string APMATERNO { get; set; }

        public DateTime? FECHANACIMIENTO { get; set; }

        public int? IIDSEXO { get; set; }

        [StringLength(10)]
        public string TELEFONOPADRE { get; set; }

        [StringLength(10)]
        public string TELEFONOMADRE { get; set; }

        public int? NUMEROHERMANOS { get; set; }

        public int? BHABILITADO { get; set; }

        [StringLength(1)]
        public string IIDTIPOUSUARIO { get; set; }

        public int? bTieneUsuario { get; set; }

        [NotMapped]
        public virtual Sexo Sexo { get; set; }

        [NotMapped]
        public virtual TIPOUSUARIO TIPOUSUARIO { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]

        [NotMapped]
        public virtual ICollection<Matricula> Matricula { get; set; }
    }
}
