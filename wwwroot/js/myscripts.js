// Change visible to password filds
function togglePasswordVisibility(fieldName) {
	var passwordInput = document.querySelector("input[name='" + fieldName + "']");
	var showPasswordCheckbox = document.getElementById("show" + fieldName);

	passwordInput.type = showPasswordCheckbox.checked ? "text" : "password";
}

