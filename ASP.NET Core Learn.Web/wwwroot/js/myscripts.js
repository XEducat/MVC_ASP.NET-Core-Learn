// Change visible to password filds
function togglePasswordVisibility(fieldName) {
    var passwordInput = document.querySelector("input[name='" + fieldName + "']");
    var toggleButton = document.getElementById("togglePasswordBtn");

    if (passwordInput.type === "password") {
        passwordInput.setAttribute("type", "text");
        toggleButton.innerHTML = '<i class="fa fa-eye-slash"></i>';
    } else {
        passwordInput.setAttribute("type", "password");
        toggleButton.innerHTML = '<i class="fa fa-eye"></i>';
    }
}