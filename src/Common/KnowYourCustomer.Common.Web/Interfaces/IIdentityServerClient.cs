using System.Threading.Tasks;

namespace KnowYourCustomer.Common.Web.Interfaces
{
    public interface IIdentityServerClient
    {
        Task<string> RequestClientCredentialsTokenAsync();
    }
}