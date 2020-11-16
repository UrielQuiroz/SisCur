using SisCur.Filters;
using SisCur.Models;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Security.Cryptography;
using System.Transactions;
using System.Web;
using System.Web.DynamicData;
using System.Web.Mvc;

namespace SisCur.Controllers
{
    [Seguridad]
    public class MatriculaController : Controller
    {
        AppDataConext db;


        public MatriculaController()
        {
            db = new AppDataConext();
        }


        // GET: Matricula
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
            var list = (from m in db.Matricula
                        join p in db.Periodo on m.IIDPERIODO equals p.IIDPERIODO
                        join g in db.Grado on m.IIDGRADO equals g.IIDGRADO
                        join s in db.Seccion on m.IIDSECCION equals s.IIDSECCION
                        join a in db.Alumno on m.IIDALUMNO equals a.IIDALUMNO
                        where m.BHABILITADO == 1
                        select new
                        {
                            m.IIDMATRICULA,
                            PERIODO = p.NOMBRE,
                            GRADO = g.NOMBRE,
                            SECCION = s.NOMBRE,
                            ALUMNO = a.NOMBRE + " " + a.APPATERNO + " " + a.APMATERNO
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
                            p.NOMBRE

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

        public JsonResult listarAlumnos()
        {
            var list = (from a in db.Alumno
                        where a.BHABILITADO == 1
                        select new
                        {
                            IID = a.IIDALUMNO,
                            NOMBRE = a.NOMBRE + " " + a.APPATERNO + " " + a.APMATERNO
                        });
            return Json(list, JsonRequestBehavior.AllowGet);
        }


        public int guardar(Matricula matricula, int IIDGRADOSECCION, string valorEnviar, string valorDeshabilitar)
        {
            int regAfec = 0;
            GradoSeccion gs = (db.GradoSeccion.Where(p => p.IID == IIDGRADOSECCION)).FirstOrDefault();

            int idGrado = Convert.ToInt32(gs.IIDGRADO);
            int idSeccion = Convert.ToInt32(gs.IIDSECCION);

            matricula.IIDGRADO = idGrado;
            matricula.IIDSECCION = idSeccion;
            matricula.FECHA = DateTime.Now;

            try
            {
                using (var transaccion = new TransactionScope())
                {
                    //Agregar
                    if (matricula.IIDMATRICULA == 0)
                    {
                        int cantidad = db.Matricula.Where(p => p.IIDALUMNO == matricula.IIDALUMNO && p.IIDPERIODO == matricula.IIDPERIODO).Count();

                        if (cantidad >= 1)
                        {
                            return -1;
                        }

                        db.Matricula.Add(matricula);
                        db.SaveChanges();

                        int idMatriculaGenerada = matricula.IIDMATRICULA;
                        //var list = db.PeriodoGradoCurso.Where(p => p.IIDPERIODO == matricula.IIDPERIODO
                        //  && p.IIDGRADO == idGrado && p.BHABILITADO == 1).Select(p => p.IIDCURSO);
                        //var list = (from p in db.PeriodoGradoCurso
                        //            where p.IIDPERIODO == matricula.IIDPERIODO
                        //            && p.IIDGRADO == idGrado && p.BHABILITADO == 1
                        //            select p.IIDCURSO);

                        if (valorEnviar != "" && valorEnviar != null)
                        {
                            string[] cursos = valorEnviar.Split('$');

                            foreach (string curso in cursos)
                            {
                                DetalleMatricula dm = new DetalleMatricula();
                                dm.IIDMATRICULA = idMatriculaGenerada;
                                dm.IIDCURSO = int.Parse(curso);
                                dm.NOTA1 = 0;
                                dm.NOTA2 = 0;
                                dm.NOTA3 = 0;
                                dm.NOTA4 = 0;
                                dm.PROMEDIO = 0;
                                dm.bhabilitado = 1;
                                db.DetalleMatricula.Add(dm);
                            }
                        }

                        if (valorDeshabilitar != "" && valorDeshabilitar != null)
                        {
                            string[] cursos = valorDeshabilitar.Split('$');

                            foreach (string curso in cursos)
                            {
                                DetalleMatricula dm = new DetalleMatricula();
                                dm.IIDMATRICULA = idMatriculaGenerada;
                                dm.IIDCURSO = int.Parse(curso);
                                dm.NOTA1 = 0;
                                dm.NOTA2 = 0;
                                dm.NOTA3 = 0;
                                dm.NOTA4 = 0;
                                dm.PROMEDIO = 0;
                                dm.bhabilitado = 0;
                                db.DetalleMatricula.Add(dm);
                            }
                        }

                        db.SaveChanges();
                        transaccion.Complete();
                        regAfec = 1;
                    }

                    //Editar
                    else
                    {
                        int cantidad = db.Matricula.Where(p => p.IIDALUMNO == matricula.IIDALUMNO && p.IIDPERIODO == matricula.IIDPERIODO && p.IIDMATRICULA != matricula.IIDMATRICULA).Count();

                        if (cantidad >= 1)
                        {
                            return -1;
                        }

                        Matricula m = (db.Matricula.Where(p => p.IIDMATRICULA == matricula.IIDMATRICULA)).First();
                        m.IIDPERIODO = matricula.IIDPERIODO;
                        m.IIDGRADO = idGrado;
                        m.IIDSECCION = idSeccion;
                        m.IIDALUMNO = matricula.IIDALUMNO;

                        //Detalle Matricula
                        var list = (db.DetalleMatricula.Where(p => p.IIDMATRICULA == matricula.IIDMATRICULA));

                        //Deshabilitamos Todo
                        foreach (DetalleMatricula dm in list)
                        {
                            dm.bhabilitado = 0;
                        }

                        string[] valores = valorEnviar.Split('$');

                        if (valorEnviar != "")
                        {
                            int nVeces = 0;

                            for (int i = 0; i < valores.Length; i++)
                            {
                                int values = int.Parse(valores[i]);

                                nVeces = (db.DetalleMatricula.Where(p => p.IIDMATRICULA == matricula.IIDMATRICULA && p.IIDCURSO == values)).Count();

                                //Si existe
                                if (nVeces == 1)
                                {
                                    DetalleMatricula Odm = (db.DetalleMatricula.Where(p => p.IIDMATRICULA == matricula.IIDMATRICULA && p.IIDCURSO == values)).FirstOrDefault();
                                    Odm.bhabilitado = 1;
                                }
                                //Si no existe
                                else
                                {
                                    DetalleMatricula dm = new DetalleMatricula();
                                    dm.IIDMATRICULA = matricula.IIDMATRICULA;
                                    dm.IIDCURSO = values;
                                    dm.NOTA1 = 0;
                                    dm.NOTA2 = 0;
                                    dm.NOTA3 = 0;
                                    dm.NOTA4 = 0;
                                    dm.PROMEDIO = 0;
                                    dm.bhabilitado = 1;
                                    db.DetalleMatricula.Add(dm);
                                }

                            }
                        }

                        db.SaveChanges();
                        transaccion.Complete();
                        regAfec = 1;
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
                using (var transaccion = new TransactionScope())
                {
                    Matricula m = (db.Matricula.Where(p => p.IIDMATRICULA.Equals(id))).First();
                    m.BHABILITADO = 0;

                    var ListaDM = (db.DetalleMatricula.Where(p => p.IIDMATRICULA.Equals(id)));

                    foreach (DetalleMatricula dm in ListaDM)
                    {
                        dm.bhabilitado = 0;
                    }
                    db.SaveChanges();
                    transaccion.Complete();
                    regAf = 1;
                }

            }
            catch (Exception ex)
            {
                regAf = 0;
            }

            return regAf;
        }


        public JsonResult recuperarDatos(int id)
        {
            Matricula matricula = (db.Matricula.Where(p => p.IIDMATRICULA == id)).FirstOrDefault();

            int idGrado = (int)matricula.IIDGRADO;
            int idSeccion = (int)matricula.IIDSECCION;

            //int IIDGRADOSECCION = db.GradoSeccion.Where(p => p.IIDGRADO == idGrado && p.IIDSECCION == idSeccion).FirstOrDefault().IID;
            int IIDGRADOSECCION = (from gd in db.GradoSeccion
                                   where gd.IIDGRADO == idGrado && gd.IIDSECCION == idSeccion
                                   select gd.IID).FirstOrDefault();

            var list = (from m in db.Matricula
                        where m.IIDMATRICULA == id
                        select new
                        {
                            m.IIDMATRICULA,
                            m.IIDPERIODO,
                            IIDSECCION = IIDGRADOSECCION,
                            m.IIDALUMNO
                        }).First();

            return Json(list, JsonRequestBehavior.AllowGet);
        }


        public JsonResult Cursos(int id)
        {

            int IdGrado = (int) (from m in db.Matricula
                           where m.IIDMATRICULA == id
                           select m.IIDGRADO).First();

            List<int?> lista = (from p in db.PeriodoGradoCurso
                                where p.IIDGRADO == IdGrado
                                select p.IIDCURSO).ToList();

            var list = (from dm in db.DetalleMatricula
                        join c in db.Curso on dm.IIDCURSO equals c.IIDCURSO
                        where dm.IIDMATRICULA == id && lista.Contains(dm.IIDCURSO)
                        select new
                        {
                            dm.IIDMATRICULA,
                            c.IIDCURSO,
                            c.NOMBRE,
                            dm.bhabilitado
                        }).ToList();   

            return Json(list, JsonRequestBehavior.AllowGet);
        }


        public JsonResult listarCursosPorPeriodoyGrado(int idPeriodo, int idGradoSeccion)
        {
            int idGrado = (int) (from gs in db.GradoSeccion
                                   where gs.IID == idGradoSeccion
                                   select gs.IIDGRADO).First();

            var list = (from pgc in db.PeriodoGradoCurso
                        join c in db.Curso on pgc.IIDCURSO equals c.IIDCURSO
                        where pgc.IIDPERIODO == idPeriodo && pgc.IIDGRADO == idGrado && pgc.BHABILITADO == 1
                        select new CursoModel
                        {
                            IIDCURSO = c.IIDCURSO,
                            NOMBRE = c.NOMBRE
                        }).ToList();

            return Json(list, JsonRequestBehavior.AllowGet);
        }
    }
}