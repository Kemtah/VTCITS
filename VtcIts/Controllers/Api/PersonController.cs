using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.Http;
using VtcIts.Models.Api;


namespace VtcIts.Controllers.Api {


    public class PersonController : ApiController {


        // GET api/person
        public IQueryable<Person> Get() {
            var output = new List<Person>();

            using (var context = new Models.VtcContext()) {
                foreach (var item in context.People) {
                    output.Add(Person.GetPerson(item));
                }
            }
            return output.AsQueryable();
        }


        // GET api/person/5
        public Person Get(int id) {
            using (var context = new Models.VtcContext()) {
                var result = context.People.Find(id);
                if (result == null) { throw new HttpResponseException(HttpStatusCode.NotFound); }
                return Person.GetPerson(result);
            }
            return null;
        }

        //// POST api/person
        //public void Post([FromBody]Person value) {
        //}

        //// PUT api/person/5
        //public void Put(int id, [FromBody]Person value)
        //{
        //}

        //// DELETE api/person/5
        //public void Delete(int id)
        //{
        //}

    }

}
