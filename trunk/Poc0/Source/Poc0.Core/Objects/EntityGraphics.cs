using System;
using System.Drawing;
using Rb.Animation;
using Rb.Assets.Base;
using Rb.Assets.Interfaces;
using Rb.Core.Components;
using Rb.Core.Maths;
using Rb.Rendering.Interfaces;
using Rb.Rendering.Interfaces.Objects;
using Rb.World;
using Component=Rb.Core.Components.Component;
using RbGraphics = Rb.Rendering.Graphics;

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
	public class EntityGraphics : Component, IRenderable, ISceneObject, IReferencePoints
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

		#region Public Members

		/// <summary>
		/// Sets/gets the rendered graphics object
		/// </summary>
		public IRenderable Graphics
		{
			get { return m_Graphics; }
			set
			{
				m_Graphics = value;
				IReferencePoint refPt = ( ( IReferencePoints )m_Graphics )[ "Weapon" ];
				refPt.OnRender += OnWeaponRender;
			}
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
				ResolveGraphics( );
			}
		}

		#endregion

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
				float sinA = Functions.Sin( angle );
				float cosA = Functions.Cos( angle );
				localToWorld.ZAxis = new Vector3( cosA, 0, sinA );
				localToWorld.XAxis = new Vector3( -sinA, 0, cosA );
			}

			RbGraphics.Renderer.PushTransform( TransformType.LocalToWorld, localToWorld );

			m_Lights.Begin( );

			if ( ResolveGraphics( ) )
			{
				m_Graphics.Render( context );
			}
			
			if ( DebugInfo.ShowEntityNames )
			{
				string name = ( ( INamed )Parent ).Name;
				RbGraphics.Fonts.DebugFont.Write( 0, 5.0f, 0, FontAlignment.BottomCentre, Color.White, name );
			}

			m_Lights.End( );

			RbGraphics.Renderer.PopTransform( TransformType.LocalToWorld );
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

		#region IReferencePoints Members

		/// <summary>
		/// Gets a named reference point
		/// </summary>
		public IReferencePoint this[ string name ]
		{
			get
			{
				return ( ( IReferencePoints )m_Graphics )[ name ];
			}
		}

		#endregion

		#region Private Members

		[NonSerialized]
		private IRenderable m_Graphics;
		private readonly LightMeter m_Lights = new LightMeter( );
		private readonly AssetHandle m_GraphicsAsset = new AssetHandle( );

		private static void OnWeaponRender( IRenderContext context )
		{
			if ( DebugInfo.ShowTagTransforms )
			{
				Rb.Rendering.Graphics.Draw.Line( RbGraphics.Pens.Red, Point3.Origin, Point3.Origin + Vector3.XAxis * 2 );
				Rb.Rendering.Graphics.Draw.Line( RbGraphics.Pens.Blue, Point3.Origin, Point3.Origin + Vector3.YAxis * 2 );
				Rb.Rendering.Graphics.Draw.Line( RbGraphics.Pens.Green, Point3.Origin, Point3.Origin + Vector3.ZAxis * 2 );
			}
		}
		
		/// <summary>
		/// Resolves the m_Graphics field
		/// </summary>
		private bool ResolveGraphics( )
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
				Graphics = ( IRenderable )builder.CreateInstance( Builder.Instance );
			}
			else
			{
				Graphics = ( IRenderable )m_GraphicsAsset;
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
		
		#endregion
	}
}
