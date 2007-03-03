using System;
using RbEngine.Rendering;
using RbEngine.Maths;
using RbOpenGlRendering;
using Tao.OpenGl;

namespace RbOpenGlMd3Loader
{
	/// <summary>
	/// OpenGL mesh with support for MD3 animation
	/// </summary>
	public class Mesh : IRender, RbEngine.Components.INamedObject
	{
		/// <summary>
		/// Constructor
		/// </summary>
		public Mesh( )
		{
			m_Technique.RenderCallback = new RenderTechnique.RenderDelegate( RenderMesh );
		}

		#region	Mesh tags

		/// <summary>
		/// Tag information
		/// </summary>
		public class Tag
		{
			public string	Name;
			public Point3	Origin;
			public Vector3	XAxis;
			public Vector3	YAxis;
			public Vector3	ZAxis;
		}

		/// <summary>
		/// Access to the tag that specifies the coordinate frame for the nested mesh
		/// </summary>
		public int		TransformTagIndex
		{
			set
			{
				m_TransformTagIndex = value;
			}
			get
			{
				return m_TransformTagIndex;
			}
		}

		public int		TagsPerFrame
		{
			set
			{
				m_TagsPerFrame = value;
			}
			get
			{
				return m_TagsPerFrame;
			}
		}

		int m_TagsPerFrame;

		/// <summary>
		/// Tag array access
		/// </summary>
		public Tag[]	Tags
		{
			set
			{
				m_Tags = value;
			}
			get
			{
				return m_Tags;
			}
		}
		
		/// <summary>
		/// Finds a named tag
		/// </summary>
		public int		GetTagIndex( string name )
		{
			for ( int tagIndex = 0; tagIndex < m_Tags.Length; ++tagIndex )
			{
				if ( string.Compare( m_Tags[ tagIndex ].Name, name, true ) == 0 )
				{
					return tagIndex;
				}
			}
			return -1;
		}

		#endregion

		#region	Nested mesh

		public Mesh	NestedMesh
		{
			get
			{
				return m_NestedMesh;
			}
			set
			{
				m_NestedMesh = value;
			}

		}

		#endregion

		#region	FrameSet setup

		/// <summary>
		/// Information about a frame
		/// </summary>
		public class FrameInfo
		{
			/// <summary>
			/// Frame origin
			/// </summary>
			public Point3	Origin
			{
				get
				{
					return m_Origin;
				}
				set
				{
					m_Origin = value;
				}
			}

			private Point3	m_Origin;
		}

		/// <summary>
		/// A surface frame stores some vertex buffers
		/// </summary>
		public class SurfaceFrame
		{
			/// <summary>
			/// Creates vertex buffers
			/// </summary>
			public void	CreateVertexBuffers( int numVertexBuffers )
			{
				m_VertexBuffers = new OpenGlVertexBuffer[ numVertexBuffers ];
			}

			/// <summary>
			/// Sets a vertex buffer at an index in the vertex buffer array
			/// </summary>
			public void	SetVertexBuffer( int index, OpenGlVertexBuffer buffer )
			{
				m_VertexBuffers[ index ] = buffer;
			}

			/// <summary>
			/// Gets the number of stored vertex buffers
			/// </summary>
			public int	NumVertexBuffers
			{
				get
				{
					return m_VertexBuffers.Length;
				}
			}

			/// <summary>
			/// Gets an indexed vertex buffer
			/// </summary>
			public OpenGlVertexBuffer	GetVertexBuffer( int index )
			{
				return m_VertexBuffers[ index ];
			}


			private OpenGlVertexBuffer[]	m_VertexBuffers;
		}

		/// <summary>
		/// Surface. Stores a set of frames
		/// </summary>
		public class Surface
		{
			/// <summary>
			/// Creates the frame list
			/// </summary>
			public void	CreateFrames( int numFrames )
			{
				m_Frames = new SurfaceFrame[ numFrames ];
			}

			/// <summary>
			/// Sets an indexed frame
			/// </summary>
			public void	SetFrame( int index, SurfaceFrame frame )
			{
				m_Frames[ index ] = frame;
			}

			/// <summary>
			/// Gets an indexed frame
			/// </summary>
			public SurfaceFrame	GetFrame( int index )
			{
				return m_Frames[ index ];
			}

			/// <summary>
			/// Access to the surface's indexed group
			/// </summary>
			public OpenGlIndexedGroup	Group
			{
				get
				{
					return m_Group;
				}
				set
				{
					m_Group = value;
				}
			}

			private SurfaceFrame[]		m_Frames;
			private OpenGlIndexedGroup	m_Group;
		}

		/// <summary>
		/// The current frame is the one that is rendered
		/// </summary>
		public int	CurrentFrame
		{
			get
			{
				return m_Frame;
			}
			set
			{
				m_Frame = value;
			}
		}

		/// <summary>
		/// Creates the frame information array
		/// </summary>
		public void			CreateFrameInfo( int numFrames )
		{
			m_FrameInfo = new FrameInfo[ numFrames ];
		}

		/// <summary>
		/// Sets an indexed frame information object
		/// </summary>
		public void			SetFrameInfo( int frameIndex, FrameInfo info )
		{
			m_FrameInfo[ frameIndex ] = info;
		}

		/// <summary>
		///	Creates the surfaces array
		/// </summary>
		public void			CreateSurfaces( int numSurfaces )
		{
			m_Surfaces = new Surface[ numSurfaces ];
		}

		/// <summary>
		/// Sets an indexed surface
		/// </summary>
		public void			SetSurface( int surfaceIndex, Surface surface )
		{
			m_Surfaces[ surfaceIndex ] = surface;
		}

		/// <summary>
		/// Gets an indexed surface
		/// </summary>
		public Surface		GetSurface( int surfaceIndex )
		{
			return m_Surfaces[ surfaceIndex ];
		}

		private Surface[]	m_Surfaces;

		#endregion

		#region	Texture setup

		/// <summary>
		/// Sets up a texture
		/// </summary>
		public void AddTexture( Texture2d texture )
		{
			m_Textures[ m_NumTextures++ ].Texture = texture;
		}

		/// <summary>
		/// Gets a texture
		/// </summary>
		public Texture2d	GetTexture( int index )
		{
			return m_Textures[ index ].Texture;
		}

		#endregion

		#region	Mesh rendering properties

		/// <summary>
		/// The mesh render effect
		/// </summary>
		public RenderEffect	Effect
		{
			get
			{
				return m_Technique.Effect;
			}
			set
			{
				m_Technique.Effect = value;
			}
		}

		/// <summary>
		/// The selected render technique's name
		/// </summary>
		public string		SelectedTechniqueName
		{
			get
			{
				return ( m_Technique.Technique == null ) ? string.Empty : m_Technique.Technique.Name;
			}
			set
			{
				m_Technique.SelectTechnique( value );
			}
		}

		#endregion
		
		#region IRender Members

		/// <summary>
		/// Renders this mesh
		/// </summary>
		public void Render( )
		{
			m_Technique.Apply( );

			if ( m_NestedMesh != null )
			{
				if ( m_TransformTagIndex != -1 )
				{
					Tag transformTag = Tags[ ( CurrentFrame * m_TagsPerFrame ) + m_TransformTagIndex ];

					Matrix44 transform = new Matrix44( transformTag.Origin, transformTag.XAxis, transformTag.YAxis, transformTag.ZAxis );
					Renderer.Inst.PushTransform( Transform.LocalToView, transform );

				}

				m_NestedMesh.Render( );

				if ( m_TransformTagIndex != -1 )
				{
					Renderer.Inst.PopTransform( Transform.LocalToView );
				}
			}

		}


		private int	m_Frame;

		/// <summary>
		/// Renders the mesh
		/// </summary>
		private void RenderMesh( )
		{
			//	Apply textures
			//	TODO: This should be part of the render technique
			for ( int textureIndex = 0; textureIndex < m_NumTextures; ++textureIndex )
			{
				m_Textures[ textureIndex ].Apply( );
			}

			//	Run through all stored surfaces
			for ( int surfaceIndex = 0; surfaceIndex < m_Surfaces.Length; ++surfaceIndex )
			{
				SurfaceFrame curFrame = m_Surfaces[ surfaceIndex ].GetFrame( CurrentFrame );

				//	Apply all vertex buffers in the current frame
				for ( int vbIndex = 0; vbIndex < curFrame.NumVertexBuffers; ++vbIndex )
				{
					curFrame.GetVertexBuffer( vbIndex ).Apply( );
				}

				m_Surfaces[ surfaceIndex ].Group.Draw( );

				//	TODO: Don't need to unapply the vertex buffers like this - could just enable all client arrays before hand, then
				//	disable all after
				for ( int vbIndex = 0; vbIndex < curFrame.NumVertexBuffers; ++vbIndex )
				{
					curFrame.GetVertexBuffer( vbIndex ).UnApply( );
				}
			}

		}

		#endregion

		#region INamedObject Members

		/// <summary>
		/// Event, invoked when the named of this mesh is changed
		/// </summary>
		public event RbEngine.Components.NameChangedDelegate NameChanged;

		/// <summary>
		/// The name of this mesh
		/// </summary>
		public string Name
		{
			get
			{
				return m_Name;
			}
			set
			{
				m_Name = value;
				if ( NameChanged != null )
				{
					NameChanged( this );
				}
			}
		}

		#endregion	

		#region	Private stuff

		private string					m_Name;
		private SelectedTechnique		m_Technique = new SelectedTechnique( );
		private TextureSampler2d[]		m_Textures = new TextureSampler2d[ 8 ];
		private int						m_NumTextures;
		private FrameInfo[]				m_FrameInfo;
		private Tag[]					m_Tags;
		private int						m_TransformTagIndex = -1;
		private Mesh					m_NestedMesh;

		#endregion
	}
}
