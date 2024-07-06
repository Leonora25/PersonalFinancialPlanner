namespace proba_proekt
{
    partial class ChartForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea1 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend1 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series1 = new System.Windows.Forms.DataVisualization.Charting.Series();
            this.chartAllMonths = new System.Windows.Forms.DataVisualization.Charting.Chart();
            ((System.ComponentModel.ISupportInitialize)(this.chartAllMonths)).BeginInit();
            this.SuspendLayout();
            // 
            // chartAllMonths
            // 
            this.chartAllMonths.BackColor = System.Drawing.Color.Transparent;
            this.chartAllMonths.BorderlineColor = System.Drawing.Color.Black;
            this.chartAllMonths.BorderlineWidth = 3;
            chartArea1.Name = "ChartArea1";
            this.chartAllMonths.ChartAreas.Add(chartArea1);
            legend1.Name = "Legend1";
            this.chartAllMonths.Legends.Add(legend1);
            this.chartAllMonths.Location = new System.Drawing.Point(0, 1);
            this.chartAllMonths.Name = "chartAllMonths";
            series1.ChartArea = "ChartArea1";
            series1.Legend = "Legend1";
            series1.Name = "Series1";
            this.chartAllMonths.Series.Add(series1);
            this.chartAllMonths.Size = new System.Drawing.Size(799, 450);
            this.chartAllMonths.TabIndex = 8;
            this.chartAllMonths.Text = "chart1";
            this.chartAllMonths.Click += new System.EventHandler(this.chart1_Click);
            // 
            // ChartForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.chartAllMonths);
            this.Name = "ChartForm";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.Text = "Графикон";
            this.Load += new System.EventHandler(this.ChartForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.chartAllMonths)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataVisualization.Charting.Chart chartAllMonths;
    }
}