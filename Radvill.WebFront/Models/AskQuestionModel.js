Radvill.Models.AskQuestionModel = function (categories) {
    this.Question = ko.observable();
    this.Category = ko.observable();
    this.Categories = categories;
};

