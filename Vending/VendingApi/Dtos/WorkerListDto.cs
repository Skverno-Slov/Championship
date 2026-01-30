namespace WEMMApi.Dtos
{
    public class WorkerListDto
    {
        public int WorkerId { get; set; }

        public string UserId { get; set; } = null!;

        public string LastName { get; set; } = null!;

        public bool IsManager { get; set; }

        public bool IsEngineer { get; set; }

        public bool IsTechnician { get; set; }
    }
}
