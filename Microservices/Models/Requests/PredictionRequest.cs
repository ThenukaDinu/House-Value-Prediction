using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Requests
{
    public class PredictionRequest
    {
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
    }
}
