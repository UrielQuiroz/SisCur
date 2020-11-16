using SisCur.Filters;
using SisCur.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;
using System.Web;
using System.Web.Mvc;

namespace SisCur.Controllers
{
    [Seguridad]
    public class RolPaginaController : Controller
    {
        AppDataConext db;

        public RolPaginaController()
        {
            db = new AppDataConext();
        }
        // GET: RolPagina
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

        public JsonResult listarRoles()
        {

            //Listamos los roles
            var list = (from r in db.Rol
                        where r.BHABILITADO == 1
                        select new
                        {
                            r.IIDROL,
                            r.NOMBRE,
                            r.DESCRIPCION
                        }).ToList();

            return Json(list, JsonRequestBehavior.AllowGet);
        }


        public JsonResult listarRolPagina(int idRol)
        {
            var list = (from rp in db.RolPagina
                        where rp.IIDROL == idRol && rp.BHABILITADO == 1
                        select new
                        {
                            rp.IIDPAGINA,
                            rp.IIDROL,
                            rp.BHABILITADO

                        }).ToList();

            return Json(list, JsonRequestBehavior.AllowGet);
        }


        public JsonResult listarPagina()
        {
            var list = (from p in db.Pagina
                        where p.BHABILITADO == 1
                        select new
                        {
                            p.IIDPAGINA,
                            p.MENSAJE,
                            p.BHABILITADO
                        }).ToList();

            return Json(list, JsonRequestBehavior.AllowGet);
        }

        public JsonResult obtenerRol(int id)
        {
            var list = (from r in db.Rol
                        where r.IIDROL == id
                        select new
                        {
                            r.IIDROL,
                            r.NOMBRE,
                            r.DESCRIPCION
                        }).FirstOrDefault();

            return Json(list, JsonRequestBehavior.AllowGet);
        }

        public int guardar(Rol oRol, string CheckEnviado)
        {
            int regAf = 0;

            try
            {

                using (var transaccion = new TransactionScope())
                {
                    var id = oRol.IIDROL;

                    if (id == 0)
                    {
                        //Agregar
                        Rol o = new Rol();
                        o.NOMBRE = oRol.NOMBRE;
                        o.DESCRIPCION = oRol.DESCRIPCION;
                        o.BHABILITADO = oRol.BHABILITADO;
                        db.Rol.Add(o);
                        db.SaveChanges();

                        string[] checkboxEnabled = CheckEnviado.Split('$');
                        for (int i = 0; i < checkboxEnabled.Length; i++)
                        {
                            RolPagina oRolPag = new RolPagina();
                            oRolPag.IIDROL = o.IIDROL;
                            oRolPag.IIDPAGINA = int.Parse(checkboxEnabled[i]);
                            oRolPag.BHABILITADO = 1;
                            db.RolPagina.Add(oRolPag);
                        }

                        regAf = 1;
                        db.SaveChanges();
                        transaccion.Complete();
                    }
                    else
                    {
                        //Editar
                        Rol o = (db.Rol.Where(p => p.IIDROL == id)).FirstOrDefault();
                        o.NOMBRE = oRol.NOMBRE;
                        o.DESCRIPCION = oRol.DESCRIPCION;

                        //Deshabililitamos todos loc checkbox
                        var list = (db.RolPagina.Where(p => p.IIDROL == id));


                        foreach (RolPagina rp in list)
                        {
                            rp.BHABILITADO = 0;
                        }

                        //Habilitamos
                        string[] checkboxEnabled = CheckEnviado.Split('$');
                        for (int i = 0; i < checkboxEnabled.Length; i++)
                        {

                            var idRolPagina = Convert.ToInt32(checkboxEnabled[i]);

                            //int cantidad = list.Where(p => p.IIDROL == id && p.IIDPAGINA == idRol).Count();

                            var Edit  = list.Where(p => p.IIDROL == id && p.IIDPAGINA == idRolPagina).FirstOrDefault();

                            //int cantidad = (from p in db.RolPagina
                            //                where p.IIDROL == id && p.IIDPAGINA == int.Parse(checkboxEnabled[i])
                            //                select p).Count();


                            // Si existe ya el RolPagina entonces actualizamos el BHABILITADO
                            if (Edit != null && Edit.IIDROL > 0)
                            {
                                //RolPagina oRolPag = new RolPagina();
                                //oRolPag.IIDROL = o.IIDROL;
                                //oRolPag.IIDPAGINA = int.Parse(checkboxEnabled[i]);
                                //oRolPag.BHABILITADO = 1;
                                //db.RolPagina.Add(oRolPag);

                                Edit.BHABILITADO = 1;                      
                                db.RolPagina.Add(Edit);
                                db.Entry(Edit).State = System.Data.Entity.EntityState.Modified;
                                db.SaveChanges();
                            }
                            else
                            {

                                //Si no existe entonces Agregamos rl RolPagina
                                RolPagina oRolPag = new RolPagina();
                                oRolPag.IIDROL = o.IIDROL;
                                oRolPag.IIDPAGINA = idRolPagina;
                                oRolPag.BHABILITADO = 1;
                                db.RolPagina.Add(oRolPag);
                                db.SaveChanges();
                            }


                            //else
                            //{
                            //    RolPagina oRolPag = (db.RolPagina.Where(p => p.IIDROL == id && p.IIDPAGINA == idRol )).FirstOrDefault();
                            //    oRol.BHABILITADO = 1;
                            //}
                        }

                        regAf = 1;
                        //db.SaveChanges();
                        transaccion.Complete();

                    }
                }
            }
            catch (Exception ex)
            {
                regAf = 0;
            }

            return regAf;
        }
    }
}