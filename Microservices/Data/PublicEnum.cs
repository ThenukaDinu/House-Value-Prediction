namespace Data
{
    public class PublicEnum
    {
        public enum ListingStatus
        {
            Sold = 0,
            UnAvailable = 1,
            Available = 2
        }

        public enum InquiryStatus
        {
            Open = 0,
            Closed = 1,
            Pendding = 2,
            Responded = 3
        }

        public enum NotificationType
        {
            Inquery = 0,
            Listing = 1,
            AccountVerification = 2,
        }

        public enum NotificationStatus
        {
            Pending = 0,
            Sent = 1,
            Failed = 2,
            Queued = 3,
        }

        public enum EmailTemplateType
        {
            Welcome = 0,
            PredictionSuccess = 1,
            PredictionFailure = 2,
            PredictionPendding = 3,
            WebScrapingFinished = 4,
            ModelTrainingFinished = 5
        }

        public enum InterestType
        {
            Location = 0,
            NoOfRooms = 1,
            NoOfBathRooms = 2,
            NoOfVehiclesInGarage = 3,
            ConstructionYear = 4,
            TotalSquareFeet = 5,
            RemodelYear = 6,
        }
    }
}
