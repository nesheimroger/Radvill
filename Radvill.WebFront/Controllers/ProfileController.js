Radvill.Controllers.ProfileController = (function () {
    var profileController = {
        
        Index: function() {
            Radvill.CallApi("Profile", null, "GET", function(data) {
                var model = new Radvill.Models.ProfileModel(data);
                Radvill.Controllers.View("Profile", "Index", model);
            });
        },
        
        Save: function() {

            var viewModel = Radvill.ViewModel();

            var postData = {
                DisplayName: viewModel.DisplayName(),
                Description: viewModel.Description()
            };

            Radvill.CallApi("Profile", postData, "PUT", function() {
                Radvill.Controllers.View("Profile", "Index", viewModel);
            });
        }

    };
    return profileController;
})();