using SisCur.Filters;
using SisCur.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;

namespace SisCur.Controllers
{
    [Seguridad]
    public class AlumnoController : Controller
    {
        AppDataConext db;

        public AlumnoController()
        {
            db = new AppDataConext();
        }

        // GET: Alumno
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


        public JsonResult listarSexo()
        {
            using (db)
            {
                var list = (from s in db.Sexo
                            where s.BHABILITADO == 1
                            select new
                            {
                                IID = s.IIDSEXO,
                                s.NOMBRE
                            }).ToList();

                return Json(list, JsonRequestBehavior.AllowGet);
            }
        }


        public JsonResult filtrarPorSexo(int idSexo)
        {
            using (db)
            {
                var list = (from a in db.Alumno
                            where a.BHABILITADO == 1 && a.IIDSEXO == idSexo
                            select new
                            {
                                a.IIDALUMNO,
                                a.NOMBRE,
                                a.APPATERNO,
                                a.APMATERNO,
                                a.TELEFONOPADRE
                            }).ToList();

                return Json(list, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult listar()
        {
            using (db)
            {
                var list = (from a in db.Alumno
                            where a.BHABILITADO == 1
                            select new
                            {
                                a.IIDALUMNO,
                                a.NOMBRE,
                                a.APPATERNO,
                                a.APMATERNO,
                                a.TELEFONOPADRE
                            }).ToList();

                return Json(list, JsonRequestBehavior.AllowGet);
            }
        }

        public int Eliminar(int id)
        {
            int regAfectados = 0;
            try
            {
                Alumno alumno = db.Alumno.Where(p => p.IIDALUMNO.Equals(id)).First();
                alumno.BHABILITADO = 0;
                db.SaveChanges();
                regAfectados = 1;
            }
            catch (Exception ex)
            {
                regAfectados = 0;
            }
            return regAfectados;
        }

        public JsonResult recuperarInfo(int id)
        {
            var list = (from a in db.Alumno
                        where a.IIDALUMNO == id
                        select new
                        {
                            a.IIDALUMNO,
                            a.NOMBRE,
                            a.APPATERNO, 
                            a.APMATERNO,
                            fnac = a.FECHANACIMIENTO.ToString(), 
                            a.IIDSEXO,
                            a.NUMEROHERMANOS,
                            a.TELEFONOPADRE,
                            a.TELEFONOMADRE
                        }).ToList();

            return Json(list, JsonRequestBehavior.AllowGet);
        }

        public int Guardar(Alumno alumno)
        {
            int regAfectados = 0;
            try
            {
                int idAlumno = alumno.IIDALUMNO;

                if (idAlumno == 0)
                {
                    //Nuevo
                    int nVeces = (db.Alumno.Where(p => p.NOMBRE == alumno.NOMBRE && p.APPATERNO == alumno.APPATERNO && p.APMATERNO == alumno.APMATERNO)).Count();
                    if (nVeces == 0)
                    {
                        alumno.IIDTIPOUSUARIO = "A";
                        alumno.bTieneUsuario = 0;
                        db.Alumno.Add(alumno);
                        db.SaveChanges();
                        regAfectados = 1;
                    }
                    else
                    {
                        regAfectados = -1;
                    }


                }
                else
                {
                    //Editar
                    int nVeces = (db.Alumno.Where(p => p.NOMBRE == alumno.NOMBRE && p.APPATERNO == alumno.APPATERNO &&
                                    p.APMATERNO == alumno.APMATERNO && p.IIDALUMNO != alumno.IIDALUMNO)).Count();

                    //Se valida que no exista un alumno con el mismo nombre y el mismo ID
                    if (nVeces == 0)
                    {
                        Alumno alum = db.Alumno.Where(p => p.IIDALUMNO.Equals(idAlumno)).First();

                        alum.NOMBRE = alumno.NOMBRE;
                        alum.APPATERNO = alumno.APPATERNO;
                        alum.APMATERNO = alumno.APMATERNO;
                        alum.FECHANACIMIENTO = alumno.FECHANACIMIENTO;
                        alum.IIDSEXO = alumno.IIDSEXO;
                        alum.NUMEROHERMANOS = alumno.NUMEROHERMANOS;
                        alum.TELEFONOPADRE = alumno.TELEFONOPADRE;
                        alum.TELEFONOMADRE = alumno.TELEFONOMADRE;
                        db.SaveChanges();
                        regAfectados = 1;
                    }
                    else
                    {
                        regAfectados = -1;
                    }

                }
            }
            catch (Exception ex)
            {
                regAfectados = 0;
            }

            return regAfectados;
        }
    }
}