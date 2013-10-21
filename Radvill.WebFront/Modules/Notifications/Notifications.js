Radvill.Notifications = (function () {
    var notifications = {};

    notifications.Initialize = function() {
        notifications.RequestReceived.Initialize();
        notifications.ResponseSent.Initialize();
    };

    return notifications;

})();