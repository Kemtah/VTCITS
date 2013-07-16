using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.Http;
using VtcIts.Models.Api;


namespace VtcIts.Controllers.Api {


    public class BuildingController : ApiController {


        // GET api/building
        public IQueryable<Building> Get() {
            var output = new List<Building>();

            using (var context = new Models.VtcContext()) {
                foreach (var item in context.Buildings.OrderBy(b => b.Name)) {
                    output.Add(Building.GetBuilding(item));
                }
            }
            return output.AsQueryable();
        }


        // GET api/building/5
        public Building Get(int id) {
            using (var context = new Models.VtcContext()) {
                var result = context.Buildings.Find(id);
                if (result == null) { throw new HttpResponseException(HttpStatusCode.NotFound); }
                return Building.GetBuilding(result);
            }
            return null;
        }

        //// POST api/building
        //public void Post([FromBody]Building value) {
        //}

        //// PUT api/building/5
        //public void Put(int id, [FromBody]Building value)
        //{
        //}

        //// DELETE api/building/5
        //public void Delete(int id)
        //{
        //}

    }

}
