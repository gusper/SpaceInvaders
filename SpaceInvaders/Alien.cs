using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Windows.Forms;

namespace SpaceInvaders
{
    class Alien : Actor
    {
        Image[] sprites = new Image[2];
        int currentSprite = 0;
        double spriteSwitchTime = 0d;
        double spriteDelay = 600d;
        bool exploding = false;
        bool dead = false;
        
        internal bool Dead { get { return dead; } }

        internal Alien(PointF startLocation)
        {
            location = startLocation;
        
            sprites[0] = Bitmap.FromFile(Global.AlienSpriteFilename + "1.bmp");
            sprites[1] = Bitmap.FromFile(Global.AlienSpriteFilename + "2.bmp");            
            sprite = sprites[0];

            bounds = new Rectangle((int)location.X, (int)location.Y, sprite.Size.Width, sprite.Size.Height);
            
            imgAttr.SetColorKey(Color.Black, Color.Black);
        }

        internal override void Step(double elapsed)
        {
            if (!dead)
                fireBullets(elapsed);
                        
            switch (Global.AlienDirection)
            {
                case Directions.Left:
                    location.X -= (float)(elapsed * Global.AlienSpeed);
                    bounds.X = (int)location.X;
                    break;
                case Directions.Right:
                    location.X += (float)(elapsed * Global.AlienSpeed);
                    bounds.X = (int)location.X;
                    break;
                default:
                    throw new Exception("Invalid direction for alien!");
            }

            spriteSwitchTime += elapsed;
            if (spriteSwitchTime > spriteDelay) 
            {
                if (exploding)
                {
                    active = false;
                    exploding = false;
                }

                nextSprite();
                spriteSwitchTime = 0d;
            }
        }

        internal override void Render(Graphics g)
        {
            g.DrawImage(sprite, bounds, 0f, 0f, (float)sprite.Width, (float)sprite.Height, GraphicsUnit.Pixel, imgAttr);
        }

        internal void HitByBullet()
        {
            exploding = true;
            dead = true;
            sprite = getExplosionSprite();
            spriteSwitchTime = 0d;
            Global.Score += 10;
        }

        private Image getExplosionSprite()
        {
            Image image = Bitmap.FromFile(Global.ExplodeSpriteFilename + ".bmp");

            switch (Global.GetRandomNumber(3))
            {
                case 1:
                    image.RotateFlip(RotateFlipType.Rotate90FlipNone);
                    break;
                case 2:
                    image.RotateFlip(RotateFlipType.Rotate180FlipNone);
                    break;
                case 3:
                    image.RotateFlip(RotateFlipType.Rotate270FlipNone);
                    break;
            }

            return image;
        }

        private void nextSprite()
        {
            if (++currentSprite == sprites.Length)
                currentSprite = 0;

            sprite = sprites[currentSprite];
        }

        private PointF getBulletStartLocation()
        {
            return new PointF(location.X + (sprite.Width / 2) - (Global.BulletSize.Width / 2), location.Y + (Global.AlienSize.Height / 2));
        }

        private void fireBullets(double elapsed)
        {
            // Should this alien launch a bullet?  If not, return immediately
            float cutoff = 10000 * Global.BulletChancePerSecond * Global.BulletChanceLevelMultiplier
                           * (float)(elapsed / 1000);

            if (Global.GetRandomNumber(10000) < cutoff) 
                Game.Instance.FireBullet(new Bullet(getBulletStartLocation(), Directions.Down));
        }
    }
}
