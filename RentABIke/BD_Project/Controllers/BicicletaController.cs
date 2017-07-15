using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BD_Project.Controllers
{
    public class BicicletaController : Controller
    {
        DBContext db = new DBContext();

        // GET: Bicicleta
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        [Authorize(Users = "admin")]
        public ActionResult AdaugaBicicleta()
        {
            var bicicleta = new Bicicleta();
            return View(bicicleta);
        }

        [HttpPost]
        [Authorize(Users = "admin")]
        public ActionResult AdaugaBicicleta(Bicicleta obj)
        {

            if (ModelState.IsValid)
            {
                Bicicleta bicicleta = new Bicicleta();
                bicicleta.Categorie = obj.Categorie;
                bicicleta.Model = obj.Model;
                bicicleta.Pret_h = obj.Pret_h;
                bicicleta.Detalii = obj.Detalii;
                bicicleta.ImageLink = obj.ImageLink;
                        
                db.Bicicleta.Add(bicicleta);
                db.SaveChanges();
            }
            else
            {
                return View(obj);
            }
            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        [Authorize(Users = "admin")]
        public ActionResult EditareBicicleta(int? id)
        {
            Bicicleta bicicleta = new Bicicleta();

            SqlConnection connection = new SqlConnection("data source = DESKTOP-IBNRKP3\\SQLEXPRESS; initial catalog = BD_Project; integrated security = True");
            SqlCommand cmd = new SqlCommand(@"
                                                    select * from Bicicleta b
                                                    where b.IdBicicleta = "+ id, connection);
          
            connection.Open();
            SqlDataReader reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                try { bicicleta.IdBicicleta = reader.GetInt32(0); }
                catch (Exception e) { }
                try { bicicleta.Categorie = reader.GetString(1); }
                catch (Exception e) { }
                try { bicicleta.Model = reader.GetString(2); }
                catch (Exception e) { }
                try { bicicleta.Pret_h = reader.GetDecimal(3); }
                catch (Exception e) { }
                try { bicicleta.Detalii = reader.GetString(4); }
                catch (Exception e) { }
                try { bicicleta.ImageLink = reader.GetString(5); }
                catch (Exception e) { }
            }
            reader.Close();
            return View(bicicleta);
        }

        [HttpPost]
        [Authorize(Users = "admin")]
        public ActionResult EditareBicicleta(Bicicleta obj)
        {
            SqlConnection connection = new SqlConnection("data source = DESKTOP-IBNRKP3\\SQLEXPRESS; initial catalog = BD_Project; integrated security = True");
            connection.Open();

                SqlCommand cmd = new SqlCommand(@"update Bicicleta
                                    set  Categorie = @Categorie, Model = @Model, Pret_h = @Pret_h, Detalii = @Detalii, ImageLink=@ImageLink 
                                    where IdBicicleta = @IdBicicleta", connection);

                cmd.Parameters.AddWithValue("IdBicicleta", obj.IdBicicleta);
                cmd.Parameters.AddWithValue("Categorie", obj.Categorie);
                cmd.Parameters.AddWithValue("Model", obj.Model);
                cmd.Parameters.AddWithValue("Pret_h", obj.Pret_h);
                cmd.Parameters.AddWithValue("Detalii", obj.Detalii);
                cmd.Parameters.AddWithValue("ImageLink", obj.ImageLink);

                cmd.ExecuteNonQuery();
                connection.Close();

            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        [Authorize(Users = "admin")]
        public ActionResult ListaBiciclete()
        {
            List<Bicicleta> biciclete = new List<Bicicleta>();

            SqlConnection connection = new SqlConnection("data source = DESKTOP-IBNRKP3\\SQLEXPRESS; initial catalog = BD_Project; integrated security = True");
            SqlCommand cmd = new SqlCommand(@" select * from Bicicleta b ", connection);
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
                try { bicicleta.Detalii = reader.GetString(4); }
                catch (Exception e) { }
                try { bicicleta.ImageLink = reader.GetString(5); }
                catch (Exception e) { }

                biciclete.Add(bicicleta);
            }

            reader.Close();

            return View(biciclete);
        }

        [HttpGet]
        public ActionResult ListaBiciclete2()
        {
            List<Bicicleta> biciclete = new List<Bicicleta>();

            SqlConnection connection = new SqlConnection("data source = DESKTOP-IBNRKP3\\SQLEXPRESS; initial catalog = BD_Project; integrated security = True");
            SqlCommand cmd = new SqlCommand(@" select * from Bicicleta b ", connection);
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
                try { bicicleta.Detalii = reader.GetString(4); }
                catch (Exception e) { }
                try { bicicleta.ImageLink = reader.GetString(5); }
                catch (Exception e) { }

                biciclete.Add(bicicleta);
            }

            reader.Close();

            return View(biciclete);
        }

        [HttpGet]
        public ActionResult DetaliiBicicleta(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("Index");
            }

            Bicicleta bicicleta = new Bicicleta();

            SqlConnection connection = new SqlConnection("data source = DESKTOP-IBNRKP3\\SQLEXPRESS; initial catalog = BD_Project; integrated security = True");
            SqlCommand cmd = new SqlCommand(@"   select * from Bicicleta b
                                                 where b.IdBicicleta = @IdBicicleta ", connection);

            cmd.Parameters.AddWithValue("IdBicicleta", id);
            connection.Open();
            SqlDataReader reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                try { bicicleta.IdBicicleta = reader.GetInt32(1); }
                catch (Exception e) { }
                try { bicicleta.Categorie = reader.GetString(1); }
                catch (Exception e) { }
                try { bicicleta.Model = reader.GetString(2); }
                catch (Exception e) { }
                try { bicicleta.Pret_h = reader.GetDecimal(3); }
                catch (Exception e) { }
                try { bicicleta.Detalii = reader.GetString(4); }
                catch (Exception e) { }
                try { bicicleta.ImageLink = reader.GetString(5); }
                catch (Exception e) { }
            }
            reader.Close();

            return View(bicicleta);
        }

        [HttpGet]
        public ActionResult DetaliiBicicleta2(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("Index");
            }

            Bicicleta bicicleta = new Bicicleta();

            SqlConnection connection = new SqlConnection("data source = DESKTOP-IBNRKP3\\SQLEXPRESS; initial catalog = BD_Project; integrated security = True");
            SqlCommand cmd = new SqlCommand(@"   select * from Bicicleta b
                                                 where b.IdBicicleta = @IdBicicleta ", connection);

            cmd.Parameters.AddWithValue("IdBicicleta", id);
            connection.Open();
            SqlDataReader reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                try { bicicleta.IdBicicleta = reader.GetInt32(1); }
                catch (Exception e) { }
                try { bicicleta.Categorie = reader.GetString(1); }
                catch (Exception e) { }
                try { bicicleta.Model = reader.GetString(2); }
                catch (Exception e) { }
                try { bicicleta.Pret_h = reader.GetDecimal(3); }
                catch (Exception e) { }
                try { bicicleta.Detalii = reader.GetString(4); }
                catch (Exception e) { }
                try { bicicleta.ImageLink = reader.GetString(5); }
                catch (Exception e) { }
            }

            reader.Close();

            return View(bicicleta);
        }

        [HttpGet]
        [Authorize(Users = "admin")]
        public ActionResult StergeBicicleta(int? id)
        {
            if (id != null)
            {
                var bicicleta = db.Bicicleta.Find(id);
                if (bicicleta != null)
                {
                    db.Bicicleta.Remove(bicicleta);
                    db.SaveChanges();
                }
            }
            return RedirectToAction("ListaBiciclete");
        }
    }
}