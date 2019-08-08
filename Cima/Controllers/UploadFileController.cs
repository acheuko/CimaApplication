using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Collections.ObjectModel;
using Kendo.Mvc.UI;
using Kendo.Mvc.Extensions;

namespace Cima.Controllers
{
    public class UploadFileController : Controller
    {
        //
        // GET: /UploadFile/

        public ActionResult Index([DataSourceRequest]DataSourceRequest request)
        {
            return View("UploadFile");
        }

    }
}
