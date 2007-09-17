using System;
using System.IO;
using System.Collections;
using Rb.Core;
using Rb.Core.Assets;
using Rb.Core.Components;
using Rb.Core.Maths;
using Rb.Rendering;
using Rb.Rendering.OpenGl;
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
		public override string Extension
		{
			get { return "md3"; }
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

			//	create the model
			Model model = new Model( );

			//	Load animations
			model.Animations = LoadAnimations( AnimationFile( source ) );

			//	Run through all the parts
			for ( int partIndex = 0; partIndex < ( int )ModelPart.NumParts; ++partIndex )
			{
				ModelPart curPart = ( ModelPart )partIndex;

				//	Load the skin file for the current part
				Hashtable surfaceTextureTable = LoadSkin( source, DefaultSkinFile( source, curPart ) );

				//	Load the MD3 associated with the current part
				Mesh partMesh = LoadMd3( curPart, scale, MeshFile( source, curPart ), surfaceTextureTable );
				model.SetPartMesh( curPart, partMesh );

				//	Nest the current mesh in the previous mesh
				if ( partIndex > 0 )
				{
					model.GetPartMesh( ( ModelPart )( partIndex - 1 ) ).NestedMesh = partMesh;
				}
			}

			//	Set the transform tags
			//	TODO: Should be data driver
			model.GetPartMesh( ModelPart.Lower ).TransformTagIndex = model.GetPartMesh( ModelPart.Lower ).GetTagIndex( "tag_torso" );
			model.GetPartMesh( ModelPart.Upper ).TransformTagIndex = model.GetPartMesh( ModelPart.Upper ).GetTagIndex( "tag_head" );

			return model;
		}

		/// <summary>
		/// Returns true if the specified source can be loaded
		/// </summary>
		public override bool CanLoad( ISource source )
		{
			return MeshFile( source, ModelPart.Head ).Exists;
		}

		#endregion
		
		#region	Filename builders

		/// <summary>
		/// Builds the name of the animation file
		/// </summary>
		private static ISource AnimationFile( ISource source )
		{
			return source.GetSource( "animation.cfg" );
		}


		/// <summary>
		/// Returns the mesh filename for a part
		/// </summary>
		private static ISource MeshFile( ISource source, ModelPart part )
		{
			return source.GetSource( part + ".md3" );
		}

		/// <summary>
		/// Returns the default skin filename for a part
		/// </summary>
		private static ISource DefaultSkinFile( ISource source, ModelPart part )
		{
			return source.GetSource( part + "_default.skin" );
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
		private ISource TextureFile( ISource source, string path )
		{
			//	Remove the extension
			string name = Path.GetFileNameWithoutExtension( path );
			foreach ( string ext in TextureExtensions )
			{
				ISource textureSource = source.GetSource( name + ext );
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
		private Hashtable LoadSkin( ISource source, ISource skinSource )
		{
			using ( Stream inputStream = skinSource.Open( ) )
			{
				TextReader	reader				= new StreamReader( inputStream );
				Hashtable	surfaceTextureMap	= new Hashtable( );

				char[]		tokenSplit			= new char[ ] { ',' };

				for ( string curLine = reader.ReadLine( ); curLine != null; curLine = reader.ReadLine( ) )
				{
					string[] tokens = curLine.Split( tokenSplit );

					if ( !tokens[ 0 ].StartsWith( "tag_" ) )
					{
						//	TODO: AP: Texture loading should be done through the asset manager
						Texture2d newTexture = RenderFactory.Instance.NewTexture2d( );

						using ( Stream textureStream = TextureFile( source, tokens[ 1 ] ).Open( ) )
						{
							newTexture.Load( textureStream );
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
		private Mesh LoadMd3( ModelPart part, Vector3 scale, ISource md3Source, Hashtable surfaceTextureTable )
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
				int version			= reader.ReadInt32( );
				string name			= ReadString( reader, MaxPathLength );
				int flags			= reader.ReadInt32( );
				int numFrames		= reader.ReadInt32( );
				int numTags			= reader.ReadInt32( );
				int numSurfaces		= reader.ReadInt32( );
				int numSkins		= reader.ReadInt32( );
				int framesOffset	= reader.ReadInt32( );
				int tagsOffset		= reader.ReadInt32( );
				int surfacesOffset	= reader.ReadInt32( );
				int eofOffset		= reader.ReadInt32( );

				//	TODO: Can load directly into mesh frame, tag and surface structures - don't do this intermediate step
				Mesh mesh = new Mesh( part );

				ReadFrames( reader, framesOffset, numFrames, mesh, scale );
				ReadTags( reader, tagsOffset, numTags, numFrames, mesh, scale );
				ReadSurfaces( reader, surfacesOffset, numSurfaces, numFrames, mesh, surfaceTextureTable, scale );


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
					mesh.DefaultFrame = 100;
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
		private static void ReadFrames( BinaryReader reader, long offset, int numFrames, Mesh mesh , Vector3 scale )
		{
			reader.BaseStream.Seek( offset, SeekOrigin.Begin );

			Mesh.FrameInfo[] frames = new Mesh.FrameInfo[ numFrames ];

			float scaleLength = scale.Length;

			for ( int frameCount = 0; frameCount < numFrames; ++frameCount )
			{
				frames[ frameCount]		= new Mesh.FrameInfo( );
				Mesh.FrameInfo curFrame	= frames[ frameCount ];
				curFrame.MinBounds		= ReadPoint( reader ) * scale;
				curFrame.MaxBounds		= ReadPoint( reader ) * scale;
				curFrame.Origin			= ReadPoint( reader ) * scale;
				curFrame.Radius			= reader.ReadSingle( ) * scaleLength;
				curFrame.Name			= ReadString( reader, FrameNameLength );
			}

			mesh.FrameInfoList = frames;
		}

		/// <summary>
		/// Reads tag information
		/// </summary>
		private static void ReadTags( BinaryReader reader, long offset, int numTags, int numFrames, Mesh mesh, Vector3 scale )
		{
			reader.BaseStream.Seek( offset, SeekOrigin.Begin );

			mesh.TagNames = new string[ numTags ];

			for ( int frameIndex = 0; frameIndex < numFrames; ++frameIndex )
			{
				Mesh.Tag[] tags = new Mesh.Tag[ numTags ];
				for ( int tagCount = 0; tagCount < numTags; ++tagCount )
				{
					string tagName		= ReadString( reader, MaxPathLength );

					if ( frameIndex == 0 )
					{
						mesh.TagNames[ tagCount ] = tagName;
					}

					Mesh.Tag curTag		= new Mesh.Tag( );
					curTag.Origin		= ReadPoint( reader ) * scale;
					curTag.XAxis		= ReadVector( reader );
					curTag.YAxis		= ReadVector( reader );
					curTag.ZAxis		= ReadVector( reader );

					tags[ tagCount ] = curTag;
				}
				mesh.FrameInfoList[ frameIndex ].Tags = tags;
			}
		}
		
		/// <summary>
		/// Reads surface information
		/// </summary>
		private static void ReadSurfaces( BinaryReader reader, long offset, int numSurfaces, int numFrames, Mesh mesh, IDictionary surfaceTextureTable, Vector3 scale )
		{
			//	Move to the start of the first surface
			reader.BaseStream.Seek( offset, SeekOrigin.Begin );

			//	Allocate mesh surface array
			mesh.Surfaces = new Mesh.Surface[ numSurfaces ];

			for ( int surfaceCount = 0; surfaceCount < numSurfaces; ++surfaceCount )
			{
				//	Create a new surface
				Mesh.Surface curSurface		= new Mesh.Surface( );

				int		ident				= reader.ReadInt32( );
				string	name				= ReadString ( reader, MaxPathLength );
				int		flags				= reader.ReadInt32( );
				int		surfaceNumFrames	= reader.ReadInt32( );
				int		numShaders			= reader.ReadInt32( );
				int		numVertices			= reader.ReadInt32( );
				int		numTriangles		= reader.ReadInt32( );
				int		trianglesOffset		= reader.ReadInt32( );
				int		shadersOffset		= reader.ReadInt32( );
				int		texturesOffset		= reader.ReadInt32( );
				int		verticesOffset		= reader.ReadInt32( );
				int		endOffset			= reader.ReadInt32( );

				//	Assign surface texture
				curSurface.Texture			= ( Texture2d )surfaceTextureTable[ name ];
			
				//	Assign surface shaders
			//	ReadShaders( reader, offset + shadersOffset, numShaders );

				//	Read in surface index group and texture UVs
				curSurface.Group			= ReadTriangles( reader, offset + trianglesOffset, numTriangles );
				curSurface.TextureUVs		= ReadTextureUVs( reader, offset + texturesOffset, numVertices );

				//	Read in surface vertices
			//	curSurface.NumVertices		= numVertices;
				ReadVertices( reader, offset + verticesOffset, numVertices, numFrames, curSurface, scale );

				//	Assign the new surface to the mesh
				mesh.Surfaces[ surfaceCount ] = curSurface;

				//	Move the stream to the next surface
				reader.BaseStream.Seek( offset + endOffset, SeekOrigin.Begin );
				offset += endOffset;
			}
		}

		/// <summary>
		/// Reads surface shaders
		/// </summary>
		private static void ReadShaders(BinaryReader reader, long offset, int numShaders)
		{
			reader.BaseStream.Seek( offset, SeekOrigin.Begin );

			for ( int shaderCount = 0; shaderCount < numShaders; ++shaderCount )
			{
				string shaderName  = ReadString( reader, MaxPathLength );
				int index = reader.ReadInt32( );
			}
		}

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
				triangles[ curTriIndex + 1 ] = reader.ReadInt32( );
				triangles[ curTriIndex + 2 ] = reader.ReadInt32( );
				curTriIndex += 3;
			}

			return new OpenGlIndexedGroup( Gl.GL_TRIANGLES, triangles );
		}

		/// <summary>
		/// Reads surface texture coordinates
		/// </summary>
		private static OpenGlVertexBuffer ReadTextureUVs(BinaryReader reader, long offset, int numCoordinates)
		{
			reader.BaseStream.Seek( offset, SeekOrigin.Begin );
			
			float[] texCoords = new float[ numCoordinates * 2 ];
			int coordinateIndex = 0;
			for ( int coordinateCount = 0; coordinateCount < numCoordinates; ++coordinateCount )
			{
				texCoords[ coordinateIndex++ ] = reader.ReadSingle( );
				texCoords[ coordinateIndex++ ] = reader.ReadSingle( );
			}

			return new OpenGlVertexBuffer( numCoordinates, 0, Gl.GL_TEXTURE_COORD_ARRAY, 0, 2, Gl.GL_STATIC_DRAW, texCoords );
		}


		/// <summary>
		/// Reads surface vertices (positions and normals)
		/// </summary>
		private static void ReadVertices(BinaryReader reader, long offset, int numVertices, int numFrames, Mesh.Surface surface, Vector3 scale)
		{
			reader.BaseStream.Seek( offset, SeekOrigin.Begin );

			//	Allocate surface frames
			surface.SurfaceFrames = new Mesh.SurfaceFrame[ numFrames ];

			//	Allocate temporary arrays for storing position and normal data
			float[] positions	= new float[ numVertices * 3 ];
			float[] normals		= new float[ numVertices * 3 ];

			float xScale = XyzScale * scale.X;
			float yScale = XyzScale * scale.Y;
			float zScale = XyzScale * scale.Z;

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
					positions[ positionIndex + 2 ] = reader.ReadInt16( ) * zScale;
					positions[ positionIndex + 0 ] = reader.ReadInt16( ) * xScale;
					positions[ positionIndex + 1 ] = reader.ReadInt16( ) * yScale;
					positionIndex += 3;

					float s		= reader.ReadByte( ) * ByteToAngle;
					float t		= reader.ReadByte( ) * ByteToAngle;

					normals[ normalIndex + 2 ] = ( float )( Math.Cos( s ) * Math.Sin( t ) );
					normals[ normalIndex + 0 ] = ( float )( Math.Sin( s ) * Math.Sin( t ) );
					normals[ normalIndex + 1 ] = ( float )( Math.Cos( t ) );
					normalIndex += 3;
				}

				//	Convert position and normal data into vertex buffer objects
				frame.VertexBuffers = new OpenGlVertexBuffer[ 2 ];

				frame.VertexBuffers[ 0 ] = new OpenGlVertexBuffer( numVertices, 0, Gl.GL_VERTEX_ARRAY, 0, 3, Gl.GL_STATIC_DRAW_ARB, positions );
				frame.VertexBuffers[ 1 ] = new OpenGlVertexBuffer( numVertices, 0, Gl.GL_NORMAL_ARRAY, 0, 3, Gl.GL_STATIC_DRAW_ARB, normals );

				//	Assign the frame to the surface
				surface.SurfaceFrames[ frameIndex ] = frame;
			}
		}

		#endregion

	}
}
