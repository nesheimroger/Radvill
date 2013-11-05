Radvill.Models.AskQuestionModel = function (categories) {

    var questionModel = {
        Question: ko.observable(),
        Category: ko.observable(),
        Categories: categories
    };

    questionModel.Submit = function() {
        var postdata = {
            CategoryId: questionModel.Category(),
            Question: questionModel.Question()
        };

        Radvill.CallApi("Question", postdata, "POST", function (success) {
            if (success) {
                Radvill.Controllers.RequestsController.Index();
            } else {
                alert("Beklager, men ingen kunne motta forespørselen din.");
            }
        });
    };

    return questionModel;
};

