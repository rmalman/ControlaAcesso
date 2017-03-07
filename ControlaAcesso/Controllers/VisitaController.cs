using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ControlaAcesso.DAL;
using ControlaAcesso.Models;
using System.Data.Entity.Core.Objects;

namespace ControlaAcesso.Controllers
{
    public class VisitaController : Controller
    {
        private ControlaAcessoContext db = new ControlaAcessoContext();

        // GET: Visita
        [Authorize]
        public ActionResult Index(string sortOrder, string searchString, string searchString2, int CrachaAberto = 0, bool Relatorio = false)
        {
            DateTime DataIni, DataFim;
            var visitas = db.Visitas.Include(v => v.Visitante);
            if (Relatorio == true)
            {
                DataIni = DateTime.Today.AddMonths(-1);
                DataFim = DateTime.Today;
                visitas = visitas.OrderBy(e => e.Visitante.Nome).Where(s => s.RegistroEntrada >= DataIni && s.RegistroEntrada <= DataFim);
            }

            if (!String.IsNullOrEmpty(searchString))
            {
                
                Int64 CPF = 0;
                bool isNumeric = Int64.TryParse(searchString, out CPF);
                

                if (isNumeric && sortOrder == "CPF")
                    visitas = visitas.OrderBy(e => e.Visitante.Nome).Where(s => s.Visitante.CPF.Equals(CPF));
                else if(!String.IsNullOrEmpty(searchString) || !String.IsNullOrEmpty(searchString2))
                {
                    if (!String.IsNullOrEmpty(searchString) && !String.IsNullOrEmpty(searchString2))
                    {
                        
                        DataIni = DateTime.ParseExact(searchString, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                        DataFim = DateTime.ParseExact(searchString2, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                        if(sortOrder == "Saida")
                            visitas = visitas.OrderBy(e => e.Visitante.Nome).Where(s => s.RegistroSaida >= DataIni && s.RegistroSaida <= DataFim);
                        else
                            visitas = visitas.OrderBy(e => e.Visitante.Nome).Where(s => s.RegistroEntrada >= DataIni && s.RegistroEntrada <= DataFim);

                    }
                    else
                    {
                        if (sortOrder == "Saida")
                        {
                            if (!String.IsNullOrEmpty(searchString))
                            {
                                DataIni = DateTime.ParseExact(searchString, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                                visitas = visitas.OrderBy(e => e.Visitante.Nome).Where(s => s.RegistroSaida >= DataIni);
                            }
                            if (!String.IsNullOrEmpty(searchString2))
                            {
                                DataFim = DateTime.ParseExact(searchString2, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                                visitas = visitas.OrderBy(e => e.Visitante.Nome).Where(s => s.RegistroSaida <= DataFim);
                            }
                        }
                        else
                        {
                            if (!String.IsNullOrEmpty(searchString))
                            {
                                DataIni = DateTime.ParseExact(searchString, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                                visitas = visitas.OrderBy(e => e.Visitante.Nome).Where(s => s.RegistroEntrada >= DataIni);
                            }
                            if (!String.IsNullOrEmpty(searchString2))
                            {
                                DataFim = DateTime.ParseExact(searchString2, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                                visitas = visitas.OrderBy(e => e.Visitante.Nome).Where(s => s.RegistroEntrada <= DataFim);
                            }
                        }

                        
                    }
                    
                }
                else
                    visitas = visitas.OrderBy(e => e.Visitante.Nome).Where(s => s.CodigoCracha.Contains(searchString));

                ViewBag.Visitantes = new SelectList(visitas, "VisitanteID", "Nome");
                
            }
            
            if (CrachaAberto >= 1)
            {
                visitas = visitas.OrderBy(v => v.Visitante.Nome).Where(v => v.RegistroSaida.HasValue == false);
            }
            return View(visitas.ToList());
        }

        // GET: Visita/Details/5
        [Authorize]
        public ActionResult Details(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Visita visita = db.Visitas.Find(id);
            if (visita == null)
            {
                return HttpNotFound();
            }
            return View(visita);
        }

        // GET: Visita/Create
        [Authorize]
        public ActionResult Create(string VisitanteID)
        {
            Int64 id = 0;

            if (Int64.TryParse(VisitanteID, out id))
            {
                // you know that the parsing attempt
                // was successful
                Visitante visitante = db.Visitantes.Find(id);
                ViewBag.VisitanteID = visitante;
            }
            else 
            {
                ViewBag.VisitanteID = new SelectList(db.Visitantes, "VisitanteID", "Nome");
            }           
            
            return View();
        }

        // POST: Visita/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "VisitaID,RegistroEntrada,RegistroSaida,CodigoCracha,Empresa,VisitanteID,Assunto")] Visita visita)
        {
            if (ModelState.IsValid)
            {
                var Dado_Visitante = Request.Form["Dado_Visitante"];
                db.Visitas.Add(visita);
                db.SaveChanges();
                return RedirectToAction("Index", new { Relatorio = true });
            }

            ViewBag.VisitanteID = new SelectList(db.Visitantes, "VisitanteID", "Nome", visita.VisitanteID);
            return View(visita);
        }

        // GET: Visita/Edit/5
        [Authorize]
        public ActionResult Edit(long? id, int EfetuaBaixa = 0)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Visita visita = db.Visitas.Find(id);
            if (visita == null)
            {
                return HttpNotFound();
            }
            ViewBag.VisitanteID = new SelectList(db.Visitantes, "VisitanteID", "Nome", visita.VisitanteID);
            return View(visita);
        }

        // POST: Visita/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "VisitaID,RegistroEntrada,RegistroSaida,CodigoCracha,Empresa,VisitanteID,Assunto")] Visita visita)
        {
            if (ModelState.IsValid)
            {
                db.Entry(visita).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index", new { CrachaAberto = 1 });
            }
            ViewBag.VisitanteID = new SelectList(db.Visitantes, "VisitanteID", "Nome", visita.VisitanteID);
            return View(visita);
        }

        // GET: Visita/Delete/5
        [Authorize]
        public ActionResult Delete(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Visita visita = db.Visitas.Find(id);
            if (visita == null)
            {
                return HttpNotFound();
            }
            return View(visita);
        }

        // POST: Visita/Delete/5
        [Authorize]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(long id)
        {
            Visita visita = db.Visitas.Find(id);
            db.Visitas.Remove(visita);
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
