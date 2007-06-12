using System;

namespace Rb.Core.Interaction
{
	/// <summary>
	/// Works with cursor input bindings. Intersects a ray from the camera, through the cursor, with the scene
	/// </summary>
	public class CommandScenePickInterpreter : CommandInputInterpreter
	{
		/// <summary>
		/// Generates an intersection point from a cursor input binding object
		/// </summary>
		public Maths.Ray3Intersection GetIntersection( CommandInputBinding binding )
		{
			CommandCursorInputBinding cursorInput = ( CommandCursorInputBinding )binding;

			Scene.SceneDb db = binding.View.Scene;
			if ( db == null )
			{
				return null;
			}

			Scene.IRayCaster rayCaster = ( Scene.IRayCaster )db.GetSystem( typeof( Scene.IRayCaster ) );
			if ( rayCaster == null )
			{
				return null;
			}
			
			//	Create a pick ray using the input binding's cursor position, and camera
			Maths.Ray3				pickRay				= ( ( Cameras.Camera3 )cursorInput.View.Camera ).PickRay( cursorInput.X, cursorInput.Y );
			Maths.Ray3Intersection	pickIntersection	= rayCaster.GetFirstIntersection( pickRay, null );

			return pickIntersection;
		}

		/// <summary>
		/// Creates a CommandScenePickMessage
		/// </summary>
		public override CommandMessage CreateMessage( CommandInputBinding binding )
		{
			Maths.Ray3Intersection pickIntersection = GetIntersection( binding );
			if ( pickIntersection == null )
			{
				return null;
			}

			return new CommandScenePickMessage( binding.Command, pickIntersection );
		}
	}
}
