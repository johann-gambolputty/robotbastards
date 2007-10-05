using System;
using Rb.Core.Maths;
using Rb.Rendering;
using Rb.Rendering.Cameras;
using Rb.Rendering.Windows;
using Rb.Tools.LevelEditor.Core.Selection;
using Rb.World;

namespace Rb.Tools.LevelEditor.Core.Controls.Forms
{
	/// <summary>
	/// Simple implementation of IPicker that builds on a standard rendering display
	/// </summary>
	public class PickDisplay : Display, IPicker
	{
		/// <summary>
		/// Initialises the display
		/// </summary>
		public PickDisplay( )
		{
			m_Raycaster = new RayCaster( );
			m_Raycaster.AddIntersector( new Plane3( new Vector3( 0, 1, 0 ), 0 ) );
		}

		#region IPicker Members

		/// <summary>
		/// Gets the pick raycast options
		/// </summary>
		public RayCastOptions PickOptions
		{
			get { return m_PickOptions; }
		}

		/// <summary>
		/// Creates a pick ray, and returns the first intersection in the scene
		/// </summary>
		public ILineIntersection FirstPick( int cursorX, int cursorY )
		{
			Viewer viewer = GetViewerUnderCursor( cursorX, cursorY );
			if ( ( viewer == null ) || ( viewer.Camera == null ) )
			{
				return null;
			}
			if ( viewer.Camera is ICamera3 )
			{
				Ray3 ray = ( ( ICamera3 )viewer.Camera ).PickRay( cursorX, cursorY );
				Line3Intersection intersection = m_Raycaster.GetFirstIntersection( ray, PickOptions );
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

		/// <summary>
		/// Creates cursor pick information
		/// </summary>
		/// <param name="cursorX">Cursor X position</param>
		/// <param name="cursorY">Cursor Y position</param>
		/// <returns>Returns pick information</returns>
		public PickInfoCursor CreateCursorPickInfo( int cursorX, int cursorY )
		{
			Viewer viewer = GetViewerUnderCursor( cursorX, cursorY );
			if ( ( viewer == null ) || ( viewer.Camera == null ) )
			{
				return null;
			}
			if ( viewer.Camera is ICamera3 )
			{
				Ray3 ray = ( ( ICamera3 )viewer.Camera ).PickRay( cursorX, cursorY );
				Line3Intersection intersection = m_Raycaster.GetFirstIntersection( ray, null );
				return intersection == null ? null : new PickInfoRay3( cursorX, cursorY, ray, intersection );
			}

			return null;
		}

		/// <summary>
		/// Creates a pick box
		/// </summary>
		/// <param name="topLeft">Box top left corner</param>
		/// <param name="bottomRight">Box bottom right corner</param>
		/// <returns>Returns the created pick box</returns>
		public IPickInfo CreatePickBox( PickInfoCursor topLeft, PickInfoCursor bottomRight )
		{
			if ( topLeft is IPickInfo2 )
			{
				Point2 tl = ( ( IPickInfo2 )topLeft ).PickPoint;
				Point2 br = ( ( IPickInfo2 )bottomRight ).PickPoint;
				return new PickInfoBox2( tl, br );
			}

			//	TODO: AP: Two pick rays should form a frustum
			throw new NotImplementedException( "The method or operation is not implemented." );
		}

		#endregion

		private readonly IRayCaster m_Raycaster;
		private readonly RayCastOptions m_PickOptions = new RayCastOptions( );
	}
}
