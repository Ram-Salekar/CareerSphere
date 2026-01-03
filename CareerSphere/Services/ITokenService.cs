using CareerSphere.ApiModels.AuthModels;
using CareerSphere.Models.UserTableModel;

namespace CareerSphere.Services
{
    public interface ITokenService
    {
        string CreateToken(User details);
    }
}
