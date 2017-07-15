using BD_Project.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BD_Project.Controllers
{
    public class ComenziController : Controller
    {

        private DBContext dbContext = new DBContext();

        // GET: Comenzi
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult AdaugaComanda()
        {
            Comenzi_Final model = new Comenzi_Final();
            List<Bicicleta> bic = new List<Bicicleta>();
            #region biciclete         

            SqlConnection connection = new SqlConnection("data source = DESKTOP-IBNRKP3\\SQLEXPRESS; initial catalog = BD_Project; integrated security = True");
            SqlCommand cmd = new SqlCommand(@" select b.IdBicicleta, b.Categorie, b.Model, b.Pret_h from Bicicleta b ", connection);
            connection.Open();
            SqlDataReader reader = cmd.ExecuteReader();


            while (reader.Read())
            {
                Bicicleta bicicleta = new Bicicleta();
                try { bicicleta.IdBicicleta = reader.GetInt32(0); }
                catch (Exception e) { }
                try { bicicleta.Categorie = reader.GetString(1); }
                catch (Exception e) { }
                try { bicicleta.Model = reader.GetString(2); }
                catch (Exception e) { }
                try { bicicleta.Pret_h = reader.GetDecimal(3); }
                catch (Exception e) { }

                bic.Add(bicicleta);
            }

            reader.Close();

            #endregion

            //dropdownlist centre
            CentreDisponibile();
            model.biciclete = bic;
            Comenzi pret = new Comenzi();
            pret.Pret = 0;
            
            model.comanda = pret;

            return View(model);
        }

        private void CentreDisponibile(long? selectedCenterId = null)
        {
            #region Centre
            List<Centru> centre = new List<Centru>();

            SqlConnection connection2 = new SqlConnection("data source = DESKTOP-IBNRKP3\\SQLEXPRESS; initial catalog = BD_Project; integrated security = True");
            SqlCommand cmd2 = new SqlCommand(@" select c.IdCentru, c.Nume from Centru c ", connection2);

            connection2.Open();
            SqlDataReader reader2 = cmd2.ExecuteReader();

            while (reader2.Read())
            {
                Centru centru = new Centru();

                centru.IdCentru = reader2.GetInt32(0);
                centru.Nume = reader2.GetString(1);

                centre.Add(centru);
            }
            reader2.Close();

            #endregion

            #region DropDownList

            ViewBag.CentreDisponibile = new SelectList(centre, "IdCentru", "Nume", selectedCenterId);

            #endregion
        }

        public JsonResult PretPlus(DateTime datastart, DateTime datastop, decimal pretcurent, decimal pret_h)
        {

            var res = true;
            try
            {
                var ore = (datastop - datastart).TotalHours;
                pretcurent = (pretcurent + pret_h) * (decimal)ore;
            }
            catch (Exception ex)
            {
                res = false;
            }
            return Json(new { ceva = pretcurent });

        }

        public JsonResult PretMinus(DateTime datastart, DateTime datastop, decimal pretcurent, decimal pret_h)
        {

            var res = true;
            try
            {
                var ore = (datastop - datastart).TotalHours;
                pretcurent = (pretcurent - pret_h) * (decimal)ore;
            }
            catch (Exception ex)
            {
                res = false;
            }
            return Json(new { ceva = pretcurent });

        }


        [HttpPost]
        public ActionResult AdaugaComanda(Comenzi_Final model)
        {

            //CALCULARE PRET FINAL 
            var ore = (model.comanda.TimpStop - model.comanda.TimpStart).TotalHours;
            List < Bicicleta > bicicleteAlese = model.biciclete.Where(x => x.IsEnabled == true).ToList();
            decimal pret = 0;
            foreach (var bicicleta in bicicleteAlese)
            {
                pret  =  pret + bicicleta.Pret_h;
            }

            decimal pretFinal = pret * (decimal)ore;



            //insert into Comenzi
            #region inserare comanda
            SqlConnection connection = new SqlConnection("data source = DESKTOP-IBNRKP3\\SQLEXPRESS; initial catalog = BD_Project; integrated security = True");
            connection.Open();

            SqlCommand cmd = new SqlCommand(@"insert into Comenzi
                                values (
                                @Tip_Plata, 
                                @TimpStart,
                                @TimpStop,
                                @IdClient,
                                @IdCentru,
                                @Pret);
                                ", connection);

            cmd.Parameters.AddWithValue("Tip_Plata", model.comanda.Tip_Plata);
            cmd.Parameters.AddWithValue("TimpStart", model.comanda.TimpStart);
            cmd.Parameters.AddWithValue("TimpStop", model.comanda.TimpStop);
            cmd.Parameters.AddWithValue("IdClient", Session["IdClient"]);
            cmd.Parameters.AddWithValue("IdCentru", model.IdCentru);
            cmd.Parameters.AddWithValue("Pret", pretFinal);

            cmd.ExecuteNonQuery();
            connection.Close();
            #endregion


            #region lastIdFromSql
            SqlCommand cmd2 = new SqlCommand(@" SELECT ident_current( 'Comenzi' ) ", connection); //intoarce ultimul id introdus in baza
            connection.Open();
  
              
               model.comanda.IdComenzi = (int) Convert.ToInt32(cmd2.ExecuteScalar()); 
    
            connection.Close();
            #endregion

            //insert into comenzi_biciclete
            #region inserareBiciclete
            connection.Open();

            foreach (var x in bicicleteAlese)
            {
                SqlCommand cmd3 = new SqlCommand(@"insert into Comenzi_Biciclete
                                values (
                                 @IdComenzi, 
                                 @IdBicicleta);
                                ", connection);

                cmd3.Parameters.AddWithValue("IdComenzi", model.comanda.IdComenzi);
                cmd3.Parameters.AddWithValue("IdBicicleta", x.IdBicicleta);

                cmd3.ExecuteNonQuery();
            }
            connection.Close();
            #endregion


            CentreDisponibile();
            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public ActionResult ListaComenziActive()
        {
            List<Comenzi>  comenzi = new List<Comenzi>();
            DateTime azi = DateTime.Now;

            SqlConnection connection = new SqlConnection("data source = DESKTOP-IBNRKP3\\SQLEXPRESS; initial catalog = BD_Project; integrated security = True");
            SqlCommand cmd = new SqlCommand(@"
                                                select c.IdComenzi, c.TimpStart, c.TimpStop, c.IdClient from Comenzi c
                                                inner join Comenzi_Biciclete cb on c.IdComenzi = cb.IdComenzi
                                                where IdClient = " + Session["IdClient"] + " group by c.IdComenzi, c.TimpStart, c.TimpStop, c.IdClient", connection);

            connection.Open();
            SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                Comenzi comanda = new Comenzi();

                comanda.IdComenzi = reader.GetInt32(0);
                comanda.TimpStart = reader.GetDateTime(1); 
                comanda.TimpStop = reader.GetDateTime(2); 


                if ( comanda.TimpStop != null && azi < comanda.TimpStop)
                comenzi.Add(comanda);
            }

            reader.Close();

            return View(comenzi);
        }

        [HttpGet]
        public ActionResult ListaComenziExpirate()
        {
            List<Comenzi> comenzi = new List<Comenzi>();
            DateTime azi = DateTime.Now;

            SqlConnection connection = new SqlConnection("data source = DESKTOP-IBNRKP3\\SQLEXPRESS; initial catalog = BD_Project; integrated security = True");
            SqlCommand cmd = new SqlCommand(@"
                                                select c.IdComenzi, c.TimpStart, c.TimpStop, c.IdClient from Comenzi c
                                                inner join Comenzi_Biciclete cb on c.IdComenzi = cb.IdComenzi
                                                where IdClient = " + Session["IdClient"] + " group by c.IdComenzi, c.TimpStart, c.TimpStop, c.IdClient", connection);

            connection.Open();
            SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                Comenzi comanda = new Comenzi();

                comanda.IdComenzi = reader.GetInt32(0);
                comanda.TimpStart = reader.GetDateTime(1);
                comanda.TimpStop = reader.GetDateTime(2);


                if (comanda.TimpStop != null && azi > comanda.TimpStop)
                    comenzi.Add(comanda);
            }

            reader.Close();

            return View(comenzi);
        }

        [HttpGet]

        public ActionResult ListaComenziActivePerClient(int ? id = null)
        {
            List<Comenzi> comenzi = new List<Comenzi>();
            DateTime azi = DateTime.Now;

            SqlConnection connection = new SqlConnection("data source = DESKTOP-IBNRKP3\\SQLEXPRESS; initial catalog = BD_Project; integrated security = True");
            SqlCommand cmd = new SqlCommand(@"
                                                select c.IdComenzi, c.TimpStart, c.TimpStop, c.IdClient from Comenzi c
                                                inner join Comenzi_Biciclete cb on c.IdComenzi = cb.IdComenzi
                                                where IdClient = " + id + " group by c.IdComenzi, c.TimpStart, c.TimpStop, c.IdClient", connection);

            connection.Open();
            SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                Comenzi comanda = new Comenzi();

                comanda.IdComenzi = reader.GetInt32(0);
                comanda.TimpStart = reader.GetDateTime(1);
                comanda.TimpStop = reader.GetDateTime(2);


                if (comanda.TimpStop != null && azi < comanda.TimpStop)
                    comenzi.Add(comanda);
            }

            reader.Close();

            return View(comenzi);
        }

        [HttpGet]
        public ActionResult ListaComenziExpiratePerClient(int? id = null)
        {
            List<Comenzi> comenzi = new List<Comenzi>();
            DateTime azi = DateTime.Now;

            SqlConnection connection = new SqlConnection("data source = DESKTOP-IBNRKP3\\SQLEXPRESS; initial catalog = BD_Project; integrated security = True");
            SqlCommand cmd = new SqlCommand(@"
                                                select c.IdComenzi, c.TimpStart, c.TimpStop, c.IdClient from Comenzi c
                                                inner join Comenzi_Biciclete cb on c.IdComenzi = cb.IdComenzi
                                                where IdClient = " + id + " group by c.IdComenzi, c.TimpStart, c.TimpStop, c.IdClient", connection);

            connection.Open();
            SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                Comenzi comanda = new Comenzi();

                comanda.IdComenzi = reader.GetInt32(0);
                comanda.TimpStart = reader.GetDateTime(1);
                comanda.TimpStop = reader.GetDateTime(2);


                if (comanda.TimpStop != null && azi > comanda.TimpStop)
                    comenzi.Add(comanda);
            }

            reader.Close();

            return View(comenzi);
        }

        [HttpGet]
        public ActionResult DetaliiComanda(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("Index");
            }

            Comenzi_Final comandafinala = new Comenzi_Final();

            SqlConnection connection = new SqlConnection("data source = DESKTOP-IBNRKP3\\SQLEXPRESS; initial catalog = BD_Project; integrated security = True");
            SqlCommand cmd = new SqlCommand(@" select * from Comenzi c
                                                inner join Centru ce on c.IdCentru = ce.IdCentru 
                                                where c.IdComenzi = " + id, connection);
            connection.Open();
            SqlDataReader reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                Comenzi comanda = new Comenzi();
                Centru centru = new Centru();

                comanda.IdComenzi = reader.GetInt32(0);
                comanda.Tip_Plata = reader.GetString(1);
                comanda.TimpStart = reader.GetDateTime(2);
                comanda.TimpStop = reader.GetDateTime(3);
                centru.IdCentru = reader.GetInt32(5);
                comanda.Pret = reader.GetDecimal(6);
                centru.Nume = reader.GetString(8);
                centru.Nr_telefon = reader.GetString(9);
                centru.Program = reader.GetString(10);

                comandafinala.comanda = comanda;
                comandafinala.centru = centru;
            }
            reader.Close();
            connection.Close();


            SqlConnection connection2 = new SqlConnection("data source = DESKTOP-IBNRKP3\\SQLEXPRESS; initial catalog = BD_Project; integrated security = True");
            SqlCommand cmd2 = new SqlCommand(@" 
                                                select * from Comenzi_Biciclete cb 
                                                inner join Bicicleta b on b.IdBicicleta = cb.IdBicicleta
                                                where cb.IdComenzi = " + id, connection2);
            connection2.Open();
            SqlDataReader reader2 = cmd2.ExecuteReader();
            List<Bicicleta> listabiciclete = new List<Bicicleta>();

            while (reader2.Read())
            {
                Bicicleta bic = new Bicicleta();

                bic.IdBicicleta = reader2.GetInt32(3);
                bic.Categorie = reader2.GetString(4);
                bic.Model = reader2.GetString(5);
                bic.Pret_h = reader2.GetDecimal(6);
                bic.Detalii = reader2.GetString(7);

                listabiciclete.Add(bic);
            }
            reader2.Close();
            connection2.Close();
            comandafinala.biciclete = listabiciclete;
            return View( comandafinala);
        }

        [HttpGet]
        public ActionResult StergeComanda(int? id)
        {
            if (id != null)
            {
                var comanda = dbContext.Comenzi.Find(id);
                if (comanda != null)
                {
                    dbContext.Comenzi.Remove(comanda);
                    dbContext.SaveChanges();
                }
            }
            return RedirectToAction("Index", "Cont");
        }

    }
}