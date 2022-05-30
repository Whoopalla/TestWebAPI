namespace TestWebAPI.Core.Interfaces {
    public interface ITokenService {
        Task<string> GetTokenAsync(string login);
    }
}
