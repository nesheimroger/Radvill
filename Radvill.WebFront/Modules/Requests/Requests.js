Radvill.Requests = (function () {
    var requests = {};

    requests.Initialize = function () {
        requests.Current.Initialize();
    };

    requests.PopulateTemplate = function (template, callback) {


        Radvill.CallApi("Request", null, "GET", function(data) {
            if (data.length > 0) {
                var html = $(template);
                var tableBody = html.find('tbody');
                for (var i = 0; i < data.length; i++) {
                    tableBody.append('<tr data-id="' + data[i].ID + '"><td>' + getStatus(data[i].status, data[i].IsQuestion) + '</td><td>' + getType(data[i].IsQuestion) + '</td><td>' + data[i].Category + '</td><td>' + data[i].TimeStamp + '</td>');
                }

                callback(html);
            } else {
                callback('<p>Du har ingen forespørsler.</p>');
            }

        });
    };

    function getStatus(status, isQuestion) {
        
        if (isQuestion == null) {
            return "Venter på deg";
        }
        //Null indicates that noone have touched it yet
        if (status == null) {
            if (isQuestion) {
                return "Venter på rådgiver";
            }
            return "Sendt";
        }
        
        //True meens that its complete from your or the others side
        if (status) {
            if (isQuestion) {
                return "Svar godkjent";
            } 
            return "Akseptert";
            
        }
        
        //False meens that someone have started to answer or your answer was declined
        if (isQuestion) {
            return "Venter på svar";
        } else {
            return "Avslått";
        }
    }
    function getType(isQuestion)
    {
        if (isQuestion) {
            return "Utgående";
        }
        return "Inngående";
    }
    

    return requests;

})();