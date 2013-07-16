using System.Collections.Generic;


namespace VtcIts.Models.Api {

    public class Location {


        public int Id { get; set; }
        public string Name { get; set; }
        public ICollection<Building> Buildings { get; set; }

        private  Location() {}

        
        public static Location GetLocation(Models.Location source) {
            Location output = null;

            if (source != null) {
                output = new Location {
                    Id = source.Id,
                    Name = source.Name,
                    Buildings = new HashSet<Building>()
                };

                foreach (var building in source.Buildings) {
                    output.Buildings.Add(Building.GetBuilding(building));
                }
            }

            return output;
        }
    

        public int BuildingCount {
            get { return Buildings.Count; }
        }

    }

}
