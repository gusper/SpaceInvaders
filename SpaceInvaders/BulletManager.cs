using System;
using System.Collections;
using System.Drawing;

namespace SpaceInvaders
{
	class BulletManager
	{
        private ArrayList playerBullets = new ArrayList();
        private ArrayList alienBullets = new ArrayList();
        
        /// <summary>
        /// Exposes the set of bullets fired by the player
        /// </summary>
        internal ArrayList PlayerBullets { get { return playerBullets; } }
        
        /// <summary>
        /// Exposes the set of bullets fired by aliens
        /// </summary>
        internal ArrayList AlienBullets { get { return alienBullets; } }

        /// <summary>
        /// Call step() on each bullet that's still active.
        /// </summary>
        /// <param name="elapsed"></param>
        internal void Step(double elapsed)
        {
            foreach (Bullet bullet in alienBullets)
            {
                if (bullet.Active) 
                    bullet.Step(elapsed);
            }

            foreach (Bullet bullet in playerBullets)
            {
                if (bullet.Active) 
                    bullet.Step(elapsed);
            }
        }

        /// <summary>
        /// Render each of the remaining active bullets to the screen.
        /// </summary>
        /// <param name="graphics"></param>
        internal void Render(Graphics graphics)
        {
            foreach (Bullet bullet in playerBullets)
            {
                if (bullet.Active) 
                    bullet.Render(graphics);
            }

            foreach (Bullet bullet in alienBullets)
            {
                if (bullet.Active) 
                    bullet.Render(graphics);
            }
        }    
    }
}
