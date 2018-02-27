using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EmojiMemoryMatch
{
    public partial class Form1 : Form
    {

        // properties of our Form1 class
        bool allowClick = false;
        Random rnd = new Random();
        Timer clickTimer = new Timer();
        int time = 0;
        Timer timer = new Timer { Interval = 1000 };    // create a timer that 'ticks' every second

        PictureBox firstGuess;
        PictureBox pic1;
        PictureBox pic2;

        int winCount = 8;   // numer of matches required to win
        int matchCount = 0; // count the number of matches!

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private PictureBox[] pictureBoxes
        {
            get { return Controls.OfType<PictureBox>().ToArray(); }
        }

        private static IEnumerable<Image> images
        {
            get
            {
                return new Image[]
                {
                    Properties.Resources.img1,
                    Properties.Resources.img2,
                    Properties.Resources.img3,
                    Properties.Resources.img4,
                    Properties.Resources.img5,
                    Properties.Resources.img6,
                    Properties.Resources.img7,
                    Properties.Resources.img8
                };
            }
        }

        private void startGameTimer()
        {
            timer.Start();
            timer.Tick += delegate
            {
                time++;
                /*
                // our version doesn't count down till you lose, 
                // it counts up!

                time--;
                if (time < 0)
                {
                    timer.Stop();
                    MessageBox.Show("Out of Time");
                    ResetImages();
                }
                */

                var ssTime = TimeSpan.FromSeconds(time);

                //label1.Text = "00: " + time.ToString();
                label1.Text = string.Format("{0:HH:mm:ss}", ssTime.ToString());
            };
        }

        private void ResetImages()
        {
            foreach (var pic in pictureBoxes)
            {
                pic.Tag = null;
                pic.Visible = true;

            }

            HideImages();
            setRandomImages();
            time = 0;
            timer.Start();
        }

        private void HideImages()
        {
            foreach (var pic in pictureBoxes)
            {
                pic.Image = Properties.Resources.question;
            }
        }

        private PictureBox getFreeSlot()
        {
            int num;

            do
            {
                num = rnd.Next(0, pictureBoxes.Count());
            }
            while (pictureBoxes[num].Tag != null);

            return pictureBoxes[num];
        }

        private void setRandomImages()
        {
            foreach (var image in images)
            {
                getFreeSlot().Tag = image;
                getFreeSlot().Tag = image;
            }
        }

        private void CLICKTIMER_TICK(object sender, EventArgs e)
        {
            //HideImages();
            // hide the two clicked images instead of all of them
            pic1.Image = Properties.Resources.question;
            pic2.Image = Properties.Resources.question;

            allowClick = true;
            clickTimer.Stop();
        }

        private void clickImage(object sender, EventArgs e)
        {
            if (!allowClick) return;

            var pic = (PictureBox)sender;

            // we need to check if it's a card that's already been flipped
            // and do nothing if it's already a matched card
            //if (pic.Image != Properties.Resources.question) return;

            // using class props so the clickTimer event handler 
            // knows which two cards to flip back over
            pic1 = firstGuess;
            pic2 = pic;
            
            if (firstGuess == null)
            {
                firstGuess = pic;
                pic.Image = (Image)pic.Tag;
                return;
            }

            // this is the pic flip, 
            // the actual image is stored in the pic.tag
            pic.Image = (Image)pic.Tag;

            // if you get a match
            if (pic.Image == firstGuess.Image && pic != firstGuess)
            {
                //MessageBox.Show("YOU GOT A MATCH YAYA");
                /*
                pic.Visible = firstGuess.Visible = false;
                {
                    firstGuess = pic;
                }
                HideImages();
                */
                matchCount++;
            }
            // else it's not a match, start the clickTimer
            // let the user look at it then flip those two back over. 
            else
            {
                //MessageBox.Show("YOU SUCK");
                allowClick = false;
                clickTimer.Start();
            }

            firstGuess = null;

            //if (pictureBoxes.Any(p => p.Visible)) return;
            if (matchCount == winCount)
            {
                MessageBox.Show("Congratulations! Now try again!", "YOU WIN!");
                ResetImages();
                matchCount = 0;
            }

        }

        private void startGame(object sender, EventArgs e)
        {
            allowClick = true;
            setRandomImages();
            HideImages();
            startGameTimer();
            clickTimer.Interval = 1000;
            clickTimer.Tick += CLICKTIMER_TICK;
            button1.Enabled = false;

        }

        private void playNewGameClick(object sender, EventArgs e)
        {
            allowClick = true;
            /*
            timer.Stop();
            timer.Start();
            */
            //startGameTimer();

            ResetImages();
            
            label1.Text = "00:00:00";

        }
    }
}
