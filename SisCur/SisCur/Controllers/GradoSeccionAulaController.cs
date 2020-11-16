using SisCur.Filters;
using SisCur.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SisCur.Controllers
{
    [Seguridad]
    public class GradoSeccionAulaController : Controller
    {
        AppDataConext db;

        public GradoSeccionAulaController()
        {
            db = new AppDataConext();
        }

        // GET: GradoSeccionAula
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


        public JsonResult Listar()
        {
            var list = (from gsa in db.GradoSeccionAula
                        join p in db.Periodo on gsa.IIDPERIODO equals p.IIDPERIODO
                        join gs in db.GradoSeccion on gsa.IIDGRADOSECCION equals gs.IID
                        join a in db.Aula on gsa.IIDAULA equals a.IIDAULA
                        join d in db.Docente on gsa.IIDDOCENTE equals d.IIDDOCENTE
                        join c in db.Curso on gsa.IIDCURSO equals c.IIDCURSO
                        join g in db.Grado on gs.IIDGRADO equals g.IIDGRADO
                        where gsa.BHABILITADO == 1
                        select new
                        {
                            gsa.IID,
                            PERIODO = p.NOMBRE,
                            GRADO = g.NOMBRE, 
                            CURSO = c.NOMBRE,
                            DOCENTE = d.NOMBRE + " " + d.APPATERNO

                        });

            return Json(list, JsonRequestBehavior.AllowGet);
        }


        public JsonResult listarPeriodos()
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


        public JsonResult listarGradoSeccion()
        {
            var list = (from gs in db.GradoSeccion
                        join g in db.Grado on gs.IIDGRADO equals g.IIDGRADO
                        join s in db.Seccion on gs.IIDSECCION equals s.IIDSECCION
                        where gs.BHABILITADO == 1
                        select new
                        {
                            gs.IID,
                            NOMBRE = g.NOMBRE + " - " + s.NOMBRE
                        });

            return Json(list, JsonRequestBehavior.AllowGet);
        }

        public JsonResult listarAulas()
        {
            var list = (from a in db.Aula
                        where a.BHABILITADO == 1
                        select new
                        {
                            IID = a.IIDAULA,
                            NOMBRE = a.NOMBRE
                        });
            return Json(list, JsonRequestBehavior.AllowGet);
        }

        public JsonResult ListarDocentes()
        {
            var list = (from d in db.Docente
                        where d.BHABILITADO == 1
                        select new
                        {
                            IID = d.IIDDOCENTE,
                            NOMBRE = d.NOMBRE + " " + d.APPATERNO
                        });

            return Json(list, JsonRequestBehavior.AllowGet);
        }


        public JsonResult listarCursos(int IIDPERIODO, int IIDGRADOSECCION)
        {
            int iidgrado = (int)db.GradoSeccion.Where(p => p.IID.Equals(IIDGRADOSECCION)).First().IIDGRADO;
            var lista = from pgc in db.PeriodoGradoCurso
                        join curso in db.Curso
                        on pgc.IIDCURSO equals curso.IIDCURSO
                        join periodo in db.Periodo
                        on pgc.IIDPERIODO equals periodo.IIDPERIODO
                        where pgc.BHABILITADO == 1
                        && pgc.IIDPERIODO == IIDPERIODO
                        && pgc.IIDGRADO == iidgrado
                        select new
                        {
                            IID = pgc.IIDCURSO,
                            curso.NOMBRE
                        };

            return Json(lista, JsonRequestBehavior.AllowGet);

        }




        //public JsonResult listarCursos(int periodoID, int gradoSeccionID)
        //{
        //    int gradoID = (int) db.GradoSeccion.Where(p => p.IID.Equals(gradoSeccionID)).First().IIDGRADO;
        //    var list = (from pgc in db.PeriodoGradoCurso
        //                join c in db.Curso on pgc.IIDCURSO equals c.IIDCURSO
        //                join p in db.Periodo on pgc.IIDPERIODO equals p.IIDPERIODO
        //                where pgc.BHABILITADO == 1 && pgc.IIDPERIODO == periodoID && pgc.IIDCURSO == gradoID
        //                select new
        //                {
        //                    IID = pgc.IIDCURSO,
        //                    c.NOMBRE
        //                });

        //    return Json(list, JsonRequestBehavior.AllowGet);
        //}



        public JsonResult recuperarDatos(int id)
        {
            var list = (from gsa in db.GradoSeccionAula
                        where gsa.IID == id
                        select new
                        {
                            gsa.IID,
                            gsa.IIDPERIODO,
                            gsa.IIDGRADOSECCION,
                            gsa.IIDCURSO,
                            gsa.IIDAULA,
                            gsa.IIDDOCENTE

                        });

            return Json(list, JsonRequestBehavior.AllowGet);
        }



        public int guardar(GradoSeccionAula gsa)
        {
            int regAfec = 0;

            try
            {
                int id = gsa.IID;

                if (id == 0)
                {
                    //Agregar
                    int nVeces = (db.GradoSeccionAula.Where(p => p.IIDPERIODO == gsa.IIDPERIODO && p.IIDGRADOSECCION == gsa.IIDGRADOSECCION
                                    && p.IIDCURSO == gsa.IIDCURSO)).Count();

                    if (nVeces == 0)
                    {
                        db.GradoSeccionAula.Add(gsa);
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
                    int nVeces = (db.GradoSeccionAula.Where(p => p.IIDPERIODO == gsa.IIDPERIODO && p.IIDGRADOSECCION == gsa.IIDGRADOSECCION
                                    && p.IIDCURSO == gsa.IIDCURSO && p.IID != id)).Count();

                    if (nVeces == 0)
                    {
                        GradoSeccionAula o = db.GradoSeccionAula.Where(p => p.IID.Equals(id)).First();
                        o.IIDCURSO = gsa.IIDCURSO;
                        o.IIDPERIODO = gsa.IIDPERIODO;
                        o.IIDGRADOSECCION = gsa.IIDGRADOSECCION;
                        o.IIDCURSO = gsa.IIDCURSO;
                        o.IIDAULA = gsa.IIDAULA;
                        o.IIDDOCENTE = gsa.IIDDOCENTE;
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
                GradoSeccionAula o = db.GradoSeccionAula.Where(p => p.IID.Equals(id)).First();
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