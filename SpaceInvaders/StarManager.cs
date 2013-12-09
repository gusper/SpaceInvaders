using System;
using System.Drawing;

namespace SpaceInvaders
{
	class StarManager
	{
        RectangleF[] nearStars, farStars;
        int maxX = Global.FormSize.Width;
        int maxY = Global.FormSize.Height;
        float nearStarSpeed = 1f / 5f;
        float farStarSpeed = 1f / 12f;

        /// <summary>
        /// Initialize two sets of stars: far and near.  The
        /// near stars should be a little bigger than the far
        /// away stars.
        /// </summary>
		internal StarManager()
		{
            nearStars = new RectangleF[15];
            farStars = new RectangleF[25];

            generateStars(nearStars, 2f, 1f);
            generateStars(farStars, 1f, 1f);
		}

        /// <summary>
        /// Step method to calculates new current location of stars.
        /// </summary>
        internal void Step(double elapsed)
        {
            stepStars(nearStars, nearStarSpeed, elapsed);
            stepStars(farStars, farStarSpeed, elapsed);
        }

        /// <summary>
        /// Render each of the stars to the screen.
        /// </summary>
        internal void Render(Graphics graphics)
        {
            for (int i = 0; i < nearStars.Length; i++)
                graphics.FillRectangle(Brushes.LightGray, nearStars[i]);

            for (int i = 0; i < farStars.Length; i++)
                graphics.FillRectangle(Brushes.Gray, farStars[i]);
        }

        /// <summary>
        /// Populates the passed in array of stars with the given dimensions.
        /// </summary>
        private void generateStars(RectangleF[] stars, float width, float height)
        {
            for (int i = 0; i < stars.Length; i++)
            {
                stars[i] = new RectangleF(Global.GetRandomNumber(maxX), 
                    Global.GetRandomNumber(maxY), width, height);
            }
        }

        /// <summary>
        /// Moves each of the stars in the given array accordingly using
        /// the elapsed time since the last step iteration occurred.  Also
        /// takes into account the speed that is passed in.
        /// </summary>
        private void stepStars(RectangleF[] stars, float speed, double elapsed)
        {
            for (int i = 0; i < stars.Length; i++)
            {
                stars[i].Y += (float)(elapsed * speed);

                // Move stars according to the player's current direction
                switch (Global.PlayerDirection)
                {
                    case Directions.Left:
                        stars[i].X += (float)(elapsed * speed) * .1f;
                        break;
                    case Directions.Right:
                        stars[i].X -= (float)(elapsed * speed) * .1f;
                        break;
                }

                // Once a star scrolls off the bottom of the screen, 
                // generate a new star at the top of the screen.
                if (stars[i].Y > maxY)
                {
                    stars[i].X = Global.GetRandomNumber(maxX);
                    stars[i].Y = 0;
                }
            }
        }
    }
}
