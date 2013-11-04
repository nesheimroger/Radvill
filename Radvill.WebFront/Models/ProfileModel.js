Radvill.Models.ProfileModel = function (profile) {
    var profileModel = {
        DisplayName: ko.observable(profile.DisplayName),
        Description: ko.observable(profile.Description),
        Points: profile.Points
    };

    profileModel.Edit = function() {
        Radvill.Controllers.View("Profile", "Edit", profileModel);
    };

    profileModel.Save = function() {
        Radvill.Controllers.ProfileController.Save();
    };

    return profileModel;

};