(function () {

	$(".voc_textarea").on("change keyup paste", function () {

		convertText(this);

		installTooltip();

		cursorManager.setEndOfContenteditable($(this)[0]);

	});

})();

let authorizeProcess = function () {
	let token = localStorage.getItem("token");
	if (token) {
		$(".voc_textarea").attr('contenteditable', "true");
		let userName = localStorage.getItem("userName");
		$("#loginInter").val(userName);
		console.log(`user ${userName} is authorized`)
	}
};

authorizeProcess();

function signUp() {

	var data = {
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
		console.log(data);
	});
}

function login() {
	var loginData = {
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
	});
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
		$(".voc_textarea").attr('contenteditable', "false");
		console.log("logout is done");
	});
}