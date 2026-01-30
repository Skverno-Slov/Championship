namespace WEMMApi.Dtos
{
    public class MachineDto
    {
        public string MachineId { get; set; } = null!;

        public int SerialNumber { get; set; }

        public string Name { get; set; } = null!;

        public string Location { get; set; } = null!;

        public string UserId { get; set; } = null!;
        public string ModelName { get; set; }

        public string CompanyName { get; set; }

        public string PlaceName { get; set; }
        public DateTime InstallDate { get; set; }

        public string FullAddress { get => $"{Location}\n{PlaceName}"; }
    }
}
