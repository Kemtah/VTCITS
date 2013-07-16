using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.Http;
using VtcIts.Models.Api;


namespace VtcIts.Controllers.Api {


    public class LocationController : ApiController {


        // GET api/location
        public IQueryable<Location> Get() {
            var output = new List<Location>();

            using (var context = new Models.VtcContext()) {
                foreach (var item in context.Locations) {
                    output.Add(Location.GetLocation(item));
                }
            }
            return output.AsQueryable();
        }


        // GET api/location/5
        public Location Get(int id) {
            using (var context = new Models.VtcContext()) {
                var result = context.Locations.Find(id);
                if (result == null) { throw new HttpResponseException(HttpStatusCode.NotFound); }
                return Location.GetLocation(result);
            }
            return null;
        }

        //// POST api/location
        //public void Post([FromBody]Location value) {
        //}

        //// PUT api/location/5
        //public void Put(int id, [FromBody]Location value)
        //{
        //}

        //// DELETE api/location/5
        //public void Delete(int id)
        //{
        //}

    }

}
