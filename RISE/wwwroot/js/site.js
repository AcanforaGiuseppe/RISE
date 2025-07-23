// Optional JS for future custom interactions

// Example: automatic carousel interval override (Bootstrap 5)
var newsCarousel = document.querySelector('#newsCarousel');
if (newsCarousel) {
    var carousel = new bootstrap.Carousel(newsCarousel, {
        interval: 5000,  // 5 seconds
        ride: 'carousel'
    });
}