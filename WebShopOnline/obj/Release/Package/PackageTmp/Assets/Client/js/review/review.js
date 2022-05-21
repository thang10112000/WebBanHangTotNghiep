$(function () {
    $("#rateYo").rateYo({
        rating: 0,
        numStars: 5,
        maxValue: 5,
        halfStar: true,
        onChange: function (rating, rateYoInstance) {
            $('#rating').val(rating);
        }
    });
});