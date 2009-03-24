using System;
using System.Drawing;
using System.IO;
using Poc1.Core.Classes.Profiling;
using Poc1.Core.Interfaces.Rendering;
using Rb.Assets;
using Rb.Assets.Interfaces;
using Rb.Core.Maths;
using Rb.Core.Utils;
using Rb.Rendering;
using Rb.Rendering.Interfaces;
using Rb.Rendering.Interfaces.Objects;
using Graphics=Rb.Rendering.Graphics;

namespace Poc1.Core.Classes.Astronomical.Stars
{
	/// <summary>
	/// Sky box rendering
	/// </summary>
	public class StarBox : IRenderable<IUniRenderContext>
	{
		#region Construction

		/// <summary>
		/// Builds the star box (dome, sphere, thing)
		/// </summary>
		public StarBox( ) :
			this( "Effects/Planets/stars.cgfx", "Star Fields/Default/" )
		{
		}

		/// <summary>
		/// Setup constructor
		/// </summary>
		/// <param name="starEffectPath">Path to the star effect</param>
		/// <param name="starTexturePath">Path to the star textures</param>
		public StarBox( string starEffectPath, string starTexturePath )
		{
			Arguments.CheckNotNullOrEmpty( starEffectPath, "starEffectPath" );
			Arguments.CheckNotNullOrEmpty( starTexturePath, "starEffectPath" );
			EffectAssetHandle effect = new EffectAssetHandle( starEffectPath, false );
			m_Technique = new TechniqueSelector( effect, "DefaultTechnique" );

			Draw.ISurface surface = Graphics.Draw.NewSurface( Graphics.Draw.NewBrush( Color.Black ), null );
			surface.FaceBrush.State.CullFaces = false;
			surface.FaceBrush.State.DepthWrite = false;
			surface.FaceBrush.State.DepthTest = false;

			Graphics.Draw.StartCache( );
			Graphics.Draw.Sphere( surface, Point3.Origin, 100, 10, 10 );
			m_Box = Graphics.Draw.StopCache( );

			m_Texture = Graphics.Factory.CreateCubeMapTexture( );
			LoadStarBoxTextures( m_Texture, "Star Fields/Default/" );
		}

		#endregion

		#region IRenderable Members

		/// <summary>
		/// Renders the star box
		/// </summary>
		public void Render( IUniRenderContext context )
		{
			GameProfiles.Game.Rendering.StarSphereRendering.Begin( );
			Graphics.Renderer.PushTransform( TransformType.Texture0, context.Camera.InverseFrame );
		//	m_Sampler.Begin( );
			m_Technique.Effect.Parameters[ "StarsTexture" ].Set( m_Texture );
			m_Technique.Apply( context, m_Box );
		//	m_Sampler.End( );
			Graphics.Renderer.PopTransform( TransformType.Texture0 );
			GameProfiles.Game.Rendering.StarSphereRendering.End( );
		}

		/// <summary>
		/// Renders this object
		/// </summary>
		/// <param name="context">Rendering context</param>
		public void Render( IRenderContext context )
		{
			if ( context is IUniRenderContext )
			{
				Render( ( IUniRenderContext )context );
				return;
			}
			throw new NotSupportedException( "Rendering from standard render context is not supported - use an IUniRenderContext" );
		}

		#endregion

		#region Private Members

		private readonly ITechnique m_Technique;
		private readonly ICubeMapTexture m_Texture;
		private readonly IRenderable m_Box;

		/// <summary>
		/// Loads star box textures from a given folder into a cube map texture
		/// </summary>
		private static void LoadStarBoxTextures( ICubeMapTexture texture, string path )
		{
			texture.Build
				(
					LoadCubeMapFace( path + "starBox_rt.jpg" ),
					LoadCubeMapFace( path + "starBox_lf.jpg" ),
					LoadCubeMapFace( path + "starBox_up.jpg" ),
					LoadCubeMapFace( path + "starBox_dn.jpg" ),
					LoadCubeMapFace( path + "starBox_fr.jpg" ),
					LoadCubeMapFace( path + "starBox_bk.jpg" ),
					true
				);
		}

		/// <summary>
		/// Loads a bitmap cube map face
		/// </summary>
		private static Bitmap LoadCubeMapFace( string path )
		{
			ILocation location = Locations.NewLocation( path );
			if ( location == null )
			{
				throw new ArgumentException( string.Format( "Failed to find asset location \"{0}\"", path ) );
			}
			using ( Stream stream = ( ( IStreamSource )location ).Open( ) )
			{
				return ( Bitmap )Image.FromStream( stream );
			}
		}

		#endregion
	}
}
