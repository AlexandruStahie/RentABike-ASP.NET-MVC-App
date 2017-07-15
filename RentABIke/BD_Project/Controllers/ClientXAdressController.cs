using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BD_Project.Models;
using NHibernate;
using NHibernate.Transform;
using System.Data.SqlClient;
using System.Data.Entity;

namespace BD_Project.Controllers
{
    public class ClientXAdressController : Controller
    {
        DBContext db = new DBContext();
        
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult CreareCont()
        {
            var cont = new ClinetxAdress();
            return View(cont);
        }

        [HttpPost]
        public ActionResult CreareCont(ClinetxAdress obj)
        {

            if (ModelState.IsValid)
            {
                Client client = new Client();
                client.Nume = obj.clientInfo.Nume;
                client.Prenume = obj.clientInfo.Prenume;
                client.User = obj.clientInfo.User;
                client.Password = obj.clientInfo.Password;
                client.Nr_telefon = obj.clientInfo.Nr_telefon;
                client.E_mail = obj.clientInfo.E_mail;
                client.Data_nastere = obj.clientInfo.Data_nastere;

                Adresa adresa = new Adresa();
                adresa.Bloc = obj.adressInfo.Bloc;
                adresa.Judet = obj.adressInfo.Judet;
                adresa.Nr = obj.adressInfo.Nr;
                adresa.Oras = obj.adressInfo.Oras;
                adresa.Strada = obj.adressInfo.Strada;

                db.Client.Add(client);
                db.Adresa.Add(adresa);
                db.SaveChanges();
            }
            else
            {
                return View(obj);
            }
            return RedirectToAction("Index", "Home");
        }

     
    }
}