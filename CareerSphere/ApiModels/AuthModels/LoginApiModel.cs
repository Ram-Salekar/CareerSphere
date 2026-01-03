namespace CareerSphere.ApiModels.AuthModels
{
    public class LoginApiModel
    {
        public string emailOrUsername { get; set; } = string.Empty;
        public string password { get; set; } = string.Empty;
    }
}
