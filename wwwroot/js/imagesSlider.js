/**
* @param {HTMLElement} pointer
*/
function slide(pointer, direction) {
    const elments = pointer.parentElement.querySelectorAll(".carousel-item");
    let i = 0;
    for (const item of elments) {
        console.log(i);
        if (item.classList.contains("active")) {
            item.classList.remove("active");
            if (direction === "next") {
                elments[(i + 1) % elments.length].classList.add("active");
                return;
            }
            else if (direction === "previous") {
                let ii = i === 0 ? elments.length : i;
                elments[(ii - 1) % elments.length].classList.add("active");
                return;
            }
        }
        i++;
    }
}