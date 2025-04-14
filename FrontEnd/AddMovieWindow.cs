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
        public AddMovieWindow()
        {
            InitializeComponent();
            Application.Idle += this.check;
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

            movie = new Movie(textBox1.Text, textBox2.Text, imagePath);
            movie.AddGenre(textBox3.Text);
            movie.AddDirector(director);
            foreach(Character actor in actors)
            {
                movie.AddActor(actor);
            }
            this.Close();
        }

        private void AddMovieWindow_Load(object sender, EventArgs e)
        {
            this.Location = new Point(
                (Screen.PrimaryScreen.WorkingArea.Width - this.Width) / 2,
                (Screen.PrimaryScreen.WorkingArea.Height - this.Height) / 2
            );
            if (textBox1.Text.Length <= 0)
            {
                textBox1.BackColor = Color.FromArgb(100,0,0);
                button1.Enabled = false;
            }
            else
            {
                textBox1.BackColor = Color.FromArgb(22, 22, 22);
            }
            if (textBox2.Text.Length <= 0)
            {
                textBox2.BackColor = Color.FromArgb(100, 0, 0);
            }
            else
            {
                textBox2.BackColor = Color.FromArgb(22, 22, 22);
            }
            if (textBox3.Text.Length <= 0)
            {
                textBox3.BackColor = Color.FromArgb(100, 0, 0);
            }
            else
            {
                textBox3.BackColor = Color.FromArgb(22, 22, 22);
            }
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

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            if (textBox1.Text.Length <= 0)
            {
                textBox1.BackColor = Color.FromArgb(100, 0, 0);
                button1.Enabled = false;
            }
            else
            {
                textBox1.BackColor = Color.FromArgb(22,22,22);
            }
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            if (textBox2.Text.Length <= 0)
            {
                textBox2.BackColor = Color.FromArgb(100, 0, 0);
            }
            else
            {
                textBox2.BackColor = Color.FromArgb(22, 22, 22);
            }
        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {
            if (textBox3.Text.Length <= 0)
            {
                textBox3.BackColor = Color.FromArgb(100, 0, 0);
                button1.Enabled = false;
            }
            else
            {
                textBox3.BackColor = Color.FromArgb(22, 22, 22);
            }
            if (Enum.TryParse<GenreType>(textBox3.Text, true, out _))
            {
                textBox3.BackColor = Color.FromArgb(22, 22, 22);
                button1.Enabled = false;
            }
            else
            {
                textBox3.BackColor = Color.FromArgb(100, 0, 0);
            }
        }

        private void check(object sender, EventArgs e)
        {

            if (textBox1.BackColor == Color.FromArgb(100, 0, 0) ||
                textBox2.BackColor == Color.FromArgb(100, 0, 0) ||
                textBox3.BackColor == Color.FromArgb(100, 0, 0) ||
                label7.Text.Length == 0 ||
                label8.Text.Length == 0 ||
                label9.Text.Length == 0
                )
            {
                button1.Enabled = false;
                return;
            }

            button1.Enabled = true;
        }
    }
}
