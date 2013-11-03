Radvill.Controllers.LoginController = (function() {
    var loginController = {
        Index: function () {
            Radvill.Controllers.View("Login", "Index", Radvill.Models.LoginModel);
        },
        
        Login: function () {
            
            var postdata = {
                Email: Radvill.Models.LoginModel.Email(),
                Password: Radvill.Models.LoginModel.Password(),
            };

            Radvill.CallApi("Login", postdata, "POST", function (successfull) {
                if (successfull) {
                    //Reloading to run initialization again
                    location.reload();
                } else {
                    Radvill.Error("Feil brukernavn eller passord");
                }
            });
        }
    };
    return loginController;
})();