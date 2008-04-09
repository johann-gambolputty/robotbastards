using System;
using System.Collections;
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
	public class UniCamera : IUniCamera
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
		/// Sets the rendering transform (<see cref="IRenderer.SetTransform(Transform,Matrix44)"/>)
		/// </summary>
		public static void SetRenderTransform( Transform transformType, UniTransform transform )
		{
			IUniCamera curCam = Current;
			float x = ( float )UniUnits.ToMetres( transform.Position.X - curCam.Frame.Position.X );
			float y = ( float )UniUnits.ToMetres( transform.Position.Y - curCam.Frame.Position.Y );
			float z = ( float )UniUnits.ToMetres( transform.Position.Z - curCam.Frame.Position.Z );

			Graphics.Renderer.SetTransform( transformType, new Point3( x, y, z ), transform.XAxis, transform.YAxis, transform.ZAxis );
		}

		#endregion

		#region IUniCamera Members

		/// <summary>
		/// Gets this camera's transform
		/// </summary>
		public virtual UniTransform Frame
		{
			get { return m_Transform; }
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

		#region IParent Members

		/// <summary>
		/// Gets the child object collection
		/// </summary>
		public ICollection Children
		{
			get { return m_Children; }
		}

		/// <summary>
		/// Adds a child object
		/// </summary>
		/// <param name="obj">Child object</param>
		public void AddChild( object obj )
		{
			if ( obj == null )
			{
				throw new ArgumentNullException( "obj" );
			}
			m_Children.Add( obj );
			if ( OnChildAdded != null )
			{
				OnChildAdded( this, obj );
			}
		}

		/// <summary>
		/// Removes a child object
		/// </summary>
		/// <param name="obj">Child object</param>
		public void RemoveChild( object obj )
		{
			if ( obj == null )
			{
				throw new ArgumentNullException( "obj" );
			}
			m_Children.Remove( obj );
			if ( OnChildRemoved != null )
			{
				OnChildRemoved( this, obj );
			}
		}

		/// <summary>
		/// Invoked by AddChild()
		/// </summary>
		public event OnChildAddedDelegate OnChildAdded;

		/// <summary>
		/// Invoked by RemoveChild()
		/// </summary>
		public event OnChildRemovedDelegate OnChildRemoved;

		#endregion

		#region IPass Members

		/// <summary>
		/// Begins the pass
		/// </summary>
		public virtual void Begin( )
		{
			Graphics.Renderer.Camera = this;
		}

		/// <summary>
		/// Ends the pass
		/// </summary>
		public virtual void End( )
		{
			Graphics.Renderer.Camera = null;
		}

		#endregion

		#region Private Members

		private readonly UniTransform m_Transform = new UniTransform( );
		private readonly ArrayList m_Children = new ArrayList( );

		#endregion
	}
}
