function updateCountdown(endTime,selector) {
    const now = new Date();
    const diff = endTime - now;

    if (diff <= 0) {
        document.querySelector(`.${selector}>.details>.row>.countdown-container>.countdown-display`).innerHTML = "EXPIRED!";
        return;
    }

    const days = Math.floor(diff / (1000 * 60 * 60 * 24));
    const hours = Math.floor((diff % (1000 * 60 * 60 * 24)) / (1000 * 60 * 60));
    const minutes = Math.floor((diff % (1000 * 60 * 60)) / (1000 * 60));
    const seconds = Math.floor((diff % (1000 * 60)) / 1000);

    document.querySelector(`.${selector}>.details>.row>.countdown-container>.countdown-display>.days`).textContent = days.toString().padStart(2, '0');
    document.querySelector(`.${selector}>.details>.row>.countdown-container>.countdown-display>.hours`).textContent = hours.toString().padStart(2, '0');
    document.querySelector(`.${selector}>.details>.row>.countdown-container>.countdown-display>.minutes`).textContent = minutes.toString().padStart(2, '0');
    document.querySelector(`.${selector}>.details>.row>.countdown-container>.countdown-display>.seconds`).textContent = seconds.toString().padStart(2, '0');
}