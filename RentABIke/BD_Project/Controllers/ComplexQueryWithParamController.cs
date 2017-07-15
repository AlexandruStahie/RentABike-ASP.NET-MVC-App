using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BD_Project.Models;

namespace BD_Project.Controllers
{
    public class ComplexQueryWithParamController : Controller
    {
        // GET: ComplexQueryWithParam
        public ActionResult Index()
        {
            return View();
        }


        [Authorize(Users = "admin")]
        [HttpGet]
        //top x clienti cu comenzile cele mai scumpe
        public ActionResult InterogareUnu()
        {
            List<Interogare1> y = new List<Interogare1>();
            Interogare1 z = new Interogare1();
            y.Add(z);
            return View(y);
        }

        [Authorize(Users = "admin")]
        [HttpPost]
        public ActionResult InterogareUnu(List<Interogare1> x)
        {
            List<Interogare1> clienti = new List<Interogare1>();

            SqlConnection connection = new SqlConnection("data source = DESKTOP-IBNRKP3\\SQLEXPRESS; initial catalog = BD_Project; integrated security = True");
            SqlCommand cmd = new SqlCommand(@"
                                                select c1.Nume, c1.Prenume, cl1.Pret from Client c1
                                                inner join Comenzi cl1 on c1.IdClient = cl1.IdClient
                                                where " + x[0].count + @"  >     (select count(*) from Client c2
                                                                        inner join Comenzi cl2 on cl2.IdClient = c2.IdClient
                                                                        where cl1.Pret < cl2.Pret) ", connection);

            connection.Open();
            SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                Interogare1 interogare = new Interogare1();
                Client y = new Client();
                Comenzi z = new Comenzi();

                y.Nume = reader.GetString(0);
                y.Prenume = reader.GetString(1);
                z.Pret = reader.GetDecimal(2);

                interogare.client = y;
                interogare.comanda = z;

                clienti.Add(interogare);

            }

            reader.Close();

            return View(clienti);

        }


        [Authorize(Users = "admin")]
        [HttpGet]
        public ActionResult InterogareDoi()
        {
            List<IntergareCompelxa4> y = new List<IntergareCompelxa4>();
            IntergareCompelxa4 z = new IntergareCompelxa4();
            y.Add(z);
            return View(y);
        }

        [Authorize(Users = "admin")]
        //comenzile ce contin biciclete cu pret_h mai mare ca x
        [HttpPost]
        public ActionResult InterogareDoi(List<IntergareCompelxa4> x)
        {
            List<IntergareCompelxa4> comenzi = new List<IntergareCompelxa4>();

            SqlConnection connection = new SqlConnection("data source = DESKTOP-IBNRKP3\\SQLEXPRESS; initial catalog = BD_Project; integrated security = True");
            SqlCommand cmd = new SqlCommand(@"                  
                                            Select c.IdComenzi, c.TimpStart, c.TimpStop from Comenzi c 
                                            where c.IdComenzi in (
					                                            select cb.IdComenzi from Comenzi_Biciclete cb
					                                            inner join Bicicleta b on b.IdBicicleta = cb.IdBicicleta
					                                            where b.Pret_h > " + x[0].pret +" group by cb.IdComenzi)", connection);

            connection.Open();
            SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                IntergareCompelxa4 interogare = new IntergareCompelxa4();
                Comenzi z = new Comenzi();

                z.IdComenzi = reader.GetInt32(0);
                z.TimpStart = reader.GetDateTime(1);
                z.TimpStop = reader.GetDateTime(2);

                interogare.coamnda = z;

                comenzi.Add(interogare);

            }

            reader.Close();

            return View(comenzi);

        }
    }
}