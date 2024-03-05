using System;
using System.IO;
using System.Windows.Forms;
using iTextSharp.text.pdf;
using iTextSharp.text;
using System.Collections.Generic;
using ClosedXML.Excel;
using System.Diagnostics;
using System.Linq;

namespace Excel_Test
{
    // Class representing the form that displays the quiz results to the user.
    public partial class QuizResultsForm : Form
    {
        // Fields to store the count of correct and incorrect answers.
        private int correctAns;
        private int wrongAns;
        private List<QuizAttempt> quizAttempts;
        private string username;

        // Constructor for the QuizResultsForm.
        public QuizResultsForm(int correctAns, int wrongAns,  string username)
        {
            InitializeComponent();

            this.correctAns = correctAns;
            this.wrongAns = wrongAns;
            this.username = username;
            //this.quizAttempts = attempts; // Set the quizAttempts field with the passed list

            label1.Text = $"You answered {correctAns} correct and {wrongAns} wrong answers.";
            percent(correctAns);
            this.username = username;
        }
        //public QuizResultsForm(int correctAns, int wrongAns)
        //    : this(correctAns, wrongAns, new List<QuizAttempt>()) 
        //{
           
        //}


        // Calculates the percentage of correct answers and determines the grade.
        private void percent(int correctAns)
        {
            // Total number of questions is fixed at 20 for the percentage calculation.
            double totalQuestions = 20;
            double correctPercentage = (correctAns / totalQuestions) * 100;

            // Determine the grade based on the calculated percentage.
            string grade;
            if (correctPercentage >= 90) grade = "A";
            else if (correctPercentage >= 80) grade = "B";
            else if (correctPercentage >= 70) grade = "C";
            else if (correctPercentage >= 60) grade = "D";
            else grade = "F";

            // Update label2 to display the grade and percentage of correct answers.
            label2.Text = $"Grade: {grade}\nPercentage: {correctPercentage:0.00}%";
        }

        // Placeholder for form load event; can be used for initialization tasks.
        private void Form3_Load(object sender, EventArgs e)
        {
        }

        // Placeholder for label1 click event; can be used for additional interaction.
        private void label1_Click(object sender, EventArgs e)
        {
        }

        // Handles the click event for the button to exit the application.
        private void button1_Click(object sender, EventArgs e)
        {
            //Debug.WriteLine("Exiting the application.");
            // Add a breakpoint here to inspect the quizAttempts list
            //Debug.WriteLine($"Attempting to save {quizAttempts.Count} attempts.");
            //foreach (var attempt in quizAttempts)
            //{
            //    Debug.WriteLine($"Username: {attempt.Username}, Date: {attempt.Date}, Correct: {attempt.CorrectAnswers}, Incorrect: {attempt.IncorrectAnswers}, Percentage: {attempt.Percentage}, Grade: {attempt.Grade}");
            //}

            SaveAttemptsToExcel();
            Application.Exit();
        }


        // Handles the click event for the button to restart the application.
        private void button2_Click(object sender, EventArgs e)
        {
            // Restarts the application.
            Application.Restart();
        }

        // Handles the click event for navigating to the next form to show detailed results.
        private void button3_Click(object sender, EventArgs e)
        {
            
        }

        private void button3_Click_1(object sender, EventArgs e)
        {
            // Create a SaveFileDialog to ask the user where to save the PDF report
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "PDF files (*.pdf)|*.pdf";
            saveFileDialog.Title = "Save PDF Report";
            saveFileDialog.ShowDialog();

            // If the user clicked OK and selected a file name
            if (saveFileDialog.FileName != "")
            {
                try
                {
                    // Call a method to create and save the PDF report
                    CreatePdfReport(saveFileDialog.FileName);
                    MessageBox.Show("PDF report saved successfully.");
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Failed to save PDF report: {ex.Message}");
                }
            }
        }

        private void CreatePdfReport(string filePath)
        {
            // Initialize a new document
            using (Document document = new Document())
            {
                // Create a PdfWriter instance to write to the specified file path
                PdfWriter writer = PdfWriter.GetInstance(document, new FileStream(filePath, FileMode.Create));

                // Open the document for writing
                document.Open();

                // Add content to the document
                // Here, you'll add the quiz results. This is just an example.
                document.Add(new Paragraph($"Quiz Results"));
                document.Add(new Paragraph($"Correct Answers: {correctAns}"));
                document.Add(new Paragraph($"Wrong Answers: {wrongAns}"));
                // Calculate and display the percentage
                double percentage = ((double)correctAns / (correctAns + wrongAns)) * 100;
                document.Add(new Paragraph($"Percentage: {percentage:0.00}%"));

                // Close the document
                document.Close();
                //make changes
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            // Creates an instance of Form4, passing the counts of correct and wrong answers.
            QuizScoreChart f4 = new QuizScoreChart(correctAns, wrongAns);
            // Shows Form4 and closes the current form.
            f4.Show();
            this.Close();

        }

        private void button5_Click(object sender, EventArgs e)
        {

        }
        //private void SaveAttemptsToExcel()
        //{
        //    var workbook = new XLWorkbook();
        //    var worksheet = workbook.AddWorksheet("Quiz Attempts");

        //    // Define headers for the Excel sheet
        //    worksheet.Cell("A1").Value = "Username";
        //    worksheet.Cell("B1").Value = "Date";
        //    worksheet.Cell("C1").Value = "Correct Answers";
        //    worksheet.Cell("D1").Value = "Incorrect Answers";
        //    worksheet.Cell("E1").Value = "Percentage";
        //    worksheet.Cell("F1").Value = "Grade";

        //    // Populate the Excel sheet with data from quizAttempts
        //    for (int i = 0; i < quizAttempts.Count; i++)
        //    {
        //        worksheet.Cell(i + 2, 1).Value = quizAttempts[i].Username;
        //        worksheet.Cell(i + 2, 2).Value = quizAttempts[i].Date;
        //        worksheet.Cell(i + 2, 3).Value = quizAttempts[i].CorrectAnswers;
        //        worksheet.Cell(i + 2, 4).Value = quizAttempts[i].IncorrectAnswers;
        //        worksheet.Cell(i + 2, 5).Value = quizAttempts[i].Percentage;
        //        worksheet.Cell(i + 2, 6).Value = quizAttempts[i].Grade;
        //    }

        //    // Save the workbook to the desktop
        //    string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
        //    string excelFilePath = Path.Combine(desktopPath, "QuizAttempts.xlsx");
        //    workbook.SaveAs(excelFilePath);
        //}
        private void SaveAttemptsToExcel()
        {
            double totalQuestions = 20;
            double correctPercentage = (correctAns / totalQuestions) * 100;

            string grade;
            if (correctPercentage >= 90) grade = "A";
            else if (correctPercentage >= 80) grade = "B";
            else if (correctPercentage >= 70) grade = "C";
            else if (correctPercentage >= 60) grade = "D";
            else grade = "F";

            var desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            string excelFilePath = Path.Combine(desktopPath, "QuizAttempts.xlsx");

            var workbook = new XLWorkbook();

            if (File.Exists(excelFilePath))
            {
                // Load the existing workbook if the file already exists
                workbook = new XLWorkbook(excelFilePath);
            }
            else
            {
                // Create a new worksheet if the file doesn't exist
                workbook.AddWorksheet("Quiz Attempts");

                // Define headers for the Excel sheet
                workbook.Worksheets.First().Cell("A1").Value = "Username";
                workbook.Worksheets.First().Cell("B1").Value = "Date";
                workbook.Worksheets.First().Cell("C1").Value = "Correct Answers";
                workbook.Worksheets.First().Cell("D1").Value = "Incorrect Answers";
                workbook.Worksheets.First().Cell("E1").Value = "Percentage";
                workbook.Worksheets.First().Cell("F1").Value = "Grade";
            }

            // Assuming you still want to try writing real data if available
            int row = workbook.Worksheets.First().LastRowUsed()?.RowNumber() + 1 ?? 2;

            workbook.Worksheets.First().Cell(row, 1).Value = username;
            workbook.Worksheets.First().Cell(row, 2).Value = DateTime.Now.ToShortDateString();
            workbook.Worksheets.First().Cell(row, 3).Value = correctAns;
            workbook.Worksheets.First().Cell(row, 4).Value = wrongAns;
            workbook.Worksheets.First().Cell(row, 5).Value = correctPercentage;
            workbook.Worksheets.First().Cell(row, 6).Value = grade;

            // Save the workbook to the desktop or a specified path
            workbook.SaveAs(excelFilePath);
        }



    }
}
