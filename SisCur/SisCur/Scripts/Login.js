

var btnIngresar = document.getElementById("btnIngresar");
btnIngresar.onclick = function () {

    var user = document.getElementById("txtUser").value;
    var pass = document.getElementById("txtPass").value;

    if (user == "") {
        alert("Ingrese Usuario");
        return;
    }

    if (pass == "") {
        alert("Ingrese Contrasña");
        return;
    }

    $.get("/Login/ValidarUser/?User=" + user + "&Password=" + pass, function (data) {

        if (data == 1) {
            //document.location.href = "@Url.Action('Index', 'Curso')";
            document.location.href = "/PantallaPrincipal/Index";
        }
        else {
            alert("Usuario o Contraseña incorrectos");
        }
    })
}