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
		public Mesh( ModelPart part )
		{
			m_Part = part;
		}

		/// <summary>
		/// Returns the model part that this mesh represents
		/// </summary>
		public ModelPart Part
		{
			get
			{
				return m_Part;
			}
		}

		#region	Mesh tags

		/// <summary>
		/// Tag information (each animation frame contains a set of tag data describing each named tag's coordinate frame)
		/// </summary>
		public class Tag
		{
			public Point3	Origin;
			public Vector3	XAxis;
			public Vector3	YAxis;
			public Vector3	ZAxis;
		}

		/// <summary>
		/// Access to the tag name array
		/// </summary>
		public string[]		TagNames
		{
			get
			{
				return m_TagNames;
			}
			set
			{
				m_TagNames = value;
			}
		}

		/// <summary>
		/// Finds the index of a named tag
		/// </summary>
		public int		GetTagIndex( string name )
		{
			for ( int tagNameIndex = 0; tagNameIndex < m_TagNames.Length; ++tagNameIndex )
			{
				if ( string.Compare( m_TagNames[ tagNameIndex ], name, true ) == 0 )
				{
					return tagNameIndex;
				}
			}
			return -1;
		}

		#endregion

		#region	Nested mesh

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


		/// <summary>
		/// Access to the mesh attached to this one by the transform tag
		/// </summary>
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

		#region	Frame information

		/// <summary>
		/// Information about a frame
		/// </summary>
		public class FrameInfo
		{
			public Point3	Origin;
			public Point3	MinBounds;
			public Point3	MaxBounds;
			public float	Radius;
			public string	Name;
			public Tag[]	Tags;
		}

		/// <summary>
		/// The default frame is the one that is rendered if no animation layers are passed to Render()
		/// </summary>
		public int	DefaultFrame
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
		///	Access to the frame information array
		/// </summary>
		public FrameInfo[]	FrameInfoList
		{
			get
			{
				return m_FrameInfo;
			}
			set
			{
				m_FrameInfo = value;
			}
		}

		#endregion

		#region	Surfaces

		/// <summary>
		/// A surface frame stores vertex buffers for a single animation frame
		/// </summary>
		public class SurfaceFrame
		{
			/// <summary>
			/// Access to the frame's vertex buffers
			/// </summary>
			public OpenGlVertexBuffer[] VertexBuffers
			{
				get
				{
					return m_VertexBuffers;
				}
				set
				{
					m_VertexBuffers = value;
				}
			}

			private OpenGlVertexBuffer[]	m_VertexBuffers;
		}

		/// <summary>
		/// Surface. Stores a set of frames
		/// </summary>
		public class Surface
		{
			/// <summary>
			/// Surface texture
			/// </summary>
			public Texture2d			Texture
			{
				set
				{
					m_Texture = value;
				}
				get
				{
					return m_Texture;
				}
			}

			/// <summary>
			/// Access to the surface's frame list (vertex buffers for each animation frame)
			/// </summary>
			public SurfaceFrame[]		SurfaceFrames
			{
				get
				{
					return m_Frames;
				}
				set
				{
					m_Frames = value;
				}
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

			/// <summary>
			/// Access to texture coordinates used by this surface
			/// </summary>
			public OpenGlVertexBuffer	TextureUVs
			{
				get
				{
					return m_TextureUVs;
				}
				set
				{
					m_TextureUVs = value;
				}
			}

			private SurfaceFrame[]		m_Frames;
			private OpenGlIndexedGroup	m_Group;
			private OpenGlVertexBuffer	m_TextureUVs;
			private Texture2d			m_Texture;
		}

		/// <summary>
		/// Surface collection
		/// </summary>
		public Surface[]	Surfaces
		{
			get
			{
				return m_Surfaces;
			}
			set
			{
				m_Surfaces = value;
			}
		}


		#endregion

		#region	Mesh render effect

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
		public string		AppliedTechniqueName
		{
			get
			{
				return ( m_Technique.Technique == null ) ? string.Empty : m_Technique.Technique.Name;
			}
			set
			{
				m_TextureParameter = null;
				if ( m_Technique.SelectTechnique( value ) )
				{
					//	Set up the shader parameter
					if ( m_Technique.Effect != null )
					{
						//	TODO: Hardcoded sampler (unnecessary now - first texture is automatically bound to named sampler "Texture0")
						m_TextureParameter = m_Technique.Effect.GetParameter( "Sampler" );
					}
				}
			}
		}

		#endregion
		
		#region IRender Members

		/// <summary>
		/// A bodge to allow technique application to work with individual surfaces
		/// </summary>
		private class SurfaceRenderer
		{
			/// <summary>
			/// Sets the surface to render
			/// </summary>
			public Surface	CurrentSurface
			{
				set
				{
					m_Surface = value;
				}
			}

			/// <summary>
			/// Sets the surface frame to render
			/// </summary>
			public SurfaceFrame	CurrentFrame
			{
				set
				{
					m_Frame = value;
				}
			}

			/// <summary>
			/// Renders the current surface and surface frame
			/// </summary>
			public void Render( )
			{
				//	Apply all vertex buffers in the current frame
				int numVbs = m_Frame.VertexBuffers.Length;
				for ( int vbIndex = 0; vbIndex < numVbs; ++vbIndex )
				{
					m_Frame.VertexBuffers[ vbIndex ].Begin( );
				}
				
				if ( m_Surface.TextureUVs != null )
				{
					m_Surface.TextureUVs.Begin( );
				}

				m_Surface.Group.Draw( );

				if ( m_Surface.TextureUVs != null )
				{
					m_Surface.TextureUVs.End( );
				}

				//	TODO: Don't need to unapply the vertex buffers like this - could just enable all client arrays before hand, then
				//	disable all after
				for ( int vbIndex = 0; vbIndex < numVbs; ++vbIndex )
				{
					m_Frame.VertexBuffers[ vbIndex ].End( );
				}

			}

			private Surface			m_Surface;
			private SurfaceFrame	m_Frame;
		}

		/// <summary>
		/// Renders thi
		/// </summary>
		/// <param name="layers"></param>
		public void Render( AnimationLayer[] layers )
		{
			int currentFrame = DefaultFrame;
			if ( layers != null )
			{
				currentFrame = ( layers[ ( int )Part ].CurrentAnimationFrame );
			}

			//	TODO: Make a texture stack per stage?
			Texture2d oldTexture = Renderer.Inst.GetTexture( 0 );

			//	Assign texture sampler parameters
			//	TODO: This should be part of the render technique
			for ( int surfaceIndex = 0; surfaceIndex < m_Surfaces.Length; ++surfaceIndex )
			{
				Surface curSurface = m_Surfaces[ surfaceIndex ];

				Renderer.Inst.BindTexture( 0, curSurface.Texture );
				if ( m_TextureParameter != null )
				{
					m_TextureParameter.Set( curSurface.Texture );
				}

				m_SurfaceRenderer.CurrentSurface	= curSurface;
				m_SurfaceRenderer.CurrentFrame		= curSurface.SurfaceFrames[ currentFrame ];

				m_Technique.Apply( new TechniqueRenderDelegate( m_SurfaceRenderer.Render ) );
			}

			if ( m_NestedMesh != null )
			{
				if ( m_TransformTagIndex != -1 )
				{
					FrameInfo	curFrame		= FrameInfoList[ currentFrame ];
					Tag			transformTag	= curFrame.Tags[ TransformTagIndex ];

					Matrix44 transform = new Matrix44( transformTag.Origin, transformTag.XAxis, transformTag.YAxis, transformTag.ZAxis );
					Renderer.Inst.PushTransform( Transform.LocalToWorld, transform );
				}

				m_NestedMesh.Render( );

				if ( m_TransformTagIndex != -1 )
				{
					Renderer.Inst.PopTransform( Transform.LocalToWorld );
				}
			}

			if ( oldTexture != null )
			{
				Renderer.Inst.BindTexture( 0, oldTexture );
			}
		}

		/// <summary>
		/// Renders this mesh
		/// </summary>
		public void Render( )
		{
			Render( null );
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
		private ModelPart				m_Part;
		private AppliedTechnique		m_Technique = new AppliedTechnique( );
		private string[]				m_TagNames;
		private FrameInfo[]				m_FrameInfo;
		private int						m_TransformTagIndex = -1;
		private Mesh					m_NestedMesh;
		private int						m_Frame;
		private Surface[]				m_Surfaces;
		private SurfaceRenderer			m_SurfaceRenderer = new SurfaceRenderer( );
		private ShaderParameter			m_TextureParameter;

		#endregion
	}
}
