Radvill.Requests.Current = (function () {
    var current = {};
    
    var _pendingRequestId;
    var _timer;
    var _question;
    var _status;
    var _category;
    var _timerId;

    current.Initialize = function() {
        $(document).on('timer-updated', function (e) {
            $('.timer').text(e.remaining);
        });

        Radvill.Notifications.Request.Bind('click', function () {
            Radvill.SwitchModule("Respond");
            Radvill.Notifications.Request.Hide();
        });
    };

    current.Set = function (id) {
        _pendingRequestId = id;
        Radvill.CallApi("Request", { id: _pendingRequestId }, "GET", function (data) {
            if (data != undefined) {
                _pendingRequestId = data.ID;
                _question = data.Question;
                _status = data.Status;
                _category = data.Category;
                Radvill.Notifications.Request.Show();
                startTimer(data.TimeStamp);
            }
            
        });
    };

    current.StartAnswer = function (start, callback) {
        clearTimeout(_timerId);
        Radvill.CallApi("Request", { id: _pendingRequestId, startAnswer: start }, "PUT", function (deadline) {
            if (start) {
                startTimer(deadline);
            }
            if (callback) {
                callback();
            }
        });
    };

    current.GetQuestion = function () {
        return _question;
    };

    current.GetCategory = function () {
        return _category;
    };

    current.GetId = function () {
        return _pendingRequestId;
    };

    function startTimer(deadline) {
        var now = new Date();
        var deadlineDate = new Date(deadline);
        var adjusted = deadlineDate.getTime() + (deadlineDate.getTimezoneOffset() * 60 * 1000);

        var diff = adjusted - now.getTime();

        if (diff < 1000) {
            current.StartAnswer(false);
            Radvill.Notifications.Request.Hide();
            _timer = '0';
        } else {
            var offset = now.getTimezoneOffset();
            diff = diff + (offset * 60 * 1000);

            var hours = Math.floor(diff / (1000 * 60 * 60));
            diff -= hours * (1000 * 60 * 60);

            var mins = Math.floor(diff / (1000 * 60));
            diff -= mins * (1000 * 60);

            var seconds = Math.floor(diff / (1000));

            _timer = "";

            if (hours > 0) {
                _timer = hours + ":";
            }

            if (hours > 0 || mins > 0) {
                _timer += mins + ":";
            }

            _timer += seconds;

            _timerId = setTimeout(function () {
                startTimer(deadline);
            }, 1000);
        }
        
        $(document).trigger({
            type: "timer-updated",
            remaining: _timer
        });
    };

    current.SubmitAnswer = function(postData, callback) {
        Radvill.CallApi("Answer", postData, "POST", function (data) {
            clearTimeout(_timerId);
            callback(data);
        });
    };

    return current;

})();