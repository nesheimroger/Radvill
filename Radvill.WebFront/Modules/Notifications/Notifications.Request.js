Radvill.Notifications.Request = (function () {
    var request = {};

    var _request;
    

    request.Initialize = function() {
        $.get("/Modules/Notifications/Request.html").done(function (html) {
            _request = html;
        });
    };

    request.Show = function () {
        $('#notifications').html(_request);
    };

    request.Hide = function() {
        $("#notifications").find('.requestNotification').remove();
    };

    request.Bind = function(event, callback) {
        $(document).on(event, '.requestNotification', function () {
            callback();
        });
    };

    return request;

})();