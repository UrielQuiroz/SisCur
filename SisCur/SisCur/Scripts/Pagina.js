
listar();

function listar() {
    $.get("/Pagina/listarPaginas", function (data) {
        crearListado(["ID", "Mensaje", "Accion", "Controlador"], data);
    })

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
        $.get("/Pagina/recuperarDatos/?id=" + id, function (data) {
            document.getElementById("txtIdPagina").value = data.IIDPAGINA;
            document.getElementById("txtMensaje").value = data.MENSAJE;
            document.getElementById("txtAccion").value = data.ACCION;
            document.getElementById("txtControlador").value = data.CONTROLADOR;

        });
    }
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




function Agregar() {
    if (CamposObligatorios() == true) {
        var frm = new FormData();
        var id = document.getElementById("txtIdPagina").value;
        var mensaje = document.getElementById("txtMensaje").value;
        var accion = document.getElementById("txtAccion").value;
        var controlador = document.getElementById("txtControlador").value;

        frm.append("IIDPAGINA", id);
        frm.append("MENSAJE", mensaje);
        frm.append("ACCION", accion);
        frm.append("CONTROLADOR", controlador);
        frm.append("BHABILITADO", 1);



        const swalWithBootstrapButtons = Swal.mixin({
            customClass: {
                confirmButton: 'btn btn-success',
                cancelButton: 'btn btn-danger'
            },
            buttonsStyling: false
        })

        swalWithBootstrapButtons.fire({
            title: 'Esta seguro?', text: "Desea guardar el registro!", icon: 'question', showCancelButton: true, confirmButtonText: 'Si', cancelButtonText: 'No', reverseButtons: true
        })
            .then((result) => {
                if (result.value) {

                    $.ajax({
                        type: "POST",
                        url: "/Pagina/guardar",
                        data: frm,
                        contentType: false,
                        processData: false,
                        success: function (data) {
                            if (data == 1) {
                                listar();
                                swalWithBootstrapButtons.fire('Informacion almacenada!', '', 'success')
                                document.getElementById("btnCancelat").click();
                            }
                            else {
                                if (data == -1) {
                                    Swal.fire({ icon: 'info', title: 'El docente ya existe...', text: 'Verifique!' })
                                }
                                else {
                                    Swal.fire({ icon: 'alert', title: 'Oops...', text: 'Ocurrio un error!' })
                                }
                            }
                        }
                    })




                } else if (result.dismiss === Swal.DismissReason.cancel) {
                    swalWithBootstrapButtons.fire('Operacion cancelada', '', 'error')
                }
            })


    }
}