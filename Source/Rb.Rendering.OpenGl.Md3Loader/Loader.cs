using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using Rb.Core.Assets;
using Rb.Core.Components;
using Rb.Core.Maths;
using Rb.Rendering;
using Rb.Rendering.OpenGl;
using Rb.Rendering.Textures;
using Tao.OpenGl;

namespace Rb.Rendering.OpenGl.Md3Loader
{
	/// <summary>
	/// Loads Quake3 MD3 files, generating OpenGL meshes
	/// </summary>
	public class Loader : AssetLoader
	{
		#region	AssetLoader Members

		/// <summary>
		/// Gets the asset name
		/// </summary>
		public override string Name
		{
			get { return "MD3"; }
		}

		/// <summary>
		/// Gets the asset extension
		/// </summary>
		public override string[] Extensions
		{
			get { return new string[] { "md3" }; }
		}

		/// <summary>
		/// Creates default parameters for this loader
		/// </summary>
		/// <param name="addAllProperties">If true, then all dynamic properties are added</param>
		/// <returns>Returns default loading parameters</returns>
		public override LoadParameters CreateDefaultParameters( bool addAllProperties )
		{
			LoadParameters parameters = base.CreateDefaultParameters( addAllProperties );

			if ( addAllProperties )
			{
				parameters.Properties.Add( "scaleX", 1.0f );
				parameters.Properties.Add( "scaleY", 1.0f );
				parameters.Properties.Add( "scaleZ", 1.0f );
			}

			return parameters;
		}

		/// <summary>
		/// Loads... stuff
		/// </summary>
		public override object Load( ISource source, LoadParameters parameters )
		{
			parameters.CanCache = true;

			Vector3 scale = new Vector3
				(
					DynamicProperties.GetProperty( parameters.Properties, "scaleX", 1.0f ),
					DynamicProperties.GetProperty( parameters.Properties, "scaleY", 1.0f ),
					DynamicProperties.GetProperty( parameters.Properties, "scaleZ", 1.0f )
				);

			Matrix44 transform = new Matrix44( );
			transform.Scale( scale.X, scale.Y, scale.Z );

			//	create the model
			Model model = new Model( );

			//	oh dear another md3 hack - directories mean articulated models, with skins and animations. Files
			//	mean single objects
			if ( !source.Directory )
			{
				LoadObjectModel (model, source, transform );
			}
			else
			{
				//	Load animations
				model.Animations = LoadAnimations(AnimationFile(source));
				LoadNestedModel(model, source, transform);
			}
			return model;
		}

		private static void LoadObjectModel( Model model, ISource source, Matrix44 transform )
		{
			//	Load the skin file for the current part
			IDictionary< string, ITexture2d > textureTable = LoadSkin( source, DefaultSkinFile( source, ModelPart.Weapon ) );

			//	Load the MD3 associated with the current part
			Mesh partMesh = LoadMd3( source, model, ModelPart.Weapon, transform, source, textureTable );
			model.SetPartMesh( ModelPart.Weapon, partMesh );
			model.SetRootMesh( partMesh );
		}

		private static void LoadNestedModel( Model model, ISource source, Matrix44 transform )
		{
			//	Run through all the parts
			for ( int partIndex = 0; partIndex < ( int )ModelPart.NumParts; ++partIndex )
			{
				ModelPart curPart = ( ModelPart )partIndex;

				if ( curPart == ModelPart.Weapon )
				{
					//	The weapon does not have a related mesh, so just ignore...
					continue;
				}
				//	Load the skin file for the current part
				IDictionary< string, ITexture2d > surfaceTextureTable = LoadSkin( source, DefaultSkinFile( source, curPart ) );

				//	Load the MD3 associated with the current part
				Mesh partMesh = LoadMd3( source, model, curPart, transform, MeshFile( source, curPart ), surfaceTextureTable );
				model.SetPartMesh( curPart, partMesh );
			}

			model.SetRootMesh( model.GetPartMesh( ModelPart.Lower ) );
			NestMesh( model, ModelPart.Lower, ModelPart.Upper, "tag_torso" );
			NestMesh( model, ModelPart.Upper, ModelPart.Head, "tag_head" );
			NestMesh( model, ModelPart.Upper, ModelPart.Weapon, "tag_weapon" );
		}

		/// <summary>
		/// Nests one model part's mesh inside another, using a named tag as a transform
		/// </summary>
		private static void NestMesh( Model model, ModelPart parent, ModelPart child, string tagName )
		{
			model.GetPartMesh( parent ).AddNestedPart( child, model.GetPartMesh( parent ).GetTagIndex( tagName ) );
		}

		/// <summary>
		/// Returns true if the specified source can be loaded
		/// </summary>
		public override bool CanLoad( ISource source )
		{
			if ( !source.Directory )
			{
				return source.HasExtension( "md3" );
			}
			return source.Provider.Contains( source.Path, "*.md3" );
		}

		#endregion
		
		#region	Filename builders

		/// <summary>
		/// Builds the name of the animation file
		/// </summary>
		private static ISource AnimationFile( ISource source )
		{
			return source.GetRelativeSource( "animation.cfg" );
		}


		/// <summary>
		/// Returns the mesh filename for a part
		/// </summary>
		private static ISource MeshFile( ISource source, ModelPart part )
		{
			return source.GetRelativeSource( part + ".md3" );
		}

		/// <summary>
		/// Returns the default skin filename for a part
		/// </summary>
		private static ISource DefaultSkinFile( ISource source, ModelPart part )
		{
			return source.GetRelativeSource( part + "_default.skin" );
		}

		/// <summary>
		/// Returns the texture name ()
		/// </summary>
		/// <remarks>
		/// paths are stored in the skin file relative to the q3 root directory - we just want them in the resource directory, wherever that is, so this
		/// function strips of the "models/players/modelName" bit and adds on the resource directory.
		/// Also, the textures are usually .tga files, which the texture loader can't currently cope with (TODO: ...) - change the extension to
		/// .bmp in this case
		/// </remarks>
		private static ISource TextureFile( ISource source, string path )
		{
			//	Remove the extension
			string name = Path.GetFileNameWithoutExtension( path );
			foreach ( string ext in TextureExtensions )
			{
				ISource textureSource = source.GetRelativeSource( name + ext );
				if ( textureSource.Exists )
				{
					return textureSource;
				}
			}

			throw new ApplicationException( string.Format( "Could not find texture file beginning with \"{0}\"", name ) );
		}

		private static readonly string[] TextureExtensions = new string[] { ".jpg", ".bmp" };

		#endregion

		#region	Skin loading

		/// <summary>
		/// Loads a skin file
		/// </summary>
		private static IDictionary<string, ITexture2d> LoadSkin( ISource source, ISource skinSource )
		{
			using ( Stream inputStream = skinSource.Open( ) )
			{
				TextReader reader = new StreamReader( inputStream );
				IDictionary<string, ITexture2d> surfaceTextureMap = new Dictionary<string, ITexture2d>();

				char[] tokenSplit = new char[ ] { ',' };

				for ( string curLine = reader.ReadLine( ); curLine != null; curLine = reader.ReadLine( ) )
				{
					string[] tokens = curLine.Split( tokenSplit );

				//	if ( !tokens[ 0 ].StartsWith( "tag_" ) )
					if ( ( tokens.Length == 2 ) && ( !string.IsNullOrEmpty( tokens[ 1 ] ) ) )
					{
						//	TODO: AP: Texture loading should be done through the asset manager
						ITexture2d newTexture = Graphics.Factory.NewTexture2d( );

						using ( Stream textureStream = TextureFile( source, tokens[ 1 ] ).Open( ) )
						{
							TextureUtils.Load( newTexture, textureStream, true );
						}

						surfaceTextureMap[ tokens[ 0 ] ] = newTexture;
					}
				}

				return surfaceTextureMap;
			}
		}

		#endregion

		#region	Animation loading

		/// <summary>
		/// Loads in model animations
		/// </summary>
		public AnimationSet LoadAnimations( ISource source )
		{
			if ( !source.Exists )
			{
				return null;
			}

			using ( Stream inputStream = source.Open( ) )
			{
				TextReader		reader		= new StreamReader( inputStream );
				char[]			tokenSplit	= new char[ ] { ' ', '\t' };
				AnimationSet	animations	= new AnimationSet( );

				//	Run through all the lines in the file
				int animIndex = 0;
				for ( string curLine = reader.ReadLine( ); curLine != null; curLine = reader.ReadLine( ) )
				{
					string[] tokens = curLine.Split( tokenSplit );

					if ( ( tokens.Length == 0 ) || ( tokens[ 0 ] == string.Empty ) )
					{
						//	Empty line - ignore it
						continue;
					}
					if ( tokens[ 0 ] == "//" )
					{
						//	Line begins with comment - ignore it
						continue;
					}
					if ( tokens[ 0 ] == "sex" || tokens[ 0 ] == "headoffset" || tokens[ 0 ] == "footsteps" )
					{
						//	Model gender/head offset/footsteps - ignore it
						continue;
					}

					//	Line must be an animation specification, which is 4 whitespace separated integers, A B C D
					//	where A is the first frame of the anim, B is the number of frames, C is the number of looping frames, and D
					//	is the number of frames per second
					int firstFrame		= int.Parse( tokens[ 0 ] );
					int numFrames		= int.Parse( tokens[ 1 ] );
					int loopingFrames	= int.Parse( tokens[ 2 ] );
					int framesPerSecond	= int.Parse( tokens[ 3 ] );

					Animation curAnim = new Animation( ( AnimationType )animIndex, firstFrame, numFrames, loopingFrames, framesPerSecond );
					
					animations.Animations[ animIndex++ ] = curAnim;
				}

				//	Correct leg animation frames
				int firstLegFrame	= animations.Animations[ ( int )AnimationType.FirstLegAnim ].FirstFrame;
				int firstTorsoFrame	= animations.Animations[ ( int )AnimationType.FirstTorsoAnim ].FirstFrame;
				int legCorrection	= firstLegFrame - firstTorsoFrame;
				for ( animIndex = ( int )AnimationType.FirstLegAnim; animIndex < ( int )AnimationType.NumAnimations; ++animIndex )
				{
					animations.Animations[ animIndex ].FirstFrame -= legCorrection;
				}

				return animations;
			}
		}

		#endregion

		#region	Mesh loading

		/// <summary>
		/// Loads an MD3 mesh resource from a stream
		/// </summary>
		private static Mesh LoadMd3( ISource source, Model model, ModelPart part, Matrix44 transform, ISource md3Source, IDictionary<string, ITexture2d> surfaceTextureTable )
		{
			using ( Stream inputStream = md3Source.Open( ) )
			{
				BinaryReader reader = new BinaryReader( inputStream );

				//	http://icculus.org/homepages/phaethon/q3a/formats/md3format.html

				//	Make sure of the MD3 identity
				byte[] ident		= reader.ReadBytes( 4 );
				if ( ( ident[ 0 ] != 'I' ) || ( ident[ 1 ] != 'D' ) || ( ident[ 2 ] != 'P' ) || ( ident[ 3 ] != '3' ) )
				{
					throw new ApplicationException( "Failed to load MD3 resource - stream did not start with 'IDP3' MD3 identifier" );
				}

				//	Read in header
				//int version			=
				reader.ReadInt32( );
				//string name			=
				ReadString( reader, MaxPathLength );
				//int flags			=
				reader.ReadInt32( );
				int numFrames		= reader.ReadInt32( );
				int numTags			= reader.ReadInt32( );
				int numSurfaces		= reader.ReadInt32( );
				//int numSkins		=
				reader.ReadInt32( );
				int framesOffset	= reader.ReadInt32( );
				int tagsOffset		= reader.ReadInt32( );
				int surfacesOffset	= reader.ReadInt32( );
				//int eofOffset		=
				reader.ReadInt32( );

				//	TODO: Can load directly into mesh frame, tag and surface structures - don't do this intermediate step
				Mesh mesh = new Mesh( model, part );

				ReadFrames( reader, framesOffset, numFrames, mesh, transform );
				ReadTags( reader, tagsOffset, numTags, numFrames, mesh, transform );
				ReadSurfaces( source, reader, surfacesOffset, numSurfaces, numFrames, mesh, surfaceTextureTable, transform );

				//	TODO: REMOVE. test frames
				string md3Name = md3Source.ToString( );
				if ( md3Name.IndexOf( "Upper" ) != -1 )
				{
					mesh.DefaultFrame = 151;
				}
				else if ( md3Name.IndexOf( "Head" ) != -1 )
				{
					mesh.DefaultFrame = 0;
				}
				else
				{
					mesh.DefaultFrame = 0;
				}

				return mesh;
			}
		}

		#endregion

		#region	MD3 constants

		private const int	MaxPathLength	= 64;
		private const int	FrameNameLength	= 16;
		private const float	XyzScale		= 1.0f / 64.0f;
		private const float ByteToAngle		= Constants.TwoPi / 255.0f;

		#endregion

		#region	MD3 data type read methods

		/// <summary>
		/// Reads and returns a Point3 from a BinaryReader
		/// </summary>
		private static Point3 ReadPoint( BinaryReader reader )
		{
			float z = reader.ReadSingle( );
			float x = reader.ReadSingle( );
			float y = reader.ReadSingle( );

			return new Point3( x, y, z );
		}

		/// <summary>
		/// Reads and returns a Vector3 from a BinaryReader
		/// </summary>
		private static Vector3 ReadVector( BinaryReader reader )
		{
			float x = reader.ReadSingle( );
			float y = reader.ReadSingle( );
			float z = reader.ReadSingle( );

			return new Vector3( x, y, z );
		}

		/// <summary>
		/// Reads a string
		/// </summary>
		private static string ReadString( BinaryReader reader, int maxLength )
		{
			byte[] strBytes = reader.ReadBytes( maxLength );
			char[] strChars = new char[ maxLength ];

			int byteIndex = 0;
			if ( ( strBytes[ 0 ] == 0 ) && ( strBytes[ 1 ] != 0 ) )
			{
				++byteIndex;
			}

			int numChars = 0;
			for ( ; ( strBytes[ byteIndex ] != 0 ) && ( byteIndex < strBytes.Length ); ++numChars, ++byteIndex )
			{
				strChars[ numChars ] = ( char )strBytes[ byteIndex ];
			}

			//	For some reason, MD3 paths sometimes start with a null character...
			string result = new string( strChars, 0, numChars );
			return result;
		}

		#endregion

		#region	MD3 section read methods

		/// <summary>
		/// Reads frame information
		/// </summary>
		private static void ReadFrames( BinaryReader reader, long offset, int numFrames, Mesh mesh, Matrix44 transform )
		{
			reader.BaseStream.Seek( offset, SeekOrigin.Begin );

			Mesh.FrameInfo[] frames = new Mesh.FrameInfo[ numFrames ];

			float scaleLength = ( transform * new Vector3( 1, 1, 1 ) ).Length;

			for ( int frameCount = 0; frameCount < numFrames; ++frameCount )
			{
				frames[ frameCount]		= new Mesh.FrameInfo( );
				Mesh.FrameInfo curFrame	= frames[ frameCount ];
				curFrame.MinBounds		= transform * ReadPoint( reader );
				curFrame.MaxBounds		= transform * ReadPoint( reader );
				curFrame.Origin			= transform * ReadPoint( reader );
				curFrame.Radius			= reader.ReadSingle( ) * scaleLength;
				curFrame.Name			= ReadString( reader, FrameNameLength );
			}

			mesh.FrameInfoList = frames;
		}

		/// <summary>
		/// Reads tag information
		/// </summary>
		private static void ReadTags( BinaryReader reader, long offset, int numTags, int numFrames, Mesh mesh, Matrix44 transform )
		{
			reader.BaseStream.Seek( offset, SeekOrigin.Begin );

			mesh.TagNames = new string[ numTags ];

			for ( int frameIndex = 0; frameIndex < numFrames; ++frameIndex )
			{
				Mesh.Tag[] tags = new Mesh.Tag[ numTags ];
				for ( int tagCount = 0; tagCount < numTags; ++tagCount )
				{
					string tagName = ReadString( reader, MaxPathLength );

					if ( frameIndex == 0 )
					{
						mesh.TagNames[ tagCount ] = tagName;
					}

					Point3	origin	= transform * ReadPoint( reader );
					Vector3 xAxis	= ReadVector( reader );
					Vector3 yAxis	= ReadVector( reader );
					Vector3 zAxis	= ReadVector( reader );

					Mesh.Tag curTag = new Mesh.Tag( );
					curTag.Transform = new Matrix44( origin, xAxis, yAxis, zAxis );
					curTag.Name = tagName;

					tags[ tagCount ] = curTag;
				}
				mesh.FrameInfoList[ frameIndex ].Tags = tags;
			}
		}
		
		/// <summary>
		/// Reads surface information
		/// </summary>
		private static void ReadSurfaces( ISource source, BinaryReader reader, long offset, int numSurfaces, int numFrames, Mesh mesh, IDictionary<string, ITexture2d> surfaceTextureTable, Matrix44 transform)
		{
			//	Move to the start of the first surface
			reader.BaseStream.Seek( offset, SeekOrigin.Begin );

			//	Allocate mesh surface array
			mesh.Surfaces = new Mesh.Surface[ numSurfaces ];

			for ( int surfaceCount = 0; surfaceCount < numSurfaces; ++surfaceCount )
			{
				//	Create a new surface
				Mesh.Surface curSurface		= new Mesh.Surface( );

				//int		ident				=
				reader.ReadInt32( );
				string	name				= ReadString ( reader, MaxPathLength );
				//int		flags				= 
				reader.ReadInt32( );
				//int		surfaceNumFrames	= 
				reader.ReadInt32( );
				//int		numShaders			= 
				reader.ReadInt32( );
				int		numVertices			= reader.ReadInt32( );
				int		numTriangles		= reader.ReadInt32( );
				int		trianglesOffset		= reader.ReadInt32( );
				//int		shadersOffset		= 
				reader.ReadInt32( );
				int		texturesOffset		= reader.ReadInt32( );
				int		verticesOffset		= reader.ReadInt32( );
				int		endOffset			= reader.ReadInt32( );

				//	Assign surface texture
				curSurface.Texture			= GetTexture( source, surfaceTextureTable, name );
			
				//	Assign surface shaders
			//	ReadShaders( reader, offset + shadersOffset, numShaders );

				//	Read in surface index group and texture UVs
				curSurface.Group			= ReadTriangles( reader, offset + trianglesOffset, numTriangles );
				curSurface.TextureUVs		= ReadTextureUVs( reader, offset + texturesOffset, numVertices );

				//	Read in surface vertices
			//	curSurface.NumVertices		= numVertices;
				ReadVertices( reader, offset + verticesOffset, numVertices, numFrames, curSurface, transform );

				//	Assign the new surface to the mesh
				mesh.Surfaces[ surfaceCount ] = curSurface;

				//	Move the stream to the next surface
				reader.BaseStream.Seek( offset + endOffset, SeekOrigin.Begin );
				offset += endOffset;
			}
		}

		private static ITexture2d GetTexture( ISource source, IDictionary<string, ITexture2d> textureTable, string name )
		{
			try
			{
				if ( textureTable != null )
				{
					return textureTable[ name ];
				}
				ISource textureSource = TextureFile( source, name );

				ITexture2d newTexture = Graphics.Factory.NewTexture2d( );
				using ( Stream textureStream = textureSource.Open( ) )
				{
					TextureUtils.Load( newTexture, textureStream, true );
				}
				return newTexture;
			}
			catch ( Exception ex )
			{
				throw new ApplicationException( string.Format( "Failed to load texture for name \"{0}\"", name ), ex );
			}
		}

		/// <summary>
		/// Reads surface shaders
		/// </summary>
		//private static void ReadShaders(BinaryReader reader, long offset, int numShaders)
		//{
		//    reader.BaseStream.Seek( offset, SeekOrigin.Begin );

		//    for ( int shaderCount = 0; shaderCount < numShaders; ++shaderCount )
		//    {
		//        string shaderName  = ReadString( reader, MaxPathLength );
		//        int index = reader.ReadInt32( );
		//    }
		//}

		/// <summary>
		/// Reads surface triangles
		/// </summary>
		private static OpenGlIndexedGroup ReadTriangles( BinaryReader reader, long offset, int numTriangles )
		{
			reader.BaseStream.Seek( offset, SeekOrigin.Begin );

			int[] triangles = new int[ 3 * numTriangles ];
			int curTriIndex = 0;
			for ( int triangleCount = 0; triangleCount < numTriangles; ++triangleCount )
			{
				triangles[ curTriIndex + 0 ] = reader.ReadInt32( );
				triangles[ curTriIndex + 2 ] = reader.ReadInt32( );
				triangles[ curTriIndex + 1 ] = reader.ReadInt32( );
				curTriIndex += 3;
			}

			return new OpenGlIndexedGroup( Gl.GL_TRIANGLES, triangles );
		}

		/// <summary>
		/// Reads surface texture coordinates
		/// </summary>
		private static OpenGlVertexArray ReadTextureUVs(BinaryReader reader, long offset, int numCoordinates)
		{
			reader.BaseStream.Seek( offset, SeekOrigin.Begin );
			
			float[] texCoords = new float[ numCoordinates * 2 ];
			int coordinateIndex = 0;
			for ( int coordinateCount = 0; coordinateCount < numCoordinates; ++coordinateCount )
			{
				texCoords[ coordinateIndex++ ] = reader.ReadSingle( );
				texCoords[ coordinateIndex++ ] = reader.ReadSingle( );
			}

			return new OpenGlVertexArray( numCoordinates, 0, Gl.GL_TEXTURE_COORD_ARRAY, 0, 2, Gl.GL_STATIC_DRAW, texCoords );
		}

		/// <summary>
		/// Reads surface vertices (positions and normals)
		/// </summary>
		private static void ReadVertices( BinaryReader reader, long offset, int numVertices, int numFrames, Mesh.Surface surface, Matrix44 transform )
		{
			reader.BaseStream.Seek( offset, SeekOrigin.Begin );

			//	Allocate surface frames
			surface.SurfaceFrames = new Mesh.SurfaceFrame[ numFrames ];

			//	Allocate temporary arrays for storing position and normal data
			float[] positions	= new float[ numVertices * 3 ];
			float[] normals		= new float[ numVertices * 3 ];

			//	Run through all frames
			for ( int frameIndex = 0; frameIndex < numFrames; ++frameIndex )
			{
				int positionIndex	= 0;
				int normalIndex		= 0;

				//	Allocate a surface frame
				Mesh.SurfaceFrame frame = new Mesh.SurfaceFrame( );

				//	Run through all vertices
				for ( int vertexCount = 0; vertexCount < numVertices; ++vertexCount )
				{
					//	NOTE: Re-order coordinates. I use +ve Y as up, +ve X as right, +ve Z as into, MD3 default is +ve X as into, +ve Y as right, +ve Z as up
					Point3 pt = new Point3( );
					pt.Z = reader.ReadInt16( ) * XyzScale;
					pt.X = reader.ReadInt16( ) * XyzScale;
					pt.Y = reader.ReadInt16( ) * XyzScale;

					pt = transform * pt;

					positions[ positionIndex + 0 ] = pt.X;
					positions[ positionIndex + 1 ] = pt.Y;
					positions[ positionIndex + 2 ] = pt.Z;
					positionIndex += 3;

					float s = reader.ReadByte( ) * ByteToAngle;
					float t = reader.ReadByte( ) * ByteToAngle;

					normals[ normalIndex + 2 ] = ( Functions.Cos( s ) * Functions.Sin( t ) );
					normals[ normalIndex + 0 ] = ( Functions.Sin( s ) * Functions.Sin( t ) );
					normals[ normalIndex + 1 ] = ( Functions.Cos( t ) );
					normalIndex += 3;
				}

				//	Convert position and normal data into vertex buffer objects
				frame.VertexBuffers = new OpenGlVertexArray[ 2 ];

				frame.VertexBuffers[ 0 ] = new OpenGlVertexArray( numVertices, 0, Gl.GL_VERTEX_ARRAY, 0, 3, Gl.GL_STATIC_DRAW_ARB, positions );
				frame.VertexBuffers[ 1 ] = new OpenGlVertexArray( numVertices, 0, Gl.GL_NORMAL_ARRAY, 0, 3, Gl.GL_STATIC_DRAW_ARB, normals );

				//	Assign the frame to the surface
				surface.SurfaceFrames[ frameIndex ] = frame;
			}
		}

		#endregion

	}
}
