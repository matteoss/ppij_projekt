$(document).ready(function () {
    elementFade();

    $(window).scroll(function () {
        elementFade();
    });

});

var elementFade = function () {
    $('.fade-in-element').each(function (i) {

        var middle_of_object = $(this).offset().top + $(this).outerHeight() / 2;
        var bottom_of_window = $(window).scrollTop() + $(window).height();

        if (bottom_of_window > middle_of_object) {

            $(this).animate({ 'opacity': '1' }, 600);

        }

    });
}