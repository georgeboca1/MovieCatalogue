using FileManager;
using MovieModels;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FrontEnd
{
    public partial class CharacterForm : Form
    {
        MovieManagerText fileManager;
        public List<string> info = new List<string>();
        DateTime birth;
        public CharacterForm()
        {
            fileManager = new MovieManagerText("movies.txt", "actors.txt", "directors.txt");
            InitializeComponent();
        }

        private void CharacterForm_Load(object sender, EventArgs e)
        {
            this.Location = new Point(
                (Screen.PrimaryScreen.WorkingArea.Width - this.Width) / 2,
                (Screen.PrimaryScreen.WorkingArea.Height - this.Height) / 2
                );
            //Check if the form was opened for directors of actors
            if (this.Tag.ToString() == "Director")
            {
                foreach(Character s in fileManager.GetDirectors())
                {
                    this.listBox1.Items.Add(s.FullName);
                }
            }
            else
            {
                foreach (Character s in fileManager.GetActors())
                {
                    this.listBox1.Items.Add(s.FullName);
                }
            }
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            // Show a calendar
            MonthCalendar calendar = new MonthCalendar
            {
                MaxSelectionCount = 1,
                ShowToday = true,
                ShowTodayCircle = true
            };

            Form calendarForm = new Form
            {
                Text = "Select a Date",
                StartPosition = FormStartPosition.CenterParent,
                Size = new Size(250, 250)
            };

            calendarForm.Controls.Add(calendar);
            calendar.DateSelected += (s, ev) =>
            {
                MessageBox.Show("Selected Date: " + ev.Start.Year);
                birth = ev.Start;
                calendarForm.Close();
            };
            calendarForm.ShowDialog();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (this.Tag.ToString() == "Director")
            {
                List<Character> c = fileManager.GetDirectors();
                foreach (Character ch in c)
                {
                    if (ch.FullName == listBox1.Items[listBox1.SelectedIndex].ToString())
                    {
                        this.info.Add(ch.GetUUID().ToString());
                        this.info.Add(listBox1.Items[listBox1.SelectedIndex].ToString());
                        this.info.Add(ch.birth.ToString());
                    }
                }
            }
            else
            {
                List<Character> c = fileManager.GetActors();
                foreach(Character ch in c)
                {
                    if (ch.FullName == listBox1.Items[listBox1.SelectedIndex].ToString())
                    {
                        this.info.Add(ch.GetUUID().ToString());
                        this.info.Add(listBox1.Items[listBox1.SelectedIndex].ToString());
                        this.info.Add(ch.birth.ToString());
                    }
                }
            }
            this.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (this.Tag.ToString() == "Director")
            {
                fileManager.AddDirector($"{Guid.NewGuid()};{textBox1.Text};{birth.Year}");
            }
            else
            {
                fileManager.AddActor($"{Guid.NewGuid()};{textBox1.Text};{birth.Year}");
            }
            listBox1.Items.Add(textBox1.Text);
        }
    }
}
