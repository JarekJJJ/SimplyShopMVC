using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimplyShopMVC.Application.Helpers
{
    public class xmlHelpers
    {
        public static string eanController(string ean)
        {
            if (!string.IsNullOrEmpty(ean))
            {
                if (ean[0] == '0')
                {
                    ean = ean.Remove(0, 1);
                }
            }
            return ean;
        }
    }
}
