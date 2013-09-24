Radvill.Register = (function () {
    var register = {};

    register.Initialize = function () {
        $(document).on('submit', '#registerForm', function (event) {
            event.preventDefault();

            var postData = $(this).serialize();

            Radvill.CallApi("Register", postData, "POST", function(success) {
                if (success) {
                    Radvill.SwitchModule("Home");
                } else {
                    alert("Epost er allerede registert.");
                }
            });
            
            return false;
        });
    };


    return register;

})();