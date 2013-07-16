using System;

namespace VtcIts.Models {

    public static class MeetingExtensions {

        public static string GetStatusText(this Meeting meeting) {
            DateTime? date = null;

            switch (meeting.Status) {
                case MeetingStatus.BeforeStart:
                    date = meeting.Created;
                    break;
                case MeetingStatus.InProgress:
                    date = meeting.Started;
                    break;
                case MeetingStatus.Closed:
                case MeetingStatus.Cancelled:
                    date = meeting.Closed;
                    break;
            }

            return string.Format("{0}{1}",
                meeting.Status.ToPrintText(),
                (date.HasValue ? date.Value.ToString(" MM/dd/yyyy hh:mm tt"):"")
            );
        }


        public static bool IsEditable(this Meeting meeting) {
            return meeting.Status == MeetingStatus.BeforeStart || meeting.Status == MeetingStatus.InProgress;
        }


    }

}