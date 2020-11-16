using SisCur.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace SisCur.Controllers
{
    public class LoginController : Controller
    {
        AppDataConext db;

        public LoginController()
        {
            db = new AppDataConext();
        }
        // GET: Login
        public ActionResult Index()
        {
            return View();
        }

        public int ValidarUser(string User, string Password)
        {
            int rpta = 0;

            try
            {

                //SHA256Managed sha = new SHA256Managed();
                ////Data no cifrada
                //byte[] dataNoCifrada = Encoding.Default.GetBytes(Password);
                ////Data cifrada
                //byte[] dataCifrada = sha.ComputeHash(dataNoCifrada);
                ////Contraseña
                //string passCifrada = BitConverter.ToString(dataCifrada).Replace("-", " ");

                rpta = db.Usuario.Where(p => p.NOMBREUSUARIO == User && p.CONTRA == Password).Count();

                if (rpta == 1)
                {
                    Usuario oUsuario = db.Usuario.Where(p => p.NOMBREUSUARIO == User && p.CONTRA == Password).First();
                    int idUsuario = oUsuario.IIDUSUARIO;
                    Session["idUser"] = idUsuario;

                    var roles = (from u in db.Usuario
                                 join r in db.Rol on u.IIDROL equals r.IIDROL
                                 join rp in db.RolPagina on r.IIDROL equals rp.IIDROL
                                 join p in db.Pagina on rp.IIDPAGINA equals p.IIDPAGINA
                                 where u.BHABILITADO == 1 && rp.BHABILITADO == 1 && u.IIDUSUARIO == idUsuario
                                 select new
                                 {
                                     acciones = p.ACCION,
                                     controladores = p.CONTROLADOR,
                                     mensaje = p.MENSAJE
                                 });

                    //Inicializamos
                    VariableModel.accriones = new List<string>();
                    VariableModel.controladores = new List<string>();
                    VariableModel.mensjae = new List<string>();

                    //Llenamos
                    foreach (var item in roles)
                    {
                        VariableModel.accriones.Add(item.acciones);
                        VariableModel.controladores.Add(item.controladores);
                        VariableModel.mensjae.Add(item.mensaje);
                    }
                }

            }
            catch (Exception ex)
            {
                rpta = 0;
            }

            return rpta;
        }

        public ActionResult Cerrar()
        {
            return RedirectToAction("Index");
        }
    }
}