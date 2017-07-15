namespace BD_Project
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Centru")]
    public partial class Centru
    {
        [Key]
        public int IdCentru { get; set; }

        [Required]
        [StringLength(30)]
        public string Nume { get; set; }

        [Required]
        [StringLength(10)]
        public string Nr_telefon { get; set; }

        [Required]
        [StringLength(500)]
        public string Program { get; set; }

        public int IdAdresa { get; set; }

        public virtual Adresa Adresa { get; set; }
        public virtual ICollection<Comenzi> Comenzi { get; set; }
    }
}
