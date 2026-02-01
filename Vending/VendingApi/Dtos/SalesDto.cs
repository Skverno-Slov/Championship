namespace VendingApi.Dtos
{
    public class SalesDto
    {
        private int _servedToday;

        public decimal Balance { get; set; }
        public decimal Change { get => Balance / 4; }
        public decimal ProfitToday { get; set; }
        public decimal ProfitYestarday { get; set; }
        public decimal CollectedToday { get; set; }
        public decimal CollectedYestarday { get; set; }
        
        public bool IsServed
        {
            set
            {
                if (value)
                    _servedToday++;
            }
        }

        public int ServedToday { get => _servedToday; }
    }
}
