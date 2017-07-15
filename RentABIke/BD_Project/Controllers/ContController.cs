using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using System.Configuration;

namespace BD_Project.Controllers
{
    public class ContController : Controller
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
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public ActionResult Login(Client model, string returnUrl)
        {
            using (var context = new DBContext())
            {
                if (context.Set<Client>().Any(t => t.User == model.User && t.Password == model.Password))
                {
                    string username = model.User;
                    string password = model.Password;
                    var userValid = context.Client.Where(user => user.User == username && user.Password == password).FirstOrDefault();
                    if (userValid != null)
                    {
                        Session["IdClient"] = userValid.IdClient.ToString();
                        Session["User"] = userValid.User.ToString();
                    }
                }
                else
                {
                    this.ModelState.AddModelError("", "Nu exista un utilizator cu acest email sau parola");
                    return View(model);
                }
            }

            //return RedirectToAction("Index", "Home");
            using (var context = new DBContext())
            {
                if (context.Set<Client>().Any(t => t.User == model.User && t.Password == model.Password))
                {
                    var authCookie = FormsAuthentication.GetAuthCookie(model.User, true);
                    FormsAuthentication.SetAuthCookie(model.User, true);
                    this.HttpContext.Response.Cookies.Add(authCookie);
                    if (returnUrl != null)
                        return this.Redirect(returnUrl);
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    this.ModelState.AddModelError("", "Nu exista un utilizator cu acest email sau parola");
                    return View(model);
                }
            }

        }

        public ActionResult LogOff()
        {
            FormsAuthentication.SignOut();
            Session.RemoveAll();
            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public ActionResult EditareCont(int? id)
        {
            Client client = new Client();
            client.Adresa = new Adresa();

            SqlConnection connection = new SqlConnection("data source = DESKTOP-IBNRKP3\\SQLEXPRESS; initial catalog = BD_Project; integrated security = True");
           SqlCommand cmd = new SqlCommand(@"
                                                    select * from Client c
                                                    inner join Adresa a on a.IdAdresa = c.IdAdresa
                                                    where C.IdClient = @IdClient  ", connection);
                cmd.Parameters.AddWithValue("IdClient", id);
                connection.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    client.IdClient = reader.GetInt32(0);
                    client.Nume = reader.GetString(1);
                    client.Prenume = reader.GetString(2);
                    client.User = reader.GetString(3);
                    client.Password = reader.GetString(4);
                    client.E_mail = reader.GetString(5);
                    client.Nr_telefon = reader.GetString(6);
                    try
                    {
                        client.Data_nastere = reader.GetDateTime(7);
                    }
                    catch(Exception e)
                    {
                       
                    }
                    client.IdAdresa = reader.GetInt32(8);
                    client.Adresa.Judet = reader.GetString(10);
                    client.Adresa.Oras = reader.GetString(11);
                    client.Adresa.Strada = reader.GetString(12);
                    client.Adresa.Nr = reader.GetInt32(13);
                    try
                    {
                        client.Adresa.Bloc = reader.GetString(14);
                    }
                    catch (Exception e)
                    {

                    }
               
                }
                reader.Close();
            
            return View(client);
        }

        [HttpPost]
        public ActionResult EditareCont(Client obj)
        {
            SqlConnection connection = new SqlConnection("data source = DESKTOP-IBNRKP3\\SQLEXPRESS; initial catalog = BD_Project; integrated security = True");
            connection.Open();

            if (obj.Data_nastere != null)
            {
                SqlCommand cmd = new SqlCommand(@"update Client
                                    set Nume = @Nume, Prenume = @Prenume, [User] = @User, Password = @Password, E_mail = @E_mail, Nr_telefon = @Nr_telefon, Data_nastere = @Data_nastere
                                    where IdClient = @IdClient", connection);
                cmd.Parameters.AddWithValue("Nume", obj.Nume);
                cmd.Parameters.AddWithValue("Prenume", obj.Prenume);
                cmd.Parameters.AddWithValue("User", obj.User);
                cmd.Parameters.AddWithValue("Password", obj.Password);
                cmd.Parameters.AddWithValue("E_mail", obj.E_mail);
                cmd.Parameters.AddWithValue("Nr_telefon", obj.Nr_telefon);
                cmd.Parameters.AddWithValue("Data_nastere", obj.Data_nastere);
                cmd.Parameters.AddWithValue("IdClient", obj.IdClient);

                cmd.ExecuteNonQuery();
                connection.Close();
            }
            else
            {
                SqlCommand cmd = new SqlCommand(@"update Client
                                    set Nume = @Nume, Prenume = @Prenume, [User] = @User, Password = @Password, E_mail = @E_mail, Nr_telefon = @Nr_telefon
                                    where IdClient = @IdClient", connection);
                cmd.Parameters.AddWithValue("Nume", obj.Nume);
                cmd.Parameters.AddWithValue("Prenume", obj.Prenume);
                cmd.Parameters.AddWithValue("User", obj.User);
                cmd.Parameters.AddWithValue("Password", obj.Password);
                cmd.Parameters.AddWithValue("E_mail", obj.E_mail);
                cmd.Parameters.AddWithValue("Nr_telefon", obj.Nr_telefon);
                cmd.Parameters.AddWithValue("IdClient", obj.IdClient);

                cmd.ExecuteNonQuery();
                connection.Close();
            }
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
        public ActionResult ListaConturi()
        {
            List<Client> clients = new List<Client>();

            SqlConnection connection = new SqlConnection("data source = DESKTOP-IBNRKP3\\SQLEXPRESS; initial catalog = BD_Project; integrated security = True");
            SqlCommand cmd = new SqlCommand(@"
                                                select * from Client c
                                                inner join Adresa a on a.IdAdresa = c.IdAdresa", connection);
            connection.Open();
            SqlDataReader reader = cmd.ExecuteReader();


            while (reader.Read())
            {
                Client client = new Client();
                client.IdClient = reader.GetInt32(0);
                client.Nume = reader.GetString(1);
                client.Prenume = reader.GetString(2);
                client.User = reader.GetString(3);
                client.Password = reader.GetString(4);
                client.E_mail = reader.GetString(5);
                client.Nr_telefon = reader.GetString(6);
                try
                {
                    client.Data_nastere = reader.GetDateTime(7);
                }
                catch (Exception e)
                {

                }
                try
                {
                    client.IdAdresa = reader.GetInt32(8);
                }
                catch (Exception e)
                {

                }
                try
                {
                    client.Adresa.Judet = reader.GetString(10);
                }
                catch (Exception e)
                {

                }
                try
                {
                    client.Adresa.Oras = reader.GetString(11);
                }
                catch (Exception e)
                {

                }
                try
                {
                    client.Adresa.Strada = reader.GetString(12);
                }
                catch (Exception e)
                {

                }
                try
                {
                    client.Adresa.Nr = reader.GetInt32(13);
                }
                catch (Exception e)
                {

                }
                try
                {
                    client.Adresa.Bloc = reader.GetString(14);
                }
                catch (Exception e)
                {

                }
                clients.Add(client);
            }
            
            reader.Close();

            return View(clients);
        }

        [HttpGet]
        public ActionResult DetaliiUser(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("Index");
            }

            Client client = new Client();
            client.Adresa = new Adresa();

            SqlConnection connection = new SqlConnection("data source = DESKTOP-IBNRKP3\\SQLEXPRESS; initial catalog = BD_Project; integrated security = True");
            SqlCommand cmd = new SqlCommand(@"
                                                    select * from Client c
                                                    inner join Adresa a on a.IdAdresa = c.IdAdresa
                                                    where C.IdClient = @IdClient  ", connection);
            cmd.Parameters.AddWithValue("IdClient", id);
            connection.Open();
            SqlDataReader reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                client.IdClient = reader.GetInt32(0);
                client.Nume = reader.GetString(1);
                client.Prenume = reader.GetString(2);
                client.User = reader.GetString(3);
                client.Password = reader.GetString(4);
                client.E_mail = reader.GetString(5);
                client.Nr_telefon = reader.GetString(6);
                try
                {
                    client.Data_nastere = reader.GetDateTime(7);
                }
                catch (Exception e)
                {

                }
                client.IdAdresa = reader.GetInt32(8);
                client.Adresa.Judet = reader.GetString(10);
                client.Adresa.Oras = reader.GetString(11);
                client.Adresa.Strada = reader.GetString(12);
                client.Adresa.Nr = reader.GetInt32(13);
                try
                {
                    client.Adresa.Bloc = reader.GetString(14);
                }
                catch (Exception e)
                {

                }

            }
            reader.Close();

            return View(client);
        }

        [HttpGet]
        [Authorize(Users = "admin")]
        public ActionResult StergeUser(int? id)
        {
            if (id != null)
            {
                var client = dbContext.Client.Find(id);
                if (client != null)
                {
                    dbContext.Client.Remove(client);
                    dbContext.SaveChanges();
                }
            }
            return RedirectToAction("ListaConturi");
        }

    }
}