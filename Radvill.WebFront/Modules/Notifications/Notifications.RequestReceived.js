Radvill.Notifications.RequestReceived = (function () {
    var requestReceived = {};

    var _requestReceived;
    

    requestReceived.Initialize = function () {
        $.get("/Modules/Notifications/RequestReceived.html").done(function (html) {
            _requestReceived = html;
        });
    };

    requestReceived.Show = function () {
        $('#notifications').append(_requestReceived);
    };

    requestReceived.Hide = function () {
        $("#notifications").find('.requestReceivedNotification').remove();
    };

    requestReceived.Bind = function (event, callback) {
        $(document).on(event, '.requestReceivedNotification', function () {
            callback();
        });
    };

    return requestReceived;

})();