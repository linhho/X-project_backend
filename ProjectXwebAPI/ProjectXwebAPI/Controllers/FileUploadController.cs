using ProjectXwebAPI.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;

namespace ProjectXwebAPI.Controllers
{
    public class FileUploadController : ApiController
    {
        [HttpPost()]
        [ResponseType(typeof(Image))]
        public IHttpActionResult UploadFiles()
        {
            int iUploadedCnt = 0;

            // DEFINE THE PATH WHERE WE WANT TO SAVE THE FILES.
            string sPath = "";
            sPath = System.Web.Hosting.HostingEnvironment.MapPath("~/Content/images/");

            System.Web.HttpFileCollection hfc = System.Web.HttpContext.Current.Request.Files;
            System.Web.HttpPostedFile hpf = null;
            string filename = "";

            // CHECK THE FILE COUNT.
            for (int iCnt = 0; iCnt <= hfc.Count - 1; iCnt++)
            {
                hpf = hfc[iCnt];

                if (hpf.ContentLength > 0)
                {
                    // CHECK IF THE SELECTED FILE(S) ALREADY EXISTS IN FOLDER. (AVOID DUPLICATE)
                    if (!File.Exists(sPath + Path.GetFileName(hpf.FileName)))
                    {
                        // SAVE THE FILES IN THE FOLDER.
                        filename = sPath + DateTime.Now.ToString("yyyyMMddHHmmss") + Path.GetFileName(hpf.FileName);
                        hpf.SaveAs(filename);
                        iUploadedCnt = iUploadedCnt + 1;
                    }
                }
            }

            // RETURN A MESSAGE (OPTIONAL).
            if (iUploadedCnt > 0)
            {
                Image img = new Image();
                img.ImageUrl = filename.Remove(0, sPath.Length);
                return Ok(img);
            }
            else
            {
                return NotFound();
            }
        }
    }
}
