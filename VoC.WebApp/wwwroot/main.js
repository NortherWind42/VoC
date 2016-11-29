"use strict";

var authorizeProcess = () => {

		let token = localStorage.getItem("token");
		if (token) {

		$(".voc_info-panel").removeClass("panel-danger");
		$(".voc_info-panel").addClass("panel-primary");
		$(".voc_info-panel-content").text("Enter your text down below, than hover on the word and wait to determine the language.");

		$(".voc_input").attr("contenteditable", "true");

		let userName = localStorage.getItem("userName");
		$("#greeting").text(`Hello, ${userName}!`);

		$("#loginForm").css("display", "none");
		$("#accountForm").css("display", "block");

		$(".voc_register-container").css("display", "none");

		console.log(`user ${userName} is authorized`)

		}

};

authorizeProcess();

(() => {

	$(".voc_input").on("change keyup paste", function () {

		convertText(this);

		installTooltip();

		cursorManager.setEndOfContenteditable($(this)[0]);

	});

})();

function removeError(groupId) {
	$(`#${groupId} .help-block`).text("");
	$(`#${groupId}`).removeClass("has-error");
}
function addError(groupId, error) {
	$(`#${groupId}`).addClass("has-error");
	$(`#${groupId} .help-block`).text(error);
}

function onSignUpClick() {

	let signUpData = {
		Email: $("#loginReg").val(),
		Password: $("#passwordReg").val(),
		ConfirmPassword: $("#passwordRegConfirm").val()
	};

	$.ajax({
		type: 'POST',
		url: '/api/Account/Register',
		contentType: 'application/json; charset=utf-8',
		data: JSON.stringify(signUpData)
		}).done(function () {
		let userName = $("#loginReg").val();
		let password = $("#passwordReg").val();
		console.log(`user ${userName} successfully added`);
		login(userName, password)

		}).fail(function (data) {

		for (let group of ["passwordRegConfirmGroup", "passwordRegGroup", "loginRegGroup"]) {
			removeError(group);
		}

		data.responseJSON.ModelState["model.ConfirmPassword"] ? addError("passwordRegConfirmGroup", "password and confirmation password do not match") : null

		data.responseJSON.ModelState["model.Password"] ? addError("passwordRegGroup", "password must be at least 6 characters long") : null

		data.responseJSON.ModelState[""] ? data.responseJSON.ModelState[""]["0"] ? addError("loginRegGroup", "email is invalid") : null : null

		data.responseJSON.ModelState[""] ? data.responseJSON.ModelState[""]["1"] ? addError("loginRegGroup", "email is already taken") : null : null

		data.responseJSON.ModelState["model.Email"] ? addError("loginRegGroup", "email field is required") : null

		});

}

function onLoginClick() {
	login($("#loginInter").val(), $("#passwordInter").val());
}

function login(username, password) {
	let loginData = {
		grant_type: 'password',
		username: username,
		password: password,
	};

	$.ajax({
		type: 'POST',
		url: '/Token',
		data: loginData
	}).done(function (data) {
		localStorage.setItem("token", data.access_token);
		localStorage.setItem("userName", data.userName);
		authorizeProcess();
		removeError("loginInterGroup");
		removeError("passwordInterGroup");
	}).fail(function (data) {
		addError("loginInterGroup", "user name or password is incorrect");
		addError("passwordInterGroup", "user name or password is incorrect")
	});;
}

function onLogoutClick() {
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

		$(".voc_info-panel").removeClass("panel-primary");
		$(".voc_info-panel").addClass("panel-danger");
		$(".voc_info-panel-content").text("Hello! Please, sign in.");

		$(".voc_input").empty();
		$(".voc_input").attr("contenteditable", "false");

		$("#loginForm").css("display", "block");
		$("#accountForm").css("display", "none");

		$(".voc_register-container").css("display", "block");

		console.log("logout is done");
	});
}