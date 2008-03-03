using System;
using System.Drawing;
using Rb.Core.Maths;
using Rb.Rendering;
using Rb.Rendering.Lights;
using Rb.World.Services;
using Rb.World;
using Graphics=Rb.Rendering.Graphics;

namespace Poc0.Core.Objects
{
	/// <summary>
	/// A very poorly named class that adds a <see cref="ILight"/> into the scene <see cref="ILightingService"/>
	/// </summary>
	[Serializable]
	public class PointLightSocket : IPlaceable, ISceneObject, IRenderable
	{
		/// <summary>
		/// Gets/sets the attached light
		/// </summary>
		public IPointLight Light
		{
			get { return m_Light; }
			set
			{
				DetachLight( );
				m_Light = value;
				AttachLight( );
			}
		}


		#region IPlaceable Members

		/// <summary>
		/// Event, invoked when the position of the light changes
		/// </summary>
		public event PositionChangedDelegate PositionChanged;

		/// <summary>
		/// Position of the light
		/// </summary>
		public Point3 Position
		{
			get { return m_Light.Position; }
			set
			{
				Point3 oldPos = m_Light.Position;
				m_Light.Position = value;
				if ( PositionChanged != null )
				{
					PositionChanged( this, oldPos, value );
				}
			}
		}

		/// <summary>
		/// Angle of the light
		/// </summary>
		public float Angle
		{
			get { throw new NotSupportedException( ); }
			set { throw new NotSupportedException( ); }
		}

		/// <summary>
		/// Local to world transform of the light
		/// </summary>
		public Matrix44 Frame
		{
			get { return Matrix44.Identity; }
		}

		#endregion

		#region ISceneObject Members

		/// <summary>
		/// Called when this object is added to a scene
		/// </summary>
		/// <param name="scene">Scene object</param>
		public void AddedToScene( Scene scene )
		{
			m_Scene = scene;
			AttachLight( );

			//if ( DebugInfo.ShowLights )
			{
				m_Scene.Renderables.Add( this );
			}

			DebugInfo.DebugInfoChanged += OnDebugInfoChanged;
		}

		private void OnDebugInfoChanged( )
		{
			if ( DebugInfo.ShowLights )
			{
				if ( !m_Scene.Renderables.Contains( this ) )
				{
					m_Scene.Renderables.Add( this );
				}
			}
			else
			{
				if ( m_Scene.Renderables.Contains( this ) )
				{
					m_Scene.Renderables.Remove( this );
				}
			}
		}

		/// <summary>
		/// Called when this object is removed from a scene
		/// </summary>
		/// <param name="scene">Scene object</param>
		public void RemovedFromScene(Scene scene)
		{
			DetachLight( );
			m_Scene = null;
		}

		#endregion
		
		#region Private stuff

		private IPointLight m_Light;
		private Scene m_Scene;
		
		/// <summary>
		/// Detaches the current light from the scene's lighting manager
		/// </summary>
		private void DetachLight( )
		{
			if ( ( m_Light != null ) && ( m_Scene != null ) )
			{
				ILightingService lighting = m_Scene.GetService<ILightingService>( );
				lighting.RemoveLight( m_Light );
			}
		}

		/// <summary>
		/// Attaches the current light to the scene's lighting manager
		/// </summary>
		private void AttachLight( )
		{
			if ( ( m_Light != null ) && ( m_Scene != null ) )
			{
				ILightingService lighting = m_Scene.GetService<ILightingService>( );
				lighting.AddLight( m_Light );
			}
		}

		#endregion

		#region IRenderable Members

		/// <summary>
		/// Renders this light
		/// </summary>
		public void Render( IRenderContext context )
		{
			Graphics.Draw.Sphere( Mould, Position, 1.0f );
		}

		private static Draw.IMould Mould
		{
			get
			{
				if ( ms_Mould == null )
				{
					ms_Mould = Graphics.Draw.NewMould( Color.Firebrick );
				}
				return ms_Mould;
			}
		}

		private static Draw.IMould ms_Mould;

		#endregion
	}
}
