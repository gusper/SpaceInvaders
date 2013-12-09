using System;
using System.Drawing;
using System.Drawing.Imaging;

namespace SpaceInvaders
{
    abstract class Actor
    {
        /// <summary>
        /// Coordinates representing where the actor's top left 
        /// corner is on the form.
        /// </summary>
        protected PointF location;

        /// <summary>
        /// Rectangle representing the location and size of the 
        /// actor's sprite.
        /// </summary>
        protected Rectangle bounds;

        /// <summary>
        /// Used to toggle whether the actor is live or not.
        /// </summary>
        protected bool active = true;

        /// <summary>
        /// Used to set the transparency color key if needed by
        /// an actor.
        /// </summary>
        protected ImageAttributes imgAttr = new ImageAttributes();
        
        /// <summary>
        /// Image that will be rendered to the screen.
        /// </summary>
        protected Image sprite;
                
        /// <summary>
        /// Abstract method that is called each iteration of the
        /// game loop to allow the actor to process it's state.
        /// </summary>
        /// <param name="elapsed"></param>
        internal abstract void Step(double elapsed);

        /// <summary>
        /// Abstract method that is called when actor needs to be
        /// drawn to the screen.
        /// </summary>
        /// <param name="g"></param>
        internal abstract void Render(Graphics g);

        /// <summary>
        /// If true, actor is still considered in all game logic.
        /// If false, actor can be skipped and is no longer 
        /// rendered.
        /// </summary>
        internal bool Active { get { return active; } }

        /// <summary>
        /// Coordinates representing where the actor's top left 
        /// corner is on the form.
        /// </summary>
        internal PointF Location { get { return location; } }

        /// <summary>
        /// Rectangle representing the location and size of the 
        /// actor's sprite.
        /// </summary>
        internal RectangleF Bounds { get { return bounds; } }
    }
}