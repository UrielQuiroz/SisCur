
$("#dtpFechaContrato").datepicker({
    dateFormat: "dd/mm/yy",
    changeMonth: true,
    changeYear: true
});


listar();
listarComboModalidad();

function listar() {
    $.get("Docente/listar", function (data) {
        crearListado(["Id", "Nombre", "Apellido Paterno", "Apellido Materno", "E-mail"], data)
    })
}

var cboModalidad = document.getElementById("cboModalidad");
cboModalidad.onchange = function (data) {

    var idmodalidad = document.getElementById("cboModalidad").value;

    if (idmodalidad == "") {
        listar();
    }
    else {
        $.get("Docente/filtrarPorModalidad/?idModalidad=" + idmodalidad, function (data) {
            crearListado(["Id", "Nombre", "Apellido Paterno", "Apellido Materno", "E-mail"], data)
        })
    }
}


$.get("Alumno/listarSexo", function (data) {
    llenarCombo(data, document.getElementById("cboSexoPopup"), true)
})



function listarComboModalidad() {
    $.get("Docente/listarPorModalidadContrato", function (data) {
        llenarCombo(data, document.getElementById("cboModalidad"), true);
        llenarCombo(data, document.getElementById("cboModalidadPopup"), true);
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
        contenido += "<button class='btn btn-danger' onclick='Eliminar(" + data[i].IIDDOCENTE + ")'><i class='glyphicon glyphicon-trash'></i></button>"
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
    //alert(id);
    var camposObligatorios = document.getElementsByClassName("obligatorio");
    var numeroDeCampos = camposObligatorios.length;


    for (var i = 0; i < numeroDeCampos; i++) {
        camposObligatorios[i].parentNode.classList.remove("error");
    }


    if (id == 0)
    {
        limpiarCampo();
    }
    else
    {
        $.get("Docente/recuperarDatos/?id=" + id, function (data) {
            document.getElementById("txtIdDocente").value = data[0].IIDDOCENTE;
            document.getElementById("txtNombre").value = data[0].NOMBRE;
            document.getElementById("txtAPaterno").value = data[0].APPATERNO;
            document.getElementById("txtAMAterno").value = data[0].APMATERNO;
            document.getElementById("txtDireccion").value = data[0].DIRECCION;
            document.getElementById("txtTelCel").value = data[0].TELEFONOCELULAR;
            document.getElementById("txtTelFijo").value = data[0].TELEFONOFIJO;
            document.getElementById("txtEmail").value = data[0].EMAIL;
            document.getElementById("cboSexoPopup").value = data[0].IIDSEXO;
            document.getElementById("dtpFechaContrato").value = data[0].fc;
            document.getElementById("cboModalidadPopup").value = data[0].IIDMODALIDADCONTRATO;
            //document.getElementById("imgFoto").src = "data:image/png;base64," + data[0].FOTOMOSTRAR;

        });
    }
}



function Eliminar(id) {




    const swalWithBootstrapButtons = Swal.mixin({
        customClass: {
            confirmButton: 'btn btn-success',
            cancelButton: 'btn btn-danger'
        },
        buttonsStyling: false
    })

    swalWithBootstrapButtons.fire({title: 'Desea elminar el registro?', text: "Una vez eliminado, no podra recuperar la información!", icon: 'warning',
        showCancelButton: true, confirmButtonText: 'Yes', cancelButtonText: 'No', reverseButtons: true
    }).then((result) => {
        if (result.value) {

            $.get("Docente/eliminar/?id=" + id, function (data) {
                if (data == 0) {
                    swalWithBootstrapButtons.fire('Ocurrio un error', '', 'error')
                }
                else {
                    listar();
                    swalWithBootstrapButtons.fire('Información eliminada!', 'Se ha eliminado el registro', 'success')
                }
            })
        }
        else if (result.dismiss === Swal.DismissReason.cancel)
        {
            swalWithBootstrapButtons.fire('Operacion cancelada!', 'El registro esta a salvo :)', 'error')
        }
    })




    //if (confirm("¿Desea eliminar el registro?") == 1) {
    //    $.get("Docente/eliminar/?id=" + id, function (data) {
    //        if (data == 0)
    //        {
    //            alert("Error");
    //        }
    //        else {
    //            alert("Exito");
    //            listar();
    //        }
    //    })
    //}
}




function limpiarCampo() {
    var campos = document.getElementsByClassName("limpiar");
    var numeroDeCampos = campos.length;
    for (var i = 0; i < numeroDeCampos; i++) {
        campos[i].value = "";
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



function Agregar() {
    if (CamposObligatorios() == true) {
        var frm = new FormData();
        var id = document.getElementById("txtIdDocente").value;
        var nombre = document.getElementById("txtNombre").value;
        var apaterno = document.getElementById("txtAPaterno").value;
        var amaterno = document.getElementById("txtAMAterno").value;
        var direccion = document.getElementById("txtDireccion").value;
        var telcel = document.getElementById("txtTelCel").value;
        var telfijo = document.getElementById("txtTelFijo").value;
        var email = document.getElementById("txtEmail").value;
        var sexo = document.getElementById("cboSexoPopup").value;
        var fechacontrato = document.getElementById("dtpFechaContrato").value;
        var modalidad = document.getElementById("cboModalidadPopup").value;
        //var foto = document.getElementById("imgFoto").src.replace("data:image/png;base64,", "");

        frm.append("IIDDOCENTE", id);
        frm.append("NOMBRE", nombre);
        frm.append("APPATERNO", apaterno);
        frm.append("APMATERNO", amaterno);
        frm.append("DIRECCION", direccion);
        frm.append("TELEFONOCELULAR", telcel);
        frm.append("TELEFONOFIJO", telfijo);
        frm.append("EMAIL", email);
        frm.append("IIDSEXO", sexo);
        frm.append("FECHACONTRATO", fechacontrato);
        frm.append("IIDMODALIDADCONTRATO", modalidad);
        //frm.append("fileFoto", foto);
        frm.append("BHABILITADO", 1);



        const swalWithBootstrapButtons = Swal.mixin({
            customClass: {
                confirmButton: 'btn btn-success',
                cancelButton: 'btn btn-danger'
            },
            buttonsStyling: false
        })

        swalWithBootstrapButtons.fire({title: 'Esta seguro?', text: "Desea guardar el registro!", icon: 'question', showCancelButton: true, confirmButtonText: 'Si', cancelButtonText: 'No', reverseButtons: true
        })
            .then((result) => {
            if (result.value)
            {

                $.ajax({
                    type: "POST",
                    url: "Docente/guardar",
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
                                Swal.fire({icon: 'info', title: 'El docente ya existe...', text: 'Verifique!'})
                            }
                            else {
                                Swal.fire({ icon: 'alert', title: 'Oops...', text: 'Ocurrio un error!' })
                            }
                        }
                    }
                })




            } else if (result.dismiss === Swal.DismissReason.cancel)
            {
                swalWithBootstrapButtons.fire('Operacion cancelada', '', 'error')
            }
        })




        //if (confirm("¿Desea gurdar el registro?") == 1) {
        //    $.ajax({
        //        type: "POST",
        //        url: "Docente/guardar",
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
        //                    alert("Ya existe el docente!");
        //                }
        //                else {
        //                    alert("Ocurrio un error!");
        //                }
        //            }
        //        }
        //    })
        //}
    }
}


//var btnFoto = document.getElementById("btnFoto");
//btnFoto.onchange = function (e) {

//    var file = document.getElementById("btnFoto").files[0];
//    var reader = new FileReader();
//    if (reader != null) {
//        reader.onloadend = function () {
//            var img = document.getElementById("imgFoto");
//            img.src = reader.result;
//            alert(reader.result.replace("data:image/png;base64,", ""));
//        }
//    }
//    reader.readAsDataURL(file);
//}






//function crearListado(arrayColumnas, data) {
//    var contenido = "";
//    contenido += "<table id='tablaDocente' class='table'>";
//    contenido += "<thead>";

//    contenido += "<tr>";

//    for (var i = 0; i < arrayColumnas.length; i++) {
//        contenido += "<td>";
//        contenido += arrayColumnas[i];
//        contenido += "</td>";
//    }

//    contenido += "<td class='text-center'>Acciones</td>";
//    contenido += "</tr>";

//    contenido += "</thead>";
//    var llaves = Object.keys(data[0]);   //Obtiene un arreglo de Json
//    contenido += "<tbody>"

//    for (var i = 0; i < data.length; i++) {
//        contenido += "<tr>";

//        for (var j = 0; j < llaves.length; j++) {
//            var valorLlaves = llaves[j];
//            contenido += "<td>"
//            contenido += data[i][valorLlaves];
//            contenido += "</td>"
//        }

//        contenido += "<td class='text-center'>";
//        contenido += "<button class='btn btn-success' data-toggle='modal' data-target='#exampleModal'> <i class='glyphicon glyphicon-edit'></i> </button> "
//        contenido += "<button class='btn btn-danger' data-toggle='modal' data-target='#exampleModal'><i class='glyphicon glyphicon-trash'></i></button>"
//        contenido += "</td>";

//        contenido += "</tr>";
//    }

//    contenido += "</tbody>"

//    contenido += "</table>";

//    document.getElementById("tabla").innerHTML = contenido;
//    $('#tablaDocente').dataTable({
//        "searching": false
//    });
//}