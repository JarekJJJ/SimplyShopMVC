using iText.IO.Image;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using SimplyShopMVC.Application.ViewModels.Order;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimplyShopMVC.Application.Helpers
{
    public static class GeneratePdf
    {
        public static byte[] GenertateOrderPdf(OrderFromCartVm orderFromCart)
        {
            var stream = new MemoryStream();

            var writer = new PdfWriter(stream);
            var pdf = new PdfDocument(writer);
            var document = new Document(pdf);
            AddImageToPdf(document, "wwwroot/media/primehkr24.png", 200, 64, 20, 750);
            document.Add(new Paragraph($"numer Zamówienia: {orderFromCart.orderForList.NumberOrders}").SetMarginTop(100).SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER));
            document.Close();

            return stream.ToArray();
        }
        static void AddImageToPdf(Document document, string imagePath, float width, float height, float pozX, float pozY)
        {
            try
            {
                // Tworzenie obiektu Image na podstawie ścieżki do pliku obrazka
                iText.Layout.Element.Image image = new iText.Layout.Element.Image(ImageDataFactory.Create(imagePath));

                // Ustawienie szerokości i wysokości obrazka
                image.SetWidth(width);
                image.SetHeight(height);

                // Dodanie obrazka do lewego górnego rogu dokumentu
                document.Add(image.SetFixedPosition(pozX, pozY)); // Dostosuj współrzędne X i Y według potrzeb

            }
            catch (Exception ex)
            {
                Console.WriteLine($"Wystąpił błąd podczas dodawania obrazka: {ex.Message}");
            }
        }
    }
}
