namespace WEMMApi.Dtos
{
    public class TokenResponce
    {
        public string JwtToken { get; set; } = null!;
        public string? RefreshToken { get; set; }
    }
}
