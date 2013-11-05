Radvill.Models.QuestionModel = function(question) {
    var questionModel = {
        ID: question.ID,
        Category: question.Category,
        Status: ko.observable(getQuestionStatus(question.Status)),
        Question: question.Question,
        TimeStamp: Radvill.Helpers.FormatTimeStamp(question.TimeStamp),
        Answers: ko.observableArray([])
    };
        
    for (var k = 0; k < question.Answers.length; k++) {
        addAnswerToQuestion(question.Answers[k]);
    }

    function addAnswerToQuestion(answer) {
        var answerModel = {
            ID: answer.ID,
            Accepted: ko.observable(answer.Accepted),
            Answer: answer.Text,
            Rated: ko.observable(function() {
                return answer.Accepted != null;
            }())
        };

        answerModel.Accept = function() {
            Radvill.CallApi("Answer", { AnswerID: answerModel.ID, Accepted: true }, "PUT", function() {
                answerModel.Rated(true);
                answerModel.Accepted(true);
            });
        };

        answerModel.Decline = function() {
            Radvill.CallApi("Answer", { AnswerID: answerModel.ID, Accepted: false }, "PUT", function (passedOn) {
                answerModel.Rated(false);
                answerModel.Status(false);
                if (passedOn) {
                    Radvill.Notifications.Generic("Ditt spørsmål har blitt sendt videre");
                }
            });
        };

        questionModel.Answers.push(answerModel);
    };


    $(document).on('AnswerStarted', function (event, data) {
        refresh(data);
    });
    
    $(document).on('AnswerSubmitted', function (event, data) {
        refresh(data);
    });
    
    $(document).on('QuestionStopped', function (event, data) {
        refresh(data);
    });
    


    function refresh(data) {
        if (data.ID == questionModel.ID) {

            var postData = {
                ID: data.ID
            };
            Radvill.CallApi("Question", postData, "GET", function(q) {
                questionModel.Status(getQuestionStatus(q.Status));
                questionModel.Answers.removeAll();
                for (var p = 0; p < q.Answers.length; p++) {
                    addAnswerToQuestion(q.Answers[p]);
                }
            });
        }
    }
    
    function getQuestionStatus(status) {
        switch (status) {
            case 1:
                return "Venter på rågiver";
            case 2:
                return "Venter på svar";
            case 3:
                return "Svar mottatt";
            case 4:
                return "Venter på nytt svar";
            case 5:
                return "Avsluttet uten godkjent svar";
            case 6:
                return "Svar godkjent";
            default:
                return "Ukjent status";
        }
    }


    return questionModel;
};