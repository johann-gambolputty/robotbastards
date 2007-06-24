using System;
using Rb.Core.Maths;
using Rb.Rendering.Cameras;

namespace Rb.Interaction
{
    /// <summary>
    /// Tracks the cursor position in another CursorInput object and generates PickCommandMessage messages
    /// </summary>
    public class CursorPickInput : Input
    {
        /// <summary>
        /// Setup constructor
        /// </summary>
        /// <param name="context">Input context</param>
        /// <param name="cursorInput">Cursor input</param>
        public CursorPickInput( InputContext context, CursorInput cursorInput ) :
            base( context )
        {
            m_Cursor = cursorInput;
        }

        /// <summary>
        /// Access to cursor input object
        /// </summary>
        public CursorInput CursorInput
        {
            get { return m_Cursor; }
        }

        /// <summary>
        /// Returns true if this input is active
        /// </summary>
        public override bool IsActive
        {
            get { return m_Cursor.IsActive; }
            set
            {
                throw new ApplicationException( "Can't set CursorPickInput.IsActive" );
            }
        }

        /// <summary>
        /// Creates a new command message
        /// </summary>
        public override CommandMessage CreateCommandMessage( Command cmd )
        {
            //  TODO: AP: Cast Renderable to Scene, then query for ray intersector system (otherwise, Scene must
            //  implement IRay3Intersector, which implies dimensionality)
            IRay3Intersector intersector = Context.Viewer.Renderable as IRay3Intersector;
            if ( intersector == null )
            {
                return new PickCommandMessage( cmd, null );
            }

            //	Create a pick ray using the input binding's cursor position, and camera
            Ray3 pickRay = ( ( Camera3 )Context.Viewer.Camera ).PickRay( m_Cursor.X, m_Cursor.Y );
            Ray3Intersection pickIntersection = intersector.GetIntersection( pickRay );

            return new PickCommandMessage( cmd, pickIntersection );
        }

        private CursorInput m_Cursor;
    }
}
