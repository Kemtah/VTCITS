using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Xml;
using VtcIts.Models;

namespace VtcIts.Controllers
{
	[Authorize]
	public class MeetingController : Controller
	{
		private VtcContext db = new VtcContext();

		//
		// GET: /Meeting/

		[AllowAnonymous]
		public ActionResult Index() {
			var meetings = db.Meetings.Include(m => m.KemtahTech).Include(m => m.Requester);
			return View(meetings.ToList());
		}

		//
		// GET: /Meeting/Details/5

		[AllowAnonymous]
		public ActionResult Details(int id = 0)
		{
			Meeting meeting = db.Meetings.Find(id);
			if (meeting == null)
			{
				return HttpNotFound();
			}
			return View(meeting);
		}



		//
		// GET: /Meeting/Create

		public ActionResult Create() {
			var newMeeting = db.Meetings.Create();
			newMeeting.Start = DateTime.Parse(DateTime.Now.ToString("MM/dd/yyyy") + " 8:30 AM");
			newMeeting.End = DateTime.Parse(DateTime.Now.ToString("MM/dd/yyyy") + " 9:00 AM");
			newMeeting.TechnicianId = -1;

			return View(newMeeting);
		}

		//
		// POST: /Meeting/Create

		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Create(
			string title,
			string startDateTime,
			string endDateTime,
			string description,
			string footprintsTicket,
            bool isBillable,
            int techId,
			int? requesterId,
			int source,
			string email,
			string badgeNumber,
			string firstName,
			string lastName,
			string phoneNumber,
			int? locationId,
            int? buildingId,
            int timeZoneOffset = 0
		) {

            var serverOffset = TimeZone.CurrentTimeZone.GetUtcOffset(DateTime.Now);
            var diff = ((timeZoneOffset / 60) + serverOffset.Hours) * -1;
            var start = DateTime.Parse(startDateTime).AddHours(diff);
            var end = DateTime.Parse(endDateTime).AddHours(diff);

			var meeting = db.Meetings.Create();
			meeting.Title = title;
			meeting.Description = description;
			meeting.FootprintsTicket = footprintsTicket;
			meeting.TechnicianId = techId;
            meeting.Start = start;
            meeting.End = end;
			meeting.Notes = string.Empty;
			meeting.Created = DateTime.Now;
		    meeting.Billable = isBillable;
			meeting.Source = (MeetingRequestSource)source;

			var person = (requesterId.HasValue)
			 ? db.People.Find(requesterId)
			 : InsertPerson(email, badgeNumber, firstName, lastName, phoneNumber, locationId, buildingId);

			meeting.Requester = person;
			db.Entry(person).State = EntityState.Modified;
			db.Entry(meeting).State = EntityState.Added;
			db.SaveChanges();

			return RedirectToAction("Edit", new { id = meeting.Id });
		}


		//
		// GET: /KemtahTech/Edit/5

		public ActionResult Edit(int id = 0)
		{
			Meeting meeting = db.Meetings.Find(id);
			if (meeting == null)
			{
				return HttpNotFound();
			}
			return View(meeting);
		}

		//

		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Edit(
			int meetingId,
			string title,
			string startDateTime,
			string endDateTime,
			string description,
			string footprintsTicket,
			int techId,
			bool isBillable,
			string notes,
			int timeZoneOffset = 0
		) {
		    var serverOffset = TimeZone.CurrentTimeZone.GetUtcOffset(DateTime.Now);
            var diff = ((timeZoneOffset/60) + serverOffset.Hours) * -1;
            var start = DateTime.Parse(startDateTime).AddHours(diff);
            var end = DateTime.Parse(endDateTime).AddHours(diff);
		    
			var meeting = db.Meetings.Find(meetingId);
			meeting.Title = title;
			meeting.Description = description;
			meeting.Notes = notes;
			meeting.FootprintsTicket = footprintsTicket;
			meeting.TechnicianId = techId;
			meeting.Start = start;
			meeting.End = end;
			meeting.Billable = isBillable;

			db.Entry(meeting).State = EntityState.Modified;
			db.SaveChanges();
			return RedirectToAction("Edit", new { id = meetingId });
		}


		//
		// GET: /Meeting/Delete/5
		public ActionResult Delete(int id = 0)
		{
			Meeting meeting = db.Meetings.Find(id);
			if (meeting == null)
			{
				return HttpNotFound();
			}
			return View(meeting);
		}


		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult ApplyWorkflow(
			int meetingId,
			int status
		) {
			var toStatus = (MeetingStatus) status;
			var meeting = db.Meetings.Find(meetingId);
			var changed = false;
			switch (meeting.Status) {
				case MeetingStatus.BeforeStart:
					if (toStatus == MeetingStatus.InProgress) {
						meeting.Status = toStatus;
						meeting.Started = DateTime.Now;
						changed = true;
					} else if (toStatus == MeetingStatus.Cancelled) {
						meeting.Status = toStatus;
						meeting.Closed = DateTime.Now;
						changed = true;
					}
					break;
				case MeetingStatus.InProgress:
					if (toStatus == MeetingStatus.Closed) {
						meeting.Status = toStatus;
						meeting.Closed = DateTime.Now;
						changed = true;
					}
					break;
				case MeetingStatus.Cancelled:
					if (toStatus == MeetingStatus.BeforeStart) {
						meeting.Status = toStatus;
						meeting.Closed = null;
						changed = true;
					}
					break;
			}
			if (changed) {
				db.Entry(meeting).State = EntityState.Modified;
				db.SaveChanges();
			}
			return RedirectToAction("Edit", new { id = meetingId });
		}


		#region Rooms


		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult AddRoom(int meetingId, int roomId) {
			var meeting = db.Meetings.Find(meetingId);
			var room = db.Rooms.Find(roomId);
			if (room != null) {
				meeting.Rooms.Add(room);
				db.Entry(meeting).State = EntityState.Modified;
				db.SaveChanges();
			}

			return RedirectToAction("Edit", new { id = meetingId });
		}

		
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult DeleteRoom(int meetingId, int roomId) {
			var meeting = db.Meetings.Find(meetingId);
			//var room = db.Rooms.Find(roomId);
			var room = meeting.Rooms.FirstOrDefault(r => r.Id == roomId);
			if (room != null) {
				meeting.Rooms.Remove(room);
				db.Entry(meeting).State = EntityState.Modified;
				db.SaveChanges();
			}

			return RedirectToAction("Edit", new { id = meetingId });
		}


		#endregion


		#region External Locations


		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult AddExLocation(
			int meetingId, 
			string addExLocationMethod,
			int? exLocationId, 
			string name,
			string ipAddress,
			string pointOfContact,
			string technicalContactPhone,
			string technicalContactEmail
		) {
			var meeting = db.Meetings.Find(meetingId);
			ExternalLocation exLocation = null;

			switch (addExLocationMethod) {
				case "existing":
					exLocation = db.ExternalLocations.Find(exLocationId);
					if (exLocation != null) {
						meeting.ExternalLocations.Add(exLocation);
						db.Entry(meeting).State = EntityState.Modified;
						db.SaveChanges();
					}
					break;
				case "new":
					exLocation = db.ExternalLocations.Create();
					exLocation.Name = name;
					exLocation.IpAddress = ipAddress;
					exLocation.PointOfContact = pointOfContact;
					exLocation.TechnicalContactPhone = technicalContactPhone;
					exLocation.TechnicalContactEmail = technicalContactEmail;
					db.Entry(exLocation).State = EntityState.Added;
					break;
			}
			if (exLocation != null) {
				meeting.ExternalLocations.Add(exLocation);
				db.Entry(meeting).State = EntityState.Modified;
				db.SaveChanges();
			}

			return RedirectToAction("Edit", new { id = meetingId });
		}


		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult DeleteLocation(int meetingId, int exLocationId) {
			var meeting = db.Meetings.Find(meetingId);
			var location = meeting.ExternalLocations.FirstOrDefault(l => l.Id == exLocationId);
			if (location != null) {
				meeting.ExternalLocations.Remove(location);
				db.Entry(meeting).State = EntityState.Modified;
				db.SaveChanges();
			}

			return RedirectToAction("Edit", new { id = meetingId });
		}


		#endregion


		#region Participants


		public ActionResult SetRequester(
			int meetingId, 
			int? personId,
			string email,
			string badgeNumber,
			string firstName,
			string lastName,
			string phoneNumber,
			int? locationId,
			int? buildingId,
			string method
		) {

			var person = (method != "new")
				? db.People.Find(personId)
				: InsertPerson(email, badgeNumber, firstName, lastName, phoneNumber, locationId, buildingId);
			
			if (person != null) {
				var meeting = db.Meetings.Find(meetingId);
				meeting.Requester = person;
				db.Entry(meeting).State = EntityState.Modified;
				db.SaveChanges();
			}

			return RedirectToAction("Edit", new { id = meetingId });
		}



		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult AddParticipant(
			int meetingId, 
			int? personId,
			string email,
			string badgeNumber,
			string firstName,
			string lastName,
			string phoneNumber,
			int? locationId,
			int? buildingId,
			string addParticipantMethod
		) {

			var person = (addParticipantMethod != "new")
				? db.People.Find(personId)
				: InsertPerson(email, badgeNumber, firstName, lastName, phoneNumber, locationId, buildingId);

			if (person != null) {
				var meeting = db.Meetings.Find(meetingId);
				var participant = db.MeetingParticipants.Create();
				participant.Meeting = meeting;
				participant.Person = person;
				db.Entry(participant).State = EntityState.Added;

				db.SaveChanges();
			}

			return RedirectToAction("Edit", new { id = meetingId });
		}


		private Person InsertPerson(
			string email,
			string badgeNumber,
			string firstName,
			string lastName,
			string phoneNumber,
			int? locationId,
			int? buildingId
		) {
			var person = db.People.Create();
			person.BadgeNumber = badgeNumber;
			person.FirstName = firstName;
			person.LastName = lastName;
			person.Phone = phoneNumber;
			person.Email = email;
			person.Location = db.Locations.Find(locationId);
			if (buildingId.HasValue) {
				person.Building = db.Buildings.Find(buildingId);
			}
			db.Entry(person).State = EntityState.Added;
			db.SaveChanges();

			return person;
		}



		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult UpdateParticipantType(
			int meetingId,
			int participantId,
			int type
		) {
			var participant = db.MeetingParticipants.Find(meetingId, participantId);
			if (participant != null) {
				participant.Type = (ParticipantType)type;

				db.Entry(participant).State = EntityState.Modified;
				db.SaveChanges();
			}
			return RedirectToAction("Edit", new { id = meetingId });
		}


		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult UpdateParticipantTravelAvoided(
			int meetingId,
			int participantId,
			int travelAvoided
		) {
			var participant = db.MeetingParticipants.Find(meetingId, participantId);
			if (participant != null) {
				participant.TravelAvoided = (Ternary)travelAvoided;

				db.Entry(participant).State = EntityState.Modified;
				db.SaveChanges();
			}
			return RedirectToAction("Edit", new { id = meetingId });
		}


		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult UpdateParticipantAttendance(
			int meetingId,
			int participantId,
			bool attended
		) {
			var participant = db.MeetingParticipants.Find(meetingId, participantId);
			if (participant != null) {
				participant.Attended = attended;

				db.Entry(participant).State = EntityState.Modified;
				db.SaveChanges();
			}
			return RedirectToAction("Edit", new { id = meetingId });
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult SetUnknownParticipantCount(
			int meetingId,
			int yes,
			int maybe,
			int no
		) {
			var meeting = db.Meetings.Find(meetingId);

			if (meeting != null) {
				meeting.UnknownParticipants_TravelAvoided = yes;
				meeting.UnknownParticipants_TravelMaybeAvoided = maybe;
				meeting.UnknownParticipants_TravelNotAvoided = no;
				db.Entry(meeting).State = EntityState.Modified;
				db.SaveChanges();
			}

			return RedirectToAction("Edit", new { id = meetingId });
		}



		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult RemoveParticipant(int meetingId, int participantId) {
			var participant = db.MeetingParticipants.Find(meetingId, participantId);
			if (participant != null) {
				db.MeetingParticipants.Remove(participant);
				db.Entry(participant).State = EntityState.Deleted;
				db.SaveChanges();
			}

			return RedirectToAction("Edit", new { id = meetingId });
		}



		#endregion


		#region Recurrence


		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult CreateRecurrence(int meetingId, string dateList, string recurrenceText) {
			var delimiters = new string[] {"|"};
			var sourceMeeting = db.Meetings.Find(meetingId);
			var sourceStartDate = DateTime.Parse(sourceMeeting.Start.ToString("MM/dd/yyyy"));
			var startTimeText = sourceMeeting.Start.ToString("hh:mm tt");
			var endTimeText = sourceMeeting.End.ToString("hh:mm tt");
			
			var series = db.Series.Create();
			series.RuleText = recurrenceText;
			series.StartDate = sourceStartDate;

			series.Meetings.Add(sourceMeeting);
			db.Entry(series).State = EntityState.Added;
			db.SaveChanges();


			foreach (var dateText in dateList.Split(delimiters, StringSplitOptions.RemoveEmptyEntries)) {
				var startDate = DateTime.Parse(dateText + " " + startTimeText);
				var endDate = DateTime.Parse(dateText + " " + endTimeText);

				var newMeeting = db.Meetings.Create();
				newMeeting.Start = DateTime.Parse(dateText + " " + startTimeText);
				newMeeting.End = DateTime.Parse(dateText + " " + endTimeText);
				newMeeting.Description = sourceMeeting.Description;
				newMeeting.Notes = sourceMeeting.Notes;
				newMeeting.Title = sourceMeeting.Title;
				newMeeting.Source = sourceMeeting.Source;
				newMeeting.Created = DateTime.Now;
				newMeeting.FootprintsTicket = sourceMeeting.FootprintsTicket;

				newMeeting.TechnicianId = -1;
				newMeeting.Requester = sourceMeeting.Requester;
				newMeeting.MeetingSeries = series;

				db.Entry(newMeeting).State = EntityState.Added;
				db.SaveChanges();

			  
				foreach (var participant in sourceMeeting.MeetingParticipants.ToList()) {
					var newParticipant = db.MeetingParticipants.Create();
					var person = participant.Person;

					newParticipant.Meeting = newMeeting;
					newParticipant.Person = person;
					newParticipant.TravelAvoided = participant.TravelAvoided;
					newParticipant.Type = participant.Type;
					newParticipant.Attended = false;

					db.Entry(newParticipant).State = EntityState.Added;
					newMeeting.MeetingParticipants.Add(newParticipant);
					//db.SaveChanges();
				}

				foreach (var room in sourceMeeting.Rooms.ToList()) {
					newMeeting.Rooms.Add(room);
					db.Entry(room).State = EntityState.Modified;
				}

				foreach (var exLoc in sourceMeeting.ExternalLocations.ToList()) {
					newMeeting.ExternalLocations.Add(exLoc);
					db.Entry(exLoc).State = EntityState.Modified;
				}

				db.Entry(newMeeting).State = EntityState.Modified;
				db.SaveChanges();
			}

			return RedirectToAction("Edit", new { id = meetingId });
		}


		public ActionResult DeleteRecurrence(int sourceMeetingId, int seriesId, string deleteMethod) {

			foreach (var meeting in db.Meetings.Where(meeting => meeting.SeriesId == seriesId).AsEnumerable()) {

				switch (deleteMethod) {
					case "minimal":
						meeting.SeriesId = null;
						db.Entry(meeting).State = EntityState.Modified;
						break;

					case "total":
						if (meeting.Status != MeetingStatus.BeforeStart) {
							meeting.SeriesId = null;
							db.Entry(meeting).State = EntityState.Modified;
						} else {
							// Why can't you just do this for me, EntityFramework?
							foreach (var room in meeting.Rooms.ToList()) { meeting.Rooms.Remove(room); }
							foreach (var participant in meeting.MeetingParticipants.ToList()) { meeting.MeetingParticipants.Remove(participant); }
							foreach (var exLocation in meeting.ExternalLocations.ToList()) { meeting.ExternalLocations.Remove(exLocation); }
							db.Meetings.Remove(meeting);
							db.Entry(meeting).State = EntityState.Deleted;
						}
						break;
				}
			}

			var series = db.Series.Find(seriesId);
			db.Series.Remove(db.Series.Find(seriesId));
			db.Entry(series).State = EntityState.Deleted;
			db.SaveChanges();

			return db.Meetings.Find(sourceMeetingId) != null
				? RedirectToAction("Edit", new { id = sourceMeetingId }) 
				: RedirectToAction("Index");
		}


		#endregion


		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult SendSurvey( int meetingId ) {
			var meeting = db.Meetings.Find(meetingId);
			if (!meeting.SurveySent) {

				const string templateUrl = "~/Content/Templates/SurveyEmail.txt";
				var messageText = System.IO.File.ReadAllText(Server.MapPath(templateUrl));
				messageText = messageText.Replace("[ID]", meeting.Id.ToString());
				messageText = messageText.Replace("[LINK]", Settings.SurveyLink);

				var message = new MailMessage {
					From = Settings.SystemAddress, 
					Body = messageText,
					IsBodyHtml = true
				};

				foreach (var person in meeting.MeetingParticipants) {
					try {
						var address = person.Person.Email;
						if (!string.IsNullOrEmpty(address)) {
							var name = string.Format("{0}, {1}", person.Person.LastName, person.Person.FirstName);
							message.To.Add(new MailAddress(address, name));
						}
					} catch {}
				}
				message.To.Add(new MailAddress(
					meeting.Requester.Email, 
					string.Format("{0}, {1}", meeting.Requester.LastName, meeting.Requester.FirstName)
				));

				if (Settings.EnableEmail) {
					try {
						Mail.SendMail(message);
						meeting.SurveySent = true;
						db.Entry(meeting).State = EntityState.Modified;
						db.SaveChanges();
					} catch (Exception ex) {
						return Content(ex.ToString());
					}
				} else {
					return Content(Mail.PrintMessage(message).Replace("\n","<br/>"));
				}
			}

			return RedirectToAction("Edit", new { id = meetingId });
		}



		[HttpGet]
		public ContentResult DetailedReport() {
			var tableNames = new List<string> {
				"Meetings",
				"MeetingRooms",
				"Rooms",
				"MeetingParticipants",
				"Participants",
				"MeetingExternalLocations",
				"ExternalLocations"
			};

			var data = Utils.CallProcedure("GetDetailedReport", "DetailedMeetingReport", tableNames);
			return Content(data.GetXml(), "application/xml");
		}



		[HttpGet]
		public ContentResult SummaryReport() {
			var data = Utils.CallProcedure("GetSummaryReport", "SummaryMeetingReport");

			var sb = new StringBuilder(); 
			string[] columnNames = data.Tables[0].Columns.Cast<DataColumn>().
											  Select(column => column.ColumnName).
											  ToArray();
			sb.AppendLine(string.Join("\t", columnNames));

			foreach (var fields in 
				from DataRow row in data.Tables[0].Rows 
				select row.ItemArray.Select(field => field.ToString()).ToArray()
			) {
				sb.AppendLine(string.Join("\t", fields));
			}
			return Content(sb.ToString(), "application/vnd.ms-excel");
		}

		//
		// POST: /Meeting/Delete/5

		[HttpPost, ActionName("Delete")]
		[ValidateAntiForgeryToken]
		public ActionResult DeleteConfirmed(int id) {
			Meeting meeting = db.Meetings.Find(id);
			foreach (var room in meeting.Rooms.ToList()) { meeting.Rooms.Remove(room); }
			foreach (var participant in meeting.MeetingParticipants.ToList()) { meeting.MeetingParticipants.Remove(participant); }
			foreach (var exLocation in meeting.ExternalLocations.ToList()) { meeting.ExternalLocations.Remove(exLocation); }
			db.Meetings.Remove(meeting);
			db.SaveChanges();
			return RedirectToAction("Index");
		}

		protected override void Dispose(bool disposing)
		{
			db.Dispose();
			base.Dispose(disposing);
		}

	}
}