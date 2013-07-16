using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace VtcIts.Controllers.Api {
    public class AvailableBadgeNumbersController : ApiController {

        public class AvailableBadgeNumbersRequest {
            public int id { get; set; }
            public string search { get; set; }
            public int maxResults { get; set; }
        }


        // POST api/values
        public HttpResponseMessage Post([FromBody] AvailableBadgeNumbersRequest request) {
            var output = new List<Models.AvailableBadgeNumbers_Result>();
            using (var context = new Models.VtcContext()) {
                output.AddRange(context.AvailableBadgeNumbers(request.id,request.search,request.maxResults).AsEnumerable());
            }

            return Request.CreateResponse(System.Net.HttpStatusCode.Created, output.AsQueryable());
        }

    }

}
