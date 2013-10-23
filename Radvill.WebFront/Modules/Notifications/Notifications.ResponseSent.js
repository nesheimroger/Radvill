Radvill.Notifications.ResponseSent = (function() {
    var responseSent = {};
    var _responseSent;

    responseSent.Initialize = function () {
        $.get("/Modules/Notifications/ResponseSent.html").done(function (html) {
            _responseSent = html;
        });

        $(document).on('click', '.responseSentNotification', function() {
            hide();
        });
    };

    responseSent.Show = function () {
        $('#notifications').append(_responseSent);
        setTimeout(function() {
            hide();
        }, 3500);
    };

    function hide() {
        $("#notifications").find('.responseSentNotification').remove();
    }

    return responseSent;
})();