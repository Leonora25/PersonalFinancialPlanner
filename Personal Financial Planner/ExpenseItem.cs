using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Personal_Financial_Planner
{
    public class ExpenseItem
    {
        public string Name { get; set; }
        public decimal Amount { get; set; }
        public DateTime Date { get; set; }



        public ExpenseItem(string name, decimal amount, DateTime date)
        {
            Name = name;
            Amount = amount;
            Date = date;
        }
        public override string ToString()
        {
            return $"{Name} {Amount}den. {Date.ToShortDateString()}";
        }
    }

}
