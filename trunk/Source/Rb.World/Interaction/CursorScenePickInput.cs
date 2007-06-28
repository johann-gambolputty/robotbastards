using System;
using System.Collections.Generic;
using System.Text;
using Rb.Interaction;
using Rb.Rendering.Cameras;
using Rb.Core.Maths;

namespace Rb.World.Interaction
{
    /// <summary>
    /// Tracks the cursor position in another CursorInput object and generates PickCommandMessage messages by intersecting pick rays with the scene
    /// </summary>
    public class CursorScenePickInput : CursorPickInput
    {
        /// <summary>
        /// Sets the input context and adapted cursor input
        /// </summary>
        public CursorScenePickInput( InputContext context, CursorInput cursorInput ) :
            base( context, cursorInput )
        {
        }

        /// <summary>
        /// Creates a new command message
        /// </summary>
        public override CommandMessage CreateCommandMessage( Command cmd )
        {
            //  TODO: AP: Cast Renderable to Scene, then query for ray intersector system (otherwise, Scene must
            //  implement IRay3Intersector, which implies dimensionality)
            IRayCaster caster = ( ( Scene )Context.Viewer.Renderable ).GetService< IRayCaster >( );
            if ( caster == null )
            {
                return new PickCommandMessage( cmd, null );
            }

            //	Create a pick ray using the input binding's cursor position, and camera
            Ray3 pickRay = ( ( Camera3 )Context.Viewer.Camera ).PickRay( CursorInput.X, CursorInput.Y );
            Ray3Intersection pickIntersection = caster.GetFirstIntersection( pickRay, new RayCastOptions( ) );

            return new PickCommandMessage( cmd, pickIntersection );
        }
    }
}
