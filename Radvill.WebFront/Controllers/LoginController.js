Radvill.Controllers.LoginController = (function() {
    var loginController = {

        Index: function () {
            var viewModel = new Radvill.Models.LoginModel();
            Radvill.Controllers.View("Login", "Index", viewModel);
        },
        
        Login: function () {

            var viewModel = Radvill.ViewModel();
            var postdata = {
                Email: viewModel.Email(),
                Password: viewModel.Password(),
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