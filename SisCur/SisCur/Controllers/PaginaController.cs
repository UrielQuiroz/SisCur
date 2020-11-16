using SisCur.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;

namespace SisCur.Controllers
{
    public class PaginaController : Controller
    {
        AppDataConext db;

        public PaginaController()
        {
            db = new AppDataConext();
        }
        // GET: Pagina
        public ActionResult Index()
        {
            return View();
        }

        public JsonResult listarPaginas()
        {
            var list = (from p in db.Pagina
                        where p.BHABILITADO == 1
                        select new
                        {
                            p.IIDPAGINA,
                            p.MENSAJE,
                            p.ACCION,
                            p.CONTROLADOR
                        }).ToList();

            return Json(list, JsonRequestBehavior.AllowGet);
        }

        public JsonResult recuperarDatos(int id)
        {
            var list = (from p in db.Pagina
                        where p.BHABILITADO == 1 && p.IIDPAGINA == id
                        select new
                        {
                            p.IIDPAGINA,
                            p.MENSAJE,
                            p.ACCION,
                            p.CONTROLADOR
                        }).First();

            return Json(list, JsonRequestBehavior.AllowGet);
        }

        public int guardar(Pagina oPagina)
        {
            int registrosAfectados = 0;

            try
            {
                if (oPagina.IIDPAGINA == 0)
                {
                    int nVeces = (db.Pagina.Where(p => p.MENSAJE == oPagina.MENSAJE)).Count();
                    if (nVeces == 0)
                    {
                        db.Pagina.Add(oPagina);
                        db.SaveChanges();
                        registrosAfectados = 1;
                    }
                    else
                    {
                        registrosAfectados = -1;
                    }
                }
                else
                {
                    int nVeces = (db.Pagina.Where(p => p.MENSAJE == oPagina.MENSAJE && !p.IIDPAGINA.Equals(oPagina.IIDPAGINA))).Count();
                    if (nVeces == 0)
                    {
                        Pagina pag = (from a in db.Pagina
                                          where a.IIDPAGINA == oPagina.IIDPAGINA
                                          select a).FirstOrDefault();

                        pag.MENSAJE = oPagina.MENSAJE;
                        pag.ACCION = oPagina.ACCION;
                        pag.CONTROLADOR = oPagina.CONTROLADOR;

                        db.SaveChanges();
                        registrosAfectados = 1;
                    }
                    else
                    {
                        registrosAfectados = -1;
                    }

                }
            }
            catch (Exception ex)
            {
                registrosAfectados = 0;
            }

            return registrosAfectados;
        }
    }
}