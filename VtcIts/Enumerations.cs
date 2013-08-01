namespace VtcIts {

    public enum ParticipantType {
        Physical = 0,
        Virtual = 1
    }


    public enum MeetingRequestSource {
        Exchange = 0,
        Phone = 1,
        Email = 2
    }


    public enum MeetingStatus {
        [EnumPrintText("Open")]
        BeforeStart = 0,
        [EnumPrintText("In Progress")]
        InProgress = 1,
        Closed = 2,
        Cancelled = 3,
        [EnumPrintText("No-Show")]
        NoShow = 4
    }

    public enum Ternary {
        [EnumPrintText("No")]
        False = 0,
        [EnumPrintText("Yes")]
        True = 1,
        Maybe = -1
    }
}