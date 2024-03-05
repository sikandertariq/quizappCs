using System;
using System.Linq;
using System.Windows.Forms;

// Namespace for the quiz application
namespace Excel_Test
{
    // Defines a form for user interaction in the quiz application
    public partial class User_Form : Form
    {
        // Field to hold the selected quiz topic
        public string _selectedTopic;

        // Constructor that initializes the form components
        public User_Form()
        {
            InitializeComponent();
        }

        // Event handler for the form's Load event to populate the comboBox with quiz topics
        private void User_Form_Load(object sender, EventArgs e)
        {
            // Populating the comboBox with various math topics for the quiz
            comboBox1.Items.Add("Trigonometry");
            comboBox1.Items.Add("Algebra");
            comboBox1.Items.Add("ComplexNumbers");
            comboBox1.Items.Add("SimpleMaths");
            comboBox1.Items.Add("Geometry");
        }

        // Event handler for the button click to start the quiz
        private void button1_Click(object sender, EventArgs e)
        {
            // Retrieving the username
            string username = textBox1.Text;

            // Checking if the username is empty or contains numbers
            if (string.IsNullOrWhiteSpace(username) || username.Any(char.IsDigit))
            {
                MessageBox.Show("Username cannot be empty and must not contain numbers.");
                return; // Stop further processing
            }

            // Checking if the selected topic is not in the predefined list of topics
            var topics = new[] { "Trigonometry", "Algebra", "ComplexNumbers", "SimpleMaths", "Geometry" };
            if (comboBox1.SelectedIndex == -1 || !topics.Contains(_selectedTopic))
            {
                MessageBox.Show("Please select a valid topic.");
                return; // Stop further processing
            }

            // If both conditions are satisfied, proceed with the rest of the method
            Cursor.Current = Cursors.WaitCursor;
            MessageBox.Show($"Best Wishes! {username}!");
            MessageBox.Show("Welcome to the quiz!\n\nHere are the rules:\n\n1. The questions can cover various math topics, such as arithmetic, algebra, geometry, or calculus, depending on the difficulty level of your quiz.\n2. A visible timer counts down from 20 minutes, indicating the remaining time for the quiz.\n3. Each question is presented with multiple-choice answer options.\n4. Have fun and test your knowledge!");

            QuizForm quiz = new QuizForm(_selectedTopic,username);
            quiz.Show(); //Show the current Form
            this.Hide(); // Hide the current form
        }

        // Event handler for when the selected item in the comboBox changes
        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Updating the selected topic based on user selection
            _selectedTopic = comboBox1.SelectedItem.ToString();
        }

        // Event handler for label2 click: Provides a visual cue for entering a username
        // "Enter your Username" label action - can be used to focus the username text box or show additional instructions
        private void label2_Click(object sender, EventArgs e)
        {
        }

        // Event handler for label3 click: Indicates where to choose the quiz topic
        // "Choose the Topic" label action - can highlight or show more information about selecting a quiz topic
        private void label3_Click(object sender, EventArgs e)
        {
        }

        // Event handler for label1 click: Represents the heading "Math Quiz"
        // This could be used for displaying an introductory message or help regarding the quiz
        private void label1_Click(object sender, EventArgs e)
        {
        }

        // Event handler for the picture box click: Represents a login screen avatar
        // This could be used to change the avatar, log in a user, or display user information
        private void pictureBox1_Click(object sender, EventArgs e)
        {
        }

    }
}
