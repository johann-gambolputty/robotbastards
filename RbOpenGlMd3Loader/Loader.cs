using System;
using System.IO;
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
		/// Loads the MD3 resources in the specified directory
		/// </summary>
		/// <param name="provider"></param>
		/// <param name="directory"></param>
		/// <returns></returns>
		public override object Load( RbEngine.Resources.ResourceProvider provider, string directory )
		{
			Model model = new Model( );

			for ( int partIndex = 0; partIndex < ( int )ModelPart.NumParts; ++partIndex )
			{
				Mesh partMesh = LoadMd3( provider, MeshFile( directory, ( ModelPart )partIndex ) );
				model.SetPartMesh( ( ModelPart )partIndex, partMesh );
				
				if ( partIndex > 0 )
				{
					model.GetPartMesh( ( ModelPart )( partIndex - 1 ) ).NestedMesh = partMesh;
				}
			}

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

			return CheckFileExists( provider, directory, Path.Combine( directory, "animation.cfg" ) );
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

		/// <summary>
		/// Loads a skin file
		/// </summary>
		private void LoadSkin( RbEngine.Resources.ResourceProvider provider, string inputSource )
		{
		}


		/// <summary>
		/// Loads an MD3 mesh resource from a stream
		/// </summary>
		private Mesh LoadMd3( RbEngine.Resources.ResourceProvider provider, string inputSource )
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
			Mesh mesh = new Mesh( );

			Frame[]		frames		= ReadFrames( reader, framesOffset, numFrames );
			mesh.Tags = ReadTags( reader, tagsOffset, numTags * numFrames );
			mesh.TagsPerFrame = numTags;
			Surface[]	surfaces	= ReadSurfaces( reader, surfacesOffset, numSurfaces );

			mesh.CreateSurfaces( surfaces.Length );
			mesh.CreateFrameInfo( frames.Length );

			for ( int frameIndex = 0; frameIndex < numFrames; ++frameIndex )
			{
				Mesh.FrameInfo frameInfo = new Mesh.FrameInfo( );
				frameInfo.Origin = frames[ frameIndex ].Origin;
				mesh.SetFrameInfo( frameIndex, frameInfo );
			}

			for ( int surfaceIndex = 0; surfaceIndex < surfaces.Length; ++surfaceIndex )
			{
				Surface curSurface = surfaces[ surfaceIndex ];

				Mesh.Surface meshSurface = new Mesh.Surface( );

				meshSurface.Group = new OpenGlIndexedGroup( Gl.GL_TRIANGLES, curSurface.Triangles );
				meshSurface.CreateFrames( numFrames );

				int offsetToFrame = 0;
				for ( int frameIndex = 0; frameIndex < numFrames; ++frameIndex )
				{
					Mesh.SurfaceFrame meshFrame = new Mesh.SurfaceFrame( );
					meshFrame.CreateVertexBuffers( 3 );
					meshFrame.SetVertexBuffer( 0, new OpenGlVertexBuffer( curSurface.NumVertices, offsetToFrame, Gl.GL_VERTEX_ARRAY, 0, 3, Gl.GL_STATIC_DRAW_ARB, curSurface.Positions ) );
					meshFrame.SetVertexBuffer( 1, new OpenGlVertexBuffer( curSurface.NumVertices, offsetToFrame, Gl.GL_NORMAL_ARRAY, 0, 3, Gl.GL_STATIC_DRAW_ARB, curSurface.Normals ) );
					meshFrame.SetVertexBuffer( 2, new OpenGlVertexBuffer( curSurface.NumVertices, offsetToFrame, Gl.GL_TEXTURE_COORD_ARRAY, 0, 2, Gl.GL_STATIC_DRAW_ARB, curSurface.TextureUVs ) );

					meshSurface.SetFrame( frameIndex, meshFrame );

					offsetToFrame += curSurface.NumVertices;
				}

				mesh.SetSurface( surfaceIndex, meshSurface );
			}

			//	TODO: REMOVE. test frames
			if ( inputSource.IndexOf( "Upper" ) != -1 )
			{
				mesh.CurrentFrame = 130;
			}
			else if ( inputSource.IndexOf( "Head" ) != -1 )
			{
				mesh.CurrentFrame = 0;
			}
			else
			{
				mesh.CurrentFrame = 161;
			}

			inputStream.Close( );

			return mesh;
		}

		private class Frame
		{
			public string	Name;
			public Point3	MinBounds;
			public Point3	MaxBounds;
			public Point3	Origin;
			public float	Radius;
		}

		private class Shader
		{
			public string	Name;
			public int		Index;
		}

		private class Surface
		{
			public string	Name;
			public int		Flags;
			public int[]	Triangles;
			public Shader[]	Shaders;
			public float[]	Positions;
			public float[]	Normals;
			public float[]	TextureUVs;
			public int		NumVertices;
		}

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
			return new Point3( reader.ReadSingle( ), reader.ReadSingle( ), reader.ReadSingle( ) );
		}

		/// <summary>
		/// Reads and returns a Vector3 from a BinaryReader
		/// </summary>
		private Vector3	ReadVector( BinaryReader reader )
		{
			return new Vector3( reader.ReadSingle( ), reader.ReadSingle( ), reader.ReadSingle( ) );
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
		private Frame[] ReadFrames( BinaryReader reader, long offset, int numFrames )
		{
			reader.BaseStream.Seek( offset, SeekOrigin.Begin );

			Frame[] frames = new Frame[ numFrames ];

			for ( int frameCount = 0; frameCount < numFrames; ++frameCount )
			{
				frames[ frameCount]		= new Frame( );
				Frame curFrame			= frames[ frameCount ];
				curFrame.MinBounds		= ReadPoint( reader );
				curFrame.MaxBounds		= ReadPoint( reader );
				curFrame.Origin			= ReadPoint( reader );
				curFrame.Radius			= reader.ReadSingle( );
				curFrame.Name			= ReadString( reader, FrameNameLength );
			}

			return frames;
		}

		/// <summary>
		/// Reads tag information
		/// </summary>
		private Mesh.Tag[]	ReadTags( BinaryReader reader, long offset, int numTags )
		{
			reader.BaseStream.Seek( offset, SeekOrigin.Begin );

			Mesh.Tag[] tags = new Mesh.Tag[ numTags ];
			for ( int tagCount = 0; tagCount < numTags; ++tagCount )
			{
				tags[ tagCount ]	= new Mesh.Tag( );
				Mesh.Tag curTag			= tags[ tagCount ];
				curTag.Name			= ReadString( reader, MaxPathLength );
				curTag.Origin		= ReadPoint( reader );
				curTag.XAxis		= ReadVector( reader );
				curTag.YAxis		= ReadVector( reader );
				curTag.ZAxis		= ReadVector( reader );
			}

			return tags;
		}
		
		/// <summary>
		/// Reads surface information
		/// </summary>
		private Surface[] ReadSurfaces( BinaryReader reader, long offset, int numSurfaces )
		{
			reader.BaseStream.Seek( offset, SeekOrigin.Begin );
			
			Surface[] surfaces = new Surface[ numSurfaces ];
			for ( int surfaceCount = 0; surfaceCount < numSurfaces; ++surfaceCount )
			{
				surfaces[ surfaceCount ]	= new Surface( );
				Surface curSurface			= surfaces[ surfaceCount ];
				int		ident				= reader.ReadInt32( );

				curSurface.Name				= ReadString( reader, MaxPathLength );
				curSurface.Flags			= reader.ReadInt32( );

				int		numFrames			= reader.ReadInt32( );
				int		numShaders			= reader.ReadInt32( );
				int		numVertices			= reader.ReadInt32( );
				int		numTriangles		= reader.ReadInt32( );
				int		trianglesOffset		= reader.ReadInt32( );
				int		shadersOffset		= reader.ReadInt32( );
				int		texturesOffset		= reader.ReadInt32( );
				int		verticesOffset		= reader.ReadInt32( );
				int		endOffset			= reader.ReadInt32( );

				int		totalVertices		= numVertices * numFrames;

				curSurface.NumVertices		= numVertices;
				curSurface.Shaders			= ReadShaders( reader, offset + shadersOffset, numShaders );
				curSurface.Triangles		= ReadTriangles( reader, offset + trianglesOffset, numTriangles );
				curSurface.TextureUVs		= ReadTextureUVs( reader, offset + texturesOffset, totalVertices );
				ReadVertices( reader, offset + verticesOffset, totalVertices, curSurface );

				reader.BaseStream.Seek( offset + endOffset, SeekOrigin.Begin );
				offset += endOffset;
			}

			return surfaces;
		}

		/// <summary>
		/// Reads surface shaders
		/// </summary>
		private Shader[]	ReadShaders( BinaryReader reader, long offset, int numShaders )
		{
			reader.BaseStream.Seek( offset, SeekOrigin.Begin );

			Shader[] shaders = new Shader[ numShaders ];
			for ( int shaderCount = 0; shaderCount < numShaders; ++shaderCount )
			{
				shaders[ shaderCount ]	= new Shader( );
				Shader	curShader		= shaders[ shaderCount ];
				curShader.Name			= ReadString( reader, MaxPathLength );
				curShader.Index			= reader.ReadInt32( );
			}

			return shaders;
		}

		/// <summary>
		/// Reads surface triangles
		/// </summary>
		private int[]	ReadTriangles( BinaryReader reader, long offset, int numTriangles )
		{
			reader.BaseStream.Seek( offset, SeekOrigin.Begin );

			int[] triangles = new int[ 3 * numTriangles ];
			int curTriIndex = 0;
			for ( int triangleCount = 0; triangleCount < numTriangles; ++triangleCount )
			{
				triangles[ curTriIndex++ ] = reader.ReadInt32( );
				triangles[ curTriIndex++ ] = reader.ReadInt32( );
				triangles[ curTriIndex++ ] = reader.ReadInt32( );
			}

			return triangles;
		}

		/// <summary>
		/// Reads surface texture coordinates
		/// </summary>
		/// <param name="reader"></param>
		/// <param name="offset"></param>
		/// <param name="numCoordinates"></param>
		private float[]	ReadTextureUVs( BinaryReader reader, long offset, int numCoordinates )
		{
			reader.BaseStream.Seek( offset, SeekOrigin.Begin );
			
			float[] texCoords = new float[ numCoordinates * 2 ];
			int coordinateIndex = 0;
			for ( int coordinateCount = 0; coordinateCount < numCoordinates; ++coordinateCount )
			{
				texCoords[ coordinateIndex++ ] = reader.ReadSingle( );
				texCoords[ coordinateIndex++ ] = reader.ReadSingle( );
			}

			return texCoords;
		}


		/// <summary>
		/// Reads surface vertices (positions and normals)
		/// </summary>
		private void	ReadVertices( BinaryReader reader, long offset, int numVertices, Surface surface )
		{
			reader.BaseStream.Seek( offset, SeekOrigin.Begin );

			float[] positions	= new float[ numVertices * 3 ];
			float[] normals		= new float[ numVertices * 3 ];

			int positionIndex	= 0;
			int normalIndex		= 0;
			
			for ( int vertexCount = 0; vertexCount < numVertices; ++vertexCount )
			{
				positions[ positionIndex++ ] = ( ( float )reader.ReadInt16( ) ) * XyzScale;
				positions[ positionIndex++ ] = ( ( float )reader.ReadInt16( ) ) * XyzScale;
				positions[ positionIndex++ ] = ( ( float )reader.ReadInt16( ) ) * XyzScale;

				float s		= ( float )( reader.ReadByte( ) ) * ByteToAngle;
				float t		= ( float )( reader.ReadByte( ) ) * ByteToAngle;

				normals[ normalIndex++ ] = ( float )( System.Math.Cos( s ) * System.Math.Sin( t ) );
				normals[ normalIndex++ ] = ( float )( System.Math.Sin( s ) * System.Math.Sin( t ) );
				normals[ normalIndex++ ] = ( float )( System.Math.Cos( t ) );
   			}

			surface.Positions	= positions;
			surface.Normals		= normals;
		}

		#endregion

	}
}
