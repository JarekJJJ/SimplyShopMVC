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
                document.Add(new Paragraph($"numer Zamówienia: {orderFromCart.orderForList.NumberOrders}"));
                document.Close();
            
            return stream.ToArray();
        }
    }
}
