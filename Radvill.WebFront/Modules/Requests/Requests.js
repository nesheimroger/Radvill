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
                    tableBody.append('<tr data-id="' + data[i].ID + '"><td>' + getStatus(data[i].Status, data[i].Type) + '</td><td>' + getType(data[i].Type) + '</td><td>' + data[i].Category + '</td><td>' + data[i].TimeStamp + '</td>');
                }

                callback(html);
            } else {
                callback('<p>Du har ingen forespørsler.</p>');
            }

        });
    };

    function getStatus(status, type) {
        if (type == 3) {
            switch (status) {
                case 1:
                    return "Venter på deg";
                case 2:
                    return "Avslått å svare"; //Not implmented in API
                case 3:
                    return "Svar påbegynt"; //Not implmented in API
                default:
                    return "Ukjent status";
            }
        }

        if (type == 2) {
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
            return "Svar mottat";
        case 4:
            return "Svar avslått, venter på nytt svar";
        case 5:
            return "Svar godkjent";
        default:
            return "Ukjent status";
        }
    }

    function getType(type)
    {
        if (type == 1) {
            return "Utgående";
        }
        return "Inngående";
    }
    

    return requests;

})();