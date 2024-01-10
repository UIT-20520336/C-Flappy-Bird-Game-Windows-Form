using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Flappy_Bird_Windows_Form
{
    public partial class Form1 : Form
    {

        // coded for MOO ICT Flappy Bird Tutorial

        // Variables start here

        int pipeSpeed = 8; // default pipe speed defined with an integer
        int gravity = 15; // default gravity speed defined with an integer
        int score = 0; // default score integer set to 0
        private FlappyBird flappyBirdInstance;
        private IGameState gameState;
        // variable ends

        public Form1()
        {
            InitializeComponent();
            flappyBirdInstance = FlappyBird.GetInstance();
            this.Controls.Add(flappyBirdInstance.BirdPictureBox);
            gameTimer.Stop();
            gameState = new PlayingState(this);
        }

        private void gamekeyisdown(object sender, KeyEventArgs e)
        {
            // this is the game key is down event thats linked to the main form
            if (e.KeyCode == Keys.Space)
            {
                // if the space key is pressed then the gravity will be set to -15
                gravity = -15;
            }
            else if (e.KeyCode == Keys.Escape)
            {
                gameState.PauseGame();
                gameState = new PausedState(this);
            }

        }

        private void gamekeyisup(object sender, KeyEventArgs e)
        {
            // this is the game key is up event thats linked to the main form

            if (e.KeyCode == Keys.Space)
            {
                // if the space key is released then gravity is set back to 15
                gravity = 15;
            }
            else if (e.KeyCode == Keys.R)
            {
                gameState.UnpauseGame();
                gameState = new PlayingState(this);
            }
        }

        private void endGame()
        {
            // this is the game end function, this function will when the bird touches the ground or the pipes
            gameTimer.Stop(); // stop the main timer
            scoreText.Text += " Game over!!!"; // show the game over text on the score text, += is used to add the new string of text next to the score instead of overriding it
        }

        private void gameTimerEvent(object sender, EventArgs e)
        {
            gameState.UpdateState();
            


            // if score is greater then 5 then we will increase the pipe speed to 15
            if(score > 5)
            {
                pipeSpeed = 15;
            }

        }
        public class PlayingState : IGameState
        {
            private Form1 form;

            public PlayingState(Form1 form)
            {
                this.form = form;
            }

            public void UpdateState()
            {
                form.flappyBird.Top += form.gravity; // link the flappy bird picture box to the gravity, += means it will add the speed of gravity to the picture boxes top location so it will move down
                form.pipeBottom.Left -= form.pipeSpeed; // link the bottom pipes left position to the pipe speed integer, it will reduce the pipe speed value from the left position of the pipe picture box so it will move left with each tick
                form.pipeTop.Left -= form.pipeSpeed; // the same is happening with the top pipe, reduce the value of pipe speed integer from the left position of the pipe using the -= sign
                form.scoreText.Text = "Score: " + form.score; // show the current score on the score text label

                // below we are checking if any of the pipes have left the screen

                if (form.pipeBottom.Left < -150)
                {
                    // if the bottom pipes location is -150 then we will reset it back to 800 and add 1 to the score
                    form.pipeBottom.Left = 800;
                    form.score++;
                }
                if (form.pipeTop.Left < -180)
                {
                    // if the top pipe location is -180 then we will reset the pipe back to the 950 and add 1 to the score
                    form.pipeTop.Left = 950;
                    form.score++;
                }

                // the if statement below is checking if the pipe hit the ground, pipes or if the player has left the screen from the top
                // the two pipe symbols stand for OR inside of an if statement so we can have multiple conditions inside of this if statement because its all going to do the same thing

                if (form.flappyBird.Bounds.IntersectsWith(form.pipeBottom.Bounds) ||
                    form.flappyBird.Bounds.IntersectsWith(form.pipeTop.Bounds) ||
                    form.flappyBird.Bounds.IntersectsWith(form.ground.Bounds) || form.flappyBird.Top < -25
                    )
                {
                    // if any of the conditions are met from above then we will run the end game function
                    form.endGame();
                }
            }

            public void PauseGame()
            {
                form.gameTimer.Stop();
                // Additional logic for pausing the game if needed
            }

            public void UnpauseGame()
            {
                form.gameTimer.Start();
                // Additional logic for resuming the game if needed
            }
        }

        public class PausedState : IGameState
        {
            private Form1 form;

            public PausedState(Form1 form)
            {
                this.form = form;
            }

            public void UpdateState()
            {
                // Game is paused, no update logic needed
            }

            public void PauseGame()
            {
                // Game is already paused
            }

            public void UnpauseGame()
            {
                form.gameState = new PlayingState(form);
                form.gameTimer.Start();
            }
        }



    }

    //Singleton Design Pattern
    public class FlappyBird
    {
        private static FlappyBird instance;
        public PictureBox BirdPictureBox { get; private set; }

        private FlappyBird()
        {
            BirdPictureBox = new PictureBox();
            // Initialize your picture box properties here (e.g., image, size, etc.)
        }

        public static FlappyBird GetInstance()
        {
            if (instance == null)
            {
                instance = new FlappyBird();
            }
            return instance;
        }

        // Add any other methods or properties related to the bird here
    }

    //State Design Pattern
    public interface IGameState
    {
        void UpdateState();
        void PauseGame();
        void UnpauseGame();
    }


}
