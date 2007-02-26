using System;
using System.Windows.Forms;

namespace RbEngine.Cameras
{
	/// <summary>
	/// Controls a SphereCamera view. Created by SphereCamera.CreateController()
	/// </summary>
	public class SphereCameraControl : Rendering.IRender, IDisposable
	{
		/// <summary>
		/// Sets up the controller
		/// </summary>
		public SphereCameraControl( SphereCamera cam, Control control, Scene.SceneDb scene )
		{
			m_Control	= control;
			m_Camera	= cam;
			m_Scene		= scene;

			m_Control.MouseDown += new MouseEventHandler( OnMouseDown );
			m_Control.MouseUp += new MouseEventHandler( OnMouseUp );
			m_Control.MouseMove += new MouseEventHandler( OnMouseMove );
			m_Control.MouseWheel += new MouseEventHandler( OnMouseWheel );
			m_Control.MouseLeave += new EventHandler( OnMouseLeave );
		}

		#region	Movement

		/// <summary>
		/// Movement modes
		/// </summary>
		private enum MovementModes
		{
			kIdle,
			kRotate,
			kMove
		}

		private int								m_LastX			= 0;
		private int								m_LastY			= 0;
		private MovementModes					m_Mode			= MovementModes.kIdle;
		private SphereCamera					m_Camera;
		private Control							m_Control;
		private bool							m_RenderLookAt	= true;
		private Scene.SceneDb					m_Scene;

		private const float						kMinZoomDelta	= 0.01f;
		private const float						kMaxZoomDelta	= 1.0f;
		private const float 					kMinZoom		= 0.1f;
		private const float 					kMaxZoom		= 1000.0f;

		private static Rendering.RenderState	ms_LookAtRenderState = MakeLookAtRenderState( );

		private static Rendering.RenderState	MakeLookAtRenderState( )
		{
			Rendering.RenderState renderState = Rendering.RenderFactory.Inst.NewRenderState( );

			renderState
				.SetColour( System.Drawing.Color.Red )
				;

			return renderState;
		}

		private void OnMouseLeave( Object sender, EventArgs args )
		{
			m_Mode = MovementModes.kIdle;
		}

		private void InvalidateControl( )
		{
		//	m_Control.Invalidate( );
		}
		
		private void OnMouseWheel( Object sender, MouseEventArgs args )
		{
			float BaseZoom = ZoomScale * 20.0f;
			float ZoomDelta = ( args.Delta > 0 ) ? BaseZoom : -BaseZoom;

			m_Camera.Zoom += ZoomDelta;
			InvalidateControl( );
		}

		private float ZoomScale
		{
			get
			{
				return Maths.Utils.Lerp( kMinZoomDelta, kMaxZoomDelta, ( m_Camera.Zoom - kMinZoom ) / kMaxZoom );
			}
		}

		private void UpdateLookAt( Maths.Point3 newLookAt )
		{
			m_Camera.LookAt = newLookAt;

			//	Resolve
			Maths.Ray3				pickRay			= new Maths.Ray3( m_Camera.Position, ( newLookAt - m_Camera.Position ).MakeNormal( ) );
			Maths.Ray3Intersection	intersection	= Scene.ClosestRay3IntersectionQuery.Get( pickRay, m_Scene.Objects );
			if ( intersection != null )
			{
				newLookAt = intersection.IntersectionPosition;
			}

			m_Camera.Zoom	= m_Camera.Position.DistanceTo( newLookAt );
			m_Camera.LookAt = newLookAt;
		}

		private void OnMouseMove( Object sender, MouseEventArgs args )
		{
			int deltaX = args.X - m_LastX;
			int deltaY = args.Y - m_LastY;

			switch ( m_Mode )
			{
				case MovementModes.kIdle	:
				{
					break;
				}
				case MovementModes.kRotate	:
				{
					m_Camera.S += ( float )deltaX * 0.01f;
					m_Camera.T -= ( float )deltaY * 0.01f;

					InvalidateControl( );
					break;
				}
				case MovementModes.kMove	:
				{
					Maths.Point3 newLookAt = m_Camera.LookAt;

					float moveScale = ZoomScale;

					newLookAt += m_Camera.XAxis * deltaX * moveScale;
					newLookAt += m_Camera.YAxis * deltaY * moveScale;

					UpdateLookAt( newLookAt );

					InvalidateControl( );
					break;
				}
			}

			m_LastX = args.X;
			m_LastY = args.Y;
		}

		private void OnMouseDown( Object sender, MouseEventArgs args )
		{
			if ( args.Button == MouseButtons.Middle )
			{
				m_Mode = MovementModes.kRotate;
			}
			else if ( args.Button == MouseButtons.Left )
			{
				m_Mode = MovementModes.kMove;
			}
		}

		private void OnMouseUp( Object sender, MouseEventArgs args )
		{
			m_Mode = MovementModes.kIdle;
		}

		#endregion

		#region IRender Members

		/// <summary>
		/// Renders this camera controller (displays the look at point)
		/// </summary>
		public void Render( )
		{
			if ( m_RenderLookAt )
			{
				Rendering.Renderer.Inst.PushRenderState( ms_LookAtRenderState );
				Rendering.ShapeRenderer.Inst.DrawSphere( m_Camera.LookAt, 0.5f, 6, 6 );
				Rendering.Renderer.Inst.PopRenderState( );
			}
		}

		#endregion

		#region IDisposable Members

		public void Dispose()
		{
			m_Control.MouseDown -= new MouseEventHandler( OnMouseDown );
			m_Control.MouseUp -= new MouseEventHandler( OnMouseUp );
			m_Control.MouseMove -= new MouseEventHandler( OnMouseMove );
			m_Control.MouseWheel -= new MouseEventHandler( OnMouseWheel );
			m_Control.MouseLeave -= new EventHandler( OnMouseLeave );
		}

		#endregion
	}

}
