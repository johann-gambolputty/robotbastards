using System;
using Poc1.Universe.Interfaces;
using Rb.Core.Maths;
using Rb.Rendering;
using Rb.Rendering.Cameras;
using Rb.Rendering.Interfaces;
using Rb.Rendering.Interfaces.Objects.Cameras;

namespace Poc1.Universe.Classes.Cameras
{
	/// <summary>
	/// Base class implementation of <see cref="IUniCamera"/> interface
	/// </summary>
	public class UniCamera : CameraBase, IUniCamera
	{
		#region Current camera helper operations

		/// <summary>
		/// Gets the current <see cref="IUniCamera"/> applied to the rendering pipeline
		/// </summary>
		public static IUniCamera Current
		{
			get
			{
				return ( IUniCamera )Graphics.Renderer.Camera;
			}
		}

		
		/// <summary>
		/// Pushes a rendering transform suitable for astronomical distances
		/// </summary>
		public static void PushAstroRenderTransform( TransformType transformType, UniTransform transform )
		{
			Graphics.Renderer.PushTransform( transformType );
			SetAstroRenderTransform( transformType, transform );
		}

		/// <summary>
		/// Sets up a rendering transform suitable for astronomical distances
		/// </summary>
		public static void SetAstroRenderTransform( TransformType transformType, UniTransform transform )
		{
			IUniCamera curCam = Current;
			float x = ( float )Units.Convert.UniToAstroRender( transform.Position.X - curCam.Position.X );
			float y = ( float )Units.Convert.UniToAstroRender( transform.Position.Y - curCam.Position.Y );
			float z = ( float )Units.Convert.UniToAstroRender( transform.Position.Z - curCam.Position.Z );

			Graphics.Renderer.SetTransform( transformType, new Point3( x, y, z ), transform.XAxis, transform.YAxis, transform.ZAxis );
		}

		/// <summary>
		/// Pushes a rendering transform
		/// </summary>
		public static void PushRenderTransform( TransformType transformType, UniTransform transform )
		{
			Graphics.Renderer.PushTransform( transformType );
			SetRenderTransform( transformType, transform );
		}

		/// <summary>
		/// Sets the rendering transform (<see cref="IRenderer.SetTransform(TransformType,InvariantMatrix44)"/>)
		/// </summary>
		public static void SetRenderTransform( TransformType transformType, UniTransform transform )
		{
			IUniCamera curCam = Current;
			float x = ( float )Units.Convert.UniToRender( transform.Position.X - curCam.Position.X );
			float y = ( float )Units.Convert.UniToRender( transform.Position.Y - curCam.Position.Y );
			float z = ( float )Units.Convert.UniToRender( transform.Position.Z - curCam.Position.Z );

			Graphics.Renderer.SetTransform( transformType, new Point3( x, y, z ), transform.XAxis, transform.YAxis, transform.ZAxis );
		}

		#endregion

		#region Public Properties

		/// <summary>
		/// Gets or sets the field of view
		/// </summary>
		public float PerspectiveFovDegrees
		{
			get { return m_PerspectiveFov; }
			set { m_PerspectiveFov = value; }
		}

		/// <summary>
		/// The z value of the near clipping plane. The greater the better, really (means more z buffer precision)
		/// </summary>
		public float PerspectiveZNear
		{
			get { return m_PerspectiveZNear; }
			set { m_PerspectiveZNear = value; }
		}

		/// <summary>
		/// The z value of the far clipping plane
		/// </summary>
		public float PerspectiveZFar
		{
			get { return m_PerspectiveZFar; }
			set { m_PerspectiveZFar = value; }
		}

		#endregion

		#region IUniCamera Members

		/// <summary>
		/// Gets the position of the camera
		/// </summary>
		public virtual UniPoint3 Position
		{
			get { return m_Position; }
			set { m_Position = value; }
		}

		/// <summary>
		/// Gets this camera's transform
		/// </summary>
		public virtual InvariantMatrix44 Frame
		{
			get { return m_LocalView; }
			set { throw new NotSupportedException( ); }
		}

		/// <summary>
		/// Gets this camera's transform
		/// </summary>
		public virtual InvariantMatrix44 InverseFrame
		{
			get { return m_InvLocalView; }
		}

		/// <summary>
		/// Creates a pick ray from a screen position
		/// </summary>
		/// <param name="x">Screen X position</param>
		/// <param name="y">Screen Y position</param>
		/// <returns>Returns a universe ray</returns>
		public UniRay3 PickRay( int x, int y )
		{
			throw new Exception( "The method or operation is not implemented." );
		}

		#endregion

		#region IPass Members

		/// <summary>
		/// Begins the pass
		/// </summary>
		public override void Begin( )
		{
			base.Begin( );
			IRenderer renderer = Graphics.Renderer;
			int width = renderer.ViewportWidth;
			int height = renderer.ViewportHeight;

			float aspectRatio = ( height == 0 ) ? 1.0f : width / ( float )height;

			Graphics.Renderer.PushTransform( TransformType.WorldToView, m_LocalView );
			Graphics.Renderer.PushTransform( TransformType.ViewToScreen );
			Graphics.Renderer.SetPerspectiveProjectionTransform( m_PerspectiveFov, aspectRatio, m_PerspectiveZNear, m_PerspectiveZFar );

		//	m_ProjectionMatrix = Graphics.Renderer.GetTransform( TransformType.ViewToScreen );
		}

		/// <summary>
		/// Ends the pass
		/// </summary>
		public override void End( )
		{
			Graphics.Renderer.PopTransform( TransformType.WorldToView );
			Graphics.Renderer.PopTransform( TransformType.ViewToScreen );
			base.End( );
		}

		#endregion

		#region Protected Members

		/// <summary>
		/// Sets the camera view frame
		/// </summary>
		protected void SetViewFrame( Vector3 xAxis, Vector3 yAxis, Vector3 zAxis )
		{
			m_InvLocalView.Set( Point3.Origin, xAxis, yAxis, zAxis );
			m_LocalView.Copy( m_InvLocalView );
			m_LocalView.Transpose( );
		}

		/// <summary>
		/// Sets the camera view frame from a quaternion
		/// </summary>
		protected void SetViewFrame( Quaternion orientation )
		{
			Matrix44.MakeQuaternionMatrix( m_InvLocalView, orientation );
			m_LocalView.Copy( m_InvLocalView );
			m_LocalView.Transpose( );
		}

		#endregion

		#region Private Members

		private UniPoint3 m_Position = new UniPoint3( );
		private readonly Matrix44 m_LocalView = new Matrix44( );
		private readonly Matrix44 m_InvLocalView = new Matrix44( );
		private float m_PerspectiveFov = 45.0f;
		private float m_PerspectiveZNear = 5.0f;
		private float m_PerspectiveZFar = 1000.0f;

		#endregion

		#region ICamera3 Members

		public Point3 Unproject( int x, int y, float depth )
		{
			throw new NotImplementedException( );
		}

		Ray3 ICamera3.PickRay( int x, int y )
		{
			throw new NotImplementedException( );
		}

		#endregion
	}
}
