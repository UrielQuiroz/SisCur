

listar();

function listar() {
    $.get("GradoSeccion/ListarGradoSeccion", function (data) {
        crearListado(["ID GS", "Grado", "Sección"], data);
    })


    $.get("GradoSeccion/listarSeccion", function (data) {
        llenarCombo(data, document.getElementById("cboSeccion"), true)
    })

    $.get("GradoSeccion/listarGrado", function (data) {
        llenarCombo(data, document.getElementById("cboGrado"), true)
    })
}


function llenarCombo(data, control, primerElemento) {
    var contenido = "";

    if (primerElemento == true) {
        contenido += "<option value=''>---SELECCIONE---</option>"
    }

    for (var i = 0; i < data.length; i++) {
        contenido += "<option value='" + data[i].IID + "'>";
        contenido += data[i].NOMBRE;
        contenido += "</option>";
    }

    control.innerHTML = contenido;
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

        var llaveID = llaves[0];

        contenido += "<td class='text-center'>";
        contenido += "<button class='btn btn-success' onclick='abrirModal(" + data[i][llaveID] + ")' data-toggle='modal' data-target='#exampleModal'> <i class='glyphicon glyphicon-edit'></i> </button> "
        contenido += "<button class='btn btn-danger' onclick='Eliminar(" + data[i][llaveID] + ")'><i class='glyphicon glyphicon-trash'></i></button>"
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




function abrirModal(id) {

    var camposObligatorios = document.getElementsByClassName("obligatorio");
    var numeroDeCampos = camposObligatorios.length;


    for (var i = 0; i < numeroDeCampos; i++) {
        camposObligatorios[i].parentNode.classList.remove("error");
    }


    if (id == 0) {
        limpiarCampo();
    } else {
        $.get("GradoSeccion/recuperarDatos/?id=" + id, function (data) {
            document.getElementById("txtIdGradoSeccion").value = data[0].IID;
            document.getElementById("cboGrado").value = data[0].IIDGRADO;
            document.getElementById("cboSeccion").value = data[0].IIDSECCION;

        });
    }
}





function Agregar() {
    if (CamposObligatorios() == true) {
        var frm = new FormData();
        var id = document.getElementById("txtIdGradoSeccion").value;
        var idGrado = document.getElementById("cboGrado").value;
        var idSeccion = document.getElementById("cboSeccion").value;

        frm.append("IID", id);
        frm.append("IIDGRADO", idGrado);
        frm.append("IIDSECCION", idSeccion);
        frm.append("BHABILITADO", 1);

        if (confirm("¿Desea gurdar el registro?") == 1) {
            $.ajax({
                type: "POST",
                url: "GradoSeccion/guardar",
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
                            alert("Ya exixte el registro!");
                        }
                        else {
                            alert("Ocurrio un error!");
                        }
                    }
                }
            })
        }
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



function limpiarCampo() {
    var campos = document.getElementsByClassName("limpiar");
    var numeroDeCampos = campos.length;
    for (var i = 0; i < numeroDeCampos; i++) {
        campos[i].value = "";
    }
}



function Eliminar(id) {
    if (confirm("¿Desea eliminar el registro?") == 1) {
        $.get("GradoSeccion/eliminar/?id=" + id, function (data) {
            if (data == 0) {
                alert("Error");
            }
            else {
                alert("Exito");
                listar();
            }
        })
    }
}