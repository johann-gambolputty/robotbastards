using System;
using Poc1.Universe.Interfaces;
using Rb.Core.Components;
using Rb.Core.Maths;
using Rb.Rendering;
using Rb.Rendering.Interfaces;

namespace Poc1.Universe.Classes.Cameras
{
	/// <summary>
	/// Base class implementation of <see cref="IUniCamera"/> interface
	/// </summary>
	public class UniCamera : Component, IUniCamera
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

		public const double UniUnitsToAstroRenderUnits = 0.00001;

		public static float ToAstroRenderUnits( long uniUnits )
		{
			return ( float )( UniUnits.ToMetres( uniUnits ) * UniUnitsToAstroRenderUnits );
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
			float x = ToAstroRenderUnits( transform.Position.X - curCam.Position.X );
			float y = ToAstroRenderUnits( transform.Position.Y - curCam.Position.Y );
			float z = ToAstroRenderUnits( transform.Position.Z - curCam.Position.Z );

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
		/// Sets the rendering transform (<see cref="IRenderer.SetTransform(TransformType,Matrix44)"/>)
		/// </summary>
		public static void SetRenderTransform( TransformType transformType, UniTransform transform )
		{
			IUniCamera curCam = Current;
			float x = ( float )UniUnits.ToMetres( transform.Position.X - curCam.Position.X );
			float y = ( float )UniUnits.ToMetres( transform.Position.Y - curCam.Position.Y );
			float z = ( float )UniUnits.ToMetres( transform.Position.Z - curCam.Position.Z );

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
		public virtual Matrix44 Frame
		{
			get { return m_LocalView; }
		}

		/// <summary>
		/// Gets this camera's transform
		/// </summary>
		public virtual Matrix44 InverseFrame
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
		public virtual void Begin( )
		{
			Graphics.Renderer.Camera = this;
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
		public virtual void End( )
		{
			Graphics.Renderer.Camera = null;
			Graphics.Renderer.PopTransform( TransformType.WorldToView );
			Graphics.Renderer.PopTransform( TransformType.ViewToScreen );
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

		#endregion

		#region Private Members

		private UniPoint3 m_Position = new UniPoint3( );
		private readonly Matrix44 m_LocalView = new Matrix44( );
		private readonly Matrix44 m_InvLocalView = new Matrix44( );
		private float m_PerspectiveFov = 45.0f;
		private float m_PerspectiveZNear = 5.0f;
		private float m_PerspectiveZFar = 1000.0f;

		#endregion
	}
}
