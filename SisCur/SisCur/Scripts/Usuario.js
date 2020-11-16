


listar();

function listar() {
    $.get("Usuario/listarUsuarios", function (data) {
        crearListado(["ID", "Nombre", "Usuario", "Rol", "Tipo"], data);
    })

    $.get("Usuario/listarRoles", function (data) {
        llenarCombo(data, document.getElementById("cboRol"), true)
    })

    $.get("Usuario/listaPersonas", function (data) {
        llenarCombo(data, document.getElementById("cboPersona"), true)
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
        document.getElementById("lblContra").style.display = "block";
        document.getElementById("txtContraseña").style.display = "block";

        document.getElementById("lblPersona").style.display = "block";
        document.getElementById("cboPersona").style.display = "block";

        limpiarCampo();
    } else {

        document.getElementById("txtContraseña").value = "1";
        document.getElementById("cboPersona").value = "2";

        document.getElementById("lblContra").style.display = "none";
        document.getElementById("txtContraseña").style.display = "none";

        document.getElementById("lblPersona").style.display = "none";
        document.getElementById("cboPersona").style.display = "none";


        $.get("Usuario/recuperarInfo/?id=" + id, function (data) {
            document.getElementById("txtIdUsuario").value = data.IIDUSUARIO;
            document.getElementById("txtNombreUsuario").value = data.NOMBREUSUARIO;
            document.getElementById("cboRol").value = data.IIDROL;

        });
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





function Agregar() {
    if (CamposObligatorios() == true) {
        var frm = new FormData();
        var idUsuario = document.getElementById("txtIdUsuario").value;
        var nombre = document.getElementById("txtNombreUsuario").value;
        var Contraseña = document.getElementById("txtContraseña").value;
        var persona = document.getElementById("cboPersona").value;
        var rol = document.getElementById("cboRol").value;
        var nombrePersona = document.getElementById("cboPersona").options[document.getElementById("cboPersona").selectedIndex].text;

        frm.append("IIDUSUARIO", idUsuario);
        frm.append("NOMBREUSUARIO", nombre);
        frm.append("CONTRA", Contraseña);
        frm.append("IID", persona);
        frm.append("IIDROL", rol);
        frm.append("nombreCompleto", nombrePersona);
        frm.append("BHABILITADO", 1);
        //alert(nombrePersona);


        if (confirm("¿Desea gurdar el registro?") == 1) {
            $.ajax({
                type: "POST",
                url: "Usuario/guardar",
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

                        alert("Ocurrio un error");
                        //if (data == -1) {
                        //    alert("Ya existe el docente!");
                        //}
                        //else {
                        //    alert("Ocurrio un error!");
                        //}
                    }
                }
            })
        }
        //swal({
        //    title: "¿Desea gardar la informacion?",
        //    icon: "warning",
        //    buttons: true,
        //    dangerMode: true,
        //})
        //    .then((willDelete) => {
        //        if (willDelete) {
        //            $.ajax({
        //                type: "POST",
        //                url: "Usuario/Guardar",
        //                data: frm,
        //                contentType: false,
        //                processData: false,
        //                success: function (data) {

        //                    if (data == 1) {
        //                        listar();
        //                        swal("Informacion Almacenada", "", "success");
        //                        document.getElementById("btnCancelat").click();
        //                    }
        //                    else {
        //                        if (data == -1) {
        //                            swal("Ya existe el alumno", "Verifique!", "error");
        //                        }
        //                        else {
        //                            swal("Ocurrio un error", "", "warning");
        //                        }
        //                    }
        //                }
        //            })
        //        }
        //        else {
        //            swal("Operación cancelada!");
        //        }
        //    });
    }
}
