using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Laboratory_Activity_17
{
    class Coupon
    {
        public string Code;
        public string Type;
        public double Amount;

        public Coupon(string code)
        {
            Code = code.ToUpper().Trim();

            if (Code.EndsWith("OFF") && Code.Contains("%") == false) 
            {
                Type = "FIXED";
                Amount = double.Parse(Code.Replace("OFF", ""));
            }
            else if (Code.Contains("%")) 
            {
                Type = "PERCENT";
                Amount = double.Parse(Code.Replace("%", ""));
            }
            else if (Code == "FREESHIP")
            {
                Type = "SHIP";
                Amount = 5;
            }
        }

        public double Apply(double total)
        {
            if (Type == "FIXED") return total - Amount;
            else if (Type == "PERCENT") return total - (total * Amount / 100);
            else if (Type == "SHIP") return total - Amount;
            return total;
        }
    }

    class Program
    {
        static void Main()
        {
            Console.Write("Cart total: ");
            double cartTotal = double.Parse(Console.ReadLine());

            Console.Write("Enter coupons separated by comma: ");
            string[] codes = Console.ReadLine().Split(',');

            List<Coupon> coupons = new List<Coupon>();
            foreach (string c in codes)
            {
                coupons.Add(new Coupon(c));
            }

            double bestTotal = cartTotal;
            string bestStack = "No coupons";

            foreach (var cp in coupons)
            {
                double newTotal = cp.Apply(cartTotal);
                if (newTotal < bestTotal)
                {
                    bestTotal = newTotal;
                    bestStack = cp.Code;
                }
            }

            for (int i = 0; i < coupons.Count; i++)
            {
                for (int j = i + 1; j < coupons.Count; j++)
                {
                    if (coupons[i].Type == "PERCENT" && coupons[j].Type == "PERCENT")
                        continue;

                    double total = coupons[j].Apply(coupons[i].Apply(cartTotal));
                    if (total < bestTotal)
                    {
                        bestTotal = total;
                        bestStack = coupons[i].Code + " + " + coupons[j].Code;
                    }
                }
            }

            Console.WriteLine("\nBest stack: " + bestStack);
            Console.WriteLine("Final payable: " + bestTotal.ToString("0.00"));
            Console.WriteLine("You saved: " + (cartTotal - bestTotal).ToString("0.00"));
        }
    }
}
    