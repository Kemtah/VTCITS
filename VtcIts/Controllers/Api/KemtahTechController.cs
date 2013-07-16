using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.Http;
using VtcIts.Models.Api;


namespace VtcIts.Controllers.Api {


    public class KemtahTechController : ApiController {


        // GET api/kemtahtech
        public IQueryable<KemtahTech> Get() {
            var output = new List<KemtahTech>();

            using (var context = new Models.VtcContext()) {
                foreach (var item in context.KemtahTeches) {
                    output.Add(KemtahTech.GetKemtahTech(item));
                }
            }
            return output.AsQueryable();
        }


        // GET api/kemtahtech/5
        public KemtahTech Get(int id) {
            using (var context = new Models.VtcContext()) {
                var result = context.KemtahTeches.Find(id);
                if (result == null) { throw new HttpResponseException(HttpStatusCode.NotFound); }
                return KemtahTech.GetKemtahTech(result);
            }
            return null;
        }

        //// POST api/kemtahtech
        //public void Post([FromBody]KemtahTech value) {
        //}

        //// PUT api/kemtahtech/5
        //public void Put(int id, [FromBody]KemtahTech value)
        //{
        //}

        //// DELETE api/kemtahtech/5
        //public void Delete(int id)
        //{
        //}

    }

}
