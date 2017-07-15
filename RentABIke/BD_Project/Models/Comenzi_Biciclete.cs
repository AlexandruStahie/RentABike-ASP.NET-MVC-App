
namespace BD_Project
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Comenzi_Biciclete")]
    public partial class Comenzi_Biciclete
    {
        [Key]
        public int IdComBic { get; set; }
        [Required]
        public int IdBicicleta { get; set; }
        [Required]
        public int IdComenzi { get; set; }

        public virtual Comenzi Comenzi { get; set; }

        public virtual Bicicleta Bicicleta { get; set; }
    }
}
