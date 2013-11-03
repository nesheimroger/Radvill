Radvill.Controllers.RegisterController = (function() {
    var registerController = {
        Index: function() {
            Radvill.Controllers.View("Register", "Index");
        },
        
        Register: function() {
            var postData = {
                DisplayName: Radvill.Models.RegisterModel.DisplayName(),
                Email: Radvill.Models.RegisterModel.Email(),
                Password: Radvill.Models.RegisterModel.Password()
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