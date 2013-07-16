namespace VtcIts.Models.Api {
    
    public class Room {


        public int Id { get; set; }
        public int BuildingId { get; set; }
        public string Name { get; set; }
        public bool VtcEnabled { get; set; }


        private Room() {}


        public static Room GetRoom(Models.Room source) {
            Room output = null;

            if (source != null) {
                output = new Room {
                    Id = source.Id, 
                    BuildingId = source.BuildingId, 
                    Name = source.Name,
                    VtcEnabled = source.VtcEnabled
                };
            }

            return output;
        }


    }
}
