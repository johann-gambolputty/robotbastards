using System;
using Rb.Core.Assets;
using Rb.Core.Components;
using Rb.Rendering;
using Rb.World;
using Component=Rb.Core.Components.Component;

namespace Poc0.Core
{
	/// <summary>
	/// Class encapsulating graphics required by standard entity
	/// </summary>
	/// <remarks>
	/// A handy wrapper that makes it much easier to edit entity graphics in the editor
	/// </remarks>
	[Serializable]
	public class EntityGraphics : Component, IRenderable, ISceneObject
	{
		/// <summary>
		/// Location of the graphics asset used to display the entity
		/// </summary>
		public ISource GraphicsLocation
		{
			get { return m_GraphicsAsset.Source; }
			set { m_GraphicsAsset.Source = value; }
		}

		#region IRenderable Members

		/// <summary>
		/// Renders entity graphics
		/// </summary>
		/// <param name="context">Rendering context</param>
		public void Render( IRenderContext context )
		{
			IHasWorldFrame hasFrame = Parent as IHasWorldFrame;
			if ( hasFrame != null )
			{
				Graphics.Renderer.PushTransform( Transform.LocalToWorld, hasFrame.WorldFrame );
			}

			m_Lights.Begin( );

			//	Resolve graphics references, then render
			Resolve( );
			m_Graphics.Render( context );

			m_Lights.End( );
			
			if ( hasFrame != null )
			{
				Graphics.Renderer.PopTransform( Transform.LocalToWorld );
			}
		}

		#endregion

		#region ISceneObject Members

		/// <summary>
		/// Called when this object is added to a scene
		/// </summary>
		/// <param name="scene">Scene context</param>
		public void AddedToScene( Scene scene )
		{
			scene.Renderables.Add( this );
			m_Lights.AddedToScene( scene );
		}

		/// <summary>
		/// Called when this object is removed from a scene
		/// </summary>
		/// <param name="scene">Scene context</param>
		public void RemovedFromScene( Scene scene )
		{
			scene.Renderables.Remove( this );
			m_Lights.RemovedFromScene( scene );
		}

		#endregion

		private void Resolve( )
		{
			if ( m_Graphics != null )
			{
				return;
			}

			IInstanceBuilder builder = m_GraphicsAsset.Asset as IInstanceBuilder;
			if ( builder != null )
			{
				m_Graphics = ( IRenderable )builder.CreateInstance( Builder.Instance );
			}
			else
			{
				m_Graphics = ( IRenderable )m_GraphicsAsset;
			}
		}

		[NonSerialized]
		private IRenderable m_Graphics;
		private readonly LightMeter m_Lights = new LightMeter( );
		private readonly AssetHandle m_GraphicsAsset = new AssetHandle( );

	}
}
