Radvill.Controllers.RegisterController = (function() {
    var registerController = {

        Index: function () {
            var model = new Radvill.Models.RegisterModel();
            Radvill.Controllers.View("Register", "Index", model);
        },
        
        Register: function () {

            var viewModel = Radvill.ViewModel();
            
            var postData = {
                DisplayName: viewModel.DisplayName(),
                Email: viewModel.Email(),
                Password: viewModel.Password()
            };

            Radvill.CallApi("Register", postData, "Post", function(isSuccessfull) {
                if (isSuccessfull) {
                    //Reload to run initialization again
                    location.reload();
                } else {
                    Radvill.Error("Epost allerede registert");
                }
            });

        }
    };
    return registerController;
})();