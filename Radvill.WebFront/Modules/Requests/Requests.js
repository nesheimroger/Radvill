Radvill.Requests = (function () {
    var requests = {};

    requests.Initialize = function () {
        requests.Current.Initialize();
        
        
    };

    requests.PopulateTemplate = function (template, callback) {
        var questions, answers;

        $.when(
            Radvill.CallApi("Question", null, "GET", function(data) {
                questions = data;
            }),
            Radvill.CallApi("Answer", null, "GET", function(data) {
                answers = data;
            })
        ).then(function() {
            var html = $(template);
            html = populateTable(html, "#questions tbody", questions);
            html = populateTable(html, "#answers tbody", answers);
            callback(html);
        });

        function populateTable(html, selector, data) {
            var tableBody = html.find(selector);
            for (var i = 0; i < data.length; i++) {
                tableBody.append('<tr data-id="' + data[i].ID + '"><td>' + getTime(data[i].TimeStamp) + '</td><td>' + data[i].Question + '</td><td class="status">' + getStatus(data[i].Status, "question") + '</td>');
            }
            return html;
        }


    };

    requests.AnswerReceived = function(questionid) {
        Radvill.Notifications.AnswerReceived.Show();
        $(".requests [data-id=" + questionid + "] .status").text(getStatus(3, 1));
    };

    requests.AnswerStarted = function(questionid) {
        $(".requests [data-id=" + questionid + "] .status").text(getStatus(2, 1));
    };

    function getStatus(status, type) {

        if (type == "answer") {
            switch (status) {
                case 1:
                    return "Svar sendt";
                case 2:
                    return "Svar avslått";
                case 3:
                    return "Svar akseptert";
                default:
                    return "Ukjent status";
            }
        }

        switch (status) {
            case 1:
                return "Venter på rågiver";
            case 2:
                return "Venter på svar";
            case 3:
                return "Svar mottatt";
            case 4:
                return "Venter på nytt svar";//"Svar avslått, venter på nytt svar";
            case 5:
                return "Avsluttet uten godkjent svar";//"Alle tilgjenglige rådgivere avslått å svare, eller svar ikke godkjent";
            case 6:
                return "Svar godkjent";
            default:
                return "Ukjent status";
        }
    }
    
    function getTime(timestamp) {
        var date = new Date(timestamp + '+02:00');
        return date.getDate().padLeft() + '.' + (date.getMonth() + 1).padLeft() + '.' + date.getFullYear() + " " + date.getHours().padLeft() + ":" + date.getMinutes().padLeft();

        
    }
    
    Number.prototype.padLeft = function (base, chr) {
        var len = (String(base || 10).length - String(this).length) + 1;
        return len > 0 ? new Array(len).join(chr || '0') + this : this;
    };

    return requests;
    

})();