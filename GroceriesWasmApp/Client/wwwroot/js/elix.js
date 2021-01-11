// Attaches an event handler to components based on the OpenCloseMixIn
// and returns a boolean when the component is opened (true) or closed (false).
export function OpenCloseMixinAddOpenedChangeEventListener(element, assemblyName, method, helper) {
    if (element == null) return;
    element.addEventListener("openedchange", (event) => {
        var isOpen = event.detail.opened;
        helper.invokeMethodAsync(assemblyName, method, isOpen);
    })
};

// Attaches an event handler to the close event of a menu button and
// returns the element ID of the selected menu item.
export function MenuButtonAddCloseEventListener(element, assemblyName, method, helper) {
    element.addEventListener("close", (event) => {
        const closeResult = event.detail.closeResult;
        if (closeResult && !closeResult.canceled) {
            helper.invokeMethodAsync(assemblyName, method, closeResult.id);
        }
    })
};