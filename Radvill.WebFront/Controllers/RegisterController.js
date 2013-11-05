Radvill.Controllers.RegisterController = (function() {
    var registerController = {

        Index: function () {
            var model = new Radvill.Models.RegisterModel();
            Radvill.Controllers.View("Register", "Index", model);
        }
      
    };
    return registerController;
})();