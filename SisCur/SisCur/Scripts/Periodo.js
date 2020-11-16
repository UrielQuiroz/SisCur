

$("#datepickerInicio").datepicker({
    dateFormat: "dd/mm/yy",
    changeMonth: true,
    changeYear: true
});
$("#datepickerFin").datepicker({
    dateFormat: "dd/mm/yy",
    changeMonth: true,
    changeYear: true
});


listar();

function listar() {
    $.get("Periodo/listar", function (data) {
        crearListado(["Id", "Nombre", "Fecha Inicio", "Fecha Fin"], data);
    })
}



var nombrePeriodo = document.getElementById("txtNombre");
nombrePeriodo.onkeyup = function () {

    var nombre = document.getElementById("txtNombre").value;  //Se obtiene lo que el usuario textea
    $.get("Periodo/buscarPorNombre/?nombre=" + nombre, function (data) {
        crearListado(["Id", "Nombre", "Fecha Inicio", "Fecha Fin"], data);
    })

}

function crearListado(arrayColumnas, data) {
    var contenido = "";
    contenido += "<table id='tablaGeneral' class='table table-hover'>";
    contenido += "<thead class='bg-info text-white font-weight-bold'>";

    contenido += "<tr>";

    for (var i = 0; i < arrayColumnas.length; i++) {
        contenido += "<td>";
        contenido += arrayColumnas[i];
        contenido += "</td>";
    }

    contenido += "<td class='text-center'>Acciones</td>";
    contenido += "</tr>";

    contenido += "</thead>";
    var llaves = Object.keys(data[0]);   //Obtiene un arreglo de Json
    contenido += "<tbody>"

    for (var i = 0; i < data.length; i++) {
        contenido += "<tr>";

        for (var j = 0; j < llaves.length; j++) {
            var valorLlaves = llaves[j];
            contenido += "<td>"
            contenido += data[i][valorLlaves];
            contenido += "</td>"
        }

        contenido += "<td class='text-center'>";
        contenido += "<button class='btn btn-success' onclick='abrirModal(" + data[i].IIDPERIODO +")' data-toggle='modal' data-target='#exampleModal'> <i class='glyphicon glyphicon-edit'></i> </button> "
        contenido += "<button class='btn btn-danger' onclick='Eliminar(" + data[i].IIDPERIODO +")' ><i class='glyphicon glyphicon-trash'></i></button>"
        contenido += "</td>";
        contenido += "</tr>";
    }

    contenido += "</tbody>"

    contenido += "</table>";

    document.getElementById("tabla").innerHTML = contenido;
    $('#tablaGeneral').dataTable({
        "searching": false,
        destroy: true,
        responsive: true,
        dom: "lfrtip",
        lengthMenu: [[5, 10, 15, 20, 25, 50, -1], [5, 10, 15, 20, 25, 50, "Todos"]],
        scrollX: true,
    });
}



function Agregar() {
    if (CamposObligatorios() == true) {
        var frm = new FormData();
        var id = document.getElementById("txtIdPeriodo").value;
        var nombre = document.getElementById("txtNombrePeriodo").value;
        var fechainicio = document.getElementById("datepickerInicio").value;
        var fechafin = document.getElementById("datepickerFin").value;

        frm.append("IIDPERIODO", id);
        frm.append("NOMBRE", nombre);
        frm.append("FECHAINICIO", fechainicio);
        frm.append("FECHAFIN", fechafin);
        frm.append("BHABILITADO", 1);

        if (confirm("¿Desea gurdar el registro?") == 1) {
            $.ajax({
                type: "POST",
                url: "Periodo/Guardar",
                data: frm,
                contentType: false,
                processData: false,
                success: function (data) {
                    if (data == 1) {
                        listar();
                        alert("Se ha guardado el registro!");
                        document.getElementById("btnCancelat").click();
                    }
                    else {
                        if (data == -1) {
                            alert("Ya existe el periodo");
                        }
                        else {
                            alert("Ocurrio un error!")
                        }
                    }
                }
            })
        }
    }
}



function limpiarCampo() {
    var campos = document.getElementsByClassName("limpiar");
    var numeroDeCampos = campos.length;
    for (var i = 0; i < numeroDeCampos; i++) {
        campos[i].value = "";
    }
}


function Eliminar(id) {
    var frm = new FormData();
    frm.append("IIDPERIODO", id);
    if (confirm("¿Desea eliminar el registro?") == 1) {
        $.ajax({
            type: "POST",
            url: "Periodo/eliminar",
            data: frm,
            contentType: false,
            processData: false,
            success: function (data) {
                if (data != 0) {
                    listar();
                    alert("Se ha eliminado el registro!");
                    document.getElementById("btnCancelat").click();
                }
                else {
                    alert("Ocurrio un error!");
                }
            }
        })
    }
}



function CamposObligatorios() {

    var exito = true;

    var camposObligatorios = document.getElementsByClassName("obligatorio");
    var numeroDeCampos = camposObligatorios.length;

    for (var i = 0; i < numeroDeCampos; i++) {
        if (camposObligatorios[i].value == "") {
            exito = false;
            camposObligatorios[i].parentNode.classList.add("error");
        }
        else {
            camposObligatorios[i].parentNode.classList.remove("error");
        }
    }

    return exito;

}




function abrirModal(id) {

    var camposObligatorios = document.getElementsByClassName("obligatorio");
    var numeroDeCampos = camposObligatorios.length;


    for (var i = 0; i < numeroDeCampos; i++) {
        camposObligatorios[i].parentNode.classList.remove("error");
    }

    if (id == 0) {
        limpiarCampo();
    } else {
        $.get("Periodo/recuperarDatos/?id=" + id, function (data) {
            document.getElementById("txtIdPeriodo").value = data[0].IIDPERIODO;
            document.getElementById("txtNombrePeriodo").value = data[0].NOMBRE;
            document.getElementById("datepickerInicio").value = data[0].fi;
            document.getElementById("datepickerFin").value = data[0].ff;

        });
    }
}


