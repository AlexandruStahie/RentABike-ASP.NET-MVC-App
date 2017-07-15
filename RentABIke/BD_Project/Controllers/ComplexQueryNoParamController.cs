using BD_Project.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BD_Project.Controllers
{
    public class ComplexQueryNoParamController : Controller
    {
        // GET: ComplexQueryNoParam
        public ActionResult Index()
        {
            return View();
        }

        [Authorize(Users = "admin")]
        [HttpGet]
        //bicicletele care nu au fost comandate niciodata
        public ActionResult InterogareUnu()
        {
            List<Bicicleta> biciclete = new List<Bicicleta>();

            SqlConnection connection = new SqlConnection("data source = DESKTOP-IBNRKP3\\SQLEXPRESS; initial catalog = BD_Project; integrated security = True");
            SqlCommand cmd = new SqlCommand(@"
                                                select b.IdBicicleta, b.Model, b.Categorie, b.Pret_h  from Bicicleta b
                                                where b.IdBicicleta not in (select cb.IdBicicleta from Comenzi_Biciclete cb)
                                                ", connection);

            connection.Open();
            SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                Bicicleta x = new Bicicleta();

                x.IdBicicleta = reader.GetInt32(0);
                x.Model = reader.GetString(1);
                x.Categorie = reader.GetString(2);
                x.Pret_h = reader.GetDecimal(3);

                biciclete.Add(x);

            }

            reader.Close();

            return View(biciclete);

        }

        [Authorize(Users = "admin")]
        [HttpGet]
        //clientii cu pretul comenzii > media tuturor comenzilor 
        public ActionResult InterogareDoi()
        {
            List<Interogare1> clienti = new List<Interogare1>();

            SqlConnection connection = new SqlConnection("data source = DESKTOP-IBNRKP3\\SQLEXPRESS; initial catalog = BD_Project; integrated security = True");
            SqlCommand cmd = new SqlCommand(@"              
                                                select cl.Nume, cl.Prenume, c.IdComenzi, c.TimpStart, c.TimpStop, c.Pret from Comenzi c
                                                inner join Client cl on cl.IdClient = c.IdClient
                                                where c.Pret > (select avg(Pret) from Comenzi )", connection);

            connection.Open();
            SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                Interogare1 x = new Interogare1();
                Client y = new Client();
                Comenzi z = new Comenzi();

                y.Nume = reader.GetString(0);
                y.Prenume = reader.GetString(1);
                z.IdComenzi = reader.GetInt32(2);
                z.TimpStart = reader.GetDateTime(3);
                z.TimpStop = reader.GetDateTime(4);
                z.Pret = reader.GetDecimal(5);

                x.client = y;
                x.comanda = z;
                clienti.Add(x);

            }

            reader.Close();

            return View(clienti);

        }
    }
}