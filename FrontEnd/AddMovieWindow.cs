using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MovieModels;

namespace FrontEnd
{
    public partial class AddMovieWindow : Form
    {
        public Movie movie;
        private string imagePath = string.Empty;
        private Character director;
        private List<Character> actors;
        private int selectedYear;
        public AddMovieWindow()
        {
            InitializeComponent();
            this.textBox1.Validating += new System.ComponentModel.CancelEventHandler(this.textbox1_Validating);
            this.textBox2.Validating += new System.ComponentModel.CancelEventHandler(this.textbox2_Validating);
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (director == null)
            {
                MessageBox.Show("Please select a director.");
                return;
            }
            if (actors == null || actors.Count == 0)
            {
                MessageBox.Show("Please select at least one actor.");
                return;
            }
            if (imagePath == string.Empty)
            {
                MessageBox.Show("Please select an image.");
                return;
            }
            movie = new Movie(textBox1.Text, textBox2.Text, imagePath);
            foreach (RadioButton radiobutton in this.panel1.Controls)
            {
                if (radiobutton.Checked)
                {
                    movie.AddGenre(radiobutton.Text);
                }
            }
            movie.AddDirector(director);
            foreach (Character actor in actors)
            {
                movie.AddActor(actor);
            }
            movie.AddYear(selectedYear);
            this.Close();
        }

        private void AddMovieWindow_Load(object sender, EventArgs e)
        {
            this.Location = new Point(
                (Screen.PrimaryScreen.WorkingArea.Width - this.Width) / 2,
                (Screen.PrimaryScreen.WorkingArea.Height - this.Height) / 2
            );
        }

        private void button3_Click(object sender, EventArgs e)
        {
            // Load an image path
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Image Files|*.jpg;*.jpeg;*.png;*.bmp";
            openFileDialog.Title = "Select an Image File";
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                // Set the selected image path to the text box
                imagePath = openFileDialog.FileName;
            }
            if (imagePath.Count() == 0)
            {
                button3.ForeColor = Color.FromArgb(100, 0, 0);
            }
            label9.Text = imagePath;
        }

        private void label5_Click(object sender, EventArgs e)
        {

        }

        private void button4_Click(object sender, EventArgs e)
        {
            // Open a new window where the user can select a character
            CharacterForm cf = new CharacterForm();
            cf.Tag = "Director";
            cf.ShowDialog();

            if (cf.characters.Count != 0)
            {
                director = cf.characters[0];
                label7.Text = director.FullName;
            }
            if (director == null)
            {
                button4.BackColor = Color.FromArgb(100, 0, 0);
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            CharacterForm cf = new CharacterForm();
            cf.Tag = "Actor";
            cf.ShowDialog();
            actors = cf.characters;
            label8.Text = string.Join(",", cf.characters.Select(c => c.FullName));

            if (cf.characters.Count != 0)
            {
                actors = cf.characters;
                label8.Text = string.Join(",", cf.characters.Select(c => c.FullName));
            }
            if (director == null)
            {
                button5.BackColor = Color.FromArgb(100, 0, 0);
            }
        }

        private void textbox1_Validating(object sender, CancelEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(textBox1.Text))
            {
                e.Cancel = true; // prevents the control from losing focus
                textBox1.BackColor = Color.FromArgb(100, 0, 0);
            }
            else
            {
                e.Cancel = false;
                textBox1.BackColor = Color.FromArgb(22, 22, 22);
            }
        }

        private void textbox2_Validating(object sender, CancelEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(textBox2.Text))
            {
                e.Cancel = true; // prevents the control from losing focus
                textBox2.BackColor = Color.FromArgb(100, 0, 0);
            }
            else
            {
                e.Cancel = false;
                textBox2.BackColor = Color.FromArgb(22, 22, 22);
            }
        }

        private void iconPickerButton1_Click(object sender, EventArgs e)
        {
            using (Form yearPickerForm = new Form())
            {
                yearPickerForm.BackgroundImage = global::FrontEnd.Properties.Resources.pattern;
                yearPickerForm.Text = "Select Year";
                yearPickerForm.FormBorderStyle = FormBorderStyle.FixedDialog;
                yearPickerForm.StartPosition = FormStartPosition.CenterParent;
                yearPickerForm.Width = 220;
                yearPickerForm.Height = 120;
                yearPickerForm.MaximizeBox = false;
                yearPickerForm.MinimizeBox = false;
                yearPickerForm.ShowInTaskbar = false;

                Label label = new Label() { Text = "Year:", Left = 10, Top = 15, Width = 40, BackColor = Color.Transparent, ForeColor = Color.White};
                NumericUpDown yearUpDown = new NumericUpDown()
                {
                    Left = 60,
                    Top = 10,
                    Width = 100,
                    Minimum = 1900,
                    Maximum = DateTime.Now.Year + 5,
                    Value = DateTime.Now.Year
                };
                Button okButton = new Button() { Text = "OK", Left = 60, Width = 60, Top = 40, DialogResult = DialogResult.OK };
                yearPickerForm.Controls.Add(label);
                yearPickerForm.Controls.Add(yearUpDown);
                yearPickerForm.Controls.Add(okButton);
                yearPickerForm.AcceptButton = okButton;

                if (yearPickerForm.ShowDialog(this) == DialogResult.OK)
                {
                    selectedYear = (int)yearUpDown.Value;
                }
            }
        }
    }
}
