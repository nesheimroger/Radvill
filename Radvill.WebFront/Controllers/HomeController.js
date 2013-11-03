Radvill.Controllers.HomeController = (function() {
    var home = {
        Index: function() {
            Radvill.Controllers.View("Home", "Index");
        }
    };

    return home;
})();