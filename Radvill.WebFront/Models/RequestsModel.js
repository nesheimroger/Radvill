Radvill.Models.RequestsModel = function (questions, answers) {

    var requestsModel = {
        Questions: ko.observableArray(),
        Answers: ko.observableArray()
    };

    for (var i = 0; i < questions.length; i++) {
        addQuestion(questions[i]);
    }

    for (var j = 0; j < answers.length; j++) {
        addAnswer(answers[j]);
    }


    function addQuestion(question) {
        var questionModel = new Radvill.Models.QuestionModel(question);
        questionModel.ViewDetails = function () {
            Radvill.Controllers.QuestionController.View(questionModel.ID);
        };
        requestsModel.Questions.push(questionModel);
    }

    function addAnswer(answer) {
        var answerModel = new Radvill.Models.AnswerModel(answer);
        answerModel.ViewDetails = function () {
            Radvill.Controllers.AnswerController.View(answerModel.ID);
        };
        requestsModel.Answers.push(answerModel);
    }
 
    return requestsModel;
};