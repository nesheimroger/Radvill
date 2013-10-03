Radvill.Login = (function() {
    var login = {};
    
    login.Initialize = function () {

        Radvill.CallApi("Login", null, "GET", function(loggedIn) {
            if (!loggedIn) {
                Radvill.SwitchModule("Login");
            } else {
                Radvill.SwitchModule("Home");
                Radvill.InitializeSocket();
            }
        });

        $(document).on('submit', '#loginForm', function(event) {
            event.preventDefault();
            var postData = $(this).serialize();
            Radvill.CallApi("Login", postData, "POST", function(success) {
                if (success) {
                    Radvill.SwitchModule("Home");
                    Radvill.InitializeSocket();
                } else {
                    alert("Feil epost eller passord.");
                }
            });
            return false;
        });
    };


    return login;

})();