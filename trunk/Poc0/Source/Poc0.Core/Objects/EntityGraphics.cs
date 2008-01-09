using System;
using System.Drawing;
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
	/// A handy wrapper that makes it much easier to edit entity graphics in the editor.
	/// NOTE: Assumes a couple of times that the entity is the immediate parent object
	/// </remarks>
	[Serializable]
	public class EntityGraphics : Component, IRenderable, ISceneObject
	{
		#region Construction

		/// <summary>
		/// Default constructor - nothing will be rendered until <see cref="Graphics"/> or <see cref="GraphicsLocation"/>
		/// is set
		/// </summary>
		public EntityGraphics( )
		{
		}

		/// <summary>
		/// Setup constructor
		/// </summary>
		/// <param name="source">Graphics asset source</param>
		public EntityGraphics( ISource source )
		{
			GraphicsLocation = source;
		}

		#endregion

		/// <summary>
		/// Sets/gets the rendered graphics object
		/// </summary>
		public IRenderable Graphics
		{
			get { return m_Graphics; }
			set { m_Graphics = value; }
		}

		/// <summary>
		/// Location of the graphics asset used to display the entity
		/// </summary>
		public ISource GraphicsLocation
		{
			get { return m_GraphicsAsset.Source; }
			set
			{
				m_GraphicsAsset.SetSource( value, true );
			}
		}

		#region IRenderable Members

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

			Matrix44 localToWorld = Matrix44.Identity;
			IPlaceable placeable = Parent as IPlaceable;
			if ( placeable != null )
			{
				localToWorld = placeable.Frame;

				Point3 pos = placeable.Position;
				localToWorld.Translation = pos;

				float angle = placeable.Angle;
				float sinA = ( float )Math.Sin( angle );
				float cosA = ( float )Math.Cos( angle );
				localToWorld.ZAxis = new Vector3( cosA, 0, sinA );
				localToWorld.XAxis = new Vector3( -sinA, 0, cosA );
			}

			Rb.Rendering.Graphics.Renderer.PushTransform( Transform.LocalToWorld, localToWorld );

			m_Lights.Begin( );

			if ( Resolve( ) )
			{
				m_Graphics.Render( context );
			}
			
			if ( DebugInfo.ShowEntityNames )
			{
				string name = ( ( INamed )Parent ).Name;
				RenderFonts.GetDefaultFont( DefaultFont.Debug ).DrawText( RenderFont.Alignment.BottomCentre, 0, 5.0f, 0, Color.White, name );
			}

			m_Lights.End( );

			Rb.Rendering.Graphics.Renderer.PopTransform( Transform.LocalToWorld );
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
		private bool Resolve( )
		{
			if ( m_Graphics != null )
			{
				return true;
			}
			if ( m_GraphicsAsset == null )
			{
				return false;
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

			//	Add the graphics object as a child. Although this relationship isn't strictly necessary, it does
			//	mean that m_Graphics will get scene add/remove notifications, if it's an ISceneObject
			if ( Parent != null )
			{
				//	TODO: AP: Dubious hack
				//	We know that the parent object is (probably) the entity itself, which generates command
				//	messages. The graphics object may need to consume these messages (e.g. for animation).
				//	Currently, there's no way for the graphics object to discover where the entity is, and
				//	subscribe to the command messages, beyond making it an immediate child of the entity...
				//
				//	Another option is to add the entity as a dynamic property in the graphics asset load
				//	parameters, but that means that the asset is effectively uncacheable (it also implies that
				//	this object understands more about the relationship between graphics object and the entity).
				//
				//	The ideal option is for some sort of component message type discovery, but that's going
				//	to be tricky (broadcast is not an option, because the message processing must be
				//	sequential, so it has to go through the entity message hub, rather than get chucked all over
				//	the place).
				//
				( ( IParent )Parent ).AddChild( m_Graphics );
			}
			else
			{
				AddChild( m_Graphics );
			}
			return true;
		}

		[NonSerialized]
		private IRenderable m_Graphics;
		private readonly LightMeter m_Lights = new LightMeter( );
		private readonly AssetHandle m_GraphicsAsset = new AssetHandle( );
	}
}
