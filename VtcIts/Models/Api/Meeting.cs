using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;


namespace VtcIts.Models.Api {

    
    public class Meeting {
    
        public int Id { get; set; }
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
        public string Description { get; set; }
        public string Notes { get; set; }
        public string Title { get; set; }
        public int Source { get; set; }
        public string FootprintsTicket { get; set; }
        public int TechnicianId { get; set; }
        public int RequesterId { get; set; }
        public int? SeriesId { get; set; }
        public int Status { get; set; }
        public bool SurveySent { get; set; }
        public bool Billable { get; set; }

        public string StatusText {
            get { return ((MeetingStatus) Status).ToPrintText(); }
        }
        public string DateText {
            get { return Start.ToString("MM/dd/yyyy"); }
        }
        public string StartText {
            get { return Start.ToString("hh:mm tt"); }
        }
        public string EndText {
            get { return End.ToString("hh:mm tt"); }
        }
        public string DurationText {
            get { return StartText + " to " + EndText; }
        }
        public string RequesterName {
            get { return Requester.LastName + ", " + Requester.FirstName; }
        }


        public KemtahTech KemtahTech { get; set; }
        public Person Requester { get; set; }
        public ICollection<Participant> Participants { get; set; }
        public ICollection<ExternalLocation> ExternalLocations { get; set; }
        public ICollection<MeetingRoom> Rooms { get; set; }
        public ICollection<MeetingBuilding> Buildings { get; set; }

        public int RoomCount {
            get { return Rooms.Count; }
        }

        private Meeting() { }


        public static Meeting GetMeeting(Models.Meeting source) {
            Meeting output = null;

            if (source != null) {
                output = new Meeting {
                    Id = source.Id,
                    Start = source.Start,
                    End = source.End,
                    Description = source.Description,
                    Notes = source.Notes,
                    Title = source.Title,
                    FootprintsTicket = source.FootprintsTicket,
                    TechnicianId = source.TechnicianId,
                    RequesterId = source.RequesterId,
                    SeriesId =  source.SeriesId,
                    Status = (int)source.Status,
                    SurveySent =  source.SurveySent,
                    Billable = source.Billable,
                    KemtahTech = KemtahTech.GetKemtahTech(source.KemtahTech),
                    Requester = Person.GetPerson(source.Requester),
                    Participants = new HashSet<Participant>(),
                    ExternalLocations = new HashSet<ExternalLocation>(),
                    Rooms = new HashSet<MeetingRoom>(),
                    Buildings = new HashSet<MeetingBuilding>()
                };

                foreach (var room in source.Rooms) {
                    output.Rooms.Add(MeetingRoom.GetRoom(room));
                }

                foreach (var room in source.Rooms) {
                    var building = MeetingBuilding.GetBuilding(room);
                    if (!output.Buildings.Contains(building)) {
                        output.Buildings.Add(building);
                    }
                }

                foreach (var participant in source.MeetingParticipants) {
                    output.Participants.Add(Participant.GetParticipant(participant));
                }

                foreach (var exLocation in source.ExternalLocations) {
                    output.ExternalLocations.Add(ExternalLocation.GetExternalLocation(exLocation));
                }

            }

            return output;
        }


        public int ParticipantCount {
            get { return Participants.Count; }
        }


        public int ExternalLocationCount {
            get { return ExternalLocations.Count; }
        }
       
    }

    
    
    public class MeetingRoom {

        public int Id { get; set; }

        [Display(Name="Building")]
        public string BuildingName { get; set; }


        [Display(Name = "Room")]
        public string RoomName { get; set; }


        private MeetingRoom() { }


        public static MeetingRoom GetRoom(Models.Room source) {
            MeetingRoom output = null;

            if (source != null) {
                output = new MeetingRoom {
                    Id = source.Id, 
                    BuildingName = source.Building.Name, 
                    RoomName = source.Name
                };
            }

            return output;
        }
    }

    public class MeetingBuilding {

        public int Id { get; set; }

        [Display(Name="Building")]
        public string Name { get; set; }


        private MeetingBuilding() { }


        public static MeetingBuilding GetBuilding(Models.Room source) {
            MeetingBuilding output = null;

            if (source != null) {
                output = new MeetingBuilding {
                    Id = source.Building.Id,
                    Name = source.Building.Name,
                };
            }

            return output;
        }
    }

}
