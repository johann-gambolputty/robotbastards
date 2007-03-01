using System;
using System.IO;
using RbEngine;
using RbEngine.Maths;

namespace RbOpenGlMd3Loader
{
	/// <summary>
	/// Loads Quake3 MD3 files, generating OpenGL meshes
	/// </summary>
	public class Loader : RbEngine.Resources.ResourceLoader
	{
		/// <summary>
		/// Loads a resource from a stream
		/// </summary>
		/// <param name="input"> Input stream to load the resource from </param>
		/// <param name="inputSource"> Source of the input stream (e.g. file path) </param>
		/// <returns> The loaded resource </returns>
		public override Object Load( Stream input, string inputSource )
		{
			BinaryReader reader = new BinaryReader( input );

			//	http://icculus.org/homepages/phaethon/q3a/formats/md3format.html

			//	Make sure of the MD3 identity
			int ident	= reader.ReadInt32( );
			if ( ident != 'IDP3' )
			{
				throw new ApplicationException( "Failed to load MD3 resource - stream did not start with 'IDP3' MD3 identifier" );
			}

			//	Read in header
			int start			= reader.ReadInt32( );
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

			ReadFrames( reader, framesOffset );
			ReadTags( reader, tagsOffset );
			ReadSurfaces( reader, surfacesOffset );

			return null;
		}

		/// <summary>
		/// Returns true if this loader can load the specified stream
		/// </summary>
		/// <param name="path"> Stream path (contains extension that the loader can check)</param>
		/// <param name="input"> Input stream (file types can be identified by peeking at header bytes) </param>
		/// <returns> Returns true if the stream can </returns>
		/// <remarks>
		/// path can be null, in which case, the loader must be able to identify the resource type by checking the content in input (e.g. by peeking
		/// at the header bytes).
		/// </remarks>
		public override bool CanLoadStream( string path, System.IO.Stream input )
		{
			return path.EndsWith( ".md3" );
		}

		#region	MD3 constants

		private const int	MaxPathLength	= 64;
		private const int	FrameNameLength	= 16;
		private const float	XyzScale		= 1.0f / 64.0f;
		private const float ByteToAngle		= Maths.Constants.TwoPi / 255.0f;

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
			return new string( reader.ReadBytes( maxLength ) );
		}

		#endregion

		#region	MD3 section read methods

		/// <summary>
		/// Reads frame information
		/// </summary>
		private void	ReadFrames( BinaryReader reader, long offset, int numFrames )
		{
			reader.BaseStream.Seek( offset, SeekOrigin.Begin );

			for ( int frameCount = 0; frameCount < numFrames; ++frameCount )
			{
				Point3 	minBounds 	= ReadPoint( reader );
				Point3 	maxBounds 	= ReadPoint( reader );
				Point3 	localOrigin	= ReadPoint( reader );
				float	radius		= reader.ReadSingle( );
				string	name		= ReadString( reader, FrameNameLength );
			}
		}

		/// <summary>
		/// Reads tag information
		/// </summary>
		private void	ReadTags( BinaryReader reader, long offset, int numTags )
		{
			reader.BaseStream.Seek( offset, SeekOrigin.Begin );

			for ( int tagCount = 0; tagCount < numTags; ++tagCount )
			{
				string	name		= ReadString( reader, MaxPathLength );
				Point3	origin		= ReadPoint( reader );
				Vector3	xAxis		= ReadVector( reader );
				Vector3	yAxis		= ReadVector( reader );
				Vector3	zAxis		= ReadVector( reader );
			}
		}
		
		/// <summary>
		/// Reads surface information
		/// </summary>
		private void	ReadSurfaces( BinaryReader reader, long offset, int numSurfaces )
		{
			reader.BaseStream.Seek( offset, SeekOrigin.Begin );
			
			for ( int surfaceCount = 0; surfaceCount < numSurfaces; ++surfaceCount )
			{
				int		start			= reader.ReadInt32( );
				int		ident			= reader.ReadInt32( );
				string	name			= ReadString( reader, MaxPathLength );
				int		flags			= reader.ReadInt32( );
				int		numFrames		= reader.ReadInt32( );
				int		numShaders		= reader.ReadInt32( );
				int		numVerts		= reader.ReadInt32( );
				int		numTriangles	= reader.ReadInt32( );
				int		trianglesOffset	= reader.ReadInt32( );
				int		shadersOffset	= reader.ReadInt32( );
				int		texturesOffset	= reader.ReadInt32( );
				int		verticesOffset	= reader.ReadInt32( );
				int		endOffset		= reader.ReadInt32( );

				ReadShaders( reader, offset + shadersOffset, numShaders );
				ReadTriangles( reader, offset + trianglesOffset, numTriangles );
				ReadVertices( reader, offset + verticesOffset, numVertices );

				reader.BaseStream.Seek( endOffset );
			}
		}

		/// <summary>
		/// Reads surface shaders
		/// </summary>
		private void	ReadShaders( BinaryReader reader, long offset, int numShaders )
		{
			reader.BaseStream.Seek( offset );

			for ( int shaderCount = 0; shaderCount < numShaders; ++shaderCount )
			{
				string	name		= ReadString( reader, MaxPathLength );
				int		shaderIndex	= reader.ReadInt32( );
			}
		}

		/// <summary>
		/// Reads surface triangles
		/// </summary>
		private void	ReadTriangles( BinaryReader reader, long offset, int numTriangles )
		{
			reader.BaseStream.Seek( offset );

			for ( int triangleCount = 0; triangleCount < numTriangles; ++triangleCount )
			{
				int index0 = reader.ReadInt32( );
				int index1 = reader.ReadInt32( );
				int index2 = reader.ReadInt32( );
			}
		}

		/// <summary>
		/// Reads surface texture coordinates
		/// </summary>
		/// <param name="reader"></param>
		/// <param name="offset"></param>
		/// <param name="numCoordinates"></param>
		private void	ReadTextureCoordinates( BinaryReader reader, long offset, int numCoordinates )
		{
			reader.BaseStream.Seek( offset );
			
			for ( int coordinateCount = 0; coordinateCount < numCoordinates; ++coordinateCount )
			{
				float u = reader.ReadSingle( );
				float v = reader.ReadSingle( );
			}
		}


		/// <summary>
		/// Reads surface vertices (positions and normals)
		/// </summary>
		private void	ReadVertices( BinaryReader reader, long offset, int numVertices )
		{
			reader.BaseStream.Seek( offset );
			
			for ( int vertexCount = 0; vertexCount < numVertices; ++vertexCount )
			{
				float x 	= ( ( float )reader.ReadInt16( ) ) * XyzScale;
				float y 	= ( ( float )reader.ReadInt16( ) ) * XyzScale;
				float z 	= ( ( float )reader.ReadInt16( ) ) * XyzScale;

				float s		= ( float )( reader.ReadByte( ) ) * ByteToAngle;
				float t		= ( float )( reader.ReadByte( ) ) * ByteToAngle;

				float nX	= ( float )( System.Math.Cos( s ) * System.Math.Sin( t ) );
				float nY	= ( float )( System.Math.Sin( s ) * System.Math.Sin( t ) );
				float nZ	= ( float )( System.Math.Cos( t ) );
   			}
		}

		#endregion

	}
}
