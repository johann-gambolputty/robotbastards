using System;
using System.Drawing;
using Poc1.Universe.Interfaces.Planets.Renderers.Patches;
using Rb.Core.Maths;
using Rb.Core.Utils;
using Rb.Rendering;
using Rb.Rendering.Interfaces.Objects;
using Rb.Rendering.Interfaces.Objects.Cameras;
using Graphics=Rb.Rendering.Graphics;

namespace Poc1.Universe.Planets.Spherical.Renderers.Patches
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
	///		Each new child node adds a build request to the TerrainPatchBuilder
	///		When all 4 child nodes are built, their "Building" flags are set to false, and they inform the parent patch, which moves
	///		them from the PendingChildren list to the Children list.
	///
	///	Patch detail is decreased when the distance from the patch to the camera is greater than the error threshold
	/// in a non-leaf node.
	///		If any child below the patch is marked as "Building", then the detail reduction fails.
	///		Children list is moved to CachedChildren (recursive), making the non-leaf node into a leaf node.
	///			All children moved in this way release their allocated vertex areas
	///		The node is marked as "Building".
	///		The node adds a build request to the TerrainPatchBuilder
	///		When the node has been build, its "Building" flag is set to false.
	/// 
	/// </remarks>
	public class TerrainPatch : ITerrainPatch
	{
		#region Public Construction

		//	TODO: AP: UV patch resolution should be a function of patch size, not ply

		public TerrainPatch( TerrainPatchVertices vertices, Point3 origin, Vector3 uAxis, Vector3 vAxis, float uvRes ) :
			this( null, vertices, origin, uAxis, vAxis, float.MaxValue, uvRes )
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
			get { return m_LocalUAxis / ( TerrainPatchConstants.PatchResolution - 1 ); }
		}

		/// <summary>
		/// Gets the V step of this patch
		/// </summary>
		public Vector3 LocalVStep
		{
			get { return m_LocalVAxis / ( TerrainPatchConstants.PatchResolution - 1 ); }
		}

		/// <summary>
		/// Gets the terrain texture resolution of this patch
		/// </summary>
		public float UvResolution
		{
			get { return m_UvRes; }
			set { m_UvRes = value; }
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
				foreach ( TerrainPatch patch in m_Children )
				{
					count += patch.CountNodesWithVertexData( );
				}
			}
			return count;
		}

		/// <summary>
		/// Called by <see cref="TerrainPatchBuilder"/> when it has finished building vertex data for this patch
		/// </summary>
		public unsafe void OnBuildComplete( byte* vertexData, float increaseDetailDistance, int[] baseIndices )
		{
			m_Building = false;
			m_IncreaseDetailDistance = increaseDetailDistance;
			SetVertexData( vertexData );

			for ( int index = 0; index < baseIndices.Length; ++index )
			{
				baseIndices[ index ] += m_VbIndex;
			}
			m_Ib.Create( baseIndices, true );

			if ( m_Parent != null )
			{
				m_Parent.ChildBuildIsComplete( );
			}

			if ( m_Children != null )
			{
				for ( int i = 0; i < m_Children.Length; ++i )
				{
				    m_Children[ i ].ReleaseVertices( );
				}
				m_CachedChildren = m_Children;
				m_Children = null;
			}
		}

		/// <summary>
		/// Updates the level of detail of this patch. 
		/// </summary>
		public void UpdateLod( Point3 cameraPos, ITerrainPatchGenerator generator, IProjectionCamera camera )
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
					IncreaseDetail( generator, camera );
				}
				else if ( m_Children != null )
				{
					foreach ( TerrainPatch childPatch in m_Children )
					{
						childPatch.UpdateLod( cameraPos, generator, camera );
					}
				}
			}
			else if ( distanceToPatch > m_IncreaseDetailDistance )
			{
				if ( !IsLeafNode && CanReduceDetail )
				{
					ReduceDetail( generator, camera );
				}
			}
		}

		/// <summary>
		/// Updates this patch
		/// </summary>
		public void Update( IProjectionCamera camera, ITerrainPatchGenerator generator )
		{
			if ( m_Children != null )
			{
				foreach ( TerrainPatch childPatch in m_Children )
				{
					childPatch.Update( camera, generator );
				}
			}
			else
			{
				//	Bodge to generate vertices (blocking) on the first frame
				if ( m_Parent == null && m_VbIndex == -1 && m_IncreaseDetailDistance == float.MaxValue )
				{
					TerrainPatchBuildItem builder = TerrainPatchBuildItem.Allocate( camera, generator, this, ( PatchError == float.MaxValue ) );
					builder.Build( );
				}
			}
		}

		/// <summary>
		/// Renders this patch
		/// </summary>
		public void Render( IRenderContext context )
		{
			if ( m_VbIndex != -1 )
			{
				m_Ib.Draw( PrimitiveType.TriList );
			}
			else if ( m_Children != null )
			{
				for ( int i = 0; i < m_Children.Length; ++i )
				{
					m_Children[ i ].Render( context );
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
				Graphics.Draw.Billboard( s_Brush, m_Centre, 100.0f, 100.0f );

				Graphics.Draw.Sphere( Graphics.Surfaces.TransparentBlue, m_Centre, ( float )m_Radius );
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

		private readonly TerrainPatch			m_Parent;
		private readonly TerrainPatchVertices	m_Vertices;
		private readonly IIndexBuffer			m_Ib = Graphics.Factory.CreateIndexBuffer( );
		private readonly Point3					m_LocalOrigin;
		private readonly Vector3				m_LocalUAxis;
		private readonly Vector3				m_LocalVAxis;
		private Point3							m_Centre;
		private float							m_PatchError;
		private float							m_DistToPatch;
		private float							m_IncreaseDetailDistance;
		private int								m_VbIndex = -1;
		private bool							m_Building;
		private TerrainPatch[]					m_Children;
		private TerrainPatch[]					m_PendingChildren;
		private TerrainPatch[]					m_CachedChildren;
		private double							m_Radius;
		private float							m_UvRes;
		
		private readonly static DrawBase.IBrush s_Brush;


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
				foreach ( TerrainPatch childPatch in m_Children )
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

		static TerrainPatch( )
		{
			s_Brush = Graphics.Draw.NewBrush( Color.Blue );
			s_Brush.State.DepthTest = false;
			s_Brush.State.DepthWrite = false;
		}

		public unsafe void SetVertexData( byte* srcData )
		{
			m_VbIndex = m_Vertices.Allocate( );
			if ( m_VbIndex == -1 )
			{
				return;
			}
			using ( IVertexBufferLock vbLock = m_Vertices.VertexBuffer.Lock( m_VbIndex, TerrainPatchConstants.PatchTotalVertexCount, false, true ) )
			{
				MsvCrt.memcpy( vbLock.Bytes, srcData, TerrainPatchConstants.PatchTotalVertexCount * m_Vertices.VertexBuffer.VertexSizeInBytes );
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

		private void Build( ITerrainPatchGenerator generator, IProjectionCamera camera )
		{
			m_Building = true;
			TerrainPatchBuildItem builder = TerrainPatchBuildItem.Allocate( camera, generator, this, ( PatchError == float.MaxValue ) );
			TerrainPatchBuilder.QueueWork( builder );
		}

		private void IncreaseDetail( ITerrainPatchGenerator generator, IProjectionCamera camera )
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

				TerrainPatch tl = new TerrainPatch( this, m_Vertices, m_LocalOrigin, uOffset, vOffset, error, uvRes );
				TerrainPatch tr = new TerrainPatch( this, m_Vertices, m_LocalOrigin + uOffset, uOffset, vOffset, error, uvRes );
				TerrainPatch bl = new TerrainPatch( this, m_Vertices, m_LocalOrigin + vOffset, uOffset, vOffset, error, uvRes );
				TerrainPatch br = new TerrainPatch( this, m_Vertices, m_LocalOrigin + uOffset + vOffset, uOffset, vOffset, error, uvRes );

				m_PendingChildren = new TerrainPatch[] { tl, tr, bl, br };
			}

			foreach ( TerrainPatch patch in m_PendingChildren )
			{
				patch.Build( generator, camera );
			}
		}

		private void ReduceDetail( ITerrainPatchGenerator generator, IProjectionCamera camera )
		{
			Build( generator, camera );
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
			foreach ( TerrainPatch patch in m_PendingChildren )
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

		private TerrainPatch( TerrainPatch parent, TerrainPatchVertices vertices, Point3 origin, Vector3 uAxis, Vector3 vAxis, float patchError, float uvRes )
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
