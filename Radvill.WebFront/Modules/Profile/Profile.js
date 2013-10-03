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

    profile.PopulateDisplayTemplate = function(template, callback) {
        Radvill.CallApi("Profile", null, "GET", function (data) {
            var html = $(template);
            html.find('.displayName').text(data.DisplayName);
            html.find('.description').text(data.Description);
            callback(html);
        });
    };
    
    profile.PopulateEditorTemplate = function (template, callback) {
        Radvill.CallApi("Profile", null ,"GET", function (data) {
            var html = $(template);
            html.find('#displayName').val(data.DisplayName);
            html.find('#description').val(data.Description);
            callback(html);
        });
    };

    return profile;

})();