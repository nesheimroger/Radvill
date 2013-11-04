Radvill.Controllers.RequestsController = (function() {
    var requestsController = {

        Index: function () {
            var questions, answers;

            $.when(
                Radvill.CallApi("Question", null, "GET", function(data) {
                    questions = data;
                }),
                Radvill.CallApi("Answer", null, "GET", function(data) {
                    answers = data;
                })
            ).then(function() {
                var viewModel = new Radvill.Models.RequestsModel(questions, answers);
                Radvill.Controllers.View("Requests", "Index", viewModel);
            });
        }
    };

    return requestsController;
})();