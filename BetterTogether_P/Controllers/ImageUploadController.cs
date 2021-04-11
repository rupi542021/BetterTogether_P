﻿using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Hosting;
using System.Web.Http;

namespace BetterTogetherProj.Controllers
{
    public class ImageUploadController : ApiController
    {

        [HttpPost]
        public HttpResponseMessage Post()
        {
            List<string> imageLinks = new List<string>();
            var httpContext = HttpContext.Current;

            // Check for any uploaded file  
            if (httpContext.Request.Files.Count > 0)
            {
                //Loop through uploaded files  
                for (int i = 0; i < httpContext.Request.Files.Count; i++)
                {
                    HttpPostedFile httpPostedFile = httpContext.Request.Files[i];

                    // this is an example of how you can extract addional values from the Ajax call
                    string Imagename = httpContext.Request.Form["Imagename"];

                    if (httpPostedFile != null)
                    {


                        // Construct file save path  
                        //var fileSavePath = Path.Combine(HostingEnvironment.MapPath(ConfigurationManager.AppSettings["fileUploadFolder"]), httpPostedFile.FileName);
                        string fname = httpPostedFile.FileName.Split('\\').Last();

                        string[] arr;
                        arr = fname.Split('.');
                        DateTime dt = DateTime.Now;
                        string text = dt.ToString();
                        text = text.Replace("/", ".").Replace(" ", "_").Replace(":", ".");

                        fname = text + "." + arr[arr.Length - 1];




                        var fileSavePath = Path.Combine(HostingEnvironment.MapPath("~/uploadedImages"), fname);
                        // Save the uploaded file  
                        httpPostedFile.SaveAs(fileSavePath);
                        imageLinks.Add("uploadedImages/" + fname);
                    }
                }
            }

            // Return status code  
            return Request.CreateResponse(HttpStatusCode.Created, imageLinks);
        }

    }
}
