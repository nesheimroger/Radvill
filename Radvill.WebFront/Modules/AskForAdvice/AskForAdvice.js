Radvill.AskForAdvice = (function () {
    var askForAdvice = {};
    var _categories;

    askForAdvice.Initialize = function () {

        Radvill.CallApi("Category", null, "GET", function (categories) {
            _categories = categories;
        });

        $(document).on('submit', '#askForAdviceForm', function (event) {
            event.preventDefault();
            var postData = $(this).serialize();
            Radvill.CallApi("Request", postData, "POST", function (success) {
                if (success) {
                    Radvill.SwitchModule("Requests");
                } else {
                    alert("Beklager, men ingen kunne motta forespørselen din.");
                }
            });
            return false;
        });
        

    };

    askForAdvice.PopulateTemplate = function (template) {
        var html = $(template);
        var select = html.find('select');
        
        for (var i = 0; i < _categories.length; i++) {
            select.append('<option value="' + _categories[i].Value + '">' + _categories[i].Text + '</option>');
        }

        return html;
    };


    return askForAdvice;

})();