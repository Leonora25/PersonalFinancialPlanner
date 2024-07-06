using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Printing;
using System.Drawing.Text;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using Excel = Microsoft.Office.Interop.Excel;
using System.IO;


namespace proba_proekt
{
    public partial class UserInfo : Form
    {
        private User currentUser;

        private decimal totalFoodExpenses = 0;
        private decimal totalClothesExpenses = 0;
        private decimal totalUtilityExpenses = 0;
        private decimal totalEntertainmentExpenses = 0;
        private decimal totalOverallExpenses = 0;
        public UserInfo(User currentUser)
        {
            InitializeComponent();
            this.StartPosition = FormStartPosition.CenterScreen;
            this.currentUser = currentUser;

            LoadUserData();
            InitializeDatePickers();
        }

        private void InitializeDatePickers()
        {
            dtpMonth.Format = DateTimePickerFormat.Custom;
            dtpMonth.CustomFormat = "MM/yyyy";
            dtpMonth.ShowUpDown = false;

            dtpMonth.ValueChanged += dtpMonth_ValueChanged;

            UpdateExpenseDateLimits(dtpMonth.Value);
        }
        private void dtpMonth_ValueChanged(object sender, EventArgs e)
        {
            UpdateExpenseDateLimits(dtpMonth.Value);
        }
        private void UpdateExpenseDateLimits(DateTime selectedMonth)
        {
            DateTime firstDayOfMonth = new DateTime(selectedMonth.Year, selectedMonth.Month, 1);
            DateTime lastDayOfMonth = firstDayOfMonth.AddMonths(1).AddDays(-1);

            dtpExpenseDate.MinDate = firstDayOfMonth;
            dtpExpenseDate.MaxDate = lastDayOfMonth;
            dtpExpenseDate.Value = firstDayOfMonth; 
        }

        private void LoadUserData()
        {
            tbUsername.Text = currentUser.Username;
            nudMonthlyIncome.Value = currentUser.UserData.MonthlyIncome;
            nudAdditionalIncome.Value = currentUser.UserData.AdditionalIncome;
            txtMonthlySavings.Text = currentUser.UserData.MonthlySavings.ToString();
            nudTransportCost.Value = currentUser.UserData.TransportCost;
            txtSpendableAmount.Text = currentUser.UserData.SpendableAmount.ToString();

            PopulateListBox(lbFood, currentUser.UserData.FoodExpenses);
            PopulateListBox(lbClothes, currentUser.UserData.ClothesExpenses);
            PopulateListBox(lbEntertainment, currentUser.UserData.EntertainmentExpenses);
            PopulateListBox(lbUtility, currentUser.UserData.UtilityExpenses);




            totalFoodExpenses = GetTotalExpense(currentUser.UserData.FoodExpenses);
            totalClothesExpenses = GetTotalExpense(currentUser.UserData.ClothesExpenses);
            totalUtilityExpenses = GetTotalExpense(currentUser.UserData.UtilityExpenses);
            totalEntertainmentExpenses = GetTotalExpense(currentUser.UserData.EntertainmentExpenses);

            tbTotalFoodExpenses.Text = totalFoodExpenses.ToString();
            tbTotalClothesExpenses.Text = totalClothesExpenses.ToString();
            tbTotalUtilityExpenses.Text = totalUtilityExpenses.ToString();
            tbTotalEntertainmentExpenses.Text = totalEntertainmentExpenses.ToString();

            UpdateOverallTotal();
            CalculateAndDisplaySavings();

        }
        
        private void PopulateListBox(ListBox listBox, List<ExpenseItem> expenses)
        {
            listBox.Items.Clear();
            foreach (var expense in expenses)
            {
                listBox.Items.Add($"{expense.Name} {expense.Amount}ден. {expense.Date.ToShortDateString()}");
            }
        }

        private void SaveUserData()
        {
            currentUser.UserData.MonthlyIncome = nudMonthlyIncome.Value;
            currentUser.UserData.AdditionalIncome = nudAdditionalIncome.Value;
            currentUser.UserData.MonthlySavings = decimal.Parse(txtMonthlySavings.Text);
            currentUser.UserData.SpendableAmount = decimal.Parse(txtSpendableAmount.Text);
            currentUser.UserData.TransportCost = nudTransportCost.Value;

            UserDataManager.SaveUserData(currentUser);
        }
        private void CalculateAndDisplaySavings()
        {
            decimal monthlyIncome = nudMonthlyIncome.Value;
            decimal additionalIncome = nudAdditionalIncome.Value;
            decimal transportCost = nudTransportCost.Value;
            decimal savingsPercentage = 0.15m; // 15% savings
            decimal savingsAmount = (monthlyIncome + additionalIncome) * savingsPercentage;
            decimal spendableAmount = (monthlyIncome + additionalIncome) - savingsAmount - transportCost;

            currentUser.UserData.MonthlySavings = savingsAmount;


            spendableAmount -= GetTotalExpense(currentUser.UserData.FoodExpenses);
            spendableAmount -= GetTotalExpense(currentUser.UserData.ClothesExpenses);
            spendableAmount -= GetTotalExpense(currentUser.UserData.EntertainmentExpenses);
            spendableAmount -= GetTotalExpense(currentUser.UserData.UtilityExpenses);

            currentUser.UserData.SpendableAmount = spendableAmount;

            txtMonthlySavings.Text = savingsAmount.ToString();
            txtSpendableAmount.Text = spendableAmount.ToString();
        }
        private string GetSelectedCategory()
        {
            foreach (Control control in grpCategories.Controls)
            {
                if (control is RadioButton radioButton && radioButton.Checked)
                {
                    return radioButton.Text;
                }
            }
            return null;
        }

        private void btnAddExpense_Click(object sender, EventArgs e)
        {
            string expenseName = txtExpenseName.Text;
            DateTime expenseDate = dtpExpenseDate.Value;
            decimal expenseAmount = nudExpenseAmount.Value;



            if (string.IsNullOrEmpty(expenseName))
            {
                MessageBox.Show("Внесете име за трошокот.");
                return;
            }


            if (expenseAmount <= 0)
            {
                MessageBox.Show("Внесете валиден износ за трошокот.");
                return;
            }

            string selectedCategory = GetSelectedCategory();
            if (selectedCategory == null)
            {
                MessageBox.Show("Селектирајте категорија");
                return;
            }

            decimal spendableAmount = decimal.Parse(txtSpendableAmount.Text);
            if (spendableAmount < expenseAmount)
            {
                MessageBox.Show("Нема доволно пари за трошење.");
                return;
            }



            ExpenseItem newExpense = new ExpenseItem(expenseName, expenseAmount, expenseDate);

            switch (selectedCategory)
            {
                case "Храна":
                    currentUser.UserData.FoodExpenses.Add(newExpense);
                    PopulateListBox(lbFood, currentUser.UserData.FoodExpenses);
                    UpdateCategoryTotal("Храна", expenseAmount); 
                    break;
                case "Облека":
                    currentUser.UserData.ClothesExpenses.Add(newExpense);
                    PopulateListBox(lbClothes, currentUser.UserData.ClothesExpenses);
                    UpdateCategoryTotal("Облека", expenseAmount); 
                    break;
                case "Забава/релаксација":
                    currentUser.UserData.EntertainmentExpenses.Add(newExpense);
                    PopulateListBox(lbEntertainment, currentUser.UserData.EntertainmentExpenses);
                    UpdateCategoryTotal("Забава/релаксација", expenseAmount); 
                    break;
                case "Одржување":
                    currentUser.UserData.UtilityExpenses.Add(newExpense);
                    PopulateListBox(lbUtility, currentUser.UserData.UtilityExpenses);
                    UpdateCategoryTotal("Одржување", expenseAmount); 
                    break;
                default:
                    break;
            }

            SaveUserData();
            LoadUserData();
            UpdateOverallTotal();

            txtExpenseName.Text = "";
            nudExpenseAmount.Value = 0;
            ClearRadioButtons();

        }

        private void UpdateCategoryTotal(string categoryName, decimal expenseAmount)
        {
            switch (categoryName)
            {
                case "Храна":
                    totalFoodExpenses += expenseAmount;
                    tbTotalFoodExpenses.Text = totalFoodExpenses.ToString(); 
                    break;
                case "Облека":
                    totalClothesExpenses += expenseAmount;
                    tbTotalClothesExpenses.Text = totalClothesExpenses.ToString();
                    break;
                case "Одржување":
                    totalUtilityExpenses += expenseAmount;
                    tbTotalUtilityExpenses.Text = totalUtilityExpenses.ToString(); 
                    break;
                case "Забава/релаксација":
                    totalEntertainmentExpenses += expenseAmount;
                    tbTotalEntertainmentExpenses.Text = totalEntertainmentExpenses.ToString();
                    break;
                default:
                    break;
            }

            UpdateOverallTotal();
        }


        private void UpdateOverallTotal()
        {
            decimal totalFoodExpenses = decimal.Parse(tbTotalFoodExpenses.Text);
            decimal totalClothesExpenses = decimal.Parse(tbTotalClothesExpenses.Text);
            decimal totalUtilityExpenses = decimal.Parse(tbTotalUtilityExpenses.Text);
            decimal totalEntertainmentExpenses = decimal.Parse(tbTotalEntertainmentExpenses.Text);
            decimal transport = nudTransportCost.Value;

            decimal totalOverallExpenses = totalFoodExpenses + totalClothesExpenses + totalUtilityExpenses + totalEntertainmentExpenses+transport;

            tbTotalOverallExpenses.Text = totalOverallExpenses.ToString();
        }


        private void ClearRadioButtons()
        {
            foreach (Control control in grpCategories.Controls)
            {
                if (control is RadioButton radioButton)
                {
                    radioButton.Checked = false;
                }
            }

        }


        private decimal GetTotalExpense(List<ExpenseItem> expenses)
        {
            decimal total = 0;
            foreach (var expense in expenses)
            {
                total += expense.Amount;
            }
            return total;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            SaveUserData();
        }

        private void nudMonthlyIncome_ValueChanged(object sender, EventArgs e)
        {
            CalculateAndDisplaySavings();
        }

        private void nudTransportCost_ValueChanged(object sender, EventArgs e)
        {
            CalculateAndDisplaySavings();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            decimal spendableAmount = decimal.Parse(txtSpendableAmount.Text);

            var listBoxExpensePairs = new List<(ListBox listBox, List<ExpenseItem> expenses)>
                {
                    (lbFood, currentUser.UserData.FoodExpenses),
                    (lbClothes, currentUser.UserData.ClothesExpenses),
                    (lbEntertainment, currentUser.UserData.EntertainmentExpenses),
                    (lbUtility, currentUser.UserData.UtilityExpenses)
                };

            foreach (var pair in listBoxExpensePairs)
            {
                var selectedItems = pair.listBox.SelectedItems;

                foreach (var selectedItem in selectedItems)
                {
                    string selectedText = selectedItem.ToString();

                    var expense = pair.expenses.FirstOrDefault(el =>
                        $"{el.Name} {el.Amount}ден. {el.Date.ToShortDateString()}" == selectedText);

                    if (expense != null)
                    {
                        spendableAmount += expense.Amount; 
                        pair.expenses.Remove(expense); 
                    }
                }

                PopulateListBox(pair.listBox, pair.expenses);
            }

            txtSpendableAmount.Text = spendableAmount.ToString();
            SaveUserData();
            LoadUserData();

        }

        private void logOutbtn_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Дали сте сигурни дека сакате да се одјавите?", "Одјава", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                SaveUserData();
                this.Close();

                LoginForm loginForm = new LoginForm();
                loginForm.Show();
            }
        }

        private void btnShowChart_Click(object sender, EventArgs e)
        {
            Dictionary<string, decimal> expenseData = new Dictionary<string, decimal>
            {
                { "Храна", GetTotalExpense(currentUser.UserData.FoodExpenses) },
                { "Облека", GetTotalExpense(currentUser.UserData.ClothesExpenses) },
                { "Забава и релаксација", GetTotalExpense(currentUser.UserData.EntertainmentExpenses) },
                { "Одржување", GetTotalExpense(currentUser.UserData.UtilityExpenses) }
            };
            ChartForm chartForm = new ChartForm(expenseData);
            chartForm.Show();
        }

        private void nudAdditionalIncome_ValueChanged(object sender, EventArgs e)
        {
            CalculateAndDisplaySavings();
        }

        private void excelToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ExportToExcel();
        }
        private void ExportToExcel()
        {
            Excel.Application xlApp = new Excel.Application();
            if (xlApp == null)
            {
                MessageBox.Show("Excel is not properly installed!!");
                return;
            }

            Excel.Workbook xlWorkBook;
            Excel.Worksheet xlWorkSheet;
            object misValue = System.Reflection.Missing.Value;

            xlWorkBook = xlApp.Workbooks.Add(misValue);
            xlWorkSheet = (Excel.Worksheet)xlWorkBook.Worksheets.get_Item(1);

            xlWorkSheet.Cells[1, 1] = "Корисничко име:";
            xlWorkSheet.Cells[1, 2] = currentUser.Username;

            Excel.Range usernameRange = xlWorkSheet.get_Range("A1", "A1");
            usernameRange.Font.Bold = true;

            xlWorkSheet.Cells[3, 1] = "Категорија";
            xlWorkSheet.Cells[3, 2] = "Име на трошок";
            xlWorkSheet.Cells[3, 3] = "Цена на трошок";
            xlWorkSheet.Cells[3, 4] = "Датум на трошок";

            Excel.Range headersRange = xlWorkSheet.get_Range("A3", "D3");
            headersRange.Font.Bold = true;

            
            int row = 4;

            // Food Expenses
            decimal totalFood = 0;
            foreach (var expense in currentUser.UserData.FoodExpenses)
            {
                xlWorkSheet.Cells[row, 1] = "Храна";
                xlWorkSheet.Cells[row, 2] = expense.Name;
                xlWorkSheet.Cells[row, 3] = expense.Amount;
                xlWorkSheet.Cells[row, 4] = expense.Date.ToShortDateString();
                totalFood += expense.Amount;
                row++;
            }

            // Clothes Expenses
            decimal totalClothes = 0;
            foreach (var expense in currentUser.UserData.ClothesExpenses)
            {
                xlWorkSheet.Cells[row, 1] = "Облека";
                xlWorkSheet.Cells[row, 2] = expense.Name;
                xlWorkSheet.Cells[row, 3] = expense.Amount;
                xlWorkSheet.Cells[row, 4] = expense.Date.ToShortDateString();
                totalClothes += expense.Amount;
                row++;
            }

            // Entertainment Expenses
            decimal totalEntertainment = 0;
            foreach (var expense in currentUser.UserData.EntertainmentExpenses)
            {
                xlWorkSheet.Cells[row, 1] = "Забава/релаксација";
                xlWorkSheet.Cells[row, 2] = expense.Name;
                xlWorkSheet.Cells[row, 3] = expense.Amount;
                xlWorkSheet.Cells[row, 4] = expense.Date.ToShortDateString();
                totalEntertainment += expense.Amount;
                row++;
            }

            // Utility Expenses
            decimal totalUtility = 0;
            foreach (var expense in currentUser.UserData.UtilityExpenses)
            {
                xlWorkSheet.Cells[row, 1] = "Одржување";
                xlWorkSheet.Cells[row, 2] = expense.Name;
                xlWorkSheet.Cells[row, 3] = expense.Amount;
                xlWorkSheet.Cells[row, 4] = expense.Date.ToShortDateString();
                totalUtility += expense.Amount;
                row++;
            }

            // Transport Expenses
            decimal totalTransport = nudTransportCost.Value;
            

            // Total calculations
            decimal grandTotal = totalFood + totalClothes + totalEntertainment + totalUtility + totalTransport;

            row++; 

            xlWorkSheet.Cells[row, 1] = "Вкупно за храна:";
            xlWorkSheet.Cells[row, 2] = totalFood + " ден.";
            Excel.Range totalFoodRange = xlWorkSheet.get_Range("A" + row, "A" + row);
            totalFoodRange.Font.Bold = true;
            row++;

            xlWorkSheet.Cells[row, 1] = "Вкупно за облека:";
            xlWorkSheet.Cells[row, 2] = totalClothes + " ден.";
            Excel.Range totalClothesRange = xlWorkSheet.get_Range("A" + row, "A" + row);
            totalClothesRange.Font.Bold = true;
            row++;

            xlWorkSheet.Cells[row, 1] = "Вкупно за забава/релаксација:";
            xlWorkSheet.Cells[row, 2] = totalEntertainment + " ден.";
            Excel.Range totalEntertainmentRange = xlWorkSheet.get_Range("A" + row, "A" + row);
            totalEntertainmentRange.Font.Bold = true;
            row++;

            xlWorkSheet.Cells[row, 1] = "Вкупно за одржување:";
            xlWorkSheet.Cells[row, 2] = totalUtility + " ден.";
            Excel.Range totalUtilityRange = xlWorkSheet.get_Range("A" + row, "A" + row);
            totalUtilityRange.Font.Bold = true;
            row++;

            xlWorkSheet.Cells[row, 1] = "Транспорт:";
            xlWorkSheet.Cells[row, 2] = totalTransport + " ден.";
            Excel.Range totalTransportRange = xlWorkSheet.get_Range("A" + row, "A" + row);
            totalTransportRange.Font.Bold = true;
            row++;

            xlWorkSheet.Cells[row, 1] = "Вкупно:";
            xlWorkSheet.Cells[row, 2] = grandTotal + " ден.";
            Excel.Range grandTotalRange = xlWorkSheet.get_Range("A" + row, "A" + row);
            grandTotalRange.Font.Bold = true;

            string fileName = $"UserExpenses_{DateTime.Now.ToString("yyyyMMdd_HHmmss")}.xlsx";
            xlWorkBook.SaveAs(fileName, Excel.XlFileFormat.xlWorkbookDefault, misValue, misValue, misValue, misValue,
                Excel.XlSaveAsAccessMode.xlExclusive, misValue, misValue, misValue, misValue, misValue);
            xlWorkBook.Close(true, misValue, misValue);
            xlApp.Quit();

            MessageBox.Show($"Фајлот е зачуван како: {fileName}");

            releaseObject(xlApp);
            releaseObject(xlWorkBook);
            releaseObject(xlWorkSheet);
        }

        private void releaseObject(object obj)
        {
            try
            {
                System.Runtime.InteropServices.Marshal.ReleaseComObject(obj);
                obj = null;
            }
            catch (Exception ex)
            {
                obj = null;
                MessageBox.Show("Exception Occured while releasing object " + ex.ToString());
            }
            finally
            {
                GC.Collect();
            }
        }

        
    }


}

















































