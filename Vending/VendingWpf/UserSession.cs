namespace WEMMWpf
{
    public class UserSession
    {
        private static UserSession? _instance;

        private UserSession() { }

        public string? Jwt { get; set; }

        public static UserSession Instance
        {
            get
            {
                if (_instance is null)
                    _instance = new UserSession();
                return _instance;
            }
        }
    }
}
