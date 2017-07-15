namespace BD_Project
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Client")]
    public partial class Client
    {
        public Client()
        {
            Comenzi = new HashSet<Comenzi>();
        }

        [Key]
        public int IdClient { get; set; }

   
        [StringLength(30)]
        [Required]
        public string Nume { get; set; }

        [StringLength(30)]
        [Required]
        public string Prenume { get; set; }

        [Required]
        [Column("User")]
        [StringLength(30)]
        public string User { get; set; }

        [Required]
        [StringLength(30)]
        public string Password { get; set; }

        [Column("E_mail")]
        [StringLength(30)]
        [Required]
        public string E_mail { get; set; }

        [StringLength(10)]
        [Required]
        public string Nr_telefon { get; set; }


        [Column(TypeName = "date")]
        public DateTime? Data_nastere { get; set; }

        public int IdAdresa { get; set; }

        public virtual Adresa Adresa { get; set; }

        public virtual ICollection<Comenzi> Comenzi { get; set; }
    }
}
