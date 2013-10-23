Radvill.Notifications.AnswerReceived = (function () {
    var answerReceived = {};
    var _answerReceived;

    answerReceived.Initialize = function () {
        $.get("/Modules/Notifications/AnswerReceived.html").done(function (html) {
            _answerReceived = html;
        });

        $(document).on('click', '.answerReceivedNotification', function () {
            hide();
        });
    };

    answerReceived.Show = function () {
        $('#notifications').append(_answerReceived);
        setTimeout(function () {
            hide();
        }, 3500);
    };

    function hide() {
        $("#notifications").find('.answerReceivedNotification').remove();
    }

    return answerReceived;
})();