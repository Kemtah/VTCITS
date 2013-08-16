using System.Collections.Generic;


namespace VtcIts.Models.Api {

    public class State {


        public int Id { get; set; }
        public string Name { get; set; }
        public string Abbreviation { get; set; }

        private State() { }

        
        public static State GetState(Models.Ref_State source) {
            State output = null;

            if (source != null) {
                output = new State {
                    Id = source.Id,
                    Name = source.Name,
                    Abbreviation = source.Abbreviation,
                };
            }

            return output;
        }

   
    }

}
