Radvill.Notifications.Respond = (function() {
    var respond = {};
    var _respond;

    respond.Initialize = function() {
        $.get("/Modules/Notifications/Respond.html").done(function (html) {
            _respond = html;
        });

        $(document).on('click', '.respondNotification', function() {
            hide();
        });
    };

    respond.Show = function() {
        $('#notifications').append(_respond);
        setTimeout(function() {
            hide();
        }, 2500);
    };

    function hide() {
        $("#notifications").find('.respondNotification').remove();
    }

    return respond;
})();