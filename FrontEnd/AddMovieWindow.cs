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
        private Character actors;
        public AddMovieWindow()
        {
            InitializeComponent();
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
            movie.AddActor(actors);
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

            director = new Character(Guid.Parse(cf.info[0]),cf.info[1], Int32.Parse(cf.info[2]));
        }

        private void button5_Click(object sender, EventArgs e)
        {
            CharacterForm cf = new CharacterForm();
            cf.Tag = "Actor";
            cf.ShowDialog();
            actors = new Character(Guid.Parse(cf.info[0]), cf.info[1], Int32.Parse(cf.info[2]));
        }
    }
}
