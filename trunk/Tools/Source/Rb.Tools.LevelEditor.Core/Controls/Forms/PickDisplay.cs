using System;
using Rb.Core.Maths;
using Rb.Rendering;
using Rb.Rendering.Cameras;
using Rb.Rendering.Windows;
using Rb.Tools.LevelEditor.Core.Selection;
using Rb.World;
using Rectangle=System.Drawing.Rectangle;

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
		/// Creates cursor pick information
		/// </summary>
		/// <param name="cursorX">Cursor X position</param>
		/// <param name="cursorY">Cursor Y position</param>
		/// <returns>Returns pick information</returns>
		public PickInfoCursor CreateCursorPickInfo( int cursorX, int cursorY )
		{
			Rectangle rect = Bounds;
			foreach ( Viewer viewer in Viewers )
			{
				if ( viewer.Camera == null )
				{
					continue;
				}
				if ( viewer.GetWindowRectangle( rect ).Contains( cursorX, cursorY ) )
				{
					if ( viewer.Camera is ICamera3 )
					{
						Ray3 ray = ( ( ICamera3 )viewer.Camera ).PickRay( cursorX, cursorY );
						Line3Intersection intersection = m_Raycaster.GetFirstIntersection( ray, null );
						if ( intersection == null )
						{
							return null;
						}

						return new PickInfoRay3( cursorX, cursorY, ray, intersection );
					}
					else
					{
						throw new NotImplementedException( "2D cameras not supported yet" );
					}
				}
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
	}
}
