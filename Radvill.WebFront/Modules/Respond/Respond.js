Radvill.Respond = (function () {
    var respond = {};

    respond.Initialize = function() {
        $(document).on('click', '#btnStartAnswer', function () {
            Radvill.Requests.Current.StartAnswer(true, function() {
                $(".respond .step1").hide();
                $(".respond .step2").show();
            });
            
        });

        $(document).on('click', '#btnPass', function () {
            Radvill.Requests.Current.StartAnswer(false, function() {
                Radvill.SwitchModule('Home');
            });
            
        });

        $(document).on('submit', '#respondForm', function (event) {
            event.preventDefault();
            var postData = $(this).serialize();
            Radvill.Requests.Current.SubmitAnswer(postData, function (data) {
                Radvill.SwitchModule("Requests");
                Radvill.Notifications.ResponseSent.Show();
            });
            return false;
        });
    };

    respond.PopulateTemplate = function(template, callback) {
        var html = $(template);
        html.find('input[name=RequestID]').val(Radvill.Requests.Current.GetId());
        html.find('.category').text(Radvill.Requests.Current.GetCategory());
        html.find('.question').text(Radvill.Requests.Current.GetQuestion());
        html.find('.step2').hide();
        callback(html);
    };

    return respond;
})();

