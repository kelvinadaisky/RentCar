namespace RentCar.Models
{
    public class TeslimatViewModel
    {
        public int IdSozlesme { get; set; }
        public string MusteriAdi { get; set; } // Optional: Display customer name
        public string AracPlaka { get; set; }  // Optional: Display car plate
        public string HasarDurumu { get; set; }
    }

}
