using System;
using System.Windows.Forms;
using System.Collections;
using System.Drawing;

namespace SpaceInvaders
{
	class Game
	{
        private AlienManager aliens;
        private Player player;
        private Saucer saucer;
        private BulletManager bullets;
        private StarManager stars;
        private Image buffer; 
        private Graphics bufferGraphics;
        private Graphics displayGraphics;
        private Form form;
        private Font font = new Font("Impact", 14);
        private Font largeFont = new Font("Impact", 26);
        private Brush fontBrush = Brushes.White;
        private double renderElapsed = 0d;
        bool saucerLaunchedThisLevel = false;

        private Game() {}
        private static Game instance = new Game();
        internal static Game Instance { get { return instance; } }

        internal void FireBullet(Bullet bullet)
        {
            bullets.AlienBullets.Add(bullet);
        }        

        internal void Initialize(Form mainForm)
        {
            this.form = mainForm;
            
            // Set up the off-screen buffer used for double-buffering
            buffer = new Bitmap(mainForm.Width, mainForm.Height);
            bufferGraphics = Graphics.FromImage(buffer);
            displayGraphics = mainForm.CreateGraphics();

            stars = new StarManager();
        }

        /// <summary>
        /// Input handler for the whole game.
        /// </summary>
        internal void OnKeyDown(object sender, KeyEventArgs e)
        {
            // The escape key will exit the application regardless
            // what state the game is in.
            if (e.KeyCode == Keys.Escape)
                Application.Exit();
            
            // If game is not active
            if (Global.GameOver)
            {
                if (e.KeyCode == Keys.F5)
                    startNewGame();
            }
            else
            {
                // In game allowed keys
                switch (e.KeyCode)
                {
                    case Keys.Left:
                        Global.PlayerDirection = Directions.Left;
                        break;
                    case Keys.Right:
                        Global.PlayerDirection = Directions.Right;
                        break;
                    case Keys.Down:
                        Global.PlayerDirection = Directions.None;
                        break;
                    case Keys.Space:
                        if (!player.Dead)
                            bullets.PlayerBullets.Add(new Bullet(player.GetBulletStartLocation(), Directions.Up));
                        break;
                }
            }
        }

        /// <summary>
        /// Main game loop.  
        /// </summary>
        internal void GameLoop()
        {
            DateTime start;
            double elapsed = 0d;
            
            while (form.Created)
            {
                start = DateTime.Now;

                stars.Step(elapsed);
                render(elapsed);
                Application.DoEvents();
                
                if (!Global.GameOver)
                {
                    step(elapsed);
                    detectCollision();
                }

                elapsed = (DateTime.Now - start).TotalMilliseconds;
            }
        }

        /// <summary>
        /// Main step method.
        /// </summary>
        private void step(double elapsed)
        {
            bullets.Step(elapsed);
            aliens.Step(elapsed);
            player.Step(elapsed);

            // Should a saucer be launched now?
            shouldSaucerLaunch();
            
            if (saucer != null && saucer.Active)
            {
                saucer.Step(elapsed);
                if (!saucer.Active)
                    saucer = null;
            }

            if (Global.PlayersRemaining == 0)
            {
                Global.GameOver = true;
                return;
            }

            if (Global.LevelFinished)
            {
                Global.CurrentLevel++;
                Global.BulletChancePerSecond *= 1.2f;
                Global.AlienSpeed *= 1.1f;
                startNewBoard();
            }
        }

        /// <summary>
        /// Main render method.
        /// </summary>
        private void render(double elapsed)
        {
            // Shouldn't render every game loop iteration, so
            // throttle it down to a reasonable level 
            renderElapsed += elapsed;
            if (renderElapsed < 30) 
                return;
            renderElapsed = 0;

            bufferGraphics.Clear(Color.Black);
            stars.Render(bufferGraphics);

            // While the game is in progress
            if (!Global.GameOver)
            {
                aliens.Render(bufferGraphics);
                
                if (saucer != null && saucer.Active)
                    saucer.Render(bufferGraphics);

                bullets.Render(bufferGraphics);
                player.Render(bufferGraphics);
            }
            else
            {
                // Show "Game Over" message in the center of the screen
                bufferGraphics.DrawString("Game Over", largeFont, fontBrush, 310, 290);
                bufferGraphics.DrawString("Press F5 to start a new game", font, fontBrush, 278, 350);
            }
            
            // Display banner
            bufferGraphics.DrawString("Score: " + Global.Score, font, fontBrush, 10, 10);
            bufferGraphics.DrawString("Level: " + Global.CurrentLevel, font, fontBrush, 630, 10);
            bufferGraphics.DrawString("Players: " + Global.PlayersRemaining, font, fontBrush, 710, 10);

            // Blit the off-screen buffer on to the display
            displayGraphics.DrawImage(buffer, 0, 0);
        }

        /// <summary>
        /// Called to check for any collisions between bullets and
        /// the player and/or aliens.
        /// </summary>
        private void detectCollision()
        {
            // Check to see if the player hit any aliens
            foreach (Bullet bullet in bullets.PlayerBullets)
            {
                if (!bullet.Active) 
                    continue;

                aliens.CheckForCollision(bullet);
                if (saucer != null && saucer.Active)
                    saucer.CheckForCollision(bullet);
            }

            // Check to see if the aliens have hit the player
            if (!player.Dead)
            {
                foreach (Bullet bullet in bullets.AlienBullets)
                {
                    if (!bullet.Active) 
                        continue;

                    player.CheckForCollision(bullet);
                }
            }
        }

        private void shouldSaucerLaunch()
        {
            // Should launch a saucer only once per level, when there are 10 aliens left
            // in the level.  
            if (saucer == null && !saucerLaunchedThisLevel && aliens.RemainingAliens == 10)
            {
                saucer = new Saucer();
                saucerLaunchedThisLevel = true;
            }                
        }

        /// <summary>
        /// Resets the game's state to start a new game
        /// </summary>
        private void startNewGame()
        {
            Global.GameOver = false;
            Global.Score = 0;
            Global.PlayersRemaining = 3;
            Global.CurrentLevel = 1;
            Global.BulletChancePerSecond = Global.BulletChancePerSecondStartValue;
            Global.AlienSpeed = Global.AlienSpeedStartValue;

            player = new Player(new Point(form.ClientSize.Width / 2 - 20, 
                                          form.ClientSize.Height - 50));

            startNewBoard();
        }

        /// <summary>
        /// Called at the beginning of a new game and at the start
        /// of each new level a player reaches.  Initializes bullet lists
        /// and places a new set of aliens at their starting positions.
        /// </summary>
        private void startNewBoard()
        {
            bullets = new BulletManager();
            
            Global.LevelFinished = false;
            saucerLaunchedThisLevel = false;

            aliens = new AlienManager(Global.AliensPerRow, Global.AlienRows);
        }
	}
}
