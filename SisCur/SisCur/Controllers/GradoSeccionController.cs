using SisCur.Filters;
using SisCur.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;

namespace SisCur.Controllers
{
    [Seguridad]
    public class GradoSeccionController : Controller
    {
        AppDataConext db;

        public GradoSeccionController()
        {
            db = new AppDataConext();
        }

        // GET: GradoSeccion
        public ActionResult Index()
        {
            int idUser = (int)Session["idUser"];


            string FullName = "";
            Usuario oUser = db.Usuario.Where(p => p.IIDUSUARIO == idUser).FirstOrDefault();

            if (oUser.TIPOUSUARIO == "D")
            {
                Docente oDocente = db.Docente.Where(p => p.IIDDOCENTE == oUser.IID).First();
                FullName = oDocente.NOMBRE + " " + oDocente.APPATERNO + " " + oDocente.APMATERNO;
                ViewBag.FullName = FullName;
            }
            else
            {
                Alumno oAlumno = db.Alumno.Where(p => p.IIDALUMNO == oUser.IID).First();
                FullName = oAlumno.NOMBRE + " " + oAlumno.APPATERNO + " " + oAlumno.APMATERNO;
                ViewBag.FullName = FullName;
            }
            return View();
        }


        public JsonResult ListarGradoSeccion()
        {
            var list = (from gs in db.GradoSeccion
                        join sec in db.Seccion
                        on gs.IIDSECCION equals sec.IIDSECCION
                        join g in db.Grado
                        on gs.IIDGRADO equals g.IIDGRADO
                        where gs.BHABILITADO == 1
                        select new
                        {
                            gs.IID,
                            NombreGrado = g.NOMBRE,
                            NombreSeccion = sec.NOMBRE

                        }).ToList();

            return Json(list, JsonRequestBehavior.AllowGet);
        }



        public JsonResult recuperarDatos(int id)
        {
            var consulta = db.GradoSeccion.Where(p => p.IID.Equals(id))
                .Select(g => new
                {
                    g.IID,
                    g.IIDSECCION,
                    g.IIDGRADO
                });

            return Json(consulta, JsonRequestBehavior.AllowGet);
        }


        public JsonResult listarGrado()
        {
            var list = (from g in db.Grado
                        where g.BHABILITADO == 1
                        select new
                        {
                            IID = g.IIDGRADO,
                            NOMBRE = g.NOMBRE
                        });
            return Json(list, JsonRequestBehavior.AllowGet);
        }


        public JsonResult listarSeccion()
        {
            var list = (from s in db.Seccion
                        where s.BHABILITADO == 1
                        select new
                        {
                            IID = s.IIDSECCION,
                            NOMBRE = s.NOMBRE
                        });
            return Json(list, JsonRequestBehavior.AllowGet);

        }


        public int guardar(GradoSeccion gs)
        {
            int regAf = 0;
            try
            {
                int id = gs.IID;
                if (id == 0)
                {
                    //Agregar
                    int nVeces = (db.GradoSeccion.Where(p => p.IIDGRADO == gs.IIDGRADO && p.IIDSECCION == gs.IIDSECCION)).Count();
                    if (nVeces == 0)
                    {
                        db.GradoSeccion.Add(gs);
                        db.SaveChanges();
                        regAf = 1;
                    }
                    else
                    {
                        regAf = -1;
                    }
                }
                else
                {
                    //Nuevo
                    int nVeces = (db.GradoSeccion.Where(p => p.IIDGRADO == gs.IIDGRADO && p.IIDSECCION == gs.IIDSECCION && p.IID != id)).Count();
                    if (nVeces == 0)
                    {
                        GradoSeccion o = db.GradoSeccion.Where(p => p.IID.Equals(id)).First();
                        o.IIDGRADO = gs.IIDGRADO;
                        o.IIDSECCION = gs.IIDSECCION;
                        db.SaveChanges();
                        regAf = 1;
                    }
                    else
                    {
                        regAf = -1;
                    }

                }
            }
            catch (Exception ex)
            {
                regAf = 0;
            }

            return regAf;
        }



        public int eliminar(int id)
        {
            int regAf = 0;
            try
            {
                GradoSeccion o = db.GradoSeccion.Where(p => p.IID.Equals(id)).First();
                o.BHABILITADO = 0;
                db.SaveChanges();
                regAf = 1;
            }
            catch (Exception)
            {
                regAf = 0;
            }
            
            return regAf;
        }
    }
}