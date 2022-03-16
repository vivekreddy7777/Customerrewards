using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace rewards
{
    public class Rewards
    {
        public int CustomerId { get; set; }
        public string CustomerName { get; set; }

        public Dictionary<int, int> MonthlyRewards { get; set; }

        public int TotalRewards { get; set; }
    }
}
