Radvill.Controllers.LoginController = (function() {
    var loginController = {

        Index: function () {
            var viewModel = new Radvill.Models.LoginModel();
            Radvill.Controllers.View("Login", "Index", viewModel);
        },
        
        Logout: function() {
            Radvill.CallApi("Login", null, "DELETE", function() {
                loginController.Index();
            });
        }
       
    };
    return loginController;
})();