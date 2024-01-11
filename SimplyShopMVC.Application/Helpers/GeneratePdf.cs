using iText.IO.Image;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using SimplyShopMVC.Application.Interfaces;
using SimplyShopMVC.Application.ViewModels.Order;
using SimplyShopMVC.Domain.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimplyShopMVC.Application.Helpers
{
    public class GeneratePdf: IGeneratePdf
    {
        private readonly IItemRepository _itemRepo;
        public GeneratePdf(IItemRepository itemRepository)
        {
            _itemRepo = itemRepository;
        }
        public  byte[] GenertateOrderPdf(OrderFromCartVm orderFromCart)
        {
            var stream = new MemoryStream();
            var writer = new PdfWriter(stream);
            var pdf = new PdfDocument(writer);
            var document = new Document(pdf);
            AddImageToPdf(document, "wwwroot/media/primehkr24.png", 200, 64, 20, 750);
            document.Add(new Paragraph($"numer Zamówienia: {orderFromCart.orderForList.NumberOrders}").SetMarginTop(100).SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER));
            int lp = 0;
            foreach(var orderItem in orderFromCart.orderItems)
            {
                lp++;
                var vat = _itemRepo.GetAllVatRate().FirstOrDefault(v => v.Id == orderItem.VatRateId);
                var prizeNetto = (decimal)orderItem.PriceB / ((decimal)vat.Value / 100 + 1);
                var prizeNString = prizeNetto.ToString("N2");
                var prizeBString = orderItem.PriceB.ToString("N2");
                document.Add(new Paragraph($"Lp.{lp}  {orderItem.Name}  {orderItem.Quantity} szt.  {prizeNString}").SetFontSize(8));

            }
            document.Close();
            return stream.ToArray();
        }
        static void AddImageToPdf(Document document, string imagePath, float width, float height, float pozX, float pozY)
        {
            try
            {              
                iText.Layout.Element.Image image = new iText.Layout.Element.Image(ImageDataFactory.Create(imagePath));
                // Ustawienie szerokości i wysokości obrazka
                image.SetWidth(width);
                image.SetHeight(height);               
                document.Add(image.SetFixedPosition(pozX, pozY)); 
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Wystąpił błąd podczas dodawania obrazka: {ex.Message}");
            }
        }
    }
}
