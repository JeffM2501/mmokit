using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace _3dSpeeders
{
    public partial class GameWindow : Form
    {
        Game game;

        public GameWindow(Game g)
        {
            game = g;
            game.window = this;

            InitializeComponent();

            label1.Text = "Whee I'm a game window\r\nI'm gonna connect to " + game.conInfo.server + "\r\nAnd be ";
            label1.Text += game.config.resolutionX.ToString() + " by " + game.config.resolutionY.ToString();
            if (game.config.fullscreen)
                label1.Text += " Fullscreen";
            else
                label1.Text += " in a window";

            setup();
       }

        void setup ()
        {
            if (game.config.fullscreen)
            {
                Bounds = Screen.PrimaryScreen.Bounds;
                TopMost = true;

                WindowState = FormWindowState.Maximized;
            }
            else
            {
                Width = game.config.resolutionX;
                Height = game.config.resolutionY;
            }
            game.init();
        }

        private void GameWindow_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (game != null)
            {
                game.shutdown();
                game = null;
            }
        }
    }
}
