using System;
using System.Data;
using System.Data.OleDb;
using System.Windows.Forms;

namespace Excel_Test
{
    public partial class QuizForm : Form
    {
        // OleDbDataReader to execute SQL queries and read data from the Excel file.
        private OleDbDataReader myreader;

        // Stores the correct option for the current question displayed to the user.
        private string currentCorrectOption;

        // Index to keep track of the current question being displayed from the Excel data.
        public int currentQuestionIndex = 1;

        // Count of the number of questions answered correctly by the user.
        private int correctAnswers = 0;

        // Count of the number of questions answered incorrectly by the user.
        private int wrongAnswers = 0;

        // Stores a hint for the current question, if available, to assist the user.
        private string hint;

        // Tracks the number of consecutive wrong answers to potentially adjust difficulty.
        private int consecutiveWrongAnswers;

        // The total number of questions that the user has attempted so far.
        private int questionsAttempted = 0;

        // Represents the user's progress through the quiz, which could be used for visual feedback.
        private int userProgress = 0;

        // Not explicitly used in the provided code snippet but could represent the ID of the first question for a particular topic.
        private int startQuestionId;

        // Not explicitly used in the provided code snippet but could represent the ID of the last question for a particular topic.
        private int endQuestionId;

        // Tracks the number of consecutive correct answers to potentially reward the user or adjust difficulty.
        int consecutiveCorrectAnswers = 0;

        private string user;


        private enum DifficultyMode
        {
            Easy,
            Medium,
            Hard
        }
        private DifficultyMode currentDifficultyMode = DifficultyMode.Easy;

        public QuizForm(string SelectedTopic, string username)
        {
            // Initializes the components/design of the form.
            InitializeComponent();
            user=username;
            // Sets the timer interval to 1 second (1000 milliseconds).
            timer1.Interval = 1000;

            // Subscribes to the Tick event of the timer. This event is raised every second as per the interval set above.
            timer1.Tick += timer1_Tick;

            // Starts the timer to begin counting down, used for controlling quiz timing or question time limits.
            timer1.Start();

            // Checks the selected topic and sets the starting question index accordingly.
            // This allows the quiz to load questions relevant to the selected topic.

            if (SelectedTopic == "Trigonometry")
            {
                // Sets the starting question index for Trigonometry.
                currentQuestionIndex = 1;
                // Loads the first question for the Trigonometry topic.
                LoadQuestion(currentQuestionIndex);
            }
            else if (SelectedTopic == "Algebra")
            {
                // Sets the starting question index for Algebra, assuming Algebra questions start at index 21.
                currentQuestionIndex = 21;
                // Loads the first question for the Algebra topic.
                LoadQuestion(currentQuestionIndex);
            }
            else if (SelectedTopic == "ComplexNumbers")
            {
                // Sets the starting question index for Complex Numbers, assuming these questions start at index 41.
                currentQuestionIndex = 41;
                // Loads the first question for the Complex Numbers topic.
                LoadQuestion(currentQuestionIndex);
            }
            else if (SelectedTopic == "SimpleMaths")
            {
                // Sets the starting question index for Simple Maths, assuming these questions start at index 61.
                currentQuestionIndex = 61;
                // Loads the first question for the Simple Maths topic.
                LoadQuestion(currentQuestionIndex);
            }
            else if (SelectedTopic == "Geometry")
            {
                // Sets the starting question index for Geometry, assuming Geometry questions start at index 81.
                currentQuestionIndex = 81;
                // Loads the first question for the Geometry topic.
                LoadQuestion(currentQuestionIndex);
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
        
        }

        private int userScore = 0;
        private void button1_Click(object sender, EventArgs e)
        {
            // Flag to determine if the user's answer is correct.
            bool isCorrect = false;

            // Check each radio button to see if it is selected and matches the correct answer.
            if (radioButton1.Checked && radioButton1.Text == currentCorrectOption)
            {
                userScore += 10; // Increment user score for correct answer.
                isCorrect = true; // Set flag to true indicating correct answer.
            }
            else if (radioButton2.Checked && radioButton2.Text == currentCorrectOption)
            {
                userScore += 10; // Increment user score for correct answer.
                isCorrect = true; // Set flag to true indicating correct answer.
            }
            else if (radioButton3.Checked && radioButton3.Text == currentCorrectOption)
            {
                userScore += 10; // Increment user score for correct answer.
                isCorrect = true; // Set flag to true indicating correct answer.
            }
            else if (radioButton4.Checked && radioButton4.Text == currentCorrectOption)
            {
                userScore += 10; // Increment user score for correct answer.
                isCorrect = true; // Set flag to true indicating correct answer.
            }

            // Respond to the user based on whether their answer was correct.
            if (isCorrect)
            {
                correctAnswers++; // Increment the count of correct answers.
                MessageBox.Show("Correct Answer! You got 10 Points."); // Inform the user of their correct answer.
            }
            else
            {
                wrongAnswers++; // Increment the count of wrong answers.
                MessageBox.Show("Wrong Answer"); // Inform the user that their answer was incorrect.
            }

            // Update the score display with the current user score.
            label4.Text = $"Score: {userScore}";

            // Increment the count of questions attempted.
            questionsAttempted++;

            // If all questions have been attempted, display the results.
            if (questionsAttempted == 20)
            {
                DisplayResults();
            }

            // Update the progress bar to reflect the number of questions attempted.
            UpdateProgressBar();

            // Load the next question.
            LoadNextQuestion();
        }

        private void UpdateProgressBar()
        {
            // Assuming progressBar is the name of your ProgressBar control
            double percentage = (double)questionsAttempted / 20 * 100;
            progressBar1.Value = (int)percentage;
        }
        private void LoadNextQuestion()
        {
            currentQuestionIndex++;
            //currentQuestionIndexA++;
            if (currentQuestionIndex < 100)
            {
                LoadQuestion(currentQuestionIndex);
            }
            //else if (currentQuestionIndex >= 21)
            //{
            //    LoadQuestion(currentQuestionIndex);
            //}
            else
            {
                DisplayResults();
            }
            UpdateDifficultyMode();
        }


        private void UpdateDifficultyMode()
        {
            // If the user has answered 5 questions correctly while the quiz is in Easy mode,
            // the difficulty level is increased to Medium.
            if (correctAnswers == 5 && currentDifficultyMode == DifficultyMode.Easy)
            {
                currentDifficultyMode = DifficultyMode.Medium; // Update the difficulty mode to Medium.
                MessageBox.Show("Difficulty Mode changed to Medium!"); // Notify the user of the difficulty change.
            }
            // If the user has answered 10 questions correctly while the quiz is in Medium mode,
            // the difficulty level is increased to Hard.
            else if (correctAnswers == 10 && currentDifficultyMode == DifficultyMode.Medium)
            {
                currentDifficultyMode = DifficultyMode.Hard; // Update the difficulty mode to Hard.
                MessageBox.Show("Difficulty Mode changed to Hard!"); // Notify the user of the difficulty change.
            }
            // If the user has answered 3 questions incorrectly in a row and the quiz is not already in Easy mode,
            // the difficulty level is decreased to Easy.
            else if (consecutiveWrongAnswers == 3 && currentDifficultyMode != DifficultyMode.Easy)
            {
                currentDifficultyMode = DifficultyMode.Easy; // Reset the difficulty mode to Easy.
                MessageBox.Show("Difficulty Mode changed to Easy due to 3 consecutive wrong answers."); // Notify the user of the difficulty downgrade.
            }
        }

        private void DisplayResults()
        {
            QuizResultsForm f3 = new QuizResultsForm(correctAnswers,wrongAnswers,user);
            
            f3.Show();
            this.Close();
        }
        private void LoadQuestion(int questionId)
        {
            // Reset all radio buttons to unchecked at the start of loading a new question.
            radioButton1.Checked = false;
            radioButton2.Checked = false;
            radioButton3.Checked = false;
            radioButton4.Checked = false;

            // Establish a connection to the Excel file containing quiz questions.
            OleDbConnection conn = new OleDbConnection(
                @"Provider=Microsoft.ACE.OLEDB.12.0;" +
                @"Data Source=C:\Users\siki\Downloads\Prototype_3\C#\Test.xlsx;" +
                @"Extended Properties='Excel 12.0 Xml;HDR=YES';");
            try
            {
                // Open the connection to the Excel file.
                conn.Open();

                // Prepare a SQL command to fetch the question and its options based on the given question ID.
                OleDbCommand cmd = new OleDbCommand("SELECT Question, OptionA, OptionB, OptionC, OptionD, Correct,Hint,Mode FROM [Sheet1$] WHERE ID=@id", conn);
                cmd.Parameters.AddWithValue("@id", questionId);

                // Execute the command and read the results.
                myreader = cmd.ExecuteReader();
                if (myreader.Read())
                {
                    // Populate the form controls with the data from the Excel file.
                    groupBox1.Text = myreader["Question"].ToString();
                    radioButton1.Text = myreader["OptionA"].ToString();
                    radioButton2.Text = myreader["OptionB"].ToString();
                    radioButton3.Text = myreader["OptionC"].ToString();
                    radioButton4.Text = myreader["OptionD"].ToString();
                    currentCorrectOption = myreader["Correct"].ToString();
                    hint = myreader["Hint"].ToString();

                    // Update the quiz mode based on the current question.
                    string mode = myreader["Mode"].ToString();
                    label5.Text = $"Mode: {mode}";

                    // Update question numbering displayed to the user.
                    if (currentQuestionIndex <= 20)
                    {
                        label6.Text = $"Question {currentQuestionIndex}/20";
                    }
                    else
                    {
                        label6.Text = $"Question {currentQuestionIndex}/100";
                    }
                }
                else
                {
                    // If no data is found for the specified question ID, notify the user.
                    MessageBox.Show("No Data Found!");
                }
            }
            catch (Exception ex)
            {
                // If an error occurs during the database operation, display an error message.
                MessageBox.Show("Error: " + ex.Message);
            }
            finally
            {
                // Ensure that the database connection is closed properly.
                if (conn.State == ConnectionState.Open)
                {
                    conn.Close();
                }
            }
        }

        private void label1_Click(object sender, EventArgs e)
        {}
        private void textBox1_TextChanged(object sender, EventArgs e)
        {}
        private void label4_Click(object sender, EventArgs e)
        {}

        //As decided in the documentation we have kept the time of the entire Quiz to be 20 minutes 
        //After which the screen will be locked and no further attempt or question shall be allowed
        private int timeLeft = 1200; // 20 minutes in seconds
                                     // Event handler for the timer's Tick event.
        private void timer1_Tick(object sender, EventArgs e)
        {
            // Check if there's still time left for the quiz.
            if (timeLeft > 0)
            {
                // Decrement the remaining time.
                timeLeft--;
                // Calculate minutes and seconds from timeLeft.
                int minutes = timeLeft / 60;
                int seconds = timeLeft % 60;
                // Update the label to show the remaining time in mm:ss format.
                label2.Text = $"{minutes:00}:{seconds:00}";
            }
            else
            {
                // If time has run out, stop the timer and inform the user that the quiz is over.
                timer1.Stop();
                MessageBox.Show("Time's up! Your quiz is over.");
                DisplayResults(); // Optionally call a method to display the quiz results.
            }
        }

        // Event handler for the hint button click.
        private void hint_button_Click(object sender, EventArgs e)
        {
            // Check if a hint is available for the current question.
            if (!string.IsNullOrEmpty(hint))
            {
                // Display the hint.
                label3.Text = hint;
            }
            else
            {
                // Inform the user that no hint is available for this question.
                label3.Text = "No hint available for this question.";
            }
            // Make sure the hint label is visible.
            label3.Visible = true;
        }

        // Event handler for the finish quiz button click.
        private void button4_Click(object sender, EventArgs e)
        {
            // Confirm with the user if they really want to finish the quiz early.
            DialogResult result = MessageBox.Show("Do you really want to finish the quiz?", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                // If the user confirms, display the quiz results and close the form.
                DisplayResults();
                this.Close();
            }
            // If the user selects "No," the quiz continues without any action.
        }

        // Event handler for navigating to the previous question.
        private void button3_Click(object sender, EventArgs e)
        {
            // Check if it's possible to move back to a previous question.
            if (currentQuestionIndex > 1)
            {
                // Decrement the current question index to move back.
                currentQuestionIndex--;
                // Load the previous question.
                LoadQuestion(currentQuestionIndex);
            }
            else
            {
                // Inform the user that they cannot move back from the first question.
                MessageBox.Show("You are on the first question. You cannot move back.");
            }
        }


        // Event handler for the Hint button. Displays a hint for the current question when clicked.
        private void button2_Click(object sender, EventArgs e)
        {
            // This method would show the hint associated with the current question to the user.
        }

        // Event handler for clicking label5, which displays the current quiz mode (Easy, Medium, Hard).
        private void label5_Click(object sender, EventArgs e)
        {
            // This could be used to provide more details about the current difficulty mode or to change it.
        }

        // Event handler for clicking label6, which shows the current question read from the Excel sheet.
        private void label6_Click(object sender, EventArgs e)
        {
            // This could be used to highlight or re-emphasize the current question to the user.
        }

        // Event handler for clicking label4, which displays the score of the person taking the quiz.
        private void label4_Click_1(object sender, EventArgs e)
        {
            // This could be used to give a detailed breakdown or history of the user's score.
        }

        // Event handler for clicking label1, the heading of the form which says "Math Quiz".
        private void label1_Click_1(object sender, EventArgs e)
        {
            // This could display an introductory message or additional information about the quiz.
        }

        // Event handler for clicking label3, which is used to display hints for the current question.
        private void label3_Click(object sender, EventArgs e)
        {
            // This could hide the hint after being clicked or provide additional details.
        }

        // Event handlers for the radio buttons. These represent the multiple-choice question (MCQ) options A, B, C, and D.
        private void radioButton4_CheckedChanged(object sender, EventArgs e)
        {
            // Checks if option D is selected by the user. Could be used for feedback or logging.
        }

        private void radioButton3_CheckedChanged(object sender, EventArgs e)
        {
            // Checks if option C is selected by the user. Could be used for feedback or logging.
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            // Checks if option B is selected by the user. Could be used for feedback or logging.
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            // Checks if option A is selected by the user. Could be used for feedback or logging.
        }

        // Event handler for when the user interacts with groupBox1, which contains the question and MCQ options.
        private void groupBox1_Enter(object sender, EventArgs e)
        {
            // This could be used to visually emphasize the group box when it is interacted with.
        }

        // Event handler for clicking label2, which shows the timer set to the total time for the quiz.
        private void label2_Click(object sender, EventArgs e)
        {
            // This could be used to display additional timer details or to pause/resume the countdown.
        }

        // Event handler for clicking on progressBar1. This progress bar visually represents the user's progress through the quiz, filling up as questions are answered.
        private void progressBar1_Click(object sender, EventArgs e)
        {
            // This could be used to display a message or additional details about the user's progress. The progress bar grows after every question, reaching 100% when all 20 questions have been attempted.
        }
        private void button4_click(object sender, EventArgs e)
        {
            // This could be used to display a message or additional details about the user's progress. The progress bar grows after every question, reaching 100% when all 20 questions have been attempted.
        }


    }
}



