var HtmlElement = HtmlElement || {};

HtmlElement.setProperty = function (element, k, v) {
    element[k] = v;
}

HtmlElement.setPropertyById = function (id, k, v) {
    var element = document.getElementById(id);
    element[k] = v;
}

HtmlElement.getProperty = function (element, k) {
    return element[k];
}

HtmlElement.scrollIntoView = function (element, offset) {
    // Seeing if the delay stops intermittent crashing when navigating to a
    // specific category
    setTimeout(function () {
        element.scrollIntoView();
        if (offset != null) {
            var scrolledY = window.scrollY;
            if (scrolledY) {
                window.scroll(0, scrolledY - offset);
            }
        }
    }, 250);
}

window.interopFunctions =
{
    focusElement: function (element) {
        element.focus();
    }
};