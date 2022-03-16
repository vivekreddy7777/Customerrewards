using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Results;
using HttpPostAttribute = System.Web.Http.HttpPostAttribute;
using RouteAttribute = Microsoft.AspNetCore.Mvc.RouteAttribute;
using System.Text.Json.Serialization;
using Newtonsoft.Json;


namespace rewards.Controllers
{

    [ApiController]
    public class RewardsController : Controller
    {


        [System.Web.Http.HttpGet]
        [Route("rewards/points")]
        public string Points()

        {
            return PointsDetail();
        }

        [System.Web.Http.HttpGet]
        [Route("rewards/customertotals")]
        public string CustomersAndTotal()

        {
            return CustomersAndTotals();
        }

        [System.Web.Http.HttpGet]
        [Route("rewards/monthlytotals")]
        public string CustomerAndTotalsByMonth()

        {
            return CustomersAndTotalsByMonth();
        }

        private static string CustomersAndTotals()
        {
            List<Transaction> trans = GenerateRandomTrans(100);
            var withAmounts = AddPointsToTransactions(trans);

            var totalsByCustomer = withAmounts
                        .GroupBy(f => new { f.customerName })
                        .Select(group => new { customer = group.Key, total = group.Sum(f => f.points) });

            return JsonConvert.SerializeObject(totalsByCustomer);
        }

        private static string CustomersAndTotalsByMonth()
        {
            List<Transaction> trans = GenerateRandomTrans(100);
            var withAmounts = AddPointsToTransactions(trans);

            var totalsByMonth = withAmounts
                        .GroupBy(f => new { f.customerName, f.transactionDate.Month })
                        .Select(group => new { customer = group.Key, total = group.Sum(f => f.points) });

            return JsonConvert.SerializeObject(totalsByMonth);
        }

        private static string PointsDetail()
        {
            List<Transaction> trans = GenerateRandomTrans(100);
            var withAmounts = AddPointsToTransactions(trans);
            return JsonConvert.SerializeObject(withAmounts);
        }

        private static List<Transaction> AddPointsToTransactions(List<Transaction> list)
        {
            foreach (var rec in list)
            {
                rec.points = GetPointByAmount(rec.amount);
            }

            return list;
        }

        private static DateTime GetRandomDate(int seed)
        {
            var random = new Random(seed);
            return DateTime.Now.AddDays(-random.Next(90));
        }

        private static string GetRadomCustomer(int seed)
        {
            var random = new Random(seed);
            var list = new List<string> { "Customer1", "Customer2", "Customer3", "Customer4" };
            int index = random.Next(list.Count);
            return list[index];
        }

        private static int GetRadomAmount(int seed)
        {
            var random = new Random(seed);
            return random.Next(1000);
        }

        private static List<Transaction> GenerateRandomTrans(int tot)
        {
            List<Transaction> list = new List<Transaction>();

            for (int i = 0; i <= tot; i++)
            {
                Transaction tn = new Transaction
                {
                    transactionId = Guid.NewGuid().ToString(),
                    amount = GetRadomAmount(i),
                    transactionDate = GetRandomDate(i),
                    customerName = GetRadomCustomer(i)
                };
                list.Add(tn);
            }

            return list;
        }

        private static int GetPointByAmount(int amount)
        {
            int result = 0;
            int over50 = amount - 50;

            if (over50 > 0)
            {
                int over100 = amount - 100;
                result = over100 > 0 ? ((over100 * 2) + 50) : over50;
            }

            return result;
        }
    }
    public class Transaction
    {
        public string transactionId { get; set; }
        public DateTime transactionDate { get; set; }
        public string customerName { get; set; }
        public int amount { get; set; }
        public int points { get; set; }

    }

}
