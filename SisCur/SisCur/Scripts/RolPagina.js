
listar();

function listar() {
    $.get("RolPagina/listarRoles", function (data) {
        crearListado(["ID", "Nombre", "Descripción"], data);
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



var idRol;

function abrirModal(id) {
    idRol = id;
    var camposObligatorios = document.getElementsByClassName("obligatorio");
    var numeroDeCampos = camposObligatorios.length;


    for (var i = 0; i < numeroDeCampos; i++) {
        camposObligatorios[i].parentNode.classList.remove("error");
    }


    $.get("RolPagina/listarPagina", function (data) {
        var tabla = "<tbody>";
        for (var i = 0; i < data.length; i++) {
            tabla += "<tr>";
            tabla += "<td>";
            tabla += "<input type='checkbox' id='" + data[i].IIDPAGINA +"' class='checkbox' />";
            tabla += "</td>";
            tabla += "<td>";
            tabla += data[i].MENSAJE;
            tabla += "</td>";
            tabla += "</tr>";
        }
        tabla += "</tbody>";
        document.getElementById("tablaPagina").innerHTML = tabla;

        if (id>0) {
            ObtenerPaginaRol();
        }
    })


    if (id == 0) {
        limpiarCampo();
    }
}



function ObtenerPaginaRol() {
    $.get("RolPagina/obtenerRol/?id=" + idRol, function (data) {
        document.getElementById("txtIdRol").value = data.IIDROL;
        document.getElementById("txtNombreRol").value = data.NOMBRE;
        document.getElementById("txtDescripcionRol").value = data.DESCRIPCION;

    });

    $.get("RolPagina/listarRolPagina/?idRol=" + idRol, function (data) {
        var numRegistros = data.length;
        for (var i = 0; i < numRegistros; i++) {
            if (data[i].BHABILITADO == 1)
            {
                document.getElementById(data[i].IIDPAGINA).checked = true;
            }
        }
    });
}





function Agregar() {
    if (CamposObligatorios() == true) {
        var frm = new FormData();
        var id = document.getElementById("txtIdRol").value;
        var nombre = document.getElementById("txtNombreRol").value;
        var descripcion = document.getElementById("txtDescripcionRol").value;

        frm.append("IIDROL", id);
        frm.append("NOMBRE", nombre);
        frm.append("DESCRIPCION", descripcion);
        frm.append("BHABILITADO", 1);

        var checkbox = document.getElementsByClassName("checkbox");
        var cantidadChecks = checkbox.length;
        var EnviarChecks = "";

        for (var i = 0; i < cantidadChecks; i++) {
            if (checkbox[i].checked == true) {
                EnviarChecks += checkbox[i].id;
                EnviarChecks += "$";
            }
        }
        EnviarChecks = EnviarChecks.substring(0, EnviarChecks.length - 1);
        frm.append("CheckEnviado", EnviarChecks);
        alert(EnviarChecks);

        if (confirm("¿Desea gurdar el registro?") == 1) {
            $.ajax({
                type: "POST",
                url: "RolPagina/guardar",
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
                        alert("Ocurrio error");
                        //if (data == -1) {
                        //    alert("Ya existe el registro!");
                        //}
                        //else {
                        //    alert("Ocurrio un error!");
                        //}
                    }
                }
            })
        }

        //const swalWithBootstrapButtons = Swal.mixin({
        //    customClass: {
        //        confirmButton: 'btn btn-success',
        //        cancelButton: 'btn btn-danger'
        //    },
        //    buttonsStyling: false
        //})

        //swalWithBootstrapButtons.fire({
        //    title: 'Esta seguro?', text: "Desea guardar el registro!", icon: 'question',
        //    showCancelButton: true, confirmButtonText: 'Si', cancelButtonText: 'No', reverseButtons: true
        //})
        //    .then((result) => {
        //        if (result.value) {

        //            $.ajax({
        //                type: "POST",
        //                url: "RolPagina/guardar",
        //                data: frm,
        //                contentType: false,
        //                processData: false,
        //                success: function (data) {
        //                    if (data == 1) {
        //                        listar();
        //                        swalWithBootstrapButtons.fire('Informacion almacenada!', '', 'success')
        //                        document.getElementById("btnCancelat").click();
        //                    }
        //                    else {
        //                        if (data == -1) {
        //                            Swal.fire({ icon: 'info', title: 'El docente ya existe...', text: 'Verifique!' })
        //                        }
        //                        else {
        //                            Swal.fire({ icon: 'alert', title: 'Oops...', text: 'Ocurrio un error!' })
        //                        }
        //                    }
        //                }
        //            })

        //        }
        //        else if (result.dismiss === Swal.DismissReason.cancel)
        //        {
        //            swalWithBootstrapButtons.fire('Operacion cancelada', '', 'error')
        //        }
        //    })
    }
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