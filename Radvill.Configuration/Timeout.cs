using System;
using System.Configuration;

namespace Radvill.Configuration
{
    public static class Timeout
    {
        public static int Respond
        {
            get { return Int32.Parse(ConfigurationManager.AppSettings["Timeout.Respond"]); }
        }

        public static int Answer
        {
            get { return Int32.Parse(ConfigurationManager.AppSettings["Timeout.Answer"]); }
        }
    }
}