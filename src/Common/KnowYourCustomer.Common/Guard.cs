using System;
using System.Globalization;

namespace KnowYourCustomer.Common
{
    public static class Guard
    {
        public static TArgument IsNotNull<TArgument>(TArgument argument, string parameterName = null)
        {
            if (argument == null)
            {
                throw new ArgumentNullException(parameterName, string.Format(CultureInfo.InvariantCulture, "{0} is null.", parameterName));
            }

            return argument;
        }
    }
}