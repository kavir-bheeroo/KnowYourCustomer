namespace KnowYourCustomer.Common.Web.Models
{
    public class WebResponse
    {
        public WebResponse() { }

        public WebResponse(string error) : this(error, null)
        {
        }

        public WebResponse(string error, string errorAdditionalInfo)
        {
            Error = error;
            ErrorAdditionalInfo = errorAdditionalInfo;
        }

        public string Error { get; set; }

        public string ErrorAdditionalInfo { get; set; }
    }
}