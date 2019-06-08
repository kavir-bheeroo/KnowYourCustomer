using System.Threading.Tasks;

namespace KnowYourCustomer.Common.Http.Interfaces
{
    public interface IIdentityServerClient
    {
        Task<string> RequestClientCredentialsTokenAsync();
    }
}