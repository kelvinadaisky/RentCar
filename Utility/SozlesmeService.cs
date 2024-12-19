using RentCar.Models;

namespace RentCar.Utility
{
    public class SozlesmeService
    {
        private readonly RentCarContext _context;

        public SozlesmeService(RentCarContext context)
        {
            _context = context;
        }

        public void UpdateSozlesmeStatus(int sozlesmeId)
        {
            // Retrieve the contract (Sozlesme)
            var sozlesme = _context.Sozlesmes.FirstOrDefault(s => s.IdSozlesme == sozlesmeId);
            if (sozlesme == null) return;

            // Check if the delivery (Teslimat) is completed
            var teslimat = _context.Teslimats.FirstOrDefault(t => t.IdSozlesme == sozlesmeId && t.Durum == "Teslim edildi");

            // Retrieve the associated invoice (Fatura)
            var fatura = _context.Faturas.FirstOrDefault(f => f.IdSozlesme == sozlesmeId);

            // Validate the payment (Odeme)
            var isInvoicePaid = fatura != null &&
                                fatura.Odeme != null &&
                                fatura.Odeme.Tutar.HasValue &&
                                fatura.Odeme.Tutar.Value >= fatura.OdenenTutar;

            // Update the contract status if delivery is completed and payment is made
            if (teslimat != null && isInvoicePaid)
            {
                sozlesme.Durum = "Tamamlandi";
                _context.Sozlesmes.Update(sozlesme);
                _context.SaveChanges();
            }
        }
    }
}
