$(document).ready(function (e) {
    $('#page_nav_mobile #page_menu').click(function (e) {
        e.preventDefault();
        if ($('#page_menus_mobile').css('display') == 'none') {
            $('#page_menus_mobile').slideDown();
        } else {
            $('#page_menus_mobile').slideUp();
        }
    });

    $('#page_menus_mobile h4').click(function (e) {
        if ($(this).next('ul').css('display') == 'none') {
            $(this).next('ul').slideDown();
            $('i', $(this)).removeClass('fa-plus-square-o');
            $('i', $(this)).addClass('fa-minus-square-o');
            $(this).addClass('page_nav_selected');
        } else {
            $(this).next('ul').slideUp();
            $('i', $(this)).addClass('fa-plus-square-o');
            $('i', $(this)).removeClass('fa-minus-square-o');
            $(this).removeClass('page_nav_selected');
        }
    });
    $('#contact_us_mobile').html($('#address').html());
});