Radvill.Profile = (function () {
    var profile = {};
    

    profile.Initialize = function () {

        $(document).on('submit', '#editProfileForm', function (event) {
            event.preventDefault();
            var postData = $(this).serialize();
            Radvill.CallApi("Profile", postData, "PUT", function () {
                Radvill.SwitchModule("Profile");
            });
            return false;
        });


    };

    return profile;

})();