namespace VendingApi.Dtos
{
    public class EfficiencyResponceDto
    {
        public int AllMashines { get; set; }
        public int WorkingMashines { get; set; }
        public int BrokenMashines { get; set; }
        public int ServedMashines { get; set; }
    }
}
