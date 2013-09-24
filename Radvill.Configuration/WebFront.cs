using System.Configuration;

namespace Radvill.Configuration
{
    public class WebFront
    {
        public static string Url
        {
            get { return ConfigurationManager.AppSettings["WebFront.Url"]; }
        }
    }
}