let listener = undefined;

export function addBodyKeyListener(dotnet) {
    if (listener !== undefined) {
        console.error('only one DialogControllerProvider should be attached');
    }
    listener = (e) => {
        if (e.key !== "Escape") return;
        e.stopPropagation();
        dotnet.invokeMethodAsync('OnDocumentKeyDownEscape');
    };
    document.body.addEventListener("keydown", listener);
}

export function removeBodyKeyListener(id) {
    if (!listener) {
        console.warn("listener was not set");
        return;
    }
    document.body.removeEventListener("keydown", listener);
}
