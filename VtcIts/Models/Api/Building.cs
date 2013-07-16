using System.Collections.Generic;


namespace VtcIts.Models.Api {

    
    public class Building {
    
        public int Id { get; set; }
        public int LocationId { get; set; }
        public string Name { get; set; }
        public ICollection<Room> Rooms { get; set; }


        private Building() { }


        public static Building GetBuilding(Models.Building source) {
            Building output = null;

            if (source != null) {
                output = new Building {
                    Id = source.Id,
                    LocationId = source.LocationId,
                    Name = source.Name,
                    Rooms = new HashSet<Room>()
                };

                foreach (var room in source.Rooms) {
                    output.Rooms.Add(Room.GetRoom(room));
                }
            }

            return output;
        }


        public int RoomCount {
            get { return Rooms.Count; }
        }

    }

}
