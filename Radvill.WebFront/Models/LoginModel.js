Radvill.Models.LoginModel = function () {
    var loginModel = {
        Email: ko.observable(""),
        Password: ko.observable("")
    };

    loginModel.Submit = function() {
        var postdata = {
            Email: loginModel.Email(),
            Password: loginModel.Password(),
        };

        Radvill.CallApi("Login", postdata, "POST", function (successfull) {
            if (successfull) {
                //Reloading to run initialization again
                location.reload();
            } else {
                Radvill.Error("Feil brukernavn eller passord");
            }
        });
    };

    return loginModel;
};
