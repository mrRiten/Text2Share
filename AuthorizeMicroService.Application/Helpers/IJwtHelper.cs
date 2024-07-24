namespace AuthorizeMicroService.Application.Helpers
{
    public interface IJwtHelper
    {
        public string GenerateJwtToken(string userName);
    }
}
