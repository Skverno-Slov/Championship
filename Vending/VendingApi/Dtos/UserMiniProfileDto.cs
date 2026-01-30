namespace WEMMApi.Dtos
{
    public class UserMiniProfileDto
    {
        public string UserId { get; set; } = null!;

        public string FullName { get; set; } = null!;

        public string RoleName { get; set; } = null!;

        public string? Image { get; set; }

    }
}
