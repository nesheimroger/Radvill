$(function () {
    Radvill.InitializeModules();
});

var Radvill = (function () {

    //Private variables
    var radvill = {};
    var contentContainer = $("#content");
    var apiUrl = "http://Radvill.WebApi/api/";
    var wsUrl = "ws://Radvill.WebApi/api/Socket";
  

    //Public functions
    radvill.InitializeModules = function () {
        radvill.AskForAdvice.Initialize();
        radvill.Home.Initialize();
        radvill.Info.Initialize();
        radvill.Login.Initialize();
        radvill.Profile.Initialize();
        radvill.Register.Initialize();
        radvill.Requests.Initialize();
        radvill.Respond.Initialize();
        radvill.Scores.Initialize();
        radvill.Notifications.Initialize();
    };

    radvill.SwitchModule = function (name, mode) {
        
        var templateName;

        switch (name) {
            case "Info":
                templateName = mode;
                break;
            default:
                templateName = name;
                if (mode) {
                    templateName += mode;
                }
                break;
        }

        $.get("/Modules/" + name + "/" + templateName + ".html").done(function (template) {

            switch (name) {
                case "AskForAdvice":
                    contentContainer.html(radvill.AskForAdvice.PopulateTemplate(template));
                    break;
                case "Requests":
                    radvill.Requests.PopulateTemplate(template, function(result) {
                        contentContainer.html(result);
                    });
                    break;
                case "Profile":
                    if (mode == undefined) {
                        radvill.Profile.PopulateDisplayTemplate(template, function(result) {
                            contentContainer.html(result);
                        });
                    } else {
                        radvill.Profile.PopulateEditorTemplate(template, function (result) {
                            contentContainer.html(result);
                        });
                    }
                    break;
                case "Respond":
                    radvill.Respond.PopulateTemplate(template, function (result) {
                        contentContainer.html(result);
                    });
                    break;
                default:
                    contentContainer.html(template);
                    break;
            }
            contentContainer.data("current-module", name);
        });
    };


    radvill.CurrentModule = function() {
        return contentContainer.data("current-module");
    };

    radvill.UnknownError = function() {
        alert("En ukjent feil oppstod.");
    };
    
    radvill.CallApi = function (url, data, method, callback) {
        $.ajax({
            url: apiUrl + url,
            data: data,
            method: method,
            xhrFields: {
                withCredentials: true
            },
            crossDomain: true
        }).done(function (result) {
            if (callback) {
                callback(result);
            }

        }).fail(function () {
            radvill.UnknownError();
        });
    };

    radvill.InitializeSocket = function() {
        var websocket = new FancyWebSocket(wsUrl);
        websocket.bind('QuestionAssigned', function (data) {
            Requests.Current.Set(data.ID);
        });

        websocket.bind('AnswerStarted', function (data) {
            //TODO: Implement event logic
            console.log('AnswerStarted: ' + data.ID);
        });
        
        websocket.bind('AnswerSubmitted', function (data) {
            //TODO: Implement event logic
            console.log('AnswerSubmitted: ' + data.ID);
        });
        
        websocket.bind('AllRecipientsPassed', function (data) {
            //TODO: Implement event logic
            console.log('AllRecipientsPassed: ' + data.ID);
        });

        websocket.bind('close', function() {
            radvill.CallApi("Socket", null, "DELETE");
        });
        

    };
   

    return radvill;
})();