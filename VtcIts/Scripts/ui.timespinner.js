/// -------------------------------------------------------------------
/// Used to manage the TimeSpinner
/// -------------------------------------------------------------------
function setupTimeSpinner() {
    $.widget("ui.timespinner", $.ui.spinner, {
        options: {
            step: 30 * 60 * 1000, // seconds
            page: 60 // hours
        },
        _parse: function(value) {
            if (typeof value === "string") {
                // already a timestamp
                if (Number(value) == value) {
                    return Number(value);
                }
                return +Globalize.parseDate(value);
            }
            return value;
        },

        _format: function(value) {
            return Globalize.format(new Date(value), "t");
        }
    });
}

/// -------------------------------------------------------------------
/// Converts a date to an integer representing the time, with 0 or 24
/// as the hour as appropriate. Used to compare times.
/// -------------------------------------------------------------------
// Why the hell are you so dumb/difficult with dates, Javascript?
/// -------------------------------------------------------------------
function getDateNumber(date, midnightIsEnd) {
    var hours = date.getHours();
    var minutes = date.getMinutes();

    if (midnightIsEnd && hours == 0 && minutes == 0) { hours = 24; } // The reason for the season

    return (hours * 100) + (minutes);
}


/// -------------------------------------------------------------------
// Ensures that the time the spinner is advancing to is a valid one
/// -------------------------------------------------------------------
function isValidTime(ui, startValue, endValue, fieldChanged) {
    var start = getDateNumber(Globalize.parseDate(startValue), false);
    var end = getDateNumber(Globalize.parseDate(endValue), true);
    var newTime = getDateNumber(new Date(ui.value), (fieldChanged == "end"));

    return (fieldChanged == "start")
        ? (newTime < end)
        : (newTime > start);
}