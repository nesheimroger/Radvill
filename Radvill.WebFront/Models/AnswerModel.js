Radvill.Models.AnswerModel = function(answer) {
    var answerModel = {
        ID: answer.ID,
        Question: answer.Question,
        Category: answer.Category,
        TimeStamp: Radvill.Helpers.FormatTimeStamp(answer.TimeStamp),
        Answer: answer.Answer,
        Status: ko.observable(getAnswerStatus(answer.Accepted))
    };
    
    function getAnswerStatus(status) {
        switch (status) {
            case null:
                return "Svar sendt";
            case false:
                return "Svar avslått";
            case true:
                return "Svar akseptert";
            default:
                return "Ukjent status";
        }
    }

    $(document).on('AnswerEvaluated', function(data) {
        if (data.ID == answerModel.ID) {
            answerModel.Status(getAnswerStatus(data.Accepted));
        }
    });

    return answerModel;
};