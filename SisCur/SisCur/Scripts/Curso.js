

listar();

function listar() {
    $.get("/Curso/listar", function (data) {
        crearListado(["Id", "Nombre", "Descripciòn"],data);
    });
}



var btnBuscar = document.getElementById("btnBuscar");

btnBuscar.onclick = function () {

    var nombre = document.getElementById("txtNombre").value;
    $.get("Curso/buscarPorNombre/?nombre=" + nombre, function (data) {
        crearListado(["Id", "Nombre", "Descripciòn"], data);
    });
}


var btnLimpiar = document.getElementById("btnLimpiar");
btnLimpiar.onclick = function () {
    $.get("Curso/listar", function (data) {

        crearListado(["Id", "Nombre", "Descripciòn"], data);
    });

    document.getElementById("txtNombre").value = "";
}


//function crearListado(data) {
//    var contenido = "";
//    contenido += "<table id='tablaCurso' class='table'>";
//    contenido += "<thead>";

//    contenido += "<tr>";
//    contenido += "<td>ID Curso</td>";
//    contenido += "<td>Nombre</td>";
//    contenido += "<td>Descripcion</td>";
//    contenido += "<td class='text-center'>Acciones</td>";
//    contenido += "</tr>";

//    contenido += "</thead>";

//    contenido += "<tbody>"

//    for (var i = 0; i < data.length; i++) {
//        contenido += "<tr>";
//        contenido += "<td>" + data[i].IIDCURSO + "</td>";
//        contenido += "<td>" + data[i].NOMBRE + "</td>";
//        contenido += "<td>" + data[i].DESCRIPCION + "</td>";

//        contenido += "<td class='text-center'>";
//        contenido += "<button class='btn btn-success' onclick='abrirModal(" + data[i].IIDCURSO +")' data-toggle='modal' data-target='#exampleModal'> <i class='glyphicon glyphicon-edit'></i> </button> "
//        contenido += "<button class='btn btn-danger' onclick='Eliminar(" + data[i].IIDCURSO +")'><i class='glyphicon glyphicon-trash'></i></button>"
//        contenido += "</td>";

//        contenido += "</tr>";
//    }

//    contenido += "</tbody>"

//    contenido += "</table>";

//    document.getElementById("tabla").innerHTML = contenido;
//    $('#tablaCurso').dataTable({
//        "searching": false
//    });
//}



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
        contenido += "<button class='btn btn-success' onclick='abrirModal(" + data[i].IIDCURSO +")' data-toggle='modal' data-target='#exampleModal'> <i class='glyphicon glyphicon-edit'></i> </button> "
        contenido += "<button class='btn btn-danger' onclick='Eliminar(" + data[i].IIDCURSO +")'><i class='glyphicon glyphicon-trash'></i></button>"
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
        $.get("Curso/recuperarDatos/?id=" + id, function (data) {
            document.getElementById("txtIdCurso").value = data[0].IIDCURSO;
            document.getElementById("txtNombreCurso").value = data[0].NOMBRE;
            document.getElementById("txtDescripcion").value = data[0].DESCRIPCION;

        });
    }
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
        var id = document.getElementById("txtIdCurso").value;
        var nombre = document.getElementById("txtNombreCurso").value;
        var descripcion = document.getElementById("txtDescripcion").value;
        frm.append("IIDCURSO", id);
        frm.append("NOMBRE", nombre);
        frm.append("DESCRIPCION", descripcion);
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
                        url: "Curso/Guardar",
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

        //if (confirm("¿Desea gurdar el registro?") == 1) {
        //    $.ajax({
        //        type: "POST",
        //        url: "Curso/Guardar",
        //        data: frm,
        //        contentType: false,
        //        processData: false,
        //        success: function (data) {
        //            if (data == 1) {
        //                listar();
        //                alert("Se ha guardado el registro!");
        //                document.getElementById("btnCancelat").click();
        //            }
        //            else {
        //                if (data == -1) {
        //                    alert("Ya existe el curso");
        //                }
        //                else {
        //                    alert("Ocurrio un error!")
        //                }
        //            }
        //        }
        //    })
        //}
    }
}


function Eliminar(id) {
    var frm = new FormData();
    frm.append("IIDCURSO", id);
    if (confirm("¿Desea eliminar el registro?") == 1) {
        $.ajax({
            type: "POST",
            url: "Curso/eliminar",
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