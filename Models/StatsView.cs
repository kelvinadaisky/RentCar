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
        public decimal Revenue { get; set; } 
    }
    public class RevenueByCar
    {
        public int CarId { get; set; }       
        public string CarModel { get; set; } 
        public decimal TotalRevenue { get; set; } 
    }


    public class TopCustomer
    {
        public string CustomerName { get; set; }
        public decimal TotalRevenue { get; set; }
    }

    public class MonthlyRevenueByAgency
    {
        public int AgencyId { get; set; }         
        public string AgencyName { get; set; }      
        public decimal MonthlyRevenue { get; set; } 
    }


}
