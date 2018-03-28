using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Courses.Controllers
{
    public class DropboxController : ApiController
    {

        [HttpGet]
        public string GetDropboxKey()
        {
            try
            {
                var key = ConfigurationManager.AppSettings["DropboxKey"];
                return key;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
