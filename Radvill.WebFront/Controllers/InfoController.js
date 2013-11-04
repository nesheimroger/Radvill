Radvill.Controllers.InfoController = (function () {
    var infoController = {
        About: function() {
            Radvill.Controllers.View("Info", "About");
        },
        Policy: function() {
            Radvill.Controllers.View("Info", "Policy");
        }
    };
    return infoController;
})();