using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BD_Project.Models;
using System.Data.SqlClient;


namespace BD_Project.Controllers
{
    public class SimpleQueryController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }


        //clientii care au comenzi peste 90 lei
        [Authorize(Users = "admin")]
        [HttpGet]
        public ActionResult InterogareUnu()
        {
            List<Interogare1> clienti = new List<Interogare1>();

            SqlConnection connection = new SqlConnection("data source = DESKTOP-IBNRKP3\\SQLEXPRESS; initial catalog = BD_Project; integrated security = True");
            SqlCommand cmd = new SqlCommand(@"
                                                select co.IdComenzi, c.Nume, c.Prenume, co.Pret  from Client c
                                                inner join Comenzi co on co.IdClient = c.IdClient
                                                inner join Comenzi_Biciclete cb on cb.IdComenzi = co.IdComenzi
                                                where co.Pret > 90
                                                group by c.Nume, c.Prenume, co.Pret, co.IdComenzi", connection);

            connection.Open();
            SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                Interogare1 x = new Interogare1();
                Comenzi y = new Comenzi();
                Client z = new Client();

                y.IdComenzi = reader.GetInt32(0);
                z.Nume =  reader.GetString(1);
                z.Prenume = reader.GetString(2);
                y.Pret = reader.GetDecimal(3);

                x.client = z;
                x.comanda = y;

                clienti.Add(x);
             
            }

            reader.Close();

            return View(clienti);

        }

        //top 2 biciclete care apar cele mai des in comenzi
        [Authorize(Users = "admin")]
        [HttpGet]
        public ActionResult InterogareDoi()
        {
            List<Interogare2cs> clienti = new List<Interogare2cs>();

            SqlConnection connection = new SqlConnection("data source = DESKTOP-IBNRKP3\\SQLEXPRESS; initial catalog = BD_Project; integrated security = True");
            SqlCommand cmd = new SqlCommand(@"  select top 2 cb.IdBicicleta, b.Model, b.Categorie, COUNT(cb.IdBicicleta) from Comenzi_Biciclete cb
                                                left join Bicicleta b on b.IdBicicleta = cb.IdBicicleta
                                                group by cb.IdBicicleta, b.Model, b.Categorie
                                                order by count(cb.IdBicicleta) desc", connection);

            connection.Open();
            SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                Interogare2cs x = new Interogare2cs();
                Bicicleta Y = new Bicicleta();

    
                Y.IdBicicleta= reader.GetInt32(0);
                Y.Model = reader.GetString(1);
                Y.Categorie= reader.GetString(2);
                x.count = reader.GetInt32(3);

                x.bicicleta = Y;
                clienti.Add(x);

            }

            reader.Close();

            return View(clienti);

        }


        //clientul cu cele mai multe comenzi
        [Authorize(Users = "admin")]
        [HttpGet]
        public ActionResult InterogareTrei()
        {
            List<Interogare1> clienti = new List<Interogare1>();

            SqlConnection connection = new SqlConnection("data source = DESKTOP-IBNRKP3\\SQLEXPRESS; initial catalog = BD_Project; integrated security = True");
            SqlCommand cmd = new SqlCommand(@" 
                                                select  top 1 COUNT(c.IdClient), cl.Nume, cl.Prenume, cl.IdClient  from Comenzi c
                                                inner join Client cl on cl.IdClient = c.IdClient
                                                group by cl.IdClient, cl.Prenume, cl.Nume
                                                order by COUNT(c.IdClient) desc", connection);
            connection.Open();
            SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                Interogare1 x = new Interogare1();
                Client Y = new Client();



                x.count= reader.GetInt32(0);
                Y.Nume = reader.GetString(1);
                Y.Prenume = reader.GetString(2);
                Y.IdClient = reader.GetInt32(3);

                x.client = Y;
                clienti.Add(x);

            }

            reader.Close();

            return View(clienti);

        }

    }
}