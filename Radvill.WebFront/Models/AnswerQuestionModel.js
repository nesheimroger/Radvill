Radvill.Models.AnswerQuestionModel = function (request) {
    var _timerId;

    var answerQuestionModel = {
        RequestID: request.ID,
        Category: request.Category,
        Question: request.Question,
        Answer: ko.observable(""),
        Timer: ko.observable(""),
        Status: ko.observable(null)
    };

    startTimer(request.Deadline);

    answerQuestionModel.Start = function() {
        clearTimeout(_timerId);
        Radvill.CallApi("Request", { RequestId: answerQuestionModel.RequestID, StartAnswer: true }, "PUT", function (deadline) {
            startTimer(deadline);
            answerQuestionModel.Status(true);
        });
    };

    answerQuestionModel.Pass = function() {
        clearTimeout(_timerId);
        Radvill.CallApi("Request", { RequestId: answerQuestionModel.RequestID, StartAnswer: false }, "PUT", function () {
            Radvill.Controllers.HomeController.Index();
            answerQuestionModel.Status(false);
        });
    };

    answerQuestionModel.Submit = function () {  
        if (answerQuestionModel.Answer != "") {
            clearTimeout(_timerId);
            Radvill.Controllers.AnswerController.Submit();
        }
    };
    
    
    function startTimer(deadline) {
        var now = new Date();
        var deadlineDate = new Date(deadline);

        var diff = deadlineDate.getTime() - now.getTime();

        if (diff < 1000) {
            answerQuestionModel.Pass();
            if (answerQuestionModel.Status()) {
                Radvill.Error("Tiden for å svare er utløpt");
            }
            var node = $("#notifications #questionAssigned");
            if (node != undefined) {
                ko.cleanNode(node[0]);
                node.remove();
            }
            
        } else {
            var offset = now.getTimezoneOffset();
            diff = diff + (offset * 60 * 1000);

            var hours = Math.floor(diff / (1000 * 60 * 60));
            diff -= hours * (1000 * 60 * 60);

            var mins = Math.floor(diff / (1000 * 60));
            diff -= mins * (1000 * 60);

            var seconds = Math.floor(diff / (1000));

            var timer = "";

            if (hours > 0) {
                timer = hours + ":";
            }

            if (hours > 0 || mins > 0) {
                timer += mins + ":";
            }

            timer += seconds;
            answerQuestionModel.Timer(timer);

            _timerId = setTimeout(function () {
                startTimer(deadline);
            }, 1000);
        }
    };

    return answerQuestionModel;

};
