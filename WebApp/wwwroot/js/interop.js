var ElixMenuButton = ElixMenuButton || {};

// Attaches an event listener to the "close" event on an elix-menu-button. When
// the user selects an item from the menu, the supplied method is called with
// the tag of the selected item.
ElixMenuButton.addCloseEventListener = function (element, assemblyName, method, helper) {
    element.addEventListener("close", (event) => {
        const closeResult = event.detail.closeResult;
        if (closeResult && !closeResult.canceled) {
            helper.invokeMethodAsync(assemblyName, method, closeResult.id);
        }
    })
};

// Attaches an event listener to the openedchange event on an elix-menu-button.
// When the menu is opened or closed, the supplied method is called indicating
// whether the menu is opened or closed.
ElixMenuButton.addOpenedChangeEventListener = function (element, assemblyName, method, helper) {
    // Occasionally the method is called on a null element. I don't know why.
    if (element == null)
        return;
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