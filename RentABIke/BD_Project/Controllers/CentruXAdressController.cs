using BD_Project.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BD_Project.Controllers
{
    public class CentruXAdressController : Controller
    {
        DBContext db = new DBContext();

        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        [Authorize(Users = "admin")]
        public ActionResult CreareCentru()
        {
            var centru = new CentruXAdress();
            return View(centru);
        }

        [HttpPost]
        [Authorize(Users = "admin")]
        public ActionResult CreareCentru(CentruXAdress obj)
        {

            if (ModelState.IsValid)
            {
                Centru centru = new Centru();
                centru.Nume = obj.centruInfo.Nume;
                centru.Nr_telefon = obj.centruInfo.Nr_telefon;
                centru.Program = obj.centruInfo.Program;

                Adresa adresa = new Adresa();
                adresa.Bloc = obj.adressInfo.Bloc;
                adresa.Judet = obj.adressInfo.Judet;
                adresa.Nr = obj.adressInfo.Nr;
                adresa.Oras = obj.adressInfo.Oras;
                adresa.Strada = obj.adressInfo.Strada;

                db.Centru.Add(centru);
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