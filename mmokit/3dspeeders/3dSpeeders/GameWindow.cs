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
        public ConnectionInfo config;
        Game game;

        public GameWindow( ConnectionInfo cinfo )
        {
            config = cinfo;

            InitializeComponent();

            label1.Text = "Whee I'm a game window\r\nI'm gonna connect to " + config.server + "\r\nAnd be ";
            label1.Text += config.resolutionX.ToString() + " by " + config.resolutionY.ToString();
            if (config.fullscreen)
                label1.Text += " Fullscreen";
            else
                label1.Text += " in a window";

            setup();
            game = new Game(this);
       }

        void setup ()
        {
            if (config.fullscreen)
            {
                Bounds = Screen.PrimaryScreen.Bounds;
                TopMost = true;

                WindowState = FormWindowState.Maximized;
            }
            else
            {
                Width = config.resolutionX;
                Height = config.resolutionY;
            }
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
