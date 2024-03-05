var RPCC = RPCC || {};

RPCC.clickListener = function (dotNetReference) {
    document.dotNetReference = dotNetReference;
    document.addEventListener("click", RPCC.pageClick);
}

RPCC.pageClick = function myFunction1(e) {
    //console.log("Clicked...");
    dnRef = e.currentTarget.dotNetReference;
    dnRef.invokeMethodAsync('TriggerClick');
}

RPCC.removeEventListener = () => {
    document.removeEventListener('click', RPCC.pageClick);
}

RPCC.scrollToView = () => {
    document.getElementById('mainDiv').scrollIntoView({ behavior: 'smooth' });
}

RPCC.offCanvas = () => {
    let myOffCanvas = document.getElementById('offcanvasMenu');

    const hideCanvas = () => {
        let openedCanvas = bootstrap.Offcanvas.getInstance(myOffCanvas);
        openedCanvas.hide();
    }

    hideCanvas();
}
