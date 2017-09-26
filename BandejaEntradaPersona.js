var txtNombre = null;
var txtId = null;

$(document).ready(function () {

    txtNombre = $("[id*='txtNombre']");
}
);

function validarBuscar() {

    debugger;

    cargando();
    mensajesInvisibles();

    if(txtNombre == null)
    {
        showError("Debe Ingresar un nombre")
    }

}

function limpiarFiltros() {

    txtNombre.val("");
    return false;
}


function showError(mensaje) {
    divError.show();
    divCargando.css("display", "none");
    lblResultadoError.text("Campos Obligatorios. " + mensaje);
    return;
}

function mensajesInvisibles() {

    divError.hide();
    divResultado.hide();

}

function cargando() {
    divCargando.css("display", "inline");
    return true;
}