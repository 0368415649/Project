//  Calendar
var CURRENT_DATE = new Date();
var d = new Date();
function check_back(month, month_current){
  if(month_current <= month){
    $('.prev-month').addClass('history');
  }else{
    $('.prev-month').removeClass('history');
  }

}
var content = '1 2 3 4 5 6 7 8 9 10 11 12'.split(' ');
var weekDayName = 'SUN MON TUES WED THURS FRI'.split(' ');
var daysOfMonth = [31, 28, 31, 30, 31, 30, 31, 31, 30, 31, 30, 31];

// Returns the day of week which month starts (eg 0 for Sunday, 1 for Monday, etc.)
function getCalendarStart(dayOfWeek, currentDate) {
  var date = currentDate - 1;
  var startOffset = (date % 7) - dayOfWeek;
  if (startOffset > 0) {
    startOffset -= 7;
  }
  return Math.abs(startOffset);
}

// Render Calendar
function renderCalendar(startDay, totalDays, currentDate, month) {
  var d_2 = new Date();
  var currentRow = 1;
  var currentDay = startDay;
  var $table = $('table');
  var $week = getCalendarRow();
  var $day;
  var month_crrent = d_2.getUTCMonth();
  var i = 1;
  for (; i <= totalDays; i++) {
    $day = $week.find('td').eq(currentDay);
    $day.text(i);
    if (i === currentDate) {
        $day.addClass('today');
    }
    $day.click(function(e){
        day_ship = d.getUTCFullYear() + "/" + (d.getUTCMonth() + 1) + "/" + $(this).text();
        $('#calendar_name').val(day_ship)
    });
    if(month_crrent == month && i < currentDate){
          $day.addClass('history');
    }
    // +1 next day until Saturday (6), then reset to Sunday (0)
    currentDay = ++currentDay % 7;

    // Generate new row when day is Saturday, but only if there are
    // additional days to render
    if (currentDay === 0 && (i + 1 <= totalDays)) {
      $week = getCalendarRow();
      currentRow++;
    }
  }
}

// Clear generated calendar
function clearCalendar() {
  var $trs = $('tr').not(':eq(0)');
  $trs.remove();
  $('.month-year').empty();
}

// Generates table row used when rendering Calendar
function getCalendarRow() {
  var $table = $('table');
  var $tr = $('<tr/>');
  for (var i = 0, len = 7; i < len; i++) {
    $tr.append($('<td/>'));
  }
  $table.append($tr);
  return $tr;
}

function myCalendar() {
  var month = d.getUTCMonth();
  var day = d.getUTCDay();
  var year = d.getUTCFullYear();
  var date = d.getUTCDate();
  var totalDaysOfMonth = daysOfMonth[month];

  var counter = 1;

  var $h3 = $('<h3>');

  $h3.text(content[month] + ' ' + year);
  $h3.appendTo('.month-year');

  var dateToHighlight = 0;

  // Determine if Month && Year are current for Date Highlight
  if (CURRENT_DATE.getUTCMonth() === month && CURRENT_DATE.getUTCFullYear() === year) {
    dateToHighlight = date;
  }
  //Getting February Days Including The Leap Year
  if (month === 1) {
    if ((year % 100 !== 0) && (year % 4 === 0) || (year % 400 === 0)) {
      totalDaysOfMonth = 29;
    }
  }
//     console.log(dateToHighlight)
  // Get Start Day
  renderCalendar(getCalendarStart(day, date), totalDaysOfMonth, dateToHighlight,month);
  check_back(CURRENT_DATE.getUTCMonth(), d.getUTCMonth());
};

function navigationHandler(dir) {
  d.setUTCMonth(d.getUTCMonth() + dir);
  clearCalendar();
  myCalendar();
}

$(document).ready(function() {
  // Bind Events
  $('.prev-month').click(function() {
    navigationHandler(-1);
  });
  $('.next-month').click(function() {
    navigationHandler(1);
  });
  // Generate Calendar
  myCalendar();
});

//  Calendar