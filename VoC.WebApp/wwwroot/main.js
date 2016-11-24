(function () {

    $(".voc_textarea").on("change keyup paste", function () {

        convertText(this);

        installTooltip();

        cursorManager.setEndOfContenteditable($(this)[0]);

    });
   
})();

function SignUp() {
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
        self.result("Done!");
    });
}

function Login() {
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
        //self.user(data.userName);
        // Cache the access token in session storage.
        sessionStorage.setItem("token", data.access_token);
    });
}