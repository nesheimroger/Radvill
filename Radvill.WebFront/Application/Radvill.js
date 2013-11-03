$(function () {
    $.ajaxSetup({ cache: false });
    
    var isAuthenticated;

    $.when(
        Radvill.CallApi("Login", null, "GET", function (result) {
            isAuthenticated = result;
        })
    ).then(function () {
        if (isAuthenticated) {

            var websocket = new FancyWebSocket('ws://Radvill.WebApi/api/Socket');

            bindEvent(Radvill.Events.QuestionAssigned);
            bindEvent(Radvill.Events.QuestionStopped);
            bindEvent(Radvill.Events.AnswerStarted);
            bindEvent(Radvill.Events.AnswerReceived);
            bindEvent(Radvill.Events.AnswerEvaluated);

            function bindEvent(eventName) {
                websocket.bind(eventName, function (data) {
                    $(document).trigger(eventName, data);
                });
            }

            $("#HomeLink").on('click', function() {
                Radvill.Controllers.HomeController.Index();
            });
            Radvill.Controllers.HomeController.Index();
        } else {
            Radvill.Controllers.LoginController.Index();
        }
    });

    
});

var Radvill = (function () {
    
    var radvill = {
        
        Events: {
            QuestionAssigned: 'QuestionAssigned',
            QuestionStopped: 'QuestionStopped',
            AnswerStarted: 'AnswerStarted',
            AnswerReceived: 'AnswerReceived',
            AnswerEvaluated: 'AnswerEvaluated'
        },
        
        CallApi: function (url, data, method, callback) {
            var deffered = $.Deferred();
            $.when(
                $.ajax({
                    url: 'http://Radvill.WebApi/api/' + url,
                    data: data,
                    method: method,
                    xhrFields: {
                        withCredentials: true
                    },
                    crossDomain: true
                })
            ).done(function (result) {
                if (callback) {
                    callback(result);
                }
                deffered.resolve();

            }).fail(function () {
                deffered.reject();
                radvill.Error();
            });
            return deffered.promise();
        },


        Error: function (message) {
            if (message == undefined) {
                message = "En feil oppstod";
            }
            alert(message);
        },
        

        Controllers: {
            View: function(controller, action, model) {
                
                if (controller == undefined) {
                    controller = "Home";
                }
                
                if (action == undefined) {
                    action = "Index";
                }

                
                
                $.get("/Views/" + controller + "/" + action + ".html").done(function (template) {
                    ko.cleanNode($('#content')[0]);
                    $("#content").html(template);
                    ko.applyBindings(model, $('#content')[0]);
                });
            }
        },
        
        Models : {}
        
        
    };

    return radvill;
})();