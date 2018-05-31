using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Courses.Models
{
    public class UploadModel
    {
        public HttpPostedFileBase File { get; set; }
        public string name { get; set; }
    }
}