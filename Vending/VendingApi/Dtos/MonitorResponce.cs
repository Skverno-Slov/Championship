namespace VendingApi.Dtos
{
    public class MonitorResponce
    {
        public decimal TotalContributedMoney { get; set; }
        public decimal TotalChange { get; set; }

        public int TotalBroken { get; set; }
        public int TotalServing { get; set; }
        public int TotalWorking { get; set; }
        public int TotalMachines { get => TotalBroken + TotalServing + TotalWorking; }

        public List<MachineMonitorDto> Machines { get; set; } = null!;
    }
}
