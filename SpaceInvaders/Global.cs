using System;
using System.Drawing;

namespace SpaceInvaders
{
	class Global
	{
        // Layout related constants
        internal static readonly Size FormSize          = new Size(800, 600);
        internal static readonly Size PlayerSize        = new Size(40, 40);
        internal static readonly Size BulletSize        = new Size(10, 17);
        internal static readonly Size AlienSize         = new Size(40, 40);
        internal static readonly Size AlienSeparation   = new Size(30, 20);
        internal static readonly Size SaucerSize        = new Size(40, 40);
        internal const int AliensPerRow = 10;
        internal const int AlienRows = 4;

        // Bitmap file names
        internal const string RedBulletSpriteFilename = "redbullet";
        internal const string BlueBulletSpriteFilename = "bluebullet";
        internal const string PlayerSpriteFilename = "player";
        internal const string AlienSpriteFilename  = "alien";
        internal const string SaucerSpriteFilename = "saucer";
        internal const string ExplodeSpriteFilename = "explode";

        // Probability values
        internal static float BulletChancePerSecond = 0.02f;
        internal static float BulletChancePerSecondStartValue = 0.02f;
        internal static float BulletChanceLevelMultiplier = 1f;

        // Random number related
        private static Random rand = new Random();
        internal static int GetRandomNumber(int max) { return rand.Next(max); }

        // Stats related
        internal static int Score = 0;
        internal static int PlayersRemaining = 3;
        internal static bool GameOver = true;
        internal static bool LevelFinished = false;
        internal static int CurrentLevel = 1;
        internal static int MaxSaucersPerLevel = 5;

        // Direction related
        internal static Directions PlayerDirection = Directions.None;
        internal static Directions AlienDirection  = Directions.Right;
        
        // Actor speeds
        internal static float AlienSpeedStartValue  = 1f / 20f;
        internal static float AlienSpeed            = 1f / 20f;
        internal static float PlayerSpeed           = 1f / 5f;
        internal static float BulletSpeed           = 1f / 3f;
    }
}
