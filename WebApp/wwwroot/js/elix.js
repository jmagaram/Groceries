export function OpenCloseMixinAddOpenedChangeEventListener(element, assemblyName, method, helper) {
    if (element == null) return;
    element.addEventListener("openedchange", (event) => {
        var isOpen = event.detail.opened;
        helper.invokeMethodAsync(assemblyName, method, isOpen);
    })
};