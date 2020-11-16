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
    public class PeriodoController : Controller
    {
        AppDataConext db;

        public PeriodoController()
        {
            db = new AppDataConext();
        }
        // GET: Periodo
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

        public int Guardar(Periodo periodo)
        {
            int registrosAfectados = 0;

            try
            {
                if (periodo.IIDPERIODO == 0)
                {
                    int nVeces = (db.Periodo.Where(p => p.NOMBRE == periodo.NOMBRE)).Count();
                    if (nVeces == 0)
                    {
                        db.Periodo.Add(periodo);
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
                    int nVeces = (db.Periodo.Where(p => p.NOMBRE == periodo.NOMBRE && !p.IIDPERIODO.Equals(periodo.IIDPERIODO))).Count();
                    if (nVeces == 0)
                    {
                        Periodo newPeriodo = (from a in db.Periodo
                                              where a.IIDPERIODO == periodo.IIDPERIODO
                                              select a).FirstOrDefault();
                        newPeriodo.NOMBRE = periodo.NOMBRE;
                        newPeriodo.FECHAINICIO = periodo.FECHAINICIO;
                        newPeriodo.FECHAFIN = periodo.FECHAFIN;


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

        public int eliminar(Periodo periodo)
        {
            int registrosAfectados = 0;

            try
            {
                Periodo cursoNew = (from a in db.Periodo
                                  where a.IIDPERIODO == periodo.IIDPERIODO
                                  select a).FirstOrDefault();

                cursoNew.BHABILITADO = 0;
                db.SaveChanges();
                registrosAfectados = 1;
            }
            catch (Exception ex)
            {
                registrosAfectados = 0;
            }

            return registrosAfectados;
        }

        public JsonResult listar()
        {
            using (db)
            {
                var list = (from p in db.Periodo
                            where p.BHABILITADO == 1
                            select new
                            {
                                p.IIDPERIODO,
                                p.NOMBRE,
                                FECHAINICIO = p.FECHAINICIO.ToString(),
                                FECHAFIN = p.FECHAFIN.ToString()
                            }).ToList();

                //var list = db.Periodo.Select(p => new
                //{
                //    p.IIDPERIODO,
                //    p.NOMBRE,
                //    FECHAINICIO = ((DateTime)p.FECHAINICIO).ToShortTimeString(),
                //    p.BHABILITADO
                //}).Where(o => o.BHABILITADO == 1).ToList();

                return Json(list, JsonRequestBehavior.AllowGet);
            }
        }


        public JsonResult buscarPorNombre(string nombre)
        {

                var list = (from p in db.Periodo
                            where p.BHABILITADO == 1 && p.NOMBRE.Contains(nombre)
                            select new
                            {
                                p.IIDPERIODO,
                                p.NOMBRE,
                                FECHAINICIO = p.FECHAINICIO.ToString(),
                                FECHAFIN = p.FECHAFIN.ToString()
                            }).ToList();

                return Json(list, JsonRequestBehavior.AllowGet);


        }


        public JsonResult recuperarDatos(int id)
        {
            var list = (from c in db.Periodo
                        where c.BHABILITADO == 1 && c.IIDPERIODO == id
                        select new
                        {
                            c.IIDPERIODO,
                            c.NOMBRE,
                            fi = c.FECHAINICIO.ToString(),
                            ff = c.FECHAFIN.ToString()
                        }).ToList();

            return Json(list, JsonRequestBehavior.AllowGet);
        }
    }
}
