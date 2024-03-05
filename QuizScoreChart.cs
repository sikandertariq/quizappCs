using System;
using System.Windows.Forms;

namespace Excel_Test
{
    // Class representing the form that displays the quiz scores in a chart format.
    public partial class QuizScoreChart : Form
    {
        // Constructor for the QuizScoreChartForm.
        // Takes the number of correct and incorrect answers as parameters.
        public QuizScoreChart(int correctAns, int wrongAns)
        {
            InitializeComponent();

            // Adds a title to the chart displayed on this form.
            chart1.Titles.Add("Quiz Score");

            // Customize the title's font
            chart1.Titles[0].Font = new System.Drawing.Font("Arial", 16F, System.Drawing.FontStyle.Bold);

            // Adds data points to the chart for correct and wrong answers.
            // "s1" is the name of the series used to plot data in the chart.
            chart1.Series["s1"].Points.AddXY("Correct", correctAns);
            chart1.Series["s1"].Points[0].Color = System.Drawing.Color.Green; // Custom color for correct answers
            chart1.Series["s1"].Points.AddXY("Wrong", wrongAns);
            chart1.Series["s1"].Points[1].Color = System.Drawing.Color.Red; // Custom color for wrong answers

            // Explode the correct answers slice
            chart1.Series["s1"].Points[0]["Exploded"] = "true";

            // Enable 3D effect
            chart1.ChartAreas[0].Area3DStyle.Enable3D = true;

            // Add labels with percentage
            chart1.Series["s1"].Label = "#PERCENT{P0}";
            chart1.Series["s1"].LegendText = "#VALX";

            // Enable tooltips
            chart1.Series["s1"].ToolTip = "#VALX: #VAL (#PERCENT{P2})";

            // Customize the legend
            chart1.Legends[0].Enabled = true;
            chart1.Legends[0].Docking = System.Windows.Forms.DataVisualization.Charting.Docking.Bottom;
            chart1.Legends[0].Alignment = System.Drawing.StringAlignment.Center;

            // Handle the case when there are no data points
            if (correctAns == 0 && wrongAns == 0)
            {
                chart1.Series["s1"].Points.Clear();
                chart1.Titles[0].Text = "No data available";
            }
        }


        private void Form4_Load(object sender, EventArgs e)
        {

        }

        private void chart1_Click(object sender, EventArgs e)
        {

        }
    }
}
