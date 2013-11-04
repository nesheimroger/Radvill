Radvill.Models.HighscoresModel = function(profiles) {
    var highscoresModel = {
        Profiles: ko.observableArray()
    };

    for (var i = 0; i < profiles.length; i++) {
        addProfileToList(profiles[i]);
    }

    function addProfileToList(p) {
        var profile = new Radvill.Models.ProfileModel(p);
        profile.View = function () {
            Radvill.Controllers.View("Profile", "View", profile);
        };
        highscoresModel.Profiles.push(profile);
    }

    return highscoresModel;
}