using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace BD_Project.Models
{
    public class Comenzi_Final
    {
        public List<Bicicleta> biciclete { get; set; }

        public Comenzi comanda { get; set; }

       [DisplayName("Alege Centru inchiriere")]
        public int IdCentru { get; set; }

        public Centru centru { get; set; }

    }
}