namespace RentCar.Models
{
    public class StatsView
    {
        public int Count { get; set; }

    }
    public class TotalCarsCount
    {
        public int Count { get; set; }
    }

    public class TotalCustomersCount
    {
        public int Count { get; set; }
    }
    public class MonthlyRevenue
    {
        public decimal Revenue { get; set; } // Use decimal for currency values
    }
    public class RevenueByCar
    {
        public int CarId { get; set; }       // Matches the 'carid' column
        public string CarModel { get; set; }  // Matches the 'carmodel' column
        public decimal TotalRevenue { get; set; } // Matches the 'totalrevenue' column
    }


    public class TopCustomer
    {
        public string CustomerName { get; set; }
        public decimal TotalRevenue { get; set; }
    }

    public class MonthlyRevenueByAgency
    {
        public int AgencyId { get; set; }          // Matches AgencyId
        public string AgencyName { get; set; }      // Matches AgencyName
        public decimal MonthlyRevenue { get; set; } // Matches MonthlyRevenue
    }


}
