namespace VendingApi.Dtos
{
    public class MonitorRequest
    {
        public List<string>? GeneralFilters { get; set; }
        public List<string>? OfferFilters { get; set; }
    }
}
