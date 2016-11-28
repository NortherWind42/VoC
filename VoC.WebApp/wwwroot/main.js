"use strict";

var authorizeProcess = () => {

		let token = localStorage.getItem("token");
		if (token) {
			$(".voc_private-area").removeClass("hidden");
			$("#logoutBtn").css("display", "block");
			$("#loginBtn").css("display", "none");

			$(".voc_textarea").empty();

			let userName = localStorage.getItem("userName");
			$("#greeting").text(`Hello ${userName}!`);
			$("#loginInter").val(userName);

			console.log(`user ${userName} is authorized`)
		}

};

authorizeProcess();

(() => {

	$(".voc_textarea").on("change keyup paste", function () {

		convertText(this);

		installTooltip();

		cursorManager.setEndOfContenteditable($(this)[0]);

	});

})();

function signUp() {

	let data = {
		Email: $("#loginReg").val(),
		Password: $("#passwordReg").val(),
		ConfirmPassword: $("#passwordRegConfirm").val()
	};

	$.ajax({
		type: 'POST',
		url: '/api/Account/Register',
		contentType: 'application/json; charset=utf-8',
		data: JSON.stringify(data)
	}).done(function (data) {
		console.log("user successfully added");
	}).fail(function (data) {
		console.log(data.responseText);
	});

}

function login() {
	let loginData = {
		grant_type: 'password',
		username: $("#loginInter").val(),
		password: $("#passwordInter").val(),
	};

	$.ajax({
		type: 'POST',
		url: '/Token',
		data: loginData
	}).done(function (data) {
		localStorage.setItem("token", data.access_token);
		localStorage.setItem("userName", data.userName);
		authorizeProcess();
	}).fail(function (data) {
		console.log(data.responseText);
	});;
}

function logout() {
	$.ajax({
		type: 'POST',
		url: 'api/Account/Logout',
		contentType: 'application/json; charset=utf-8',
		headers: {
			'Authorization': 'Bearer  ' + localStorage.getItem("token"),
			'Content-Type': 'application/json'
		},
	}).done(function (data) {
		localStorage.removeItem("token");
		localStorage.removeItem("userName");
		$("#loginInter").val("");
		$("#passwordInter").val("");
		$(".voc_private-area").addClass("hidden");
		$("#logoutBtn").css("display", "none");
		$("#loginBtn").css("display", "block");
		$(".voc_textarea").empty();
		console.log("logout is done");
	});
}