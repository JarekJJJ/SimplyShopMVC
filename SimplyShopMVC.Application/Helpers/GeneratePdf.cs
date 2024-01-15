using iText.IO.Font;
using iText.IO.Image;
using iText.Kernel.Font;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using SimplyShopMVC.Application.Interfaces;
using SimplyShopMVC.Application.ViewModels.Order;
using SimplyShopMVC.Domain.Interface;
using SimplyShopMVC.Domain.Model;
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
        private readonly ICompanySettingsRepository _companySettingsRepo;
        public static readonly String FONT = "wwwroot/media/font/arial.ttf";
        public GeneratePdf(IItemRepository itemRepository, ICompanySettingsRepository companySettingsRepo)
        {
            _itemRepo = itemRepository;
            _companySettingsRepo = companySettingsRepo;
        }
        public  byte[] GenertateOrderPdf(OrderFromCartVm orderFromCart)
        {
            var stream = new MemoryStream();
            var writer = new PdfWriter(stream);
            var pdf = new PdfDocument(writer);
            var document = new Document(pdf);
            PdfFont f = PdfFontFactory.CreateFont(FONT, PdfEncodings.IDENTITY_H);
            string status = "";
            if(orderFromCart.orderForList.IsAccepted==true)
            {
                status = " (zaakceptowane)";
            }
            else
            {
                status = " (oczekuje na akceptację)";
            }
            if (orderFromCart.orderForList.IsCompleted == true)
            {
                status = " (zrealizowano)";
            }
            if (orderFromCart.orderForList.IsCancelled == true)
            {
                status = " (anulowano)";
            }
                // wczytanie danych sprzedawcy
                var companyName = _companySettingsRepo.GetCompanySettings();
            string[] companyLine = new string[4];
            companyLine[0] = ($"{companyName.CompanyName}");
            companyLine[1] = ($"ul. {companyName.Street} {companyName.PostCode} {companyName.City}");
            companyLine[2] = ($"NIP: {companyName.NIP} Regon: {companyName.Regon}");
            companyLine[3] = ($"email: {companyName.Email} telefon: {companyName.Phone}");
            //Wczytaniie danych kupującego
            string[] clientName= new string[4];
            clientName[0] = ($"{orderFromCart.userDetail.FullName}");
            clientName[1] = ($"ul. {orderFromCart.userDetail.Street} {orderFromCart.userDetail.PostalCode} {orderFromCart.userDetail.City}");
            clientName[2] = ($" Nip: {orderFromCart.userDetail.NIP} {orderFromCart.userDetail.Country}");
            clientName[3] = ($"telefon: {orderFromCart.userDetail.PhoneNumber} email: {orderFromCart.userDetail.EmailAddress}");
           //Wczytanie logo
            AddImageToPdf(document, "wwwroot/media/primehkr24.png", 200, 64, 20, 750);
            //Generowaniie pdf
            document.Add(new Paragraph($"Data zamówienia: {companyName.City} {orderFromCart.orderForList.CreatedDate.ToString("dd-MM-yyyy")}").SetTextAlignment(iText.Layout.Properties.TextAlignment.RIGHT).SetFont(f));
            for (int i = 0; i <= 5; i++)
            {
                document.Add(new Paragraph(" "));
            }
            document.Add(new Paragraph("Dane sprzedawcy: ").SetBold());
            for (int i = 0; i <= 3; i++)
            {
                document.Add(new Paragraph(companyLine[i]).SetFont(f).SetMultipliedLeading(0.5f).SetFontSize(10));
            }
            document.Add(new Paragraph("Dane kupującego: ").SetBold().SetTextAlignment(iText.Layout.Properties.TextAlignment.RIGHT).SetMarginRight(30).SetFont(f));
            for (int i = 0; i <= 3; i++)
            {
                document.Add(new Paragraph(clientName[i]).SetFont(f).SetMultipliedLeading(0.5f).SetFontSize(10).SetTextAlignment(iText.Layout.Properties.TextAlignment.RIGHT));
            }
            document.Add(new Paragraph($"numer zamówienia: {orderFromCart.orderForList.NumberOrders} {status}").SetMarginTop(20).SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER));
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
            VatRate vatRate= new VatRate();
            foreach (var orderItem in orderFromCart.orderItems)
            {
                lp++;
                var vat = _itemRepo.GetAllVatRate().FirstOrDefault(v => v.Id == orderItem.VatRateId);
                var priceNetto = (decimal)orderItem.PriceB / ((decimal)vat.Value / 100 + 1);
                var priceNString = priceNetto.ToString("N2");
                var priceBString = orderItem.PriceB.ToString("N2");
                var priceValue = orderItem.PriceB * orderItem.Quantity;
                table.AddCell(lp.ToString()).SetFontSize(8);
                table.AddCell(orderItem.Name).SetFontSize(8).SetFont(f);
                table.AddCell(orderItem.Quantity.ToString()).SetFontSize(8);
                table.AddCell(priceNString).SetFontSize(8);
                table.AddCell(priceBString).SetFontSize(8);
                table.AddCell(priceValue.ToString()).SetFontSize(8);
                orderValue = orderValue + (priceNetto * orderItem.Quantity);
                vatValue = (decimal)vat.Value;
                vatRate = vat;
            }
            document.Add(table.SetHorizontalAlignment(iText.Layout.Properties.HorizontalAlignment.RIGHT).SetMarginRight(30).SetWidth(pdf.GetDefaultPageSize().GetWidth()-80));
            decimal orderVat = orderValue * ((decimal)vatValue / 100 + 1) - orderValue;
            decimal orderB_value = orderValue + orderVat;
            Table table2 = new Table(4);
            table2.AddHeaderCell("wartosc netto");
            table2.AddHeaderCell("Stawka VAT");
            table2.AddHeaderCell("wartosc vat");
            table2.AddHeaderCell("wartosc brutto");
            table2.AddCell(orderValue.ToString("N2")).SetFontSize(8);
            table2.AddCell($"{vatRate.Value}%").SetFontSize(8);
            table2.AddCell(orderVat.ToString("N2")).SetFontSize(8);
            table2.AddCell(orderB_value.ToString("N2")).SetFontSize(8);
            document.Add(table2.SetHorizontalAlignment(iText.Layout.Properties.HorizontalAlignment.RIGHT).SetMarginRight(10).SetMarginTop(10));
            if(orderFromCart.orderForList.IsAccepted == false)
            {
                document.Add(new Paragraph($"Zamówienie nie jest jeszcze zatwierdzone. Płatności prosimy dokonać dopero po zatwierdzeniu przez sprzedawcę! ").SetFont(f));
            }
            else
            {
                document.Add(new Paragraph($"Do zapłaty: {orderB_value.ToString("N2")}").SetFont(f).SetBold().SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER));
                document.Add(new Paragraph($"").SetFont(f));
                document.Add(new Paragraph($"Płatność: {orderFromCart.orderForList.PaymentMethod}").SetFont(f));
                document.Add(new Paragraph($"Przelewu prosimy dokonać na konto: ").SetFont(f));
                document.Add(new Paragraph($"Nazwa Banku: {companyName.BankName}").SetFont(f));
                document.Add(new Paragraph($"Numer konta: {companyName.BankAccount}").SetFont(f));
            }
            document.Add(new Paragraph($""));
            document.Add(new Paragraph($"Dodatkowe informacje dotyczące wysyłki zamówienia:").SetFont(f));
            document.Add(new Paragraph($"{orderFromCart.orderForList.ShipingDescription}").SetFont(f));

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
