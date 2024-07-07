using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Personal_Financial_Planner
{
    public class DataByMonth
    {

      
        public List<ExpenseItem> FoodExpenses { get; set; }
        public List<ExpenseItem> ClothesExpenses { get; set; }
        public List<ExpenseItem> EntertainmentExpenses { get; set; }
        public List<ExpenseItem> UtilityExpenses { get; set; }
        public decimal MonthlyIncome { get; set; }
        public decimal AdditionalIncome { get; set; }
        public decimal MonthlySavings { get; set; }
        public decimal SpendableAmount { get; set; }
        public decimal TransportCost { get; set; }


        public DataByMonth()
        {
            FoodExpenses = new List<ExpenseItem>();
            ClothesExpenses = new List<ExpenseItem>();
            EntertainmentExpenses = new List<ExpenseItem>();
            UtilityExpenses = new List<ExpenseItem>();
        }
    }
    
}
