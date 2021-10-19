var $star_rating = $('.star-rating .fa');

var SetRatingStar = function ($star_rating_parent) {
    return $star_rating_parent.find('.fa').each(function () {
        if (parseInt($star_rating_parent.find('input.rating-value').val()) >= parseInt($(this).data('rating'))) {
            return $(this).removeClass('fa-star-o').addClass('fa-star');
        } else {
            return $(this).removeClass('fa-star').addClass('fa-star-o');
        }
    });
};

$star_rating.on('click', function () {
    $(this).siblings('input.rating-value').val($(this).data('rating'));
    return SetRatingStar($(this).parent());
});

$star_rating.on('click', function () {
    var $count = $(this).closest(".review").find('.fa-star').length;
    $(this).closest(".figure").find('.count_place').text($count);
});