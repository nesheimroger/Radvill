using System;
using System.Configuration;

namespace Radvill.Configuration
{
    public static class Points
    {
        public static int Accepted
        {
            get { return Int32.Parse(ConfigurationManager.AppSettings["Points.Accepted"]); }
        }

        public static int Declined
        {
            get { return Int32.Parse(ConfigurationManager.AppSettings["Points.Declined"]); }
        }
    }
}