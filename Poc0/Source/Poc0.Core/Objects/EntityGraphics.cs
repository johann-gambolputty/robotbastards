using System;
using Rb.Core.Assets;
using Rb.Core.Components;
using Rb.Core.Maths;
using Rb.Rendering;
using Rb.World;
using Component=Rb.Core.Components.Component;

namespace Poc0.Core.Objects
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

		private readonly Matrix44 m_LocalToWorld = new Matrix44( );

		/// <summary>
		/// Renders entity graphics
		/// </summary>
		/// <param name="context">Rendering context</param>
		public void Render( IRenderContext context )
		{
			IMoveable moveable = Parent as IMoveable;
			if ( moveable != null )
			{
				moveable.Travel.UpdateCurrent( context.RenderTime );
			}
			IPlaceable placeable = Parent as IPlaceable;
			if ( placeable != null )
			{
				Point3 pos = placeable.Position;
				m_LocalToWorld.Translation = pos;
			}
			ITurnable turnable = Parent as ITurnable;
			if ( turnable != null )
			{
				turnable.Turn.UpdateCurrent( context.RenderTime );
			}
			IOriented oriented = Parent as IOriented;
			if ( oriented != null )
			{
				//oriented.Angle;
				float sinA = ( float )Math.Sin( oriented.Angle );
				float cosA = ( float )Math.Cos( oriented.Angle );
				m_LocalToWorld.ZAxis = new Vector3( cosA, 0, sinA );
				m_LocalToWorld.XAxis = new Vector3( -sinA, 0, cosA );
			}

			Graphics.Renderer.PushTransform( Transform.LocalToWorld, m_LocalToWorld );

			m_Lights.Begin( );

			//	Resolve graphics references, then render
			Resolve( );
			m_Graphics.Render( context );

			m_Lights.End( );
			
			Graphics.Renderer.PopTransform( Transform.LocalToWorld );
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

		/// <summary>
		/// Resolves the graphics
		/// </summary>
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
