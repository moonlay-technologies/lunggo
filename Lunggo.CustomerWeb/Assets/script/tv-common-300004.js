var NUMBER_OF_CS_IMG = 2;

// Common, global functions should be placed inside this object to avoid name
// clashes. Think of this as a poor man's namespace.
window.Tv = {};

// tvAlert to popup a traveloka generic alert message
/*
  how to use:
  $.tvAlert.addAlertMsg({
        msg: "<p>Info message here (html)</p>",    => the message
        buttons : {                         => the buttons that includes in pop up
          Close    : function() { $(this).dialog("close"); },          => the label of the buttons as the key and callback function as the value (can be more than one)
          "Another btn"    : function() { $(this).dialog("close"); }
        },
        showCSImage: true,   => show CS image on the left side
        includeHeader: true,   => include header. Default true
        headerImageClass: "tvGreyLogo",  => the Image class of the top Icon
        showCloseBtn: true,   => show the 'X' close button (only shown if includeHeader == true)
        title: "Temukan perjalanan Anda.",  => optional title
        showLoadingImage: true,  => show loading gif image
        csNumber: -1    => set the CS image number (which one to appear). Default: Random

  });
*/
(function ($) {
    $.tvAlert = $.tvAlert || {};
    $.extend($.tvAlert, {

        addAlertMsg: function (data) {
            var alertDialog = $('#tvAlert'),
                alertData = $.extend({
                    showCSImage: true,
                    includeHeader: true,
                    headerImageClass: "tvGreyLogo",
                    title: "Temukan perjalanan Anda.",
                    showCloseBtn: true,
                    showLoadingImage: true,
                    csNumber: 0
                },
                  data),
                alertHeader = alertDialog.find('#tvAlertHeaderCntr'),
                closeIcon = alertDialog.find('.tvCloseGreyIcon');

            alertDialog.find('#tvIconInDialog').addClass(alertData.headerImageClass);
            alertDialog.find('#tvAlertImage').attr('class', 'cs' + alertData.csNumber);
            if (!alertData.showCSImage) { alertDialog.find('#tvAlertImageCntr').hide(); alertDialog.addClass('wide'); } else { alertDialog.find('#tvAlertImageCntr').show(); alertDialog.removeClass('wide'); }
            if (!alertData.includeHeader) { alertHeader.hide(); } else { alertHeader.show(); }
            if (!alertData.showLoadingImage) { alertDialog.find('#tvAlertLoadingImg').hide(); } else { alertDialog.find('#tvAlertLoadingImg').show(); }
            if (alertData.includeHeader && !alertData.showCloseBtn) { alertHeader.find('.tvCloseGreyIcon').hide(); } else { alertHeader.find('.tvCloseGreyIcon').show(); }
            alertDialog.find('#tvAlertTitle').html(alertData.title);
            alertDialog.find('#tvAlertContent').html(alertData.msg);

            alertDialog.dialog({
                closeOnEscape: false,
                modal: true,
                buttons: alertData.buttons,
                open: alertData.open,
                width: 450,
                dialogClass: 'tvAlertDialog'
            });

            closeIcon.unbind();
            closeIcon.click(function () { alertDialog.dialog("close"); });

        }

    });
})(jQuery);


// tvError to popup a traveloka generic warning message
/*
  how to use:
  $.tvError.addErrorMsg({
        className: "tvWarning float",       => (optional) style of the message; can be tvInfo, tvSuccess, tvWarning, tvError. Note: add "float" (optional) to make the message float
        msg: "Info message here (html)",    => the message
        duration: 10000                     => (optional) in millisecond, how long the message is going to be displayed
  });
*/
(function ($) {
    $.tvError = $.tvError || {};
    $.extend($.tvError, {

        pendingErrorMsg: [],
        isDisplayingErroMsg: false,
        DISPLAY_TIME_MSEC: 7000,
        SLIDE_DURATION_MSEC: 1000,

        addErrorMsg: function (data) {
            // Append to tvError data
            $.tvError.pendingErrorMsg.push(data);
            $.tvError.displayAllErrorMsg();
        },

        displayAllErrorMsg: function () {
            if (!$.tvError.isDisplayingErroMsg) {
                $.tvError.isDisplayingErroMsg = true;
                var errorMsgData = $.tvError.pendingErrorMsg.shift(),
                    errorDisplayDuration = $.tvError.DISPLAY_TIME_MSEC;
                if (typeof errorMsgData.duration != "undefined") errorDisplayDuration = errorMsgData.duration;
                $('#generalErrorMsg').html(errorMsgData.msg);
                $('#generalError')
                  .removeClass('tvSuccess tvError tvInfo tvWarning')
                  .addClass(errorMsgData.className)
                  .slideDown($.tvError.SLIDE_DURATION_MSEC, function () {
                      setTimeout($.tvError.doneDisplayErrorMsg, errorDisplayDuration);
                  });
            }
        },

        doneDisplayErrorMsg: function () {
            $('#generalError').slideUp($.tvError.SLIDE_DURATION_MSEC, function () {
                $.tvError.isDisplayingErroMsg = false;
                if ($.tvError.pendingErrorMsg.length != 0) {
                    $.tvError.displayAllErrorMsg();
                }
            });
        }

    });
})(jQuery);

$.ajaxSetup({
    type: 'POST',
    contentType: "application/json; charset=utf-8",
    dataType: "json"
});

var dayNames = ['Senin', 'Selasa', 'Rabu', 'Kamis', 'Jumat', 'Sabtu', 'Minggu'];
var earliestPagiTime = 4 * 60 + 0, // convert to min in a day
    earliestSiangTime = 11 * 60 + 0,
    earliestSoreTime = 15 * 60 + 0,
    earliestMalamTime = 18 * 60 + 30;

var maxNumUrlHistory = 10;
var maxUrlLengthInCookie = 70;

var setMaxCharInField = function (element, maxChar) {
    $(element).keypress(function (e) {
        var tval = $(element).val(),
            tlength = tval.length,
            set = maxChar,
            remain = parseInt(set - tlength);
        if (remain <= 0 && e.which !== 0 && e.charCode !== 0) {
            $(element).val((tval).substring(0, tlength - 1))
        }
    })
};

var datepickerOpts = {
    numberOfMonths: 2,
    closeText: 'Tutup',
    currentText: "Bulan Ini",
    monthNames: ['Januari', 'Februari', 'Maret', 'April', 'Mei', 'Juni',
                      'Juli', 'Agustus', 'September', 'Oktober', 'November', 'Desember'],
    monthNamesShort: ['Jan', 'Feb', 'Mar', 'Apr', 'Mei', 'Jun',
                      'Jul', 'Ags', 'Sep', 'Okt', 'Nov', 'Des'],
    dayNames: ['Minggu', 'Senin', 'Selasa', 'Rabu', 'Kamis', 'Jumat', 'Sabtu'],
    dayNamesShort: ['Mgg', 'Sen', 'Sel', 'Rab', 'Kam', 'Jum', 'Sab'],
    dayNamesMin: ['M', 'S', 'S', 'R', 'K', 'J', 'S'],
    dateFormat: 'dd-mm-yy',
    altFormat: "DD, d MM yy",
    altShortFormat: "DD, d M yy",
    showButtonPanel: true,
    minDate: "today",
    maxDate: "+2y",
    hideIfNoPrevNext: true,
    showAnim: "slideDown",
    stepMonths: 2,
    beforeShowDay: function (date) {
        var dynamicHoliday = $.datepicker.formatDate('dd-mm-yy', date, datepickerOpts);
        var staticHoliday = $.datepicker.formatDate('dd-mm', date, datepickerOpts);

        if (staticHoliday === '01-01') return [1, 'red', 'Tahun Baru'];
        if (staticHoliday === '17-08') return [1, 'red', 'Kemerdekaan RI'];
        if (staticHoliday === '25-12') return [1, 'red', 'Natal'];



        if (dynamicHoliday === '14-01-2014') return [1, 'red', 'Maulid Nabi Muhammad SAW'];
        if (dynamicHoliday === '31-01-2014') return [1, 'red', 'Tahun Baru Imlek 2565 Kongzili'];
        if (dynamicHoliday === '31-03-2014') return [1, 'red', 'Hari Raya Nyepi Tahun Baru Saka 1936'];
        if (dynamicHoliday === '18-04-2014') return [1, 'red', 'Wafat Isa Almasih'];
        if (dynamicHoliday === '01-05-2014') return [1, 'red', 'Memperingati Hari Buruh Internasional'];
        if (dynamicHoliday === '15-05-2014') return [1, 'red', 'Hari Raya Waisak 2558'];
        if (dynamicHoliday === '27-05-2014') return [1, 'red', 'Isra Mikraj Nabi Muhammad SAW'];
        if (dynamicHoliday === '29-05-2014') return [1, 'red', 'Kenaikan Isa Almasih'];
        if (dynamicHoliday === '28-07-2014') return [1, 'red', 'Hari Raya Idul Fitri 1435 Hijriah'];
        if (dynamicHoliday === '29-07-2014') return [1, 'red', 'Hari Raya Idul Fitri 1435 Hijriah'];
        if (dynamicHoliday === '05-10-2014') return [1, 'red', 'Hari Raya Idul Adha 1435 Hijriah'];
        if (dynamicHoliday === '25-10-2014') return [1, 'red', 'Tahun Baru 1436 Hijriah'];


        if (date.toString().indexOf('Sun ') !== -1) return [1, 'red'];

        return [1];
    }
};

/**
 * Use goog.string.padNumber() instead.
 * @deprecated
 */
function formatTime(time) {
    if ((time * 1) < 10) {
        return '0' + time;
    }
    return time;
}

function initCommon() {

    $(function () { // make sure all document loaded first

        var optionsQtip = {
            content: {
                text: $('#LoginRegister')
            },
            position: {
                my: 'top right',
                at: 'bottom right',
                target: $('#tv-topbar').children('.tv-topbar-container'),
                adjust: {
                    x: 0,
                    y: 0
                }
            },
            show: {
                event: 'click',
                delay: 0
            },
            hide: {
                event: 'unfocus click',
                fixed: true
            },
            style: {
                tip: false,
                classes: 'ui-tooltip-lblue ui-tooltip-userOptions'
            },
            events: {
                hide: function (event, api) {
                    $('#LoginRegister').attr('stat', 'up');
                },
                show: function (event, api) {
                    $('#LoginRegister').attr('stat', 'down');
                }
            }
        };

        $('#LoginRegister').qtip(optionsQtip);
        var loginAddOptions = {
            content: { text: $('#loginOptions') },
            events: {
                hide: function (event, api) {
                    $('#LoginRegister').attr('stat', 'up');
                },
                show: function (event, api) {
                    $('#LoginRegister').attr('stat', 'down');
                }
            }
        }
        $('#LoginRegister').qtip($.extend({}, optionsQtip, loginAddOptions));
    });
}

//usage "{0} is big, but {1} is small! {0} {2}".format("elephant", "ant")
String.prototype.format = function () {
    var args = arguments;
    return this.replace(/{(\d+)}/g, function (match, number) {
        return typeof args[number] != 'undefined'
          ? args[number]
          : match
        ;
    });
};

// Capitalize.
String.prototype.capitalize = function () {
    return this.replace(/(?:^|\s)\S/g, function (a) { return a.toUpperCase(); });
};

// to use indexOf in IE
if (!Array.prototype.indexOf) {
    Array.prototype.indexOf = function (elt /*, from*/) {
        var len = this.length >>> 0;

        var from = Number(arguments[1]) || 0;
        from = (from < 0)
             ? Math.ceil(from)
             : Math.floor(from);
        if (from < 0)
            from += len;

        for (; from < len; from++) {
            if (from in this &&
                this[from] === elt)
                return from;
        }
        return -1;
    };
}

// to use forEach in IE
if (!Array.prototype.forEach) {
    Array.prototype.forEach = function (fun /*, thisArg */) {
        "use strict";

        if (this === void 0 || this === null)
            throw new TypeError();

        var t = Object(this);
        var len = t.length >>> 0;
        if (typeof fun !== "function")
            throw new TypeError();

        var thisArg = arguments.length >= 2 ? arguments[1] : void 0;
        for (var i = 0; i < len; i++) {
            if (i in t)
                fun.call(thisArg, t[i], i, t);
        }
    };
}

// to use trim in IE
if (typeof String.prototype.trim !== 'function') {
    String.prototype.trim = function () {
        return this.replace(/^\s+|\s+$/g, '');
    }
}

// Return an Object that maps key to value
function getCookies() {
    var cookies = {};
    if (document.cookie == null || document.cookie == "")
        return cookies;

    var keyValueArray = document.cookie.split(";");
    for (var i = 0; i < keyValueArray.length; i++) {
        var s = keyValueArray[i];
        var indexOfEqual = s.indexOf("="); // Take the first '=' assuming key doesn't contain '='
        var key = s.substr(0, indexOfEqual).trim();
        var value = s.substr(indexOfEqual + 1);
        cookies[key] = value;
    }
    return cookies;
}

// cookies: an Object that maps key to value
// Note: document.cookie convention: key=value; [expires=Fri, 3 Aug 2001 20:47:11 UTC;] path=/
function setCookies(key, value, expireSec) {
    if (expireSec == null || expireSec == "")
        document.cookie = key + "=" + value + "; path=/";
    else {
        var date = new Date();
        date.setSeconds(date.getSeconds() + expireSec);
        var cookieString = key + "=" + value + "; expires=" + date.toGMTString() + "; path=/";
        document.cookie = cookieString;
    }
}

// Basically encode ';', ","
function encodeUrlForCookie(raw) {
    // %SC will replace semicolon
    // %CM will replace comma
    // %QT will replace quote
    return raw.replace(/;/g, '%SC').replace(/,/g, "%CM").replace(/"/g, "%QT");
}

function decodeUrlForCookie(encoded) {
    return encoded.replace(/%SC/g, ';').replace(/%CM/g, ',').replace(/%QT/g, '"');
}

// urls is an array
function encodeUrlsForCookie(urls) {
    var segments = [];
    for (var i = 0; i < urls.length; i++) {
        if (i > 0)
            segments.push(",");
        segments.push(encodeUrlForCookie(urls[i]));
    }
    return '"' + segments.join("") + '"';
}

function decodeUrlsForCookie(encoded) {
    var urls = encoded.substr(1, encoded.length - 2).split(",");
    for (var i = 0; i < urls.length; i++) {
        urls[i] = decodeUrlForCookie(urls[i]);
    }
    return urls;
}

function putUrlToCookie(url) {
    // Truncate url to ensure url in cookie is not too long
    var indexOfFirstSlash = url.indexOf('/');
    if (indexOfFirstSlash != -1) {
        var indexOfSecondSlash = url.indexOf('/', indexOfFirstSlash + 2);
        if (indexOfSecondSlash != -1) {
            url = url.substr(indexOfSecondSlash);
        } else {
            url = "/";
        }
    } // else no need to truncate the prefix

    // Truncate url up to certain length
    if (url.length > maxUrlLengthInCookie)
        url = url.substr(0, maxUrlLengthInCookie - 3) + "...";

    // Read
    var cookies = getCookies();

    // Modify -- retain 10 last URLs
    var urls;
    if (cookies['urls'] == null)
        urls = [];
    else
        urls = decodeUrlsForCookie(cookies['urls']);
    var newUrls = [];
    var min = Math.max(0, urls.length - maxNumUrlHistory + 1);
    if (min < urls.length) { // JS workaround!
        for (var i = min; i < urls.length; i++) {
            newUrls.push(urls[i]);
        }
    }
    newUrls.push(url);
    setCookies('urls', encodeUrlsForCookie(newUrls), 1800);
}

$(function () {
    // Put current URL into cookie
    putUrlToCookie(window.location.href);

    // make dialog position fixed when scrolled
    if ($.ui) {
        $.ui.dialog.prototype._oldinit = $.ui.dialog.prototype._init;
        $.ui.dialog.prototype._init = function () {
            $(this.element).parent().css('position', 'fixed');
            $(this.element).dialog("option", {
                resizeStop: function (event, ui) {
                    var position = [(Math.floor(ui.position.left) - $(window).scrollLeft()),
                                    (Math.floor(ui.position.top) - $(window).scrollTop())];
                    $(event.target).parent().css('position', 'fixed');
                    $(event.target).parent().dialog('option', 'position', position);
                    return true;
                }
            });
            this._oldinit();
        };
    }


    //=======================================================

    $('#loginPopup').click(function () {
        window.location = travelokaHost + "/login?referer=" + encodeURIComponent(window.location.href);
        return false;
    });
    $('#registerPopup').click(function () {
        window.location = travelokaHost + "/register";
        return false;
    });

    $('body').click(function (e) {
        if (!jQuery(e.target).is('.ui-dialog, a') && !jQuery(e.target).closest('.ui-dialog').length)
            closeAllDialogBox();
    });

    //set offset time between client and server
    serverTime.localOffset = new Date().getTime() - serverTime.millis;

});

// will get the server time
(function ($) {
    $.tvClock = $.tvClock || {};
    $.extend($.tvClock, {
        getCurrentTime: function () {
            return new Date().getTime() - serverTime.localOffset;
        },
        getDayOfWeek: function () {
            var d = new Date($.tvClock.getCurrentTime());
            return (d.getDay() + 6) % 7; //start with Monday == 0 - Sunday == 6
        },
        getMinuteOfDay: function () {
            var d = new Date($.tvClock.getCurrentTime());
            return d.getMinutes() * 1 + d.getHours() * 60;
        }

    });
})(jQuery);


//value is integer value
function toDelimitedCurrency(value, partitionSize, delimiter) {
    if (partitionSize == null) partitionSize = 3;
    if (delimiter == null) delimiter = ".";

    function toDelimitedCurrencyPositive(value, partitionSize, delimiter) {
        var sb = "";
        var s = value + "";
        for (var i = 0; i < s.length; ++i) {
            if ((s.length - i) % partitionSize == 0 && i > 0)
                sb += delimiter;
            sb += s.charAt(i);
        }
        return sb;
    }

    if (value < 0) return "-" + toDelimitedCurrencyPositive(value * -1, partitionSize, delimiter);
    else return toDelimitedCurrencyPositive(value, partitionSize, delimiter);
}

function toDelimitedCurrencyBold(value) {
    var s = toDelimitedCurrency(value, 3, ".");
    return "<span class='tvBold'>{0}</span>".format(s);
}

function isNumber(n) {
    return !isNaN(parseFloat(n)) && isFinite(n);
}

// all the functions below are related to login / register popup

var dialogList = [];

function closeAllDialogBox() {
    while (dialogList.length != 0) {
        var dialogBox = dialogList.pop();
        dialogBox.dialog('destroy').remove();
    }
}


function openPopupWindow(url, title, w, h) {
    var l = (screen.width / 2) - (w / 2),
        t = (screen.height / 2) - (h / 2);
    return window.open(url, title, 'width=' + w + ',height=' + h + ',top=' + t + ',left=' + l + ',scrollbars=yes');
}

function waitingProcess(id, msg) {
    $(id).attr('disabled', true);
    $(id).html(msg);
    $(id).addClass('disabled');
}

function releaseWaitingProcess(id, msg) {
    $(id).attr('disabled', false);
    $(id).html(msg);
    $(id).removeClass('disabled');
}

function displayNotification(msg) {
    $('#notificationBannerCntr').removeClass('tv-hidden');
    $('#notificationBanner').html(msg);
}

function displayError(id1, id2, err) {
    if (err == null || err == '') return;
    $(id1).addClass('fieldError');
    $(id2).html('<div class="errorIcon"></div><div class="errorText">' + err + '</div>');
    $(id2).removeClass('tv-hidden');
}

function clearError(id1, id2) {
    $(id1).removeClass('fieldError');
    $(id2).html('');
    $(id2).addClass('tv-hidden');
}

function internalServerError() {
    $.tvError.addErrorMsg({
        className: "tvError float",
        msg: "Terjadi Kesalahan Internal. Silakan coba lagi.",
        duration: 5000
    });
}

function preventPageCloseHook(message) {
    return function () {
        if (shouldPreventPageClose) {
            return message;
        } else {
            return null;
        }
    }
}

// Add native indexOf function that is not supported by IE < 9
// https://developer.mozilla.org/en/JavaScript/Reference/Global_Objects/Array/indexOf
if (!Array.prototype.indexOf) {
    Array.prototype.indexOf = function (searchElement, fromIndex) {
        var i,
            pivot = (fromIndex) ? fromIndex : 0,
            length;

        if (!this) {
            throw new TypeError();
        }

        length = this.length;

        if (length === 0 || pivot >= length) {
            return -1;
        }

        if (pivot < 0) {
            pivot = length - Math.abs(pivot);
        }

        for (i = pivot; i < length; i++) {
            if (this[i] === searchElement) {
                return i;
            }
        }
        return -1;
    };
}

Date.prototype.addHours = function (h) {
    this.setTime(this.getTime() + (h * 60 * 60 * 1000));
    return this;
}

Date.prototype.addMinutes = function (m) {
    this.setTime(this.getTime() + (h * 60 * 1000));
    return this;
}

/**
 * Returns true if the airportCode is part of an airportArea.
 *
 * @param string airportCode
 * @param array airportAreas
 * @return bool
 */
function isAirportBelongsToAirportArea(airportCode, airportAreas) {
    var i, j, airportArea;
    for (i in airportAreas) {
        airportArea = airportAreas[i];
        for (j in airportArea.airportIds) {
            if (airportArea.airportIds[j] == airportCode) {
                return true;
            }
        }
    }
    return false;
}

/**
 * @param string airportCode
 * @param array airportAreas
 * @return bool
 */
function isAirportArea(airportCode, airportAreas) {
    var i, j, airportArea;
    for (i in airportAreas) {
        airportArea = airportAreas[i];
        if (airportArea.airportAreaId == airportCode) {
            return true;
        }
    }
    return false;
}

function getAirportOrArea(airportCode, attribute) {
    var a = airports[airportCode] != null ? airports[airportCode] : airportAreas[airportCode];

    return a;
}

/**
 * Parses the DD-MM-YYYY string to javascript Date object.
 *
 * @param string string
 * @return Date|false
 */
function parseDayMonthYear(string) {
    var regex = /^([0-9]{2})-([0-9]{2})-([0-9]{4})$/;
    var matches = regex.exec(string);
    var year = parseInt(matches[3]);
    var month = parseInt(matches[2]) - 1;
    var day = parseInt(matches[1]);
    return new Date(year, month, day);
}

/**
 * Format the given int/string value into dd-MM-YY string used widely in the
 * server. Will return null if one of the values are empty. Will also add
 * padding to single digit day/month.
 *
 * @param int year
 * @param int month
 * @param int day
 * @return string|null
 */
window.Tv.formatMonthDayYear = function (day, month, year) {
    if (
      year == 0 || year == null ||
      month == 0 || month == null ||
      day == 0 || day == null
    ) {
        return null;
    }

    day = '0' + day;
    month = '0' + month;
    day = day.substr(-2);
    month = month.substr(-2);
    return day + '-' + month + '-' + year;
};