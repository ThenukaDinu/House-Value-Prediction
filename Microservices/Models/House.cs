namespace Models
{
    public class House : CommonModel
    {
        public int Id { get; set; }
        public string Location { get; set; }
        public int OverallQual { get; set; }
        public int YearBuilt { get; set; }
        public int YearRemodAdd { get; set; }
        public int TotalBsmtSF { get; set; }
        public int A1stFlrSF { get; set; }
        public int GrLivArea { get; set; }
        public int FullBath { get; set; }
        public int TotRmsAbvGrd { get; set; }
        public int GarageCars { get; set; }
        public int GarageArea { get; set; }
        public double PredictedPrice { get; set; }
        public double PredictedPriceLKR { get; set; }
        public Guid? UserId { get; set; }
        public ICollection<Inquiry> Inquiries { get; set; }
        public Listing Listing { get; set; }
    }
}
