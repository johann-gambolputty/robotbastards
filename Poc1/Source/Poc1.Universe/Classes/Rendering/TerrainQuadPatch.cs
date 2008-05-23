
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Runtime.InteropServices;
using Poc1.Universe.Interfaces.Rendering;
using Rb.Core.Maths;
using Rb.Rendering;
using Rb.Rendering.Interfaces.Objects;
using Rb.Rendering.Interfaces.Objects.Cameras;
using Graphics=Rb.Rendering.Graphics;

namespace Poc1.Universe.Classes.Rendering
{
	/// <summary>
	/// Quad-tree terrain patch
	/// </summary>
	/// <remarks>
	/// 
	/// Description of LOD increase/decrease operations:
	/// 
	/// Patch detail is increased when the distance from the patch to the camera is less than the error threshold in
	/// a leaf node. The error threshold is the maximum allowable pixel error between two patches.
	///		if the CachedChildren list exists
	///			CachedChildren list is moved to the PendingChildren list
	///		else
	///			4 child nodes are created and added to the PendingChildren list
	///		Each child node is marked as "Building"
	///		Each new child node adds a build request to the TerrainQuadPatchBuilder
	///		When all 4 child nodes are built, their "Building" flags are set to false, and they inform the parent patch, which moves
	///		them from the PendingChildren list to the Children list.
	///
	///	Patch detail is decreased when the distance from the patch to the camera is greater than the error threshold
	/// in a non-leaf node.
	///		If any child below the patch is marked as "Building", then the detail reduction fails.
	///		Children list is moved to CachedChildren (recursive), making the non-leaf node into a leaf node.
	///			All children moved in this way release their allocated vertex areas
	///		The node is marked as "Building".
	///		The node adds a build request to the TerrainQuadPatchBuilder
	///		When the node has been build, its "Building" flag is set to false.
	/// 
	/// </remarks>
	class TerrainQuadPatch : ITerrainPatch
	{
		public const int VertexResolution = 33;
		public const int VertexArea = VertexResolution * VertexResolution;
		public const int TotalVerticesPerPatch = VertexArea + VertexResolution * 4;
		public const float ErrorThreshold = 6;

		#region Public Construction

		//	TODO: AP: UV patch resolution should be a function of patch size, not ply

		public TerrainQuadPatch( TerrainQuadPatchVertices vertices, Point3 origin, Vector3 uAxis, Vector3 vAxis ) :
			this( null, vertices, origin, uAxis, vAxis, float.MaxValue, 128.0f )
		{
		}

		#endregion

		#region Public Properties

		/// <summary>
		/// Returns true if this patch has been assigned an region of the terrain vertex buffer
		/// </summary>
		public bool HasVertexData
		{
			get { return m_VbIndex != -1; }
		}

		/// <summary>
		/// Gets the centre of this patch (origin at the planets centre)
		/// </summary>
		public Point3 PatchCentre
		{
			get { return m_Centre; }
		}

		/// <summary>
		/// Gets the local origin of this patch (origin at the planets centre, but scaled down)
		/// </summary>
		public Point3 LocalOrigin
		{
			get { return m_LocalOrigin; }
		}

		/// <summary>
		/// Gets the U axis of this patch
		/// </summary>
		public Vector3 LocalUAxis
		{
			get { return m_LocalUAxis; }
		}

		/// <summary>
		/// Gets the V axis of this patch
		/// </summary>
		public Vector3 LocalVAxis
		{
			get { return m_LocalVAxis; }
		}

		/// <summary>
		/// Gets the U step of this patch
		/// </summary>
		public Vector3 LocalUStep
		{
			get { return m_LocalUAxis / ( VertexResolution - 1 ); }
		}

		/// <summary>
		/// Gets the V step of this patch
		/// </summary>
		public Vector3 LocalVStep
		{
			get { return m_LocalVAxis / ( VertexResolution - 1 ); }
		}

		/// <summary>
		/// Gets the terrain texture resolution of this patch
		/// </summary>
		public float UvResolution
		{
			get { return m_UvRes; }
		}

		/// <summary>
		/// Gets the maximum error of this patch
		/// </summary>
		public float PatchError
		{
			get { return m_PatchError; }
			set { m_PatchError = value; }
		}

		#endregion

		#region Updates and rendering

		/// <summary>
		/// Counts the number of nodes, from the set of this node, and all child nodes, that have vertex data
		/// </summary>
		public int CountNodesWithVertexData( )
		{
			int count = ( HasVertexData ? 1 : 0 );
			if ( m_Children != null )
			{
				foreach ( TerrainQuadPatch patch in m_Children )
				{
					count += patch.CountNodesWithVertexData( );
				}
			}
			return count;
		}

		/// <summary>
		/// Called by <see cref="TerrainQuadPatchBuilder"/> when it has finished building vertex data for this patch
		/// </summary>
		public unsafe void OnBuildComplete( byte* vertexData, float increaseDetailDistance )
		{
			m_Building = false;
			m_IncreaseDetailDistance = increaseDetailDistance;
			SetVertexData( vertexData );
			BuildIndices( );
			if ( m_Parent != null )
			{
				m_Parent.ChildBuildIsComplete( );
			}
		}

		/// <summary>
		/// Updates the level of detail of this patch. 
		/// </summary>
		public void UpdateLod( Point3 cameraPos, IPlanetTerrain terrain, IProjectionCamera camera )
		{
			if ( m_IncreaseDetailDistance == float.MaxValue )
			{
				//	Detail up distance has not been calculated for this patch yet - exit
				return;
			}
			float distanceToPatch = AccurateDistanceToPatch( cameraPos, PatchCentre, m_Radius );
			m_DistToPatch = distanceToPatch;
			if ( distanceToPatch < m_IncreaseDetailDistance )
			{
				if ( IsLeafNode )
				{
					IncreaseDetail( terrain, camera );
				}
				else if ( m_Children != null )
				{
					foreach ( TerrainQuadPatch childPatch in m_Children )
					{
						childPatch.UpdateLod( cameraPos, terrain, camera );
					}
				}
			}
			else if ( distanceToPatch > m_IncreaseDetailDistance )
			{
				if ( !IsLeafNode && CanReduceDetail )
				{
					ReduceDetail( terrain, camera );
				}
			}
		}

		/// <summary>
		/// Updates this patch
		/// </summary>
		public void Update( IProjectionCamera camera, IPlanetTerrain terrain )
		{
			if ( m_Children != null )
			{
				foreach ( TerrainQuadPatch childPatch in m_Children )
				{
					childPatch.Update( camera, terrain );
				}
			}
			else
			{
				//	Bodge to generate vertices (blocking) on the first frame
				if ( m_Parent == null && m_VbIndex == -1 && m_IncreaseDetailDistance == float.MaxValue )
				{
					TerrainQuadPatchBuilder.Instance.BuildVertices( this, terrain, camera, ( PatchError == float.MaxValue ) );
				}
			}
		}

		/// <summary>
		/// Renders this patch
		/// </summary>
		public void Render( )
		{
			if ( m_VbIndex != -1 )
			{
				m_Ib.Draw( PrimitiveType.TriList );
			}
			else if ( m_Children != null )
			{
				for ( int i = 0; i < m_Children.Length; ++i )
				{
					m_Children[ i ].Render( );
				}
			}
		}

		/// <summary>
		/// Renders debug information for this patch
		/// </summary>
		public void DebugRender( )
		{
			if ( m_Children == null )
			{
				Graphics.Fonts.DebugFont.Write( m_Centre.X, m_Centre.Y, m_Centre.Z, FontAlignment.TopRight, Color.White, "{0:F2}/{1:F2}", m_DistToPatch, m_IncreaseDetailDistance );
				Graphics.Draw.Billboard( ms_Brush, m_Centre, 100.0f, 100.0f );

			//	Graphics.Draw.Sphere( Graphics.Surfaces.Red, m_Centre, ( float )m_Radius );
			}
			else
			{
				for ( int i = 0; i < m_Children.Length; ++i )
				{
					m_Children[ i ].DebugRender( );
				}
			}
		}

		#endregion

		#region Private Members

		private readonly TerrainQuadPatch			m_Parent;
		private readonly TerrainQuadPatchVertices	m_Vertices;
		private readonly IIndexBuffer				m_Ib = Graphics.Factory.CreateIndexBuffer( );
		private readonly Point3						m_LocalOrigin;
		private readonly Vector3					m_LocalUAxis;
		private readonly Vector3					m_LocalVAxis;
		private Point3								m_Centre;
		private float								m_PatchError;
		private float								m_DistToPatch;
		private float								m_IncreaseDetailDistance;
		private int									m_VbIndex = -1;
		private bool								m_Building;
		private TerrainQuadPatch[]					m_Children;
		private TerrainQuadPatch[]					m_PendingChildren;
		private TerrainQuadPatch[]					m_CachedChildren;
		private double								m_Radius;
		private readonly float						m_UvRes;
		
		private readonly static DrawBase.IBrush ms_Brush;


		private bool CanReduceDetail
		{
			get
			{
				if ( m_Building || m_PendingChildren != null )
				{
					return false;
				}
				if ( m_Children == null )
				{
					return true;
				}
				foreach ( TerrainQuadPatch childPatch in m_Children )
				{
					if ( !childPatch.CanReduceDetail )
					{
						return false;
					}
				}
				return true;
			}
		}
		
		private bool IsLeafNode
		{
			get
			{
				return ( m_Children == null ) && ( m_PendingChildren == null );
			}
		}

		static TerrainQuadPatch( )
		{
			ms_Brush = Graphics.Draw.NewBrush( Color.Blue );
			ms_Brush.State.DepthTest = false;
			ms_Brush.State.DepthWrite = false;
		}

		public unsafe void SetVertexData( byte* srcData )
		{
			m_VbIndex = m_Vertices.Allocate( );
			if ( m_VbIndex == -1 )
			{
				return;
			}
			using ( IVertexBufferLock vbLock = m_Vertices.VertexBuffer.Lock( m_VbIndex, VertexArea, false, true ) )
			{
				memcpy( vbLock.Bytes, srcData, VertexArea * m_Vertices.VertexBuffer.VertexSizeInBytes );
			}
		}

		
		private void ReleaseVertices( )
		{
			if ( m_VbIndex != -1 )
			{
				m_Vertices.Deallocate( m_VbIndex );
				m_VbIndex = -1;
			}
		}

		private void Build( IPlanetTerrain terrain, IProjectionCamera camera )
		{
			m_Building = true;
			TerrainQuadPatchBuilder.Instance.AddRequest( this, terrain, camera, ( m_PatchError == float.MaxValue ) );
		}

		private void IncreaseDetail( IPlanetTerrain terrain, IProjectionCamera camera )
		{
			if ( m_CachedChildren != null )
			{
				//	Child nodes have already been created - they just need new vertex buffers
				m_PendingChildren = m_CachedChildren;
				m_CachedChildren = null;
			}
			else
			{
				Vector3 uOffset = m_LocalUAxis * 0.5f;
				Vector3 vOffset = m_LocalVAxis * 0.5f;
				float error = m_PatchError / 2;
			//	float error = float.MaxValue;
				float uvRes = m_UvRes / 2;

				TerrainQuadPatch tl = new TerrainQuadPatch( this, m_Vertices, m_LocalOrigin, uOffset, vOffset, error, uvRes );
				TerrainQuadPatch tr = new TerrainQuadPatch( this, m_Vertices, m_LocalOrigin + uOffset, uOffset, vOffset, error, uvRes );
				TerrainQuadPatch bl = new TerrainQuadPatch( this, m_Vertices, m_LocalOrigin + vOffset, uOffset, vOffset, error, uvRes );
				TerrainQuadPatch br = new TerrainQuadPatch( this, m_Vertices, m_LocalOrigin + uOffset + vOffset, uOffset, vOffset, error, uvRes );

				m_PendingChildren = new TerrainQuadPatch[] { tl, tr, bl, br };
			}

			foreach ( TerrainQuadPatch patch in m_PendingChildren )
			{
				patch.Build( terrain, camera );
			}
		}

		private void ReduceDetail( IPlanetTerrain terrain, IProjectionCamera camera )
		{
			for ( int i = 0; i < m_Children.Length; ++i )
			{
				m_Children[ i ].ReleaseVertices( );
			}
			m_CachedChildren = m_Children;
			m_Children = null;
			Build( terrain, camera );
		}

		private static void BuildSkirtIndexBuffer( ICollection<int> indices, int srcIndex, int srcIndexOffset, int dstIndex, bool flip )
		{
			int res = VertexResolution - 1;
			for ( int i = 0; i < res; ++i )
			{
				indices.Add( srcIndex );
				if ( flip )
				{
					indices.Add( srcIndex + srcIndexOffset );
					indices.Add( dstIndex );
				}
				else
				{
					indices.Add( dstIndex );
					indices.Add( srcIndex + srcIndexOffset );
				}

				indices.Add( dstIndex );

				if ( flip )
				{
					indices.Add( srcIndex + srcIndexOffset );
					indices.Add( dstIndex + 1 );
				}
				else
				{
					indices.Add( dstIndex + 1 );
					indices.Add( srcIndex + srcIndexOffset );
				}

				++dstIndex;
				srcIndex += srcIndexOffset;
			}
		}

		private void BuildIndices( )
		{
			int res = VertexResolution;
			int triRes = res - 1;
			List<int> indices = new List<int>( triRes * triRes * 6 + triRes * 12 );

			//	Add connecting strips to patches of lower levels of detail
			int endRow = triRes;
			int endCol = triRes;

			for ( int row = 0; row < endRow; ++row )
			{
				int index = m_VbIndex + ( row * res );
				for ( int col = 0; col < endCol; ++col )
				{
					indices.Add( index );
					indices.Add( index + 1 );
					indices.Add( index + res );

					indices.Add( index + 1 );
					indices.Add( index + 1 + res );
					indices.Add( index + res );

					++index;
				}
			}

			//	First horizontal skirt
			int skirtIndex = m_VbIndex + VertexArea;
			BuildSkirtIndexBuffer( indices, m_VbIndex, 1, skirtIndex, false );

			//	First vertical skirt
			skirtIndex += VertexResolution;
			BuildSkirtIndexBuffer( indices, m_VbIndex, VertexResolution, skirtIndex, true );

			//	Last horizontal skirt
			skirtIndex += VertexResolution;
			BuildSkirtIndexBuffer( indices, m_VbIndex + VertexResolution * ( VertexResolution - 1 ), 1, skirtIndex, true );
			
			//	Last vertical skirt
			skirtIndex += VertexResolution;
			BuildSkirtIndexBuffer( indices, m_VbIndex + VertexResolution - 1, VertexResolution, skirtIndex, false );

			m_Ib.Create( indices.ToArray( ), true );
		}

		/// <summary>
		/// Calculates distance between 2 points using intermediate double precision values
		/// </summary>
		private static float AccurateDistanceToPatch( Point3 pos0, Point3 pos1, double patchRadius )
		{
			double x = pos0.X - pos1.X;
			double y = pos0.Y - pos1.Y;
			double z = pos0.Z - pos1.Z;
			return ( float )( Math.Sqrt( x * x + y * y + z * z ) - patchRadius );
		}

		/// <summary>
		/// Called by OnBuildComplete, when a child node has been built
		/// </summary>
		private void ChildBuildIsComplete( )
		{
			if ( m_PendingChildren == null )
			{
				//	Child build was caused by a ReduceDetail() call - i.e. the child node that just built, was
				//	rebuilt in response to its children being destroyed
				return;
			}
			foreach ( TerrainQuadPatch patch in m_PendingChildren )
			{
				if ( patch.m_VbIndex == -1 )
				{
					return;
				}
			}
			m_Children = m_PendingChildren;
			m_PendingChildren = null;

			//	All child patches have finished building - we can release the vertices
			ReleaseVertices( );
		}

		private TerrainQuadPatch( TerrainQuadPatch parent, TerrainQuadPatchVertices vertices, Point3 origin, Vector3 uAxis, Vector3 vAxis, float patchError, float uvRes )
		{
			m_Parent = parent;
			m_Vertices = vertices;
			m_LocalOrigin = origin;
			m_LocalUAxis = uAxis;
			m_LocalVAxis = vAxis;
			m_Radius = ( uAxis + vAxis ).Length;
			m_PatchError = patchError;
			m_IncreaseDetailDistance = float.MaxValue;
			m_UvRes = uvRes;
		}

		#endregion

		#region P/Invoke

		[DllImport( "msvcrt.dll" )]
		private unsafe static extern IntPtr memcpy( void* dest, void* src, int count );

		#endregion

		#region ITerrainPatch Members

		/// <summary>
		/// Sets planet-space parameters
		/// </summary>
		public void SetPlanetParameters( Point3 centre, float radius )
		{
			m_Centre = centre;
			m_Radius = radius;
		}

		#endregion
	}
}
