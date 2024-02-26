var RPCC = RPCC || {};

RPCC.closeOffcanvas = function (offcanvasId) {
    var offcanvas = RPCC.controlCollection[offcanvasId];
    if (offcanvas) {
        offcanvas.hide();
    }
}

RPCC.clickListener = function (dotNetReference) {
    document.dotNetReference = dotNetReference;
    document.addEventListener("click", RPCC.pageClick);
}

RPCC.pageClick = function myFunction1(e) {
    //console.log("Clicked...");
    dn = e.currentTarget.dotNetReference;
    dn.invokeMethodAsync('TriggerClick');
}

RPCC.removeEventListener = () => {
    document.removeEventListener('click', RPCC.pageClick);
}

//def: function test(dotNetReference) {
//    document.dotNetRef = dotNetReference;
//    document.addEventListener('keyup', abc)
//}

//abc: function myFunction1(e) {
//    dn = e.currentTarget.dotNetRef;
//    // Calls a method by name with the [JSInokable] attribute (above)

//    if (e.key == 7) {
//        dn.invokeMethodAsync('HandleMouseClick');
//    }
//    if (e.key == 8) {
//        dn.invokeMethodAsync('HandleMouseClick');
//    }
//    if (e.key == 3) {
//        dn.invokeMethodAsync('HandleMouseClick');
//    }

//}
