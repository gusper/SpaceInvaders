using System;
using System.Drawing;
using System.Windows.Forms;
using System.Collections;

namespace SpaceInvaders
{
	class MainForm : Form
	{
        static Game game = Game.Instance;
            
		internal MainForm()
		{
            this.SetStyle(ControlStyles.UserPaint, true);
            this.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            this.SetStyle(ControlStyles.DoubleBuffer, true);

            this.ClientSize = Global.FormSize;
            this.BackColor = Color.Black;
			this.Text = "Space Invaders";
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.KeyDown += new KeyEventHandler(game.OnKeyDown);
		}
		
        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
        }

        static void Main() 
		{
			using (MainForm form = new MainForm())
			{
				form.Show();
                game.Initialize(form);
                game.GameLoop();
			}
		}
	}
}
