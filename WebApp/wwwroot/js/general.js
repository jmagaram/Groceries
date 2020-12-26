export function setProperty(element, key, value) {
    element[key] = value;
}

export function getProperty(element, key) {
    return element[key];
}

export function startAnimation(element, animationClass, timeout = 1000) {
    element.classList.remove(animationClass);
    void element.offsetWidth; // https://bit.ly/3pilQim
    const stopAnimation = function () {
        if (element.classList.contains(animationClass)) {
            element.classList.remove(animationClass);
            element.removeEventListener("animationend", stopAnimation);
            element.removeEventListener("animationcancel", stopAnimation);
        }
    }
    element.addEventListener("animationend", stopAnimation);
    element.addEventListener("animationcancel", stopAnimation);
    element.classList.add(animationClass);
    setTimeout(stopAnimation, timeout);
}