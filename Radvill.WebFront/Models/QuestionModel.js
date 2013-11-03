Radvill.Models.QuestionModel = function() {
    var questionModel = {
        ID: ko.observable(0),
        Question: ko.observable(),
        Category: ko.observable(),
        Status: ko.observable(getStatus(1)),
        Answers: ko.observableArray()
    };
    return questionModel;


    function getStatus(statusId) {
        switch (statusId) {
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

};