using System;
using System.IO;
using System.Collections;
using RbEngine;
using RbEngine.Maths;
using RbEngine.Rendering;
using RbOpenGlRendering;
using Tao.OpenGl;

namespace RbOpenGlMd3Loader
{
	/// <summary>
	/// Loads Quake3 MD3 files, generating OpenGL meshes
	/// </summary>
	public class Loader : RbEngine.Resources.ResourceDirectoryLoader
	{
		#region	ResourceDirectoryLoader Members

		/// <summary>
		/// Loads the MD3 resources in the specified directory
		/// </summary>
		/// <param name="provider"></param>
		/// <param name="directory"></param>
		/// <returns></returns>
		public override object Load( RbEngine.Resources.ResourceProvider provider, string directory )
		{
			//	create the model
			Model model = new Model( );

			//	Load animations
			model.Animations = LoadAnimations( provider, AnimationFile( directory ) );


			//	Run through all the parts
			for ( int partIndex = 0; partIndex < ( int )ModelPart.NumParts; ++partIndex )
			{
				ModelPart curPart = ( ModelPart )partIndex;

				//	Load the skin file for the current part
				Hashtable surfaceTextureTable = LoadSkin( provider, directory, DefaultSkinFile( directory, curPart ) );

				//	Load the MD3 associated with the current part
				Mesh partMesh = LoadMd3( provider, curPart, MeshFile( directory, curPart ), surfaceTextureTable );
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
		/// Checks that the specified directory contains all the md3, skin and cfg files required to build the model
		/// </summary>
		public override bool CanLoadDirectory( RbEngine.Resources.ResourceProvider provider, string directory )
		{
			//	Does the head mesh exist in the directory?
			if ( !provider.StreamExists( MeshFile( directory, ModelPart.Head ) ) )
			{
				return false;
			}

			//	Yep - check all the other parts exist
			for ( int partIndex = 0; partIndex < ( int )ModelPart.NumParts; ++partIndex )
			{
				if ( !CheckFileExists( provider, directory, MeshFile( directory, ( ModelPart )partIndex ) ) ||
					 !CheckFileExists( provider, directory, DefaultSkinFile( directory, ( ModelPart )partIndex ) ) )
				{
					return false;
				}
			}

			return CheckFileExists( provider, directory, AnimationFile( directory ) );
		}

		#endregion
		
		#region	Filename builders

		/// <summary>
		/// Builds the name of the animation file
		/// </summary>
		private string AnimationFile( string directory )
		{
			return Path.Combine( directory, "animation.cfg" );
		}


		/// <summary>
		/// Returns the mesh filename for a part
		/// </summary>
		private static string MeshFile( string directory, ModelPart part )
		{
			return Path.Combine( directory, part.ToString( ) + ".md3" );
		}

		/// <summary>
		/// Returns the default skin filename for a part
		/// </summary>
		private static string DefaultSkinFile( string directory, ModelPart part )
		{
			return Path.Combine( directory, part.ToString( ) + "_default.skin" );
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
		private string TextureFile( string directory, string path )
		{
			path = path.Replace( ".tga", ".bmp" );

			return Path.Combine( directory, Path.GetFileName( path ) );
		}

		/// <summary>
		/// Checks that a file exists
		/// </summary>
		private bool CheckFileExists( RbEngine.Resources.ResourceProvider provider, string directory, string path )
		{
			if ( provider.StreamExists( path ) )
			{
				return true;
			}

			RbEngine.Output.WriteLineCall( RbEngine.Output.ResourceWarning, "\"{0}\" looked like an MD3 resource directory, but it was missing \"{1}\"", directory, path );

			return false;
		}

		#endregion

		#region	Skin loading

		/// <summary>
		/// Loads a skin file
		/// </summary>
		private Hashtable LoadSkin( RbEngine.Resources.ResourceProvider provider, string directory, string inputSource )
		{
			Stream		inputStream			= provider.Open( inputSource );
			TextReader	reader				= new StreamReader( inputStream );

			Hashtable	surfaceTextureMap	= new Hashtable( );

			char[]		tokenSplit			= new char[ ] { ',' };

			for ( string curLine = reader.ReadLine( ); curLine != null; curLine = reader.ReadLine( ) )
			{
				string[] tokens = curLine.Split( tokenSplit );

				if ( !tokens[ 0 ].StartsWith( "tag_" ) )
				{
					//	TODO: Texture loading should be done through the resource manager
					Texture2d newTexture = RenderFactory.Inst.NewTexture2d( );
					newTexture.Load( TextureFile( directory, tokens[ 1 ] ) );

					surfaceTextureMap[ tokens[ 0 ] ] = newTexture;
				}
			}

			inputStream.Close( );

			return surfaceTextureMap;
		}

		#endregion

		#region	Animation loading

		/// <summary>
		/// Loads in model animations
		/// </summary>
		public AnimationSet LoadAnimations( RbEngine.Resources.ResourceProvider provider, string inputSource )
		{
			Stream			inputStream	= provider.Open( inputSource );
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
				if ( tokens[ 0 ] == "sex" )
				{
					//	Model gender - ignore it
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

			inputStream.Close( );

			return animations;
		}

		#endregion

		#region	Mesh loading

		/// <summary>
		/// Loads an MD3 mesh resource from a stream
		/// </summary>
		private Mesh LoadMd3( RbEngine.Resources.ResourceProvider provider, ModelPart part, string inputSource, Hashtable surfaceTextureTable )
		{
			Stream			inputStream	= provider.Open( inputSource );
			BinaryReader	reader		= new BinaryReader( inputStream );

			//	http://icculus.org/homepages/phaethon/q3a/formats/md3format.html

			//	Get the input source directory (not using System.IO.Path.GetDirectory() because that strips of the final slash)
			int		inputFilePos	= inputSource.LastIndexOfAny( new char[] { '/', '\\' } );
			string	inputDir		= inputFilePos == -1 ? string.Empty : inputSource.Substring( 0, inputFilePos + 1 );

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

			ReadFrames( reader, framesOffset, numFrames, mesh );
			ReadTags( reader, tagsOffset, numTags, numFrames, mesh );
			ReadSurfaces( reader, surfacesOffset, numSurfaces, numFrames, mesh, surfaceTextureTable );


			//	TODO: REMOVE. test frames
			if ( inputSource.IndexOf( "Upper" ) != -1 )
			{
				mesh.DefaultFrame = 151;
			}
			else if ( inputSource.IndexOf( "Head" ) != -1 )
			{
				mesh.DefaultFrame = 0;
			}
			else
			{
				mesh.DefaultFrame = 100;
			}

			inputStream.Close( );

			return mesh;
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
		private Point3	ReadPoint( BinaryReader reader )
		{
			float z = reader.ReadSingle( );
			float x = reader.ReadSingle( );
			float y = reader.ReadSingle( );

			return new Point3( x, y, z );
		}

		/// <summary>
		/// Reads and returns a Vector3 from a BinaryReader
		/// </summary>
		private Vector3	ReadVector( BinaryReader reader )
		{
			float x = reader.ReadSingle( );
			float y = reader.ReadSingle( );
			float z = reader.ReadSingle( );

			return new Vector3( x, y, z );
		}

		/// <summary>
		/// Reads a string
		/// </summary>
		private string	ReadString( BinaryReader reader, int maxLength )
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
		private void	ReadFrames( BinaryReader reader, long offset, int numFrames, Mesh mesh )
		{
			reader.BaseStream.Seek( offset, SeekOrigin.Begin );

			Mesh.FrameInfo[] frames = new Mesh.FrameInfo[ numFrames ];

			for ( int frameCount = 0; frameCount < numFrames; ++frameCount )
			{
				frames[ frameCount]		= new Mesh.FrameInfo( );
				Mesh.FrameInfo curFrame	= frames[ frameCount ];
				curFrame.MinBounds		= ReadPoint( reader );
				curFrame.MaxBounds		= ReadPoint( reader );
				curFrame.Origin			= ReadPoint( reader );
				curFrame.Radius			= reader.ReadSingle( );
				curFrame.Name			= ReadString( reader, FrameNameLength );
			}

			mesh.FrameInfoList = frames;
		}

		/// <summary>
		/// Reads tag information
		/// </summary>
		private void	ReadTags( BinaryReader reader, long offset, int numTags, int numFrames, Mesh mesh )
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
					curTag.Origin		= ReadPoint( reader );
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
		private void	ReadSurfaces( BinaryReader reader, long offset, int numSurfaces, int numFrames, Mesh mesh, Hashtable surfaceTextureTable )
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
				ReadVertices( reader, offset + verticesOffset, numVertices, numFrames, curSurface );

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
		private void	ReadShaders( BinaryReader reader, long offset, int numShaders )
		{
			reader.BaseStream.Seek( offset, SeekOrigin.Begin );

			for ( int shaderCount = 0; shaderCount < numShaders; ++shaderCount )
			{
				string	shaderName	 = ReadString( reader, MaxPathLength );
				int		index		= reader.ReadInt32( );
			}
		}

		/// <summary>
		/// Reads surface triangles
		/// </summary>
		private OpenGlIndexedGroup	ReadTriangles( BinaryReader reader, long offset, int numTriangles )
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
		private OpenGlVertexBuffer	ReadTextureUVs( BinaryReader reader, long offset, int numCoordinates )
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
		private void	ReadVertices( BinaryReader reader, long offset, int numVertices, int numFrames, Mesh.Surface surface )
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
					positions[ positionIndex + 2 ] = ( ( float )reader.ReadInt16( ) ) * XyzScale;
					positions[ positionIndex + 0 ] = ( ( float )reader.ReadInt16( ) ) * XyzScale;
					positions[ positionIndex + 1 ] = ( ( float )reader.ReadInt16( ) ) * XyzScale;
					positionIndex += 3;

					float s		= ( float )( reader.ReadByte( ) ) * ByteToAngle;
					float t		= ( float )( reader.ReadByte( ) ) * ByteToAngle;

					normals[ normalIndex + 2 ] = ( float )( System.Math.Cos( s ) * System.Math.Sin( t ) );
					normals[ normalIndex + 0 ] = ( float )( System.Math.Sin( s ) * System.Math.Sin( t ) );
					normals[ normalIndex + 1 ] = ( float )( System.Math.Cos( t ) );
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
