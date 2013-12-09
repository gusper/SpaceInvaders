using System;
using System.Drawing;

namespace SpaceInvaders
{
	class AlienManager
	{
        private Alien[,] aliens;
        private int leftMostAlien, rightMostAlien;
        private int totalAliens, remainingAliens;

        /// <summary>
        /// Number of aliens still alive 
        /// </summary>
        internal int RemainingAliens { get { return remainingAliens; } }

        /// <summary>
        /// Number of aliens this level started out with
        /// </summary>
        internal int TotalAliens { get { return totalAliens; } }
        
        /// <summary>
        /// Set up a level's worth of aliens.
        /// </summary>
		internal AlienManager(int cols, int rows)
		{
            aliens = new Alien[cols, rows];

            for (int x = 0; x < cols; x++)
            {
                for (int y = 0; y < rows; y++)
                {
                    PointF location = new Point( 
                        x * (Global.AlienSize.Width  + Global.AlienSeparation.Width) + 10, 
                        y * (Global.AlienSize.Height + Global.AlienSeparation.Height) + 80);
                    aliens[x, y] = new Alien(location);
                }
            }

            leftMostAlien = 0; 
            rightMostAlien = aliens.GetLength(1);

            Global.AlienDirection = Directions.Right;
            Global.BulletChanceLevelMultiplier = 1f;

            remainingAliens = totalAliens = cols * rows;
		}

        /// <summary>
        /// Processes the position and state of each alien, taking
        /// into account the amount of time that has passed since 
        /// the last step.
        /// </summary>
        internal void Step(double elapsed)
        {
            leftMostAlien = -1;
            rightMostAlien = -1;
            
            for (int x = 0; x < aliens.GetLength(0); x++)
            {
                for (int y = 0; y < aliens.GetLength(1); y++)
                {
                    if (aliens[x,y].Active)
                    {
                        if (leftMostAlien == -1) leftMostAlien = x;
                        if (x > rightMostAlien) rightMostAlien = x;
                        aliens[x,y].Step(elapsed);
                    }
                }
            }

            if (leftMostAlien == -1 && rightMostAlien == -1) 
                Global.LevelFinished = true;
            else
                checkAlienDirection();
        }

        /// <summary>
        /// Renders all the live aliens to the buffer.
        /// </summary>
        internal void Render(Graphics graphics)
        {
            foreach (Alien alien in aliens)
            {
                if (alien.Active) 
                    alien.Render(graphics);
            }
        }

        /// <summary>
        /// Check to see if any alien is hit by the bullet passed in.
        /// If it is, notify the alien and bullet that they've collided and
        /// set the difficulty level accordingly.
        /// </summary>
        internal void CheckForCollision(Bullet bullet)
        {
            foreach (Alien alien in aliens)
            {
                if (!alien.Dead && alien.Active && bullet.Bounds.IntersectsWith(alien.Bounds))
                {
                    alien.HitByBullet();
                    bullet.Hit();
                    remainingAliens--;
                    adjustLevelDifficulty((double)remainingAliens / (double)totalAliens);
                    return;
                }
            }
        }

        /// <summary>
        /// As there are less and less aliens remaining in the level,
        /// increase their chances of firing bullets.
        /// </summary>
        private void adjustLevelDifficulty(double pctLeft)
        {
            if (pctLeft > .75)
                Global.BulletChanceLevelMultiplier = 1f;
            else if (pctLeft > .5)
                Global.BulletChanceLevelMultiplier = 2f;
            else if (pctLeft > .25)
                Global.BulletChanceLevelMultiplier = 4f;
            else if (pctLeft > .1)
                Global.BulletChanceLevelMultiplier = 8f;
            else if (pctLeft > .025)
                Global.BulletChanceLevelMultiplier = 16f;
            else 
                Global.BulletChanceLevelMultiplier = 32f;
        }

        /// <summary>
        /// If the left or right most alien is near the edge of the screen,
        /// change the direction they're moving.
        /// </summary>
        private void checkAlienDirection()
        {
            if (Global.AlienDirection == Directions.Left)
            {
                for (int y = 0; y < aliens.GetLength(1); y++)
                {
                    Alien alien = aliens[leftMostAlien, y];
                    if (alien.Active && alien.Location.X <= 10)
                    {
                        Global.AlienDirection = Directions.Right;
                        break;
                    }
                }
            }
            else
            {
                for (int y = 0; y < aliens.GetLength(1); y++)
                {
                    Alien alien = aliens[rightMostAlien, y];
                    if (alien.Active && alien.Location.X + Global.AlienSize.Width >= Global.FormSize.Width - 10)
                    {
                        Global.AlienDirection = Directions.Left;
                        break;
                    }
                }
            }
        }
	}
}
