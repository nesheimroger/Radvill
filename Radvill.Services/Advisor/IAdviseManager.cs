namespace Radvill.Services.Advisor
{
    public interface IAdviseManager
    {
        bool SubmitQuestion(int userid, int categoryId, string question);
    }
}