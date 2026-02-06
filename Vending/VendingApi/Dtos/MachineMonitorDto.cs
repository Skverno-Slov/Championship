namespace VendingApi.Dtos
{
    public class MachineMonitorDto
    {
        public string MachineId { get; set; } = null!;

        public int SerialNumber { get; set; }

        public string Name { get; set; } = null!;

        public string Location { get; set; } = null!;

        public string UserId { get; set; } = null!;

        public string ModelName { get; set; } = null!;

        public DateTime LastMaintenanceDate { get; set; }

        public decimal ContributedMoney { get; set; }

        public decimal CoinsChange { get; set; }

        public decimal BillsChange { get; set; }

        public string StatusName { get; set; } = null!;

        public int LastSale { get; set; }

        public decimal FullChange { get => CoinsChange + BillsChange; }

        public string FullName
        {
            get => $"{SerialNumber} \"{Name}\"";
        }

        public string FullDescription
        {
            get => $"{ModelName} {Location}";
        }
    }
}
