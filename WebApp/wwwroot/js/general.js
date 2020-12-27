export function setProperty(element, key, value) {
    element[key] = value;
}

export function getProperty(element, key) {
    return element[key];
}

export function startAnimation(element, animationClass, timeout = 1000) {

    // Blazor in server mode sometimes renders a component instance twice.
    // This causes animations to get visibly interrupted, though not when
    // running on localhost. This check below - to see if an animation is 
    // already in progress - seems to fix the issue.
    if (element==null || element.classList.contains(animationClass)) return;

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