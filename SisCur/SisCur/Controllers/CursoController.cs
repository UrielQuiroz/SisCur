using SisCur.Filters;
using SisCur.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using System.Web.UI;

namespace SisCur.Controllers
{
    [Seguridad]
    public class CursoController : Controller
    {
        AppDataConext db = new AppDataConext();

        public CursoController()
        {
            db = new AppDataConext();
        }
        // GET: Curso
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


        public string mensaje()
        {
            return "Uriel Quiroz";
        }

        public int Guardar(Curso curso)
        {
            int registrosAfectados = 0;

            try
            {
                if (curso.IIDCURSO == 0)
                {
                    int nVeces = (db.Curso.Where(p => p.NOMBRE == curso.NOMBRE)).Count();
                    if (nVeces == 0)
                    {
                        db.Curso.Add(curso);
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
                    //int nVeces = (db.Curso.Where(p => p.NOMBRE == curso.NOMBRE && !p.IIDCURSO.Equals(curso.IIDCURSO))).Count();
                    if ((db.Curso.Where(p => p.NOMBRE == curso.NOMBRE && !p.IIDCURSO.Equals(curso.IIDCURSO))).Count() == 0)
                    {
                        Curso cursoNew = (from a in db.Curso
                                          where a.IIDCURSO == curso.IIDCURSO
                                          select a).FirstOrDefault();

                        cursoNew.NOMBRE = curso.NOMBRE;
                        cursoNew.DESCRIPCION = curso.DESCRIPCION;

                        db.SaveChanges();
                        registrosAfectados = 1;
                    }
                    else
                    {
                        registrosAfectados = -1;
                    }

                    //var cursoNew = (from a in db.Curso
                    //                  where a.IIDCURSO == curso.IIDCURSO
                    //                  select a).FirstOrDefault();

                    //cursoNew.NOMBRE = curso.NOMBRE;
                    //cursoNew.DESCRIPCION = curso.DESCRIPCION;

                    //db.SaveChanges();
                    //registrosAfectados = 1;


                    //Curso cursoNew = db.Curso.Where(c => c.IIDCURSO == curso.IIDCURSO).First();
                    //cursoNew.NOMBRE = curso.NOMBRE;
                    //cursoNew.DESCRIPCION = curso.DESCRIPCION;
                    //db.SaveChanges();
                    //registrosAfectados = 1;

                }
            }
            catch (Exception ex)
            {
                registrosAfectados = 0;
            }

            return registrosAfectados;

        }

        public int eliminar(Curso curso)
        {
            int registrosAfectados = 0;

            try
            {
                Curso cursoNew = (from a in db.Curso
                                  where a.IIDCURSO == curso.IIDCURSO
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
                var list = (from c in db.Curso
                            where c.BHABILITADO == 1
                            select new
                            {
                                c.IIDCURSO,
                                c.NOMBRE,
                                c.DESCRIPCION
                            }).ToList();


                return Json(list, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult buscarPorNombre(string nombre)
        {
            using (db)
            {
                var list = (from c in db.Curso
                            where c.BHABILITADO == 1 && c.NOMBRE.Contains(nombre)
                            select new
                            {
                                c.IIDCURSO,
                                c.NOMBRE,
                                c.DESCRIPCION
                            }).ToList();

                return Json(list, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult recuperarDatos(int id)
        {
            var list = (from c in db.Curso
                        where c.BHABILITADO == 1 && c.IIDCURSO == id
                        select new {
                            c.IIDCURSO,
                            c.NOMBRE,
                            c.DESCRIPCION
                        }).ToList();

            return Json(list, JsonRequestBehavior.AllowGet);
        }
    }
}
