using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using Poc1.Universe.Classes.Cameras;
using Rb.Assets;
using Rb.Assets.Interfaces;
using Rb.Core.Maths;
using Rb.Rendering.Interfaces;
using Rb.Rendering.Interfaces.Objects;
using Graphics=Rb.Rendering.Graphics;

namespace Poc1.Universe.Classes
{
	/// <summary>
	/// Sky box rendering
	/// </summary>
	public class StarBox : IRenderable
	{

		#region Construction

		/// <summary>
		/// Builds the star box (dome, sphere, thing)
		/// </summary>
		public StarBox( )
		{
			Draw.ISurface surface = Graphics.Draw.NewSurface( Graphics.Draw.NewBrush( Color.Black ), null );
			surface.FaceBrush.State.CullFaces = false;
			surface.FaceBrush.State.DepthWrite = false;
			surface.FaceBrush.State.DepthTest = false;

			Graphics.Draw.StartCache( );
			Graphics.Draw.Sphere( surface, Point3.Origin, 100, 10, 10 );
			m_Box = Graphics.Draw.StopCache( );

			ICubeMapTexture texture = Graphics.Factory.CreateCubeMapTexture( );
			LoadStarBoxTextures( texture, "Textures/StarField0/" );

			int imageIndex = 0;
			foreach ( Bitmap bmp in texture.ToBitmaps( ) )
			{
				bmp.Save( "StarBoxCubeMapFace" + imageIndex++ + ".bmp", ImageFormat.Bmp );
			}

			m_Sampler = Graphics.Factory.CreateCubeMapTextureSampler( );
			m_Sampler.Texture = texture;
		}

		#endregion

		#region Private Members

		private readonly IRenderable m_Box;
		private readonly ICubeMapTextureSampler m_Sampler;

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

		#region IRenderable Members

		/// <summary>
		/// Renders the star box
		/// </summary>
		public void Render( IRenderContext context )
		{
			GameProfiles.Game.Rendering.StarSphereRendering.Begin( );
			Graphics.Renderer.PushTransform( TransformType.Texture0, UniCamera.Current.InverseFrame );
			m_Sampler.Begin( );
			m_Box.Render( context );
			m_Sampler.End( );
			Graphics.Renderer.PopTransform( TransformType.Texture0 );
			GameProfiles.Game.Rendering.StarSphereRendering.End( );
		}

		#endregion
	}
}
