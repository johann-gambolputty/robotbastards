using Rb.Core.Maths;
using Rb.Rendering.Base;
using Rb.Rendering.Interfaces.Objects.Cameras;
using Rb.Rendering.Windows;
using Rb.Tools.LevelEditor.Core.Selection;
using Rb.World;
using Rb.World.Services;

namespace Rb.Tools.LevelEditor.Core.Controls.Forms
{
	/// <summary>
	/// Simple implementation of IPicker that builds on a standard rendering display
	/// </summary>
	public class PickDisplay : Display, IPicker
	{
		#region IPicker Members

		/// <summary>
		/// Creates a pick ray, and returns the first intersection in the scene
		/// </summary>
		public ILineIntersection FirstPick( int cursorX, int cursorY, RayCastOptions options )
		{
			Viewer viewer = GetViewerUnderCursor( cursorX, cursorY );
			if ( ( viewer == null ) || ( viewer.Camera == null ) )
			{
				return null;
			}
			if ( viewer.Camera is ICamera3 )
			{
				IRayCastService rayCaster = EditorState.Instance.CurrentScene.GetService< IRayCastService >( );
				if ( rayCaster == null )
				{
					return null;
				}

				Ray3 ray = ( ( ICamera3 )viewer.Camera ).PickRay( cursorX, cursorY );
				Line3Intersection intersection = rayCaster.GetFirstIntersection( ray, options );
				return intersection;
			}

			return null;
		}
		
		/// <summary>
		/// Gets the objects whose shapes overlap a box (frustum in 3d)
		/// </summary>
		/// <param name="left">Top left corner X coordinate</param>
		/// <param name="top">Top left corner Y coordinate</param>
		/// <param name="right">Bottom right corner X coordinate</param>
		/// <param name="bottom">Bottom right corner Y coordinate</param>
		/// <returns>Returns a list of objects in the box</returns>
		public object[] GetObjectsInBox( int left, int top, int right, int bottom )
		{
			Viewer viewer = GetViewerUnderCursor( left, top );
			if ( ( viewer == null ) || ( viewer.Camera == null ) )
			{
				return new object[] {};
			}

			if ( viewer.Camera is ICamera3 )
			{
				//	TODO: AP: Create a frustum. Run through all objects with shapes. If shape intersects frustum, add to list
			}

			return new object[] {};
		}

		#endregion
	}
}
