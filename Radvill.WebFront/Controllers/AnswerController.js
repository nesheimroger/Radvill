Radvill.Controllers.AnswerController = (function () {
    var answerController = {
       
        View: function(id) {
            var postdata = {
                ID: id                    
            };

            Radvill.CallApi("Answer", postdata, "GET", function(data) {
                var model = new Radvill.Models.AnswerModel(data);
                Radvill.Controllers.View("Answer", "View", model);
            });
        },
        
        Submit: function() {
            var model = Radvill.ViewModel();
            var postData = {
                RequestID: model.RequestID,
                Answer: model.Answer()
            };

            Radvill.CallApi("Answer", postData, "POST", function() {
                Radvill.Controllers.HomeController.Index();
                Radvill.Notifications.Generic("Ditt svar er sendt inn.");
            });
        }
    };

    return answerController;
})();