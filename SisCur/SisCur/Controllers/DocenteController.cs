using SisCur.Filters;
using SisCur.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using System.Web.Profile;
using System.Web.UI;

namespace SisCur.Controllers
{
    [Seguridad]
    public class DocenteController : Controller
    {
        AppDataConext db;

        public DocenteController()
        {
            db = new AppDataConext();
        }

        // GET: Docente
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

        public JsonResult listar()
        {
            var list = (from d in db.Docente
                        where d.BHABILITADO == 1
                        select new
                        {
                            d.IIDDOCENTE,
                            d.NOMBRE,
                            d.APPATERNO,
                            d.APMATERNO,
                            d.EMAIL
                            
                        }).ToList();

            return Json(list, JsonRequestBehavior.AllowGet);
        }

        public JsonResult listarPorModalidadContrato()
        {
            var list = (from m in db.ModalidadContrato
                        where m.BHABILITADO == 1
                        select new
                        {
                            IID = m.IIDMODALIDADCONTRATO,
                            m.NOMBRE
                        }).ToList();

            return Json(list, JsonRequestBehavior.AllowGet);
        }

        public JsonResult filtrarPorModalidad(int idModalidad)
        {
            var list = (from d in db.Docente
                        where d.BHABILITADO == 1 && d.IIDMODALIDADCONTRATO == idModalidad
                        select new
                        {
                            d.IIDDOCENTE,
                            d.NOMBRE,
                            d.APPATERNO,
                            d.APMATERNO,
                            d.EMAIL
                        }).ToList();

            return Json(list, JsonRequestBehavior.AllowGet);
        }

        public int eliminar(int id)
        {
            int regAfectados = 0;

            try
            {
                Docente o = db.Docente.Where(p => p.IIDDOCENTE.Equals(id)).First();
                o.BHABILITADO = 0;
                db.SaveChanges();
                regAfectados = 1;
            }
            catch (Exception ex)
            {
                regAfectados = 0;
            }
            return regAfectados;
        }

        public JsonResult recuperarDatos(int id)
        {

            var list = (from d in db.Docente
                        where d.BHABILITADO == 1 && d.IIDDOCENTE == id
                        select new
                        {
                            d.IIDDOCENTE,
                            d.NOMBRE,
                            d.APPATERNO,
                            d.APMATERNO,
                            d.DIRECCION,
                            d.TELEFONOCELULAR,
                            d.TELEFONOFIJO,
                            d.EMAIL,
                            d.IIDSEXO,
                            fc = d.FECHACONTRATO.ToString(),
                            d.IIDMODALIDADCONTRATO,

                        }).ToList();

            return Json(list, JsonRequestBehavior.AllowGet);
        }


        public int guardar(Docente o, HttpPostedFileBase fileFoto)
        {
            int regAfectados = 0;

            try
            { 
                int idDocente = o.IIDDOCENTE;

                if (idDocente == 0)
                {
                    //AGREGAR
                    int nVeces = (db.Docente.Where(p => p.NOMBRE == o.NOMBRE && p.APPATERNO == o.APPATERNO && p.APMATERNO == o.APMATERNO)).Count();
                    if (nVeces == 0)
                    {
                        //if (fileFoto != null)
                        //{
                        //    byte[] fileData = null;
                        //    using (var binaryRender = new BinaryReader(fileFoto.InputStream))
                        //    {
                        //        fileData = binaryRender.ReadBytes(fileFoto.ContentLength);
                        //    }
                        //    o.FOTO = fileData;
                        //}
                        o.IIDTIPOUSUARIO = "D";
                        o.bTieneUsuario = 0;
                        db.Docente.Add(o);
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
                    int nVeces = (db.Docente.Where(p => p.NOMBRE == o.NOMBRE && p.APPATERNO == o.APPATERNO
                                    && p.APMATERNO == o.APMATERNO && p.IIDDOCENTE != idDocente)).Count();

                    //EDITAR
                    if (nVeces == 0)
                    {
                        Docente obj = db.Docente.Where(d => d.IIDDOCENTE.Equals(idDocente)).First();
                        obj.IIDDOCENTE = o.IIDDOCENTE;
                        obj.NOMBRE = o.NOMBRE;
                        obj.APPATERNO = o.APPATERNO;
                        obj.APMATERNO = o.APMATERNO;
                        obj.DIRECCION = o.DIRECCION;
                        obj.TELEFONOCELULAR = o.TELEFONOCELULAR;
                        obj.TELEFONOFIJO = o.TELEFONOFIJO;
                        obj.EMAIL = o.EMAIL;
                        obj.IIDSEXO = o.IIDSEXO;
                        obj.FECHACONTRATO = o.FECHACONTRATO;
                        obj.IIDMODALIDADCONTRATO = o.IIDMODALIDADCONTRATO;
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