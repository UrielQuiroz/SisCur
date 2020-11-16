namespace SisCur.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Docente")]
    public partial class Docente
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Docente()
        {
            DocentePeriodo = new HashSet<DocentePeriodo>();
            GradoSeccionAula = new HashSet<GradoSeccionAula>();
        }

        [Key]
        public int IIDDOCENTE { get; set; }

        [StringLength(100)]
        public string NOMBRE { get; set; }

        [StringLength(150)]
        public string APPATERNO { get; set; }

        [StringLength(150)]
        public string APMATERNO { get; set; }

        [StringLength(150)]
        public string DIRECCION { get; set; }

        [StringLength(10)]
        public string TELEFONOCELULAR { get; set; }

        [StringLength(10)]
        public string TELEFONOFIJO { get; set; }

        [StringLength(50)]
        public string EMAIL { get; set; }

        public int? IIDSEXO { get; set; }

        public DateTime? FECHACONTRATO { get; set; }

        public byte[] FOTO { get; set; }

        public int? IIDMODALIDADCONTRATO { get; set; }

        public int? BHABILITADO { get; set; }

        [StringLength(1)]
        public string IIDTIPOUSUARIO { get; set; }

        public int? bTieneUsuario { get; set; }

        public virtual ModalidadContrato ModalidadContrato { get; set; }

        public virtual Sexo Sexo { get; set; }

        public virtual TIPOUSUARIO TIPOUSUARIO { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<DocentePeriodo> DocentePeriodo { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<GradoSeccionAula> GradoSeccionAula { get; set; }
    }
}
