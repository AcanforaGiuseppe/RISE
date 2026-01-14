document.addEventListener('DOMContentLoaded', function () {

    // NEWS CAROUSEL
    const newsCarousel = document.querySelector('#newsCarousel');
    if (newsCarousel) {
        new bootstrap.Carousel(newsCarousel, {
            interval: 5000,
            ride: 'carousel'
        });
    }

    // RESULTS MODAL
    const modal = document.getElementById('resultsModal');
    if (!modal) return;

    modal.addEventListener('show.bs.modal', function (event) {

        const button = event.relatedTarget;
        if (!button) return;

        const title = button.getAttribute('data-title');
        const row = button.closest('tr');
        if (!row) return;

        const resultsDiv = row.querySelector('.competition-results');

        document.getElementById('resultsModalTitle').innerText = title;
        document.getElementById('resultsModalBody').innerHTML =
            resultsDiv ? resultsDiv.innerHTML : '<em>No results available</em>';
    });

});
