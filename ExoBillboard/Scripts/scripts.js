//Weather Widget
function getWeather() {
    $.simpleWeather({
        location: '19002',
        unit: 'f',
        success: function (weather) {
            html = '<h2>' + weather.temp + '&deg;' + weather.units.temp + '</h2>';
            html += '<ul><li>' + weather.city + ', ' + weather.region + '</li>';
            html += '<li class="currently">' + weather.currently + '</li>';
            html += '<li>' + weather.alt.temp + '&deg;C</li></ul>';

            $("#weather").html(html);
        },
        error: function (error) {
            $("#weather").html('<p>' + error + '</p>');
        }
    });
}

//Load the weather widget
$(document).ready(function () {
    getWeather(); //Get the initial weather.
    setInterval(getWeather, 600000); //Update the weather every 10 minutes.
});

//Load slider function
function loadSlider() {
    var photo = "";
    var url = "/Home/Json/";
    var id = "";
    var interval = setInterval(function () {
        $.getJSON(url + id, function (json) {
            photo = $('<div class="photo" id="' + id + '"><div class="slider-caption"><h1>' + json.Title + '</h1><p>' + json.Caption + '</p></div></div>')
                .css("background-image", "url(" + json.FilePath + ")").hide().fadeIn(2500);
            $(".photo-gallery").append(photo);
            id = json.Index;
        });
        $(".photo-gallery").find("div:first").css("z-index", "-1").delay(15000).fadeOut(2500, function () {
            $(this).remove();
        });
    }, 20000);
};

//Reload the page at the specified time.
function refreshAt(hours, minutes, seconds) {
    var now = new Date();
    var then = new Date();

    if (now.getHours() > hours ||
       (now.getHours() == hours && now.getMinutes() > minutes) ||
        now.getHours() == hours && now.getMinutes() == minutes && now.getSeconds() >= seconds) {
        then.setDate(now.getDate() + 1);
    }
    then.setHours(hours);
    then.setMinutes(minutes);
    then.setSeconds(seconds);

    var timeout = (then.getTime() - now.getTime());
    setTimeout(function () { window.location.reload(true); }, timeout);
}

//Digital clock plugin
$("#clock").MyDigitClock({
    fontSize: 50,
    fontFamily: "Open Sans",
    fontColor: "#FFF",
    fontWeight: "normal",
    bAmPm: true,
    background: null,
    //bShowHeartBeat: true
});

//Refresh the page at midnight
refreshAt(0, 0, 0);

//Hide the mouse cursor
$('html').css('cursor', 'none');