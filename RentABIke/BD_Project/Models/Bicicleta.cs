namespace BD_Project
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Bicicleta")]
    public partial class Bicicleta
    {
        public Bicicleta()
        {
            Comenzi_Biciclete = new HashSet<Comenzi_Biciclete>();
        }

        [Key]
        public int IdBicicleta { get; set; }

        [Required]
        [StringLength(20)]
        public string Categorie { get; set; }

        [Required]
        [StringLength(20)]
        public string Model { get; set; }

        [Column("Pret_h")]
        public decimal Pret_h { get; set; }

        [Required]
        [StringLength(50)]
        public string Detalii { get; set; }

        [Required]
        [StringLength(5000)]
        public string ImageLink { get; set; }

        public bool IsEnabled { get; set; }

        public ICollection<Comenzi_Biciclete> Comenzi_Biciclete { get; internal set; }
    }
}
