$(function () {
    Radvill.Initialize();
});

var Radvill = (function () {

    //Private variables
    var radvill = {};
    var contentContainer = $("#content");
    var apiUrl = "http://Radvill.WebApi/api/";
  

    //Public functions
    radvill.Initialize = function() {
        radvill.Login.Initialize();
        radvill.Register.Initialize();
    };

    radvill.SwitchModule = function(name) {
        $.get("/Modules/" + name + "/" + name + ".html").done(function (template) {
            contentContainer.html(template);
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

    return radvill;
})();


var RadvillApi = (function() {
    var radvill = {};

    radvill.Ajax = function(url,data,method,callback) {
        $.ajax({
            url: url,
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
            
        }).fail(function() {
            radvill.UnknownError();
        });
    };

    return radvill;
})();