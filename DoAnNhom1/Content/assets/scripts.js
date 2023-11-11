$(window).scroll(function () {
    if ($(this).scrollTop() > 0) {
        $('header').addClass('active');
    } else {
        $('header').removeClass('active');
    }
});

