//// angular service
// app.service('dateTimeService', function() {


/* translate month from number to formatted month
 * if startNumb is undefined, assumed monthNumb 0 = January
 */	
//// new ES6 function syntax, use this instead when ES6 is fully supported in IE and Safari Mobile
// function translateMonth (monthNumb, format = "Mmm", startNumb = 0) {
function translateMonth (monthNumb, format, startNumb) {
	//// Will be removed later after ES6
	startNumb = (typeof startNumb !== 'undefined') ? startNumb : 0;
	format = (typeof format !== 'undefined') ? format : "Mmm";
	
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
