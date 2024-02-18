var RPCC = RPCC || {};

RPCC.closeOffcanvas = function (offcanvasId) {
    var offcanvas = RPCC.controlCollection[offcanvasId];
    if (offcanvas) {
        offcanvas.hide();
    }
}