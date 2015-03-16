$(".adder").click(function () {
    if (this.id == "sequence") {
        $.ajax({
            url: this.href,
            cache: false,
            success: function(html) {
                $(".added-set#sequence").append(html);
            },
            error: function() {
                this.innerHTML = "ah";
            }
        });
    }
    return false;
});