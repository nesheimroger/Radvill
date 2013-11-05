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

            websocket.bind('QuestionAssigned', function() {
                Radvill.Notifications.QuestionAssigned();
            });

            websocket.bind('AllRecipientsPassed', function (data) {
                $(document).trigger('QuestionStopped', [data]);
                Radvill.CallApi("Question", { ID: data.ID }, "GET", function (question) {
                    var model = new Radvill.Models.QuestionModel(question);
                    Radvill.Notifications.QuestionStopped(model);
                });
            });

            websocket.bind('AnswerStarted', function(data) {
                $(document).trigger('AnswerStarted', [data]);
            });

            websocket.bind('AnswerSubmitted', function (data) {
                $(document).trigger('AnswerSubmitted', [data]);
                Radvill.CallApi("Question", { ID: data.ID }, "GET", function (question) {
                    var model = new Radvill.Models.QuestionModel(question);
                    Radvill.Notifications.AnswerReceived(model);
                });
            });

            websocket.bind('AnswerEvaluated', function(data) {
                $(document).trigger('AnswerEvaluated', [data]);
                Radvill.CallApi("Answer", { ID: data.ID }, "GET", function (answer) {
                    var model = new Radvill.Models.AnswerModel(answer);
                    Radvill.Notifications.AnswerEvaluated(model);
                });
            });

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
                
                if (model == undefined) {
                    model = {};
                }

                radvill.ViewModel = ko.observable(model);
                
                
                
                $.get("/Views/" + controller + "/" + action + ".html").done(function (template) {
                    ko.cleanNode($('#content')[0]);
                    $("#content").html(template);
                    ko.applyBindings(radvill.ViewModel, $('#content')[0]);
                });
            }
        },
        
        Models: {},
        
        ViewModel: {},
        
        Notifications: {
            QuestionAssigned: function () {
                
                radvill.CallApi("Request", null, "GET", function(data) {
                    var answerQuestionModel = new radvill.Models.AnswerQuestionModel(data);

                    $.get("/Views/Notifications/QuestionReceived.html").done(function(template) {
                        $("#notifications").append(template);
                        answerQuestionModel.GoToQuestion = function() {
                            radvill.Controllers.View("Answer", "Index", answerQuestionModel);
                            hide();
                        };
                        ko.applyBindings(answerQuestionModel, $('#notifications #questionReceived')[0]);
                    });
                });


                function hide() {
                    var node = $("#notifications #questionReceived");
                    ko.cleanNode(node[0]);
                    node.remove();
                }

            },

            Generic: function (message) {
                var _timer;
                $.get("/Views/Notifications/Generic.html").done(function(template) {
                    var model = {
                        Message: message,
                    };
                    $("#notifications").append(template);
                    ko.applyBindings(model, $('#generic')[0]);
                    _timer = setTimeout(function () {
                        hide();
                    }, 4000);

                });
                function hide() {
                    clearTimeout(_timer);
                    var node = $("#notifications #generic");
                    ko.cleanNode(node[0]);
                    node.remove();
                }
            },

            AnswerReceived: function (questionModel) {
                var _timer;

                $.get("/Views/Notifications/AnswerReceived.html").done(function (template) {
                    $("#notifications").append(template);
                    questionModel.GoToQuestion = function() {
                        radvill.Controllers.View("Question", "View", questionModel);
                        hide();
                    };
                    ko.applyBindings(questionModel, $('#notifications #answerReceived')[0]);
                    _timer = setTimeout(function () {
                        hide();
                    }, 4000);
                });

                function hide() {
                    clearTimeout(_timer);
                    var node = $("#notifications #answerReceived");
                    ko.cleanNode(node[0]);
                    node.remove();
                }
            },
            
            AnswerEvaluated: function (answerModel) {
                var _timer;
                $.get("/Views/Notifications/AnswerEvaluated.html").done(function (template) {
                    $("#notifications").append(template);
                    answerModel.GoToAnswer = function () {
                        radvill.Controllers.View("Answer", "View", answerModel);
                        hide();
                    };
                    ko.applyBindings(answerModel, $('#notifications #answerEvaluated')[0]);
                    _timer = setTimeout(function () {
                        hide();
                    }, 4000);
                });
                
                function hide() {
                    clearTimeout(_timer);
                    var node = $("#notifications #answerEvaluated");
                    ko.cleanNode(node[0]);
                    node.remove();
                }
            },
            
            QuestionStopped: function (questionModel) {
                var _timer;
                $.get("/Views/Notifications/QuestionStopped.html").done(function (template) {
                    $("#notifications").append(template);
                    questionModel.GoToQuestion = function () {
                        radvill.Controllers.View("Question", "View", questionModel);
                        $("#notifications #questionStopped").remove();
                    };
                    ko.applyBindings(questionModel, $('#notifications #questionStopped')[0]);
                    _timer = setTimeout(function () {
                        hide();
                    }, 4000);
                });
                
                function hide() {
                    clearTimeout(_timer);
                    var node = $("#notifications #questionStopped");
                    ko.cleanNode(node[0]);
                    node.remove();
                }
            }
        },
        
        Helpers: {
            FormatTimeStamp: function (timestamp) {
                var date = new Date(timestamp + '+02:00');
                return date.getDate().padLeft() + '.' + (date.getMonth() + 1).padLeft() + '.' + date.getFullYear() + " " + date.getHours().padLeft() + ":" + date.getMinutes().padLeft();
            }
        }
        
        
    };
    
    Number.prototype.padLeft = function (base, chr) {
        var len = (String(base || 10).length - String(this).length) + 1;
        return len > 0 ? new Array(len).join(chr || '0') + this : this;
    };

    return radvill;
})();