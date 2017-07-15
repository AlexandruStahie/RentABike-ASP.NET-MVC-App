using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BD_Project.Controllers
{
    public class CentruController : Controller
    {
        private DBContext dbContext = new DBContext();

        public ActionResult Index()
        {
            using (DBContext db = new DBContext())
            {
                return View();
            }
        }

        [HttpGet]
        [Authorize(Users = "admin")]
        public ActionResult EditareCentru(int? id)
        {
            Centru centru = new Centru();
            centru.Adresa = new Adresa();

            SqlConnection connection = new SqlConnection("data source = DESKTOP-IBNRKP3\\SQLEXPRESS; initial catalog = BD_Project; integrated security = True");
            SqlCommand cmd = new SqlCommand(@"
                                                    select * from Centru c
                                                    inner join Adresa a on a.IdAdresa = c.IdAdresa
                                                    where C.IdCentru = " + id, connection);

            connection.Open();
            SqlDataReader reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                centru.IdCentru = reader.GetInt32(0);
                centru.Nume = reader.GetString(1);
                centru.Nr_telefon = reader.GetString(2);
                centru.Program = reader.GetString(3);
                centru.IdAdresa = reader.GetInt32(4);

                centru.Adresa.Judet = reader.GetString(6);
                centru.Adresa.Oras = reader.GetString(7);
                centru.Adresa.Strada = reader.GetString(8);
                centru.Adresa.Nr = reader.GetInt32(9);
              

            }
            reader.Close();

            return View(centru);
        }

        [HttpPost]
        [Authorize(Users = "admin")]
        public ActionResult EditareCentru(Centru obj)
        {
            SqlConnection connection = new SqlConnection("data source = DESKTOP-IBNRKP3\\SQLEXPRESS; initial catalog = BD_Project; integrated security = True");
            connection.Open();

                SqlCommand cmd = new SqlCommand(@"update Centru
                                    set Nume = @Nume, Nr_telefon = @Nr_telefon, Program = @Program 
                                    where IdCentru = @IdCentru", connection);

                cmd.Parameters.AddWithValue("Nume", obj.Nume);
                cmd.Parameters.AddWithValue("Nr_telefon", obj.Nr_telefon);
                cmd.Parameters.AddWithValue("Program", obj.Program);
                cmd.Parameters.AddWithValue("IdCentru", obj.IdCentru);

            cmd.ExecuteNonQuery();
                connection.Close();
            
         
            if (obj.Adresa.Bloc != null)
            {
                SqlConnection connection2 = new SqlConnection("data source = DESKTOP-IBNRKP3\\SQLEXPRESS; initial catalog = BD_Project; integrated security = True");
                connection.Open();
                SqlCommand cmd2 = new SqlCommand(@"update Adresa
                                    set Judet = @Judet, Oras = @Oras, Strada = @Strada, [Nr.] = @Nr, Bloc = @Bloc
                                    where IdAdresa = @IdAdresa", connection);

                cmd2.Parameters.AddWithValue("Judet", obj.Adresa.Judet);
                cmd2.Parameters.AddWithValue("Oras", obj.Adresa.Oras);
                cmd2.Parameters.AddWithValue("Strada", obj.Adresa.Strada);
                cmd2.Parameters.AddWithValue("Nr", obj.Adresa.Nr);
                cmd2.Parameters.AddWithValue("Bloc", obj.Adresa.Bloc);
                cmd2.Parameters.AddWithValue("IdAdresa", obj.IdAdresa);

                cmd2.ExecuteNonQuery();
                connection.Close();
            }
            else
            {
                SqlConnection connection2 = new SqlConnection("data source = DESKTOP-IBNRKP3\\SQLEXPRESS; initial catalog = BD_Project; integrated security = True");
                connection.Open();
                SqlCommand cmd2 = new SqlCommand(@"update Adresa
                                    set Judet = @Judet, Oras = @Oras, Strada = @Strada, [Nr.] = @Nr
                                    where IdAdresa = @IdAdresa", connection);

                cmd2.Parameters.AddWithValue("Judet", obj.Adresa.Judet);
                cmd2.Parameters.AddWithValue("Oras", obj.Adresa.Oras);
                cmd2.Parameters.AddWithValue("Strada", obj.Adresa.Strada);
                cmd2.Parameters.AddWithValue("Nr", obj.Adresa.Nr);
                cmd2.Parameters.AddWithValue("IdAdresa", obj.IdAdresa);

                cmd2.ExecuteNonQuery();
                connection.Close();
            }
            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        [Authorize(Users = "admin")]
        public ActionResult ListaCentre()
        {
            List<Centru> centre = new List<Centru>();

            SqlConnection connection = new SqlConnection("data source = DESKTOP-IBNRKP3\\SQLEXPRESS; initial catalog = BD_Project; integrated security = True");
            SqlCommand cmd = new SqlCommand(@"
                                                select * from Centru c
                                                inner join Adresa a on a.IdAdresa = c.IdAdresa", connection);
            connection.Open();
            SqlDataReader reader = cmd.ExecuteReader();


            while (reader.Read())
            {
                Centru centru = new Centru();
                centru.IdCentru = reader.GetInt32(0);
                centru.Nume = reader.GetString(1);
                centru.Nr_telefon = reader.GetString(2);
                centru.Program = reader.GetString(3);
                centru.IdAdresa = reader.GetInt32(4);             

                try
                {
                    centru.Adresa.Judet = reader.GetString(6);
                }
                catch (Exception e)
                {

                }
                try
                {
                    centru.Adresa.Oras = reader.GetString(7);
                }
                catch (Exception e)
                {

                }
                try
                {
                    centru.Adresa.Strada = reader.GetString(8);
                }
                catch (Exception e)
                {

                }
                try
                {
                    centru.Adresa.Nr = reader.GetInt32(9);
                }
                catch (Exception e)
                {

                }
              
                centre.Add(centru);
            }

            reader.Close();

            return View(centre);
        }

        [HttpGet]
        public ActionResult ListaCentre2()
        {
            List<Centru> centre = new List<Centru>();

            SqlConnection connection = new SqlConnection("data source = DESKTOP-IBNRKP3\\SQLEXPRESS; initial catalog = BD_Project; integrated security = True");
            SqlCommand cmd = new SqlCommand(@"
                                                select * from Centru c
                                                inner join Adresa a on a.IdAdresa = c.IdAdresa", connection);
            connection.Open();
            SqlDataReader reader = cmd.ExecuteReader();


            while (reader.Read())
            {
                Centru centru = new Centru();
                centru.IdCentru = reader.GetInt32(0);
                centru.Nume = reader.GetString(1);
                centru.Nr_telefon = reader.GetString(2);
                centru.Program = reader.GetString(3);
                centru.IdAdresa = reader.GetInt32(4);

                try
                {
                    centru.Adresa.Judet = reader.GetString(6);
                }
                catch (Exception e)
                {

                }
                try
                {
                    centru.Adresa.Oras = reader.GetString(7);
                }
                catch (Exception e)
                {

                }
                try
                {
                    centru.Adresa.Strada = reader.GetString(8);
                }
                catch (Exception e)
                {

                }
                try
                {
                    centru.Adresa.Nr = reader.GetInt32(9);
                }
                catch (Exception e)
                {

                }

                centre.Add(centru);
            }

            reader.Close();

            return View(centre);
        }

        [HttpGet]
        public ActionResult DetaliiCentru(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("Index");
            }

            Centru centru = new Centru();
            centru.Adresa = new Adresa();

            SqlConnection connection = new SqlConnection("data source = DESKTOP-IBNRKP3\\SQLEXPRESS; initial catalog = BD_Project; integrated security = True");
            SqlCommand cmd = new SqlCommand(@"
                                                    select * from Centru c
                                                    inner join Adresa a on a.IdAdresa = c.IdAdresa
                                                    where C.IdCentru = " + id, connection);
            connection.Open();
            SqlDataReader reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                centru.IdCentru = reader.GetInt32(0);
                centru.Nume = reader.GetString(1);
                centru.Nr_telefon = reader.GetString(2);
                centru.Program = reader.GetString(3);
                centru.IdAdresa = reader.GetInt32(4);

                centru.Adresa.Judet = reader.GetString(6);
                centru.Adresa.Oras = reader.GetString(7);
                centru.Adresa.Strada = reader.GetString(8);
                centru.Adresa.Nr = reader.GetInt32(9);
             
            }
            reader.Close();

            return View(centru);
        }

        [HttpGet]
        [Authorize(Users = "admin")]
        public ActionResult StergeCentru(int? id)
        {
            if (id != null)
            {
                var centru = dbContext.Centru.Find(id);
                if (centru != null)
                {
                    dbContext.Centru.Remove(centru);
                    dbContext.SaveChanges();
                }
            }
            return RedirectToAction("ListaCentre");
        }

    }
}