Radvill.Requests.Current = (function () {
    var current = {};
    
    var _requestId;
    var _timer;
    var _question;
    var _status;
    
    var _timerId;

    current.Initialize = function() {
        $(document).on('timer-updated', function (e) {
            $('.timer').text(e.remaining);
            console.log(e.remaining);
        });

        Radvill.Notifications.Request.Bind('click', function () {
            //TODO: Show form
            console.log("Notification clicked");
            Radvill.Notifications.Request.Hide();
        });
    };

    current.Set = function (id) {
        _requestId = id;
        Radvill.CallApi("Request", { id: _requestId }, "GET", function (data) {
            if (data != undefined) {
                _requestId = data.ID;
                _question = data.Question;
                _status = data.Status;
                Radvill.Notifications.Request.Show();
                startTimer(data.TimeStamp);
            }
            
        });
    };

    current.StartAnswer = function(start) {
        Radvill.CallApi("Request", { id: _requestId, startAnswer: start }, "PUT", function (deadline) {
            if (start) {
                //TODO: Show answer form
                startTimer(deadline);
            } else {
                //TODO: Go back to previous page
            }
            
            
        });
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

    

    return current;

})();