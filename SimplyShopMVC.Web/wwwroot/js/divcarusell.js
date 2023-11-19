document.addEventListener("DOMContentLoaded", function () {
    const scrollContainer = document.querySelector(".scroll-container");
    const scrollContent = document.querySelector(".scroll-content");
    const leftArrow = document.querySelector(".arrow.left");
    const rightArrow = document.querySelector(".arrow.right");

    leftArrow.addEventListener("click", function () {
        scrollContainer.scrollLeft -= 200;
    });

    rightArrow.addEventListener("click", function () {
        scrollContainer.scrollLeft += 200;
    });
});