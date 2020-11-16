


$("#dtpFechaNAcimiento").datepicker({
    dateFormat: "dd/mm/yy",
    changeMonth: true,
    changeYear: true
});



listar();


function listar() {
    $.get("/Alumno/listar", function (data) {

        crearListado(["Id", "Nombre", "Apellido Paterno", "Apellido Materno", "Telefono Padre"], data);
    })
}


var btnBuscar = document.getElementById("btnBuscar");

btnBuscar.onclick = function () {

    var idsexo = document.getElementById("cboSexo").value;

    if (idsexo == "") {
        listar();
    }
    else {
        $.get("/Alumno/filtrarPorSexo/?idSexo=" + idsexo, function (data) {
            crearListado(["Id", "Nombre", "Apellido Paterno", "Apellido Materno", "Telefono Padre"], data);
        });
    }
}

$.get("/Alumno/listarSexo", function (data) {
    llenarCombo(data, document.getElementById("cboSexo"), true)
    //llenarCombo(data, document.getElementById("cboSexoPopup"), true)
})



var btnLimpiar = document.getElementById("btnLimpiar");
btnLimpiar.onclick = function () {
    listar();
    document.getElementById("cboSexo").value = "";
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

        contenido += "<td class='text-center'>";
        contenido += "<button class='btn btn-success' data-toggle='modal' onclick='abrirModal(" + data[i].IIDALUMNO +")' data-target='#exampleModal'> <i class='glyphicon glyphicon-edit'></i> </button> "
        contenido += "<button class='btn btn-danger' onclick='Eliminar(" + data[i].IIDALUMNO +")'><i class='glyphicon glyphicon-trash'></i></button>"
        contenido += "</td>";
        contenido += "</tr>";
    }

    contenido += "</tbody>"

    contenido += "</table>";

    document.getElementById("tabla").innerHTML = contenido;
    $('#tablaGeneral').dataTable({
        language: {
            processing: "Procesando...",
            search: "Buscar",
            lengthMenu: "Mostrar _MENU_  elementos",
            info: "Mostrando elementos del _START_ al _END_ de un total de _TOTAL_ elementos",
            infoEmpty: "Affichage de l'&eacute;lement 0 &agrave; 0 sur 0 &eacute;l&eacute;ments",
            infoFiltered: "(filtr&eacute; de _MAX_ &eacute;l&eacute;ments au total)",
            infoPostFix: "",
            loadingRecords: "Cargando...",
            zeroRecords: "Aucun &eacute;l&eacute;ment &agrave; afficher",
            emptyTable: "Aucune donnée disponible dans le tableau",
            paginate: {
                first: "Primero",
                previous: "Anterior",
                next: "Siguiente",
                last: "Ultimo"
            },
            aria: {
                sortAscending: ": activer pour trier la colonne par ordre croissant",
                sortDescending: ": activer pour trier la colonne par ordre décroissant"
            }
        },
        "searching": false,
        destroy: true,
        responsive: true,
        dom: "lfrtip",
        lengthMenu: [[5, 10, 15, 20, 25, 50, -1], [5, 10, 15, 20, 25, 50, "Todos"]],
        scrollX: true,
    });
}




function limpiarCampo() {
    var campos = document.getElementsByClassName("limpiar");
    var numeroDeCampos = campos.length;
    for (var i = 0; i < numeroDeCampos; i++) {
        campos[i].value = "";
    }
}



function Eliminar(id) {


    swal({
        title: "¿Está seguro?",
        text: "Una vez eliminado, no podrá recuperar este archivo!",
        icon: "warning",
        buttons: true,
        dangerMode: true,
    })
        .then((willDelete) => {
            if (willDelete) {
                $.get("Alumno/Eliminar/?id=" + id, function (data) {
                    if (data != 0) {
                        swal("El archivo ha sido eliminado!", {
                            icon: "success",
                        });
                        listar();
                        document.getElementById("btnCancelat").click();
                    }
                    else {
                        swal("Ocurrio un error", "", "warning");
                    }
                })
            }
            else {
                swal("Operación cancelada!");
            }
        });

    //if (confirm("¿Desea eliminar el registro?") == 1) {
    //    $.get("Alumno/Eliminar/?id=" + id, function (data) {
    //        if (data == 0) {
    //            alert("Ocurrio un error!");
    //        }
    //        else {
    //            alert("Se ha eliminado el registro!");
    //            listar();
    //            document.getElementById("btnCancelat").click();
    //        }
    //    })
    //}


    //var frm = new FormData();
    //frm.append("IIDALUMNO", id);
    //if (confirm("¿Desea eliminar el registro?") == 1) {
    //    $.ajax({
    //        type: "POST",
    //        url: "Alumno/Eliminar",
    //        data: frm,
    //        contentType: false,
    //        processData: false,
    //        success: function (data) {
    //            if (data != 0) {
    //                listar();
    //                alert("Se ha eliminado el registro!");
    //                document.getElementById("btnCancelat").click();
    //            }
    //            else {
    //                alert("Ocurrio un error!");
    //            }
    //        }
    //    })
    //}
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
        $.get("Alumno/recuperarInfo/?id=" + id, function (data) {
            document.getElementById("txtIdAlumno").value = data[0].IIDALUMNO;
            document.getElementById("txtNombre").value = data[0].NOMBRE;
            document.getElementById("txtAPaterno").value = data[0].APPATERNO;
            document.getElementById("txtAMAterno").value = data[0].APMATERNO;
            document.getElementById("dtpFechaNAcimiento").value = data[0].fnac;

            if (data[0].IIDSEXO == 1) {
                document.getElementById("rbM").checked = true;
            }
            else {
                document.getElementById("rbF").checked = true;
            }

            //document.getElementById("cboSexoPopup").value = data[0].IIDSEXO;
            document.getElementById("txtTelPadre").value = data[0].TELEFONOPADRE;
            document.getElementById("txtTelMadre").value = data[0].TELEFONOMADRE;
            document.getElementById("txtNoHermanos").value = data[0].NUMEROHERMANOS;

        });
    }
}



function Agregar() {
    if (CamposObligatorios() == true) {
        var frm = new FormData();
        var id = document.getElementById("txtIdAlumno").value;
        var nombre = document.getElementById("txtNombre").value;
        var aPaterno = document.getElementById("txtAPaterno").value;
        var aMaterno = document.getElementById("txtAMAterno").value;
        var FechaNac = document.getElementById("dtpFechaNAcimiento").value;
        //var sexo = document.getElementById("cboSexoPopup").value;

        var sexo;
        if (document.getElementById("rbM").checked == true) {
            sexo = 1;
        }
        else {
            sexo = 2;
        }

        var telPadre = document.getElementById("txtTelPadre").value;
        var telMadre = document.getElementById("txtTelMadre").value;
        var NumHer = document.getElementById("txtNoHermanos").value;

        frm.append("IIDALUMNO", id);
        frm.append("NOMBRE", nombre);
        frm.append("APPATERNO", aPaterno);
        frm.append("APMATERNO", aMaterno);
        frm.append("FECHANACIMIENTO", FechaNac);
        frm.append("IIDSEXO", sexo);
        frm.append("TELEFONOPADRE", telPadre);
        frm.append("TELEFONOMADRE", telMadre);
        frm.append("NUMEROHERMANOS", NumHer);
        frm.append("BHABILITADO", 1);


        swal({
            title: "¿Desea gardar la informacion?",
            icon: "warning",
            buttons: true,
            dangerMode: true,
        })
            .then((willDelete) => {
                if (willDelete) {
                    $.ajax({
                        type: "POST",
                        url: "Alumno/Guardar",
                        data: frm,
                        contentType: false,
                        processData: false,
                        success: function (data) {

                            if (data == 1) {
                                listar();
                                swal("Informacion Almacenada", "", "success");
                                document.getElementById("btnCancelat").click();
                            }
                            else {
                                if (data == -1) {
                                    swal("Ya existe el alumno", "Verifique!", "error");
                                }
                                else {
                                    swal("Ocurrio un error", "", "warning");
                                }
                            }
                        }
                    })
                }
                else
                {
                    swal("Operación cancelada!");
                }
            });	

        //if (confirm("¿Desea gurdar el registro?") == 1) {
        //    $.ajax({
        //        type: "POST",
        //        url: "Alumno/Guardar",
        //        data: frm,
        //        contentType: false,
        //        processData: false,
        //        success: function (data) {
        //            if (data == 0) {
        //                swal("Ocurrio un error", "Informacion no Enviada", "warning");
        //            }
        //            else {
        //                swal("Informacion Almacenada", "", "success");
        //                listar();
        //                document.getElementById("btnCancelat").click();
        //            }
        //        }
        //    })
        //}
    }
}
