using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.Http;
using VtcIts.Models.Api;


namespace VtcIts.Controllers.Api {


    public class ExternalLocationController : ApiController {


        // GET api/externallocation
        public IQueryable<ExternalLocation> Get() {
            var output = new List<ExternalLocation>();

            using (var context = new Models.VtcContext()) {
                foreach (var item in context.ExternalLocations) {
                    output.Add(ExternalLocation.GetExternalLocation(item));
                }
            }
            return output.AsQueryable();
        }


        // GET api/externallocation/5
        public ExternalLocation Get(int id) {
            using (var context = new Models.VtcContext()) {
                var result = context.ExternalLocations.Find(id);
                if (result == null) { throw new HttpResponseException(HttpStatusCode.NotFound); }
                return ExternalLocation.GetExternalLocation(result);
            }
            return null;
        }

        //// POST api/externallocation
        //public void Post([FromBody]ExternalLocation value) {
        //}

        //// PUT api/externallocation/5
        //public void Put(int id, [FromBody]ExternalLocation value)
        //{
        //}

        //// DELETE api/externallocation/5
        //public void Delete(int id)
        //{
        //}

    }

}
