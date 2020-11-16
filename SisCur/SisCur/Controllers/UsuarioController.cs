using SisCur.Filters;
using SisCur.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Transactions;
using System.Web;
using System.Web.Mvc;

namespace SisCur.Controllers
{
    [Seguridad]
    public class UsuarioController : Controller
    {
        AppDataConext db;

        public UsuarioController()
        {
            db = new AppDataConext();
        }

        // GET: Usuario
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

        public JsonResult listarUsuarios()
        {
            List<UsuarioModel> listUser = new List<UsuarioModel>();

            List<UsuarioModel> listAlumno = (from u in db.Usuario
                                             join a in db.Alumno on u.IID equals a.IIDALUMNO
                                             join r in db.Rol on u.IIDROL equals r.IIDROL
                                             where u.BHABILITADO == 1 && u.TIPOUSUARIO == "A"
                                             select new UsuarioModel
                                             {
                                                 IDUsuario = u.IIDUSUARIO,
                                                 NombreUsuario = u.NOMBREUSUARIO,
                                                 NombrePersona = a.NOMBRE + " " + a.APPATERNO + " " + a.APMATERNO,
                                                 NombreRol = r.NOMBRE,
                                                 NombreTipoUsuario = "ALUMNO"
                                             }).ToList();
            listUser.AddRange(listAlumno);

            List<UsuarioModel> listDocente = (from u in db.Usuario
                                             join d in db.Docente on u.IID equals d.IIDDOCENTE
                                             join r in db.Rol on u.IIDROL equals r.IIDROL
                                             where u.BHABILITADO == 1 && u.TIPOUSUARIO == "D"
                                             select new UsuarioModel
                                             {
                                                 IDUsuario = u.IIDUSUARIO,
                                                 NombreUsuario = u.NOMBREUSUARIO,
                                                 NombrePersona = d.NOMBRE + " " + d.APPATERNO + " " + d.APMATERNO,
                                                 NombreRol = r.NOMBRE,
                                                 NombreTipoUsuario = "DOCENTE"
                                             }).ToList();
            listUser.AddRange(listDocente);
            listUser = listUser.OrderBy(p => p.IDUsuario).ToList();

            return Json(listUser, JsonRequestBehavior.AllowGet);
        }

        public JsonResult listarRoles()
        {
            var list = (from r in db.Rol
                        where r.BHABILITADO == 1
                        select new
                        {
                            IID = r.IIDROL,
                            r.NOMBRE  
                        }).ToList();

            return Json(list, JsonRequestBehavior.AllowGet);
        }

        public JsonResult listaPersonas()
        {
            //Listar Alumnos
            List<Persona> persona = new List<Persona>();

            List<Persona> listAlumno = (from a in db.Alumno
                        where a.bTieneUsuario == 0
                        select new Persona
                        {
                            IID = a.IIDALUMNO,
                            NOMBRE = a.NOMBRE + " " + a.APPATERNO + " " + a.APMATERNO + " (A)"
                        }).ToList();

            persona.AddRange(listAlumno);


            //Listar Docentes
            var listDocente = (from a in db.Docente
                        where a.bTieneUsuario == 0
                        select new Persona
                        {
                            IID = a.IIDDOCENTE,
                            NOMBRE = a.NOMBRE + " " + a.APPATERNO + " " + a.APMATERNO + " (D)"
                        }).ToList();

            persona.AddRange(listDocente);
            persona = persona.OrderBy(p => p.NOMBRE).ToList();

            return Json(persona, JsonRequestBehavior.AllowGet);

        }


        public int guardar(Usuario oUsuario, string nombreCompleto)
        {
            int rpta = 0;

            try
            {
                int id = oUsuario.IIDUSUARIO;

                using (var transaccion = new TransactionScope())
                {
                    if (id == 0)
                    {
                        //string clave = oUsuario.CONTRA;
                        //SHA256Managed sha = new SHA256Managed();
                        ////Data no cifrada
                        //byte[] dataNoCifrada = Encoding.Default.GetBytes(clave);
                        ////Data cifrada
                        //byte[] dataCifrada = sha.ComputeHash(dataNoCifrada);
                        ////Contraseña
                        //oUsuario.CONTRA = BitConverter.ToString(dataCifrada).Replace("-", " ");

                        char tipo = char.Parse(nombreCompleto.Substring(nombreCompleto.Length - 2, 1));
                        oUsuario.TIPOUSUARIO = tipo.ToString();
                

                        db.Usuario.Add(oUsuario);

                        if (tipo == 'A')
                        {
                            Alumno oAlumno = new Alumno();
                            oAlumno = (db.Alumno.Where(p => p.IIDALUMNO == oUsuario.IID)).First();
                            oAlumno.bTieneUsuario = 1;
                        }
                        else
                        {
                            Docente oDocente = db.Docente.Where(p => p.IIDDOCENTE == oUsuario.IID).First();
                            oDocente.bTieneUsuario = 1;
                        }
                        db.SaveChanges();
                        transaccion.Complete();
                        rpta = 1;
                    }
                    else
                    {
                        Usuario model = (db.Usuario.Where(p => p.IIDUSUARIO == id)).FirstOrDefault();
                        model.IIDROL = oUsuario.IIDROL;
                        model.NOMBREUSUARIO = oUsuario.NOMBREUSUARIO;
                        db.SaveChanges();
                        transaccion.Complete();
                        rpta = 1;
                    }

                }
            }
            catch (Exception ex)
            {
                rpta = 0;
            }

            return rpta;
        }

        public JsonResult recuperarInfo(int id)
        {
            var list = (from u in db.Usuario
                        where u.IIDUSUARIO == id
                        select new
                        {
                            u.IIDUSUARIO,
                            u.NOMBREUSUARIO,
                            u.IIDROL
                        }).First();

            return Json(list, JsonRequestBehavior.AllowGet);
        }
    }
}