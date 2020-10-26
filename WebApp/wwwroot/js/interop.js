var ElixMenuButton = ElixMenuButton || {};

// Attaches an event listener to the "close" event on an elix-menu-button
// element. When the user selects an item from the menu, the supplied method is
// called with the tag of the selected item.
ElixMenuButton.addCloseEventListener = function (element, assemblyName, method, helper) {
    element.addEventListener("close", (event) => {
        const closeResult = event.detail.closeResult;
        if (closeResult && !closeResult.canceled) {
            helper.invokeMethodAsync(assemblyName, method, closeResult.id);
        }
    })
};

ElixMenuButton.addOpenChangedEventListener = function (element, assemblyName, method, helper) {
    element.addEventListener("openedchange", (event) => {
        const isOpened = event.currentTarget.opened;
        helper.invokeMethodAsync(assemblyName, method, isOpened);
    })
};

window.interopFunctions =
{
    focusElement: function (element) {
        element.focus();
    }
};