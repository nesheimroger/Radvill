Radvill.Controllers.QuestionController = (function () {
    var questionController = {

        Index: function () {

            var categories = [];

            $.when(
                Radvill.CallApi("Category", null, "GET", function(data) {
                    for (var i = 0; i < data.length; i++) {
                        categories.push(new Radvill.Models.CategoryModel(data[i].ID, data[i].Name));
                    }
                })
            ).then(function() {
                var viewModel = new Radvill.Models.AskQuestionModel(categories);
                Radvill.Controllers.View("Question", "Index", viewModel);
            });

        },
       
        
        View: function (id) {
            var postdata = {
                ID: id
            };
            Radvill.CallApi("Question", postdata, "GET", function(data) {
                var model = new Radvill.Models.QuestionModel(data);
                Radvill.Controllers.View("Question", "View", model);
            });
        }
    };

    return questionController;
})();