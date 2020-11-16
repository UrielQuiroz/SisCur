using SisCur.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SisCur.Filters
{
    public class Seguridad : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var user = HttpContext.Current.Session["idUser"];

            //List<string> controllers = VariableModel.controladores.Select(p => p.ToUpper()).ToList();
            //string controllerName = filterContext.ActionDescriptor.ControllerDescriptor.ControllerName;

            if (user == null /*|| !controllers.Contains(controllerName.ToUpper())*/)
            {
                filterContext.Result = new RedirectResult("~/Login/Index");
            }

            base.OnActionExecuting(filterContext);
        }
    }
}