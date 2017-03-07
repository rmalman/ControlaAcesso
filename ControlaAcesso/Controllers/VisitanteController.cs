using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ControlaAcesso.Models;
using ControlaAcesso.DAL;
using System.IO;
using System.Drawing;
using System.Drawing.Imaging;
using System.Security.Cryptography;

namespace ControlaAcesso.Controllers
{
    public class VisitanteController : Controller
    {
        private ControlaAcessoContext db = new ControlaAcessoContext();

        // GET: Visitante
        [Authorize]
        public ActionResult Index(string sortOrder, string searchString)
        {
            if (!String.IsNullOrEmpty(searchString))
            {
                var visitante = from v in db.Visitantes
                                select v;

                Int64 CPF = 0;
                bool isNumeric = Int64.TryParse(searchString, out CPF);

                if (isNumeric)
                    visitante = visitante.OrderBy(e => e.Nome).Where(s => s.CPF.Equals(CPF));
                else
                    visitante = visitante.OrderBy(e => e.Nome).Where(s => s.Nome.Contains(searchString));

                ViewBag.Visitantes = new SelectList(visitante, "VisitanteID", "Nome");
                return View(visitante.ToList());
            }
            return View(db.Visitantes.ToList());
        }

        // GET: Visitante/Details/5
        [Authorize]
        public ActionResult Details(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Visitante visitante = db.Visitantes.Find(id);
            if (visitante == null)
            {
                return HttpNotFound();
            }
            return View(visitante);
        }

        // GET: Visitante/Create
        [Authorize]
        public ActionResult Create()
        {
            return View();
        }

        // POST: Visitante/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "VisitanteID,Nome,DataNascimento,CPF,Contato")] Visitante visitante)
        {
            if (ModelState.IsValid)
            {
                if (Request.InputStream != null)
                {
                    var CamResult64 = Request.Form["CamResult64"];

                    byte[] data = System.Convert.FromBase64String(CamResult64);
                    MemoryStream ms = new MemoryStream(data);
                    Image WebCamPic = Image.FromStream(ms);
                    
                    string NomeFoto = visitante.CPF.ToString();
                    var path = Server.MapPath("~/IMG/" + NomeFoto + ".jpg");
                    WebCamPic.Save(path, ImageFormat.Jpeg);                    
                }


                db.Visitantes.Add(visitante);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(visitante);
        }
                 
        // GET: Visitante/Edit/5
        [Authorize]
        public ActionResult Edit(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Visitante visitante = db.Visitantes.Find(id);
            if (visitante == null)
            {
                return HttpNotFound();
            }

            

            return View(visitante);
        }

        // POST: Visitante/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "VisitanteID,Nome,DataNascimento,CPF,Contato")] Visitante visitante, HttpPostedFileBase image)
        {
            
            if (ModelState.IsValid)
            {
                if (image != null && image.ContentLength > 0)
                {
                    var img = Path.GetFileName(image.FileName);
                    var path = Path.Combine(Server.MapPath("~/IMG/"),
                                            System.IO.Path.GetFileName(image.FileName));
                    image.SaveAs(path);
                    //product.ImageName = img;

                }


                db.Entry(visitante).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(visitante);
        }

        // GET: Visitante/Delete/5
        [Authorize]
        public ActionResult Delete(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Visitante visitante = db.Visitantes.Find(id);
            if (visitante == null)
            {
                return HttpNotFound();
            }
            return View(visitante);
        }

        // POST: Visitante/Delete/5
        [Authorize]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(long id)
        {
            Visitante visitante = db.Visitantes.Find(id);
            db.Visitantes.Remove(visitante);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
