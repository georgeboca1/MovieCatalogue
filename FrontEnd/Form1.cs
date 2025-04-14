using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using CatalogueModel;
using FileManager;
using MovieModels;

namespace FrontEnd
{
    public partial class Form1 : Form
    {
        public Catalogue catalogue;
        public MovieManagerText fileManager;

        Panel movieGridPanel;
        Panel movieDetailsPanel;
        bool isinGrid = true;
        public Form1()
        {
            this.Load += Form1_Load;
            Environment.CurrentDirectory = Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"..\..\.."));

            catalogue = new Catalogue();
            fileManager = new MovieManagerText("movies.txt", "actors.txt", "directors.txt");
            fileManager.GetMovies(ref catalogue);

            
            InitializeComponent();
        }



        private void Form1_Load(object sender, EventArgs e)
        {
            // Set the window position to center
            this.Location = new Point(
                (Screen.PrimaryScreen.WorkingArea.Width - this.Width) / 2,
                (Screen.PrimaryScreen.WorkingArea.Height - this.Height) / 2
            );
            this.movieGridPanel = this.flowLayoutPanel1;
            foreach (Movie m in catalogue.Movies)
            {
                Panel card = CreateMovieCard(m.ImagePath, m.Name);
                foreach (Control child in card.Controls)
                {
                    child.Click += (s, args) => ShowMovieDetails(m); // Memory Leak?
                }

                flowLayoutPanel1.Controls.Add(card);
            }
        }

        public Image LoadAndResizeImage(string imagePath, int targetWidth, int targetHeight)
        {
            using (var original = Image.FromFile(imagePath))
            {
                // Create a new bitmap with the desired size
                var resized = new Bitmap(targetWidth, targetHeight);
                using (var graphics = Graphics.FromImage(resized))
                {
                    graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                    graphics.DrawImage(original, 0, 0, targetWidth, targetHeight);
                }
                return resized; // This is the downscaled image, which you can assign to your PictureBox.
            }
        }

        void ShowMovieDetails(Movie movie)
        {
            this.SuspendLayout();
            // Remove grid, create details panel
            this.Controls.Remove(this.movieGridPanel);

            movieDetailsPanel = new Panel
            {
                Height = this.Size.Height - 110,
                Dock = DockStyle.Bottom,
                BackColor = Color.FromArgb(44, 44, 44)
            };

            var Title = new Label
            {
                AutoSize = true,
                Text = movie.Name,
                ForeColor = Color.White,
                Dock = DockStyle.None,
                Font = new Font("Montserrat ExtraLight", 18, FontStyle.Bold),
                Height = 50,
                Left = 400,
                Top = 10
            };

            var description = new Label
            {
                AutoSize = true,
                MaximumSize = new Size(800, 0), // Set maximum size to allow wrapping
                Text = movie.Description,
                ForeColor = Color.White,
                Dock = DockStyle.None,
                Font = new Font("Montserrat ExtraLight", 14, FontStyle.Bold),
                Left = 400,
                Top = 50
            };

            var genre = new Label
            {
                AutoSize = true,
                Text = $"Gen: {movie.GetGenreString()}",
                ForeColor = Color.White,
                Dock = DockStyle.None,
                Font = new Font("Montserrat ExtraLight", 14, FontStyle.Bold),
                Left = 400,
                Top = description.Top + description.PreferredHeight + 20 // Position below the description label
            };

            var image = new PictureBox
            {
                Size = new Size(400, this.movieDetailsPanel.Height),
                Image = Image.FromFile(movie.ImagePath),
                SizeMode = PictureBoxSizeMode.StretchImage,
            };
            image.Paint += (s, e) => fadeImage(s, e, movie.ImagePath);

            var backButton = new Button
            {
                Text = "X",
                ForeColor = Color.White,
                Left = this.Size.Width - 40,
                Width = 20,
                Top = 10
            };
            backButton.Click += (s, e) => ReturnToGrid();

            movieDetailsPanel.Controls.Add(image);
            movieDetailsPanel.Controls.Add(Title);
            movieDetailsPanel.Controls.Add(description);
            movieDetailsPanel.Controls.Add(genre);
            movieDetailsPanel.Controls.Add(backButton);

            this.Controls.Add(movieDetailsPanel);
            this.ResumeLayout();
        }

        private void fadeImage(object sender, PaintEventArgs e, string imagePath)
        {
            var pictureBox = sender as PictureBox;
            if (pictureBox == null) return;

            using (var img = Image.FromFile(imagePath))
            {
                // Draw the image first
                e.Graphics.DrawImage(img, 0, 0, pictureBox.Width, pictureBox.Height);
            }

            // Apply the fade effect using a gradient brush for the right and bottom edges
            var fadeBrushHorizontal = new LinearGradientBrush(
                new Rectangle(pictureBox.Width/2, pictureBox.Height/2, pictureBox.Width / 2, pictureBox.Height / 2),
                Color.Transparent,
                Color.FromArgb(44, 44, 44), // The color to fade to
                LinearGradientMode.Horizontal);
            var fadeBrushVertical = new LinearGradientBrush(
                new Rectangle(pictureBox.Width / 2, pictureBox.Height / 2, pictureBox.Width / 2, pictureBox.Height / 2),
                Color.Transparent,
                Color.FromArgb(44, 44, 44), // The color to fade to
                LinearGradientMode.Vertical);

            // Draw the fade effect on the right and bottom
            e.Graphics.FillRectangle(fadeBrushHorizontal, pictureBox.Width / 2, 0, pictureBox.Width / 2, pictureBox.Height); // Right fade
            e.Graphics.FillRectangle(fadeBrushVertical, 0, pictureBox.Height / 2, pictureBox.Width, pictureBox.Height / 2); // Bottom fade
        }

        void ReturnToGrid()
        {
            this.Controls.Remove(movieDetailsPanel);
            foreach (Control item in movieDetailsPanel.Controls)
            {
                // If the control is a PictureBox and has an image, dispose it.
                if (item is PictureBox pb && pb.Image != null)
                {
                    pb.Image.Dispose();
                }
                item.Dispose();
            }
            movieDetailsPanel.Controls.Clear();
            movieDetailsPanel.Dispose(); // Ensure the panel itself is disposed

            this.Controls.Add(movieGridPanel);
        }

        Panel CreateMovieCard(string imagePath, string title)
        {
            var card = new Panel
            {
                Size = new Size(150, 240),
                Margin = new Padding(10),
                BackColor = Color.Black // optional, just for style
            };

            var pictureBox = new PictureBox
            {
                Size = new Size(150, 200),
                Image = LoadAndResizeImage(imagePath, 150, 200), // or use Image.FromStream()
                SizeMode = PictureBoxSizeMode.StretchImage
            };

            var label = new Label
            {
                Text = title,
                Dock = DockStyle.Bottom,
                TextAlign = ContentAlignment.MiddleCenter,
                ForeColor = Color.White,
                BackColor = Color.FromArgb(30, 30, 30),
                Height = 40
            };

            card.Controls.Add(pictureBox);
            card.Controls.Add(label);

            return card;
        }


        private void button1_Click(object sender, EventArgs e)
        {
            foreach(Movie m in catalogue.Movies)
            {
                flowLayoutPanel1.Controls.Add(CreateMovieCard(m.ImagePath,m.Name));
            }
            System.Diagnostics.Debug.WriteLine("Clicked load button");
        }

        private void flowLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.flowLayoutPanel1.Controls.Clear();
            this.catalogue.Movies = new List<Movie>();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            // Open the add movie window
            AddMovieWindow addMovieWindow = new AddMovieWindow();
            addMovieWindow.ShowDialog();

            if (addMovieWindow.movie == null)
            {
                return;
            }

            //TODO: Move this out of here

            catalogue.AddMovie(addMovieWindow.movie);
            Panel card = CreateMovieCard(addMovieWindow.movie.ImagePath, addMovieWindow.movie.Name);
            foreach (Control child in card.Controls)
            {
                child.Click += (s, args) => ShowMovieDetails(addMovieWindow.movie); // Memory Leak?
            }
            // Add the movie card to the flow layout panel
            this.flowLayoutPanel1.Controls.Add(card);

            fileManager.AddMovie(addMovieWindow.movie.DatabaseInfo());

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            string filterText = textBox1.Text.Trim().ToLowerInvariant();

            foreach (Control itemControl in flowLayoutPanel1.Controls)
            {
                string itemText = itemControl.Controls[1].ToString().ToLowerInvariant(); // Use ?. for null safety

                if (itemText != null)
                {
                    bool shouldBeVisible = string.IsNullOrEmpty(filterText) || itemText.Contains(filterText);

                    itemControl.Visible = shouldBeVisible;
                }
                else
                {
                    // Optional: Decide what to do if an item doesn't have text in its Tag
                    // Maybe always show it? Or always hide it if filtering?
                    // For simplicity, let's assume items without Tags are always visible,
                    // or you ensure all relevant items have a Tag.
                    // itemControl.Visible = true; // Or false, depending on requirements
                }
            }
        }
    }
}
