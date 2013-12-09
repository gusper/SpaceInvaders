using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Windows.Forms;

namespace SpaceInvaders
{
	class Player : Actor
	{
        Image[] sprites = new Image[2];
        int currentSprite = 0;
        double spriteSwitchTime = 0d;
        double spriteDelay = 100d;
        bool exploding = false;
        bool dead = false;

        internal bool Dead { get { return dead; } }

		internal Player(PointF startLocation)
		{
            location = startLocation;
            
            sprites[0] = Bitmap.FromFile(Global.PlayerSpriteFilename + "1.bmp");
            sprites[1] = Bitmap.FromFile(Global.PlayerSpriteFilename + "2.bmp");            
            sprite = sprites[0];

            bounds = new Rectangle((int)location.X, (int)location.Y, sprite.Size.Width, sprite.Size.Height);

            imgAttr.SetColorKey(Color.Black, Color.Black);
        }

        internal override void Step(double elapsed)
        {
            switch (Global.PlayerDirection)
            {
                case Directions.Left:
                    location.X -= (float)(elapsed * Global.PlayerSpeed);
                    if (location.X < 0) 
                    {
                        location.X = 0;
                        Global.PlayerDirection = Directions.None;
                    }
                    bounds.X = (int)location.X;
                    break;
                case Directions.Right:
                    location.X += (float)(elapsed * Global.PlayerSpeed);
                    if (location.X + Global.PlayerSize.Width > Global.FormSize.Width) 
                    {
                        location.X = Global.FormSize.Width - Global.PlayerSize.Width;
                        Global.PlayerDirection = Directions.None;
                    }
                    bounds.X = (int)location.X;
                    break;
            }

            spriteSwitchTime += elapsed;
            if (spriteSwitchTime > spriteDelay) 
            {
                if (exploding)
                {
                    active = true;
                    exploding = false;
                    dead = false;
                    Global.PlayersRemaining--;
                    spriteDelay = 100d;
                    sprite = sprites[0];
                }
                
                nextSprite();
                spriteSwitchTime = 0d;
            }
        }

        internal override void Render(Graphics g)
        {
            g.DrawImage(sprite, bounds, 0f, 0f, (float)sprite.Width, (float)sprite.Height, GraphicsUnit.Pixel, imgAttr);
        }

        internal void CheckForCollision(Bullet bullet)
        {
            if (bullet.Bounds.IntersectsWith(bounds))
            {
                hitByBullet();
                bullet.Hit();
            }
        }

        internal PointF GetBulletStartLocation()
        {
            return new PointF(location.X + (sprite.Width / 2) - (Global.BulletSize.Width / 2), location.Y);
        }

        private void nextSprite()
        {
            if (++currentSprite == sprites.Length)
                currentSprite = 0;

            sprite = sprites[currentSprite];
        }

        private void hitByBullet()
        {
            exploding = true;
            dead = true;
            sprite = Bitmap.FromFile(Global.ExplodeSpriteFilename + ".bmp");
            spriteSwitchTime = 0d;
            spriteDelay = 1000d;
        }
    }
}
