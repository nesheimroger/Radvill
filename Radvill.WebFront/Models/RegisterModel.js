Radvill.Models.RegisterModel = function () {

    var registerModel = {
        Email: ko.observable(""),
        Password: ko.observable(""),
        DisplayName: ko.observable("")
    };
    
    registerModel.Submit = function() {

        var postData = {
            DisplayName: registerModel.DisplayName(),
            Email: registerModel.Email(),
            Password: registerModel.Password()
        };

        Radvill.CallApi("Register", postData, "POST", function (isSuccessfull) {
            if (isSuccessfull) {
                //Reload to run initialization again
                location.reload();
            } else {
                Radvill.Error("Epost allerede registert");
            }
        });
    };

    return registerModel;
};