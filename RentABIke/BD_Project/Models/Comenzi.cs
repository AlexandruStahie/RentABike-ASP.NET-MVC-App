namespace BD_Project
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Comenzi")]
    public partial class Comenzi
    {
        [Key]
        public int IdComenzi { get; set; }


        [Required]
        [StringLength(10)]
        public string Tip_Plata { get; set; }

        public DateTime TimpStart { get; set; }

        public DateTime TimpStop { get; set; }

        public decimal Pret { get; set; }

        public int IdBicicleta { get; set; }

        public int IdClient { get; set; }

        public virtual Client Client { get; set; }

        public virtual Centru Centru { get; set; }
        public ICollection<Comenzi_Biciclete> Comenzi_Biciclete { get; internal set; }
    }
}
