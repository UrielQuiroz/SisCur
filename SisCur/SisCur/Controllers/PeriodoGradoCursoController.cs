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
    public class PeriodoGradoCursoController : Controller
    {
        AppDataConext db;

        public PeriodoGradoCursoController()
        {
            db = new AppDataConext();
        }

        // GET: PeriodoGradoCurso
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


        public JsonResult ListarPGC()
        {
            var list = (from pgc in db.PeriodoGradoCurso
                        join p in db.Periodo on pgc.IIDPERIODO equals p.IIDPERIODO
                        join g in db.Grado on pgc.IIDGRADO equals g.IIDGRADO
                        join c in db.Curso on pgc.IIDCURSO equals c.IIDCURSO
                        where pgc.BHABILITADO == 1
                        select new
                        {
                            pgc.IID,
                            PERIODO = p.NOMBRE,
                            GRADO = g.NOMBRE,
                            CURSO = c.NOMBRE

                        }).ToList();

            return Json(list, JsonRequestBehavior.AllowGet);
        }


        public JsonResult ListarPeriodo()
        {
            var list = (from p in db.Periodo
                        where p.BHABILITADO == 1
                        select new
                        {
                            IID = p.IIDPERIODO,
                            NOMBRE = p.NOMBRE

                        });

            return Json(list, JsonRequestBehavior.AllowGet);
        }

        public JsonResult ListarGrado()
        {
            var list = (from p in db.Grado
                        where p.BHABILITADO == 1
                        select new
                        {
                            IID = p.IIDGRADO,
                            NOMBRE = p.NOMBRE

                        });

            return Json(list, JsonRequestBehavior.AllowGet);
        }

        public JsonResult ListarCurso()
        {
            var list = (from p in db.Curso
                        where p.BHABILITADO == 1
                        select new
                        {
                            IID = p.IIDCURSO,
                            NOMBRE = p.NOMBRE

                        });

            return Json(list, JsonRequestBehavior.AllowGet);
        }


        public JsonResult recuperarDatos(int id)
        {
            var list = (from pgc in db.PeriodoGradoCurso
                        where pgc.IID == id
                        select new
                        {
                            pgc.IID,
                            pgc.IIDPERIODO,
                            pgc.IIDGRADO,
                            pgc.IIDCURSO

                        });

            return Json(list, JsonRequestBehavior.AllowGet);
        }


        public int guardar(PeriodoGradoCurso pgc)
        {
            int regAfec = 0;

            try
            {
                int id = pgc.IID;

                if (id == 0)
                {
                    //Nuevi
                    int nVeces = (db.PeriodoGradoCurso.Where(p => p.IIDCURSO == pgc.IIDCURSO && p.IIDGRADO == pgc.IIDGRADO && p.IIDPERIODO == pgc.IIDPERIODO)).Count();
                    if (nVeces == 0)
                    {
                        db.PeriodoGradoCurso.Add(pgc);
                        db.SaveChanges();
                        regAfec = 1;
                    }
                    else
                    {
                        regAfec = -1;
                    }

                }
                else
                {
                    //Editar
                    int nVeces = (db.PeriodoGradoCurso.Where(p => p.IIDCURSO == pgc.IIDCURSO && p.IIDGRADO == pgc.IIDGRADO 
                                    && p.IIDPERIODO == pgc.IIDPERIODO && p.IID != id)).Count();

                    if (nVeces == 0)
                    {
                        PeriodoGradoCurso o = db.PeriodoGradoCurso.Where(p => p.IID.Equals(id)).First();
                        o.IIDCURSO = pgc.IIDCURSO;
                        o.IIDPERIODO = pgc.IIDPERIODO;
                        o.IIDGRADO = pgc.IIDGRADO;
                        db.SaveChanges();
                        regAfec = 1;
                    }
                    else
                    {
                        regAfec = -1;
                    }
                }
            }
            catch (Exception ex)
            {
                regAfec = 0;
            }

            return regAfec;
        }


        public int eliminar(int id)
        {
            int regAf = 0;
            try
            {
                PeriodoGradoCurso o = db.PeriodoGradoCurso.Where(p => p.IID.Equals(id)).First();
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