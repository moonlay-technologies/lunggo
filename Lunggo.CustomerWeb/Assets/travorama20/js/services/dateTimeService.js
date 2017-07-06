//// angular service
// app.service('dateTimeService', function() {

//// translate month from number to formatted month
function translateMonth (monthNumb, format = "Mmm", startNumb = 0) {
	//// if startNumb is undefined, assumed index 0 = January
	
	// just in case the browser dont support ES6 default parameter feature. this will be removed later
	startNumb = (typeof startNumb !== 'undefined') ? startNumb : 0;
	
	monthNumb = parseInt(monthNumb);
	monthNumb += startNumb;
    var monthString = {
    	Mmm : ["Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec"],
    	mmm : ["jan", "feb", "mar", "apr", "may", "jun", "jul", "aug", "sep", "oct", "nov", "dec"],
    	Bln : ["Jan", "Feb", "Mar", "Apr", "Mei", "Jun", "Jul", "Ags", "Sep", "Okt", "Nov", "Des"],
    	Month : ["January", "February", "March", "April", "May", "June", "July", "August", "September", "October", "November", "December"],
    	Bulan : ["Januari", "Februari", "Maret", "April", "Mei", "Juni", "Juli", "Agustus", "September", "Oktober", "November", "Desember"],
	};
    return monthString[format][monthNumb];
}
