$(".flipped").hide();
$(".flipper").click(function () {
    $(".flipped#" + this.id).slideToggle("medium");
    if (this.innerHTML == "Show Less")
        this.innerHTML = "Show More";
    else
        this.innerHTML = "Show Less";
});