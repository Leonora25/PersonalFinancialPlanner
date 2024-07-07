using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace Personal_Financial_Planner
{
    public partial class ChartForm : Form
    {
        public ChartForm(Dictionary<string, decimal> expenseData)
        {
            InitializeComponent();
            this.StartPosition = FormStartPosition.CenterScreen;
            LoadChart(expenseData);
        }
        private void LoadChart(Dictionary<string, decimal> expenseData)
        {
            chartAllMonths.Series.Clear();

            Series series = new Series
            {
                ChartType = SeriesChartType.Column
            };

            foreach (var category in expenseData)
            {
                series.Points.AddXY(category.Key, category.Value);
            }

            chartAllMonths.Series.Add(series);

            chartAllMonths.ChartAreas[0].AxisX.Interval = 1; // Show labels for each category
            chartAllMonths.ChartAreas[0].AxisX.Title = "Категории";
            chartAllMonths.ChartAreas[0].AxisY.Title = "Износ (денари)";
        }



        private void ChartForm_Load(object sender, EventArgs e)
        {

        }

        private void chart1_Click(object sender, EventArgs e)
        {

        }
    }
}
