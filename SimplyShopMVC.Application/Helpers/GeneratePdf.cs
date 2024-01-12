using iText.IO.Image;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using SimplyShopMVC.Application.Interfaces;
using SimplyShopMVC.Application.ViewModels.Order;
using SimplyShopMVC.Domain.Interface;
using SimplyShopMVC.Domain.Model.Order;
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
            decimal orderValue = 0;
            decimal vatValue = 0;
            Table table = new Table(6);
            table.AddHeaderCell("lp.");
            table.AddHeaderCell("nazwa");
            table.AddHeaderCell("ilosc");
            table.AddHeaderCell("cena netto");
            table.AddHeaderCell("cena brutto");
            table.AddHeaderCell("wartosc");
            foreach (var orderItem in orderFromCart.orderItems)
            {
                lp++;
                var vat = _itemRepo.GetAllVatRate().FirstOrDefault(v => v.Id == orderItem.VatRateId);
                var priceNetto = (decimal)orderItem.PriceB / ((decimal)vat.Value / 100 + 1);
                var priceNString = priceNetto.ToString("N2");
                var priceBString = orderItem.PriceB.ToString("N2");
                var priceValue = orderItem.PriceB * orderItem.Quantity;
                table.AddCell(lp.ToString()).SetFontSize(8);
                table.AddCell(orderItem.Name).SetFontSize(8);
                table.AddCell(orderItem.Quantity.ToString()).SetFontSize(8);
                table.AddCell(priceNString).SetFontSize(8);
                table.AddCell(priceBString).SetFontSize(8);
                table.AddCell(priceValue.ToString()).SetFontSize(8);
                orderValue = orderValue + (priceNetto * orderItem.Quantity);
                vatValue = (decimal)vat.Value;
            }
            document.Add(table.SetHorizontalAlignment(iText.Layout.Properties.HorizontalAlignment.CENTER));
            decimal orderVat = orderValue * ((decimal)vatValue / 100 + 1) - orderValue;
            decimal orderB_value = orderValue + orderVat;
            Table table2 = new Table(3);
            table2.AddHeaderCell("wartosc netto");
            table2.AddHeaderCell("wartosc vat");
            table2.AddHeaderCell("wartosc brutto");
            table2.AddCell(orderValue.ToString("N2")).SetFontSize(8);
            table2.AddCell(orderVat.ToString("N2")).SetFontSize(8);
            table2.AddCell(orderB_value.ToString("N2")).SetFontSize(8);
            document.Add(table2.SetHorizontalAlignment(iText.Layout.Properties.HorizontalAlignment.RIGHT).SetMarginRight(100).SetMarginTop(20));
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
