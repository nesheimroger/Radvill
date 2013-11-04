Radvill.Controllers.HighscoresController = (function () {
    var highscoresController = {
        Index: function() {
            Radvill.CallApi("Highscores", null, "GET", function(data) {
                var model = new Radvill.Models.HighscoresModel(data);
                Radvill.Controllers.View("Highscores", "Index", model);
            });
        }
    };
    return highscoresController;
})();