Radvill.Notifications = (function () {
    var notifications = {};

    notifications.Initialize = function() {
        notifications.Request.Initialize();
        notifications.Respond.Initialize();
    };

    return notifications;

})();