/* Indonesian initialisation for the jQuery UI date picker plugin. */
/* Written by Deden Fathurahman (dedenf@gmail.com). */
( function( factory ) {
	if ( typeof define === "function" && define.amd ) {

		// AMD. Register as an anonymous module.
		define( [ "../widgets/datepicker" ], factory );
	} else {

		// Browser globals
		factory( jQuery.datepicker );
	}
}( function( datepicker ) {

datepicker.regional.id = {
	closeText: "Tutup",
	prevText: "&#x3C;Mundur",
	nextText: "Maju&#x3E;",
	currentText: "Hari Ini",
	monthNames: [ "Januari","Februari","Maret","April","Mei","Juni",
	"Juli","Agustus","September","Oktober","November","Desember" ],
	monthNamesShort: [ "Jan","Feb","Mar","Apr","Mei","Jun",
	"Jul","Agu","Sep","Okt","Nov","Des" ],
	dayNames: [ "Minggu","Senin","Selasa","Rabu","Kamis","Jumat","Sabtu" ],
	dayNamesShort: [ "Min","Sen","Sel","Rab","Kam","Jum","Sab" ],
	dayNamesMin: [ "M","S","S","R","K","J","S" ],
	weekHeader: "Mg",
	dateFormat: "dd/mm/yy",
	firstDay: 0,
	isRTL: false,
	showMonthAfterYear: false,
	yearSuffix: "" };
datepicker.setDefaults( datepicker.regional.id );

return datepicker.regional.id;

} ) );