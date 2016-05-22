window.requestAnimationFrame = window.requestAnimationFrame
    || window.mozRequestAnimationFrame
    || window.webkitRequestAnimationFrame
    || window.msRequestAnimationFrame
    || function (f) { setTimeout(f, 1000 / 60) };

var pageTitle = $('#page-title');
var viewportHeight = window.innerHeight * 0.50;

function parallax() {
    var scrolltop = window.pageYOffset;
    pageTitle.css('padding-top', (viewportHeight - scrolltop * .35)); //35% bitch!
}

window.addEventListener('scroll', function () {
    requestAnimationFrame(parallax);
}, false);