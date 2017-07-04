app.service('dateTimeService', function() {
    
	// translate month from number to formatted month
    this.translateMonth = function (monthNumb, startNumb, format) {
    	//// if startNumb is undefined, assumed index 0 = January
    	startNumb = (typeof startNumb !== 'undefined') ? startNumb : 0;
	    monthNumb += startNumb;
	    var mmm = ["Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec"];
	    return mmm[monthNumb];
    }

    // this.countDate = function (x) {
    //     return x;
    // }
});