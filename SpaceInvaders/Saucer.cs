using System;
using System.Drawing;

namespace SpaceInvaders
{
	class Saucer : Actor
	{
        Image[] sprites = new Image[2];
        int currentSprite = 0;
        double spriteSwitchTime = 0d;
        double spriteDelay;
        float saucerSpeed = 1f / 10f;
        bool exploding = false;
        bool dead = false;
        
        internal Saucer()
		{
            sprites[0] = Bitmap.FromFile(Global.SaucerSpriteFilename + "1.bmp");
            sprites[1] = Bitmap.FromFile(Global.SaucerSpriteFilename + "2.bmp");            
            sprite = sprites[0];

            resetSaucerPosition();

            imgAttr.SetColorKey(Color.Black, Color.Black);
        }

        internal override void Step(double elapsed)
        {
            location.X += (float)(elapsed * saucerSpeed);
            bounds.X = (int)location.X;

            if (location.X > Global.FormSize.Width)
            {
                active = false;
                return;
            }

            spriteSwitchTime += elapsed;
            if (spriteSwitchTime > spriteDelay) 
            {
                if (exploding)
                {
                    active = false;
                    exploding = false;
                    dead = false;
                    resetSaucerPosition();
                }

                nextSprite();
                spriteSwitchTime = 0d;
            }
        }

        internal override void Render(Graphics g)
        {
            if (active)
            {
                g.DrawImage(sprite, bounds, 0f, 0f, 
                    (float)sprite.Width, (float)sprite.Height, 
                    GraphicsUnit.Pixel, imgAttr);
            }
        }

        internal void CheckForCollision(Bullet bullet)
        {
            if (!dead && active && bullet.Bounds.IntersectsWith(bounds))
            {
                hitByBullet();
                bullet.Hit();
            }
        }

        private void resetSaucerPosition()
        {
            location = new PointF(-40, 40);
            bounds = new Rectangle((int)location.X, (int)location.Y, sprite.Size.Width, sprite.Size.Height);
            spriteDelay = 600d;
        }

        private void hitByBullet()
        {
            exploding = true;
            dead = true;
            sprite = Bitmap.FromFile(Global.ExplodeSpriteFilename + ".bmp");
            spriteSwitchTime = 0d;
            spriteDelay = 1000d;
            Global.Score += 100;
        }

        private void nextSprite()
        {
            if (++currentSprite == sprites.Length)
                currentSprite = 0;

            sprite = sprites[currentSprite];
        }
    }
}
