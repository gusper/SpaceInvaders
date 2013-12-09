using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Windows.Forms;

namespace SpaceInvaders
{
    class Bullet : Actor
    {
        Directions direction;
        int dirMultiplier;

        /// <summary>
        /// Constructor that takes a starting point and a direction.  If
        /// it's going down it was fired by an alien and is blue.  If
        /// it's going up then the player fired it and is red.
        /// </summary>
        internal Bullet(PointF startLocation, Directions startDirection)
        {
            location = startLocation;
            direction = startDirection;

            // Depending on direction, choose the appropriate sprite.  Also,
            // set the direction multiplier accordingly (e.g. -1 if going up
            // and +1 if going down).
            if (startDirection == Directions.Up)
            {
                // Fired by the player
                dirMultiplier = -1;
                sprite = Bitmap.FromFile(Global.RedBulletSpriteFilename + ".bmp");
            }
            else
            {
                // Fired by an alien
                dirMultiplier = 1;
                sprite = Bitmap.FromFile(Global.BlueBulletSpriteFilename + ".bmp");
            }

            bounds = new Rectangle((int)location.X, (int)location.Y, 
                                    sprite.Size.Width, sprite.Size.Height);
            imgAttr.SetColorKey(Color.Black, Color.Black);
        }

        /// <summary>
        /// Process bullet's position and state using time since last
        /// step.
        /// </summary>
        internal override void Step(double elapsed)
        {
            location.Y += (float)(elapsed * Global.BulletSpeed * dirMultiplier);
            bounds.Y = (int)location.Y;

            // If bullet has gone off the screen, make it inactive
            active = (location.Y >= 0 && location.Y <= Global.FormSize.Height);
        }

        /// <summary>
        /// Render bullet to the screen.
        /// </summary>
        internal override void Render(Graphics g)
        {
            g.DrawImage(sprite, bounds, 0f, 0f, (float)sprite.Width, (float)sprite.Height, GraphicsUnit.Pixel, imgAttr);
        }

        /// <summary>
        /// Called when bullet collides with another actor, at which time
        /// it deactivates itself.
        /// </summary>
        internal void Hit()
        {
            active = false;
        }
    }
}
