namespace BD_Project
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Adresa")]
    public partial class Adresa
    {
        public Adresa()
        {
            Centru = new HashSet<Centru>();
            Client = new HashSet<Client>();
        }

        [Key]
        public int IdAdresa { get; set; }

        [Required]
        [StringLength(30)]
        public string Judet { get; set; }

        [Required]
        [StringLength(30)]
        public string Oras { get; set; }

        [Required]
        [StringLength(30)]
        public string Strada { get; set; }

        [Column("Nr.")]
        public int Nr { get; set; }

        [StringLength(5)]
        public string Bloc { get; set; }

        public virtual ICollection<Centru> Centru { get; set; }
        public virtual ICollection<Client> Client { get; set; }
    }
}
