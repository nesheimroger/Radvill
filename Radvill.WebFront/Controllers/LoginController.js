Radvill.Controllers.LoginController = (function() {
    var loginController = {

        Index: function () {
            var viewModel = new Radvill.Models.LoginModel();
            Radvill.Controllers.View("Login", "Index", viewModel);
        }
       
    };
    return loginController;
})();