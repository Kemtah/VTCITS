using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace VtcIts.Controllers.Api {
    public class AvailableEmailsController : ApiController {

        public class AvailableEmailsRequest {
            public int id { get; set; }
            public string search { get; set; }
            public int maxResults { get; set; }
        }


        // POST api/values
        public HttpResponseMessage Post([FromBody] AvailableEmailsRequest request) {
            var output = new List<Models.AvailableEmails_Result>();
            using (var context = new Models.VtcContext()) {
                output.AddRange(context.AvailableEmails(request.id,request.search,request.maxResults).AsEnumerable());
            }

            return Request.CreateResponse(System.Net.HttpStatusCode.Created, output.AsQueryable());
        }

    }

}
