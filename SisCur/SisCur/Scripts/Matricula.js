

listar();




function listar() {
    $.get("Matricula/listar", function (data) {
        crearListado(["ID", "Periodo", "Grado", "Sección", "Alumno"], data);
    })


    $.get("Matricula/listarPeriodos", function (data) {
        llenarCombo(data, document.getElementById("cboPeriodo"), true)
    })

    $.get("Matricula/listarGradoSeccion", function (data) {
        llenarCombo(data, document.getElementById("cboGradoSeccion"), true)
    })

    $.get("Matricula/listarAlumnos", function (data) {
        llenarCombo(data, document.getElementById("cboAlumno"), true)
    })

}

function recuperar(idPeriodo, idGradoSeccion)
{
    $.get("Matricula/listarCursosPorPeriodoyGrado/?idPeriodo=" + idPeriodo + "&idGradoSeccion=" + idGradoSeccion, function (data) {
        contenido = "<tbody>";
        for (var i = 0; i < data.length; i++) {
            contenido += "<tr>";

            contenido += "<td>";
            contenido += "<input type='checkbox' class='checkbox' id=" + data[i].IIDCURSO + " checked='true' />"
            
            contenido += "</td>";

            contenido += "<td>";
            contenido += data[i].NOMBRE;
            contenido += "</td>";

            contenido += "</tr>";
        }
        contenido += "</tbody>";

        document.getElementById("tablaCurso").innerHTML = contenido;
    })
}

var cboPeriodo = document.getElementById("cboPeriodo");
var cboGradoSeccion = document.getElementById("cboGradoSeccion");

cboPeriodo.onchange = function () {
    if (cboPeriodo.value != "" && cboGradoSeccion.value != "") {
        recuperar(cboPeriodo.value, cboGradoSeccion.value);
    }
}

cboGradoSeccion.onchange = function () {
    if (cboPeriodo.value != "" && cboGradoSeccion.value != "") {
        recuperar(cboPeriodo.value, cboGradoSeccion.value);
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


        if (id != 0) {
            $.get("Matricula/recuperarDatos/?id=" + id, function (data) {

                document.getElementById("cboAlumno").style.display = "none";
                document.getElementById("lblAlumno").style.display = "none";

                document.getElementById("txtIdMatricula").value = data.IIDMATRICULA;
                document.getElementById("cboPeriodo").value = data.IIDPERIODO;
                document.getElementById("cboGradoSeccion").value = data.IIDSECCION;
                document.getElementById("cboAlumno").value = data.IIDALUMNO;
            });
        }
        else {
            document.getElementById("cboAlumno").style.display = "block";
            document.getElementById("lblAlumno").style.display = "block";
        }

        if (id != 0) {
            $.get("Matricula/Cursos/?id=" + id, function (data) {
                contenido = "<tbody>";
                for (var i = 0; i < data.length; i++) {
                    contenido += "<tr>";

                    contenido += "<td>";
                    if (data[i].bhabilitado == 1) {
                        contenido += "<input type='checkbox' class='checkbox' id=" + data[i].IIDCURSO + " checked='true' />"
                    }
                    else {
                        contenido += "<input type='checkbox' class='checkbox' id=" + data[i].IIDCURSO + " />"
                    }

                    contenido += "</td>";

                    contenido += "<td>";
                    contenido += data[i].NOMBRE;
                    contenido += "</td>";

                    contenido += "</tr>";
                }
                contenido += "</tbody>";

                document.getElementById("tablaCurso").innerHTML = contenido;
            })
        }
    }
}




function Agregar() {
    if (CamposObligatorios() == true) {


        //vALIDAR SI EXISTEN CURSOS ASIGNADOS AL PERIODO Y GRADO
        var checkboxes = document.getElementsByClassName("checkbox");

        if (checkboxes.length == 0) {
            alert("No hay cursos asignados a ese priodo y grado");
            return;
        }

        //REVISAMOS CUANTOS CHECBOX ESTAN SELECCIONADOS
        var c = 0;
        for (var i = 0; i < checkboxes.length; i++) {
            if (checkboxes[i].checked == true) {
                c++;
            }
        }

        if (c == 0) {
            alert("No ha seleccionado ningun curso");
            return;
        }


        var frm = new FormData();
        var id = document.getElementById("txtIdMatricula").value;
        var idPeriodo = document.getElementById("cboPeriodo").value;
        var idGradoSeccion = document.getElementById("cboGradoSeccion").value;
        var idAlumno = document.getElementById("cboAlumno").value;

        frm.append("IIDMATRICULA", id);
        frm.append("IIDPERIODO", idPeriodo);
        frm.append("IIDGRADOSECCION", idGradoSeccion);
        frm.append("IIDALUMNO", idAlumno);


        //Checboxes habilitados
        var valorEnviar = "";
        var valorDeshabilitar = "";
        var check = document.getElementsByClassName("checkbox");
        var ncheck = check.length;

        for (var i = 0; i < ncheck; i++) {
            if (check[i].checked == true) {
                valorEnviar += check[i].id;
                valorEnviar += "$";
            }
            else {
                valorDeshabilitar += check[i].id;
                valorDeshabilitar += "$";
            }
        }

        if (valorEnviar != "") {
            valorEnviar = valorEnviar.substring(0, valorEnviar.length - 1);
            //alert(valorEnviar);
        }

        if (valorDeshabilitar != "") {
            valorDeshabilitar = valorDeshabilitar.substring(0, valorDeshabilitar.length - 1);
            //alert(valorDeshabilitar);
        }


        frm.append("valorEnviar", valorEnviar);
        frm.append("valorDeshabilitar", valorDeshabilitar);
        //Solo los check que esten habilitados
        frm.append("BHABILITADO", 1);

        if (confirm("¿Desea gurdar el registro?") == 1) {
            $.ajax({
                type: "POST",
                url: "/Matricula/guardar",
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
                            alert("Ya existe la matricula");
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



    swal({
        title: "¿Está seguro?",
        text: "Una vez eliminado, no podrá recuperar este archivo!",
        icon: "warning",
        buttons: true,
    })
        .then((willDelete) => {
            if (willDelete) {
                $.get("Matricula/eliminar/?id=" + id, function (data) {
                    if (data != 0) {
                        swal("El registro ha sido eliminado!", {
                            icon: "success",
                        });
                        listar();
                        document.getElementById("btnCancelat").click();
                    }
                    else {
                        swal("Ocurrio un error", "", "error");
                    }
                })
            }
            else {
                swal("Operación cancelada!");
            }
        });



    //if (confirm("¿Desea eliminar el registro?") == 1) {
    //    $.get("Matricula/eliminar/?id=" + id, function (data) {
    //        if (data == 0) {
    //            alert("Error");
    //        }
    //        else {
    //            alert("Exito");
    //            listar();
    //        }
    //    })
    //}
}