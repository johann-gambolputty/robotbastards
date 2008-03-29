using System.Collections.Generic;
using System.Drawing;
using Rb.Rendering;
using Rb.Core.Maths;
using Rb.Core.Components;
using Rb.Rendering.Interfaces;
using Rb.Rendering.Interfaces.Objects;
using Rb.Rendering.OpenGl;

namespace Rb.Rendering.OpenGl.Md3Loader
{
	/// <summary>
	/// OpenGL mesh with support for MD3 animation
	/// </summary>
	public class ModelMesh : INamed
	{
		/// <summary>
		/// Constructor
		/// </summary>
		public ModelMesh( Model model, ModelPart part )
		{
			m_Model = model;
			m_Part = part;
		}

		/// <summary>
		/// Returns the model part that this mesh represents
		/// </summary>
		public ModelPart Part
		{
			get { return m_Part; }
		}

		#region	Mesh tags

		/// <summary>
		/// Tag information (each animation frame contains a set of tag data describing each named tag's coordinate frame)
		/// </summary>
		public class Tag
		{
			public Matrix44	Transform;
			public string	Name;
		}

		/// <summary>
		/// Access to the tag name array
		/// </summary>
		public string[] TagNames
		{
			get { return m_TagNames; }
			set { m_TagNames = value; }
		}

		/// <summary>
		/// Finds the index of a named tag
		/// </summary>
		public int GetTagIndex( string name )
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
		/// Adds a nested model part to the mesh
		/// </summary>
		public void AddNestedPart( ModelPart part, int transformTagIndex )
		{
			NestedPart nestedPart = new NestedPart( );
			nestedPart.m_Part = part;
			nestedPart.m_TransformTagIndex = transformTagIndex;

			m_NestedParts.Add( nestedPart );
		}

		private struct NestedPart
		{
			public ModelPart m_Part;
			public int m_TransformTagIndex;
		}

		private readonly List< NestedPart > m_NestedParts = new List< NestedPart >( ); 

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
			get { return m_Frame; }
			set { m_Frame = value; }
		}

		/// <summary>
		///	Access to the frame information array
		/// </summary>
		public FrameInfo[]	FrameInfoList
		{
			get { return m_FrameInfo; }
			set { m_FrameInfo = value; }
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
			public OpenGlVertexArray[] VertexBuffers
			{
				get { return m_VertexBuffers; }
				set { m_VertexBuffers = value; }
			}

			private OpenGlVertexArray[]	m_VertexBuffers;
		}

		/// <summary>
		/// Surface. Stores a set of frames
		/// </summary>
		public class Surface
		{
			/// <summary>
			/// Surface texture
			/// </summary>
			public ITexture2d Texture
			{
				set { m_Texture = value; }
				get { return m_Texture; }
			}

			/// <summary>
			/// Access to the surface's frame list (vertex buffers for each animation frame)
			/// </summary>
			public SurfaceFrame[] SurfaceFrames
			{
				get { return m_Frames; }
				set { m_Frames = value; }
			}

			/// <summary>
			/// Access to the surface's indexed group
			/// </summary>
			public OpenGlIndexedGroup Group
			{
				get { return m_Group; }
				set { m_Group = value; }
			}

			/// <summary>
			/// Access to texture coordinates used by this surface
			/// </summary>
			public OpenGlVertexArray TextureUVs
			{
				get { return m_TextureUVs; }
				set { m_TextureUVs = value; }
			}

			private SurfaceFrame[]		m_Frames;
			private OpenGlIndexedGroup	m_Group;
			private OpenGlVertexArray	m_TextureUVs;
			private ITexture2d			m_Texture;
		}

		/// <summary>
		/// Surface collection
		/// </summary>
		public Surface[] Surfaces
		{
			get { return m_Surfaces; }
			set { m_Surfaces = value; }
		}


		#endregion

		#region	Mesh render effect

		/// <summary>
		/// The mesh render effect
		/// </summary>
		public IEffect Effect
		{
			get { return m_Technique.Effect; }
			set { m_Technique.Effect = value; }
		}

		/// <summary>
		/// The selected render technique's name
		/// </summary>
		public string TechniqueName
		{
			get
			{
				return ( m_Technique.Technique == null ) ? string.Empty : m_Technique.Technique.Name;
			}
			set
			{
				m_TextureParameter = null;

                m_Technique.Select( value );

				//	Set up the shader parameter
				if ( m_Technique.Effect != null )
				{
					//	TODO: AP: Hardcoded sampler (unnecessary now - first texture is automatically bound to named sampler "Texture0")
					m_Technique.Effect.Parameters.TryGetValue( "Sampler", out m_TextureParameter );
				}
			}
		}

		#endregion
		
		#region IRenderable Members

		/// <summary>
		/// A bodge to allow technique application to work with individual surfaces
		/// </summary>
		private class SurfaceRenderer : IRenderable
		{
			/// <summary>
			/// Sets the surface to render
			/// </summary>
			public Surface	CurrentSurface
			{
				set { m_Surface = value; }
			}

			/// <summary>
			/// Sets the surface frame to render
			/// </summary>
			public SurfaceFrame	CurrentFrame
			{
				set { m_Frame = value; }
            }

            #region Private stuff

            private Surface			m_Surface;
			private SurfaceFrame	m_Frame;

            #endregion

            #region IRenderable Members

            /// <summary>
            /// Renders this surface
            /// </summary>
            /// <param name="context"></param>
            public void Render( IRenderContext context )
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

            #endregion
        }
		
		/// <summary>
		/// Debug routing for drawing the bounds of this mesh
		/// </summary>
		/// <param name="layers">Animation layer set</param>
		public void DrawMeshBounds( AnimationLayer[] layers )
		{
			FrameInfo frame = GetAnimationFrame( layers );
			Graphics.Draw.AlignedBox( m_BoundsDraw, frame.MinBounds, frame.MaxBounds );
		}

		/// <summary>
		/// Gets information about the current animation frame 
		/// </summary>
		/// <param name="layers">Animation layers</param>
		/// <returns>Returns frame information for the current animation frame</returns>
		public FrameInfo GetAnimationFrame( AnimationLayer[] layers )
		{
			int index = ( layers == null ) ? DefaultFrame : layers[ ( int )Part ].CurrentAnimationFrame;
			return FrameInfoList[ index ];
		}

		/// <summary>
		/// Renders this mesh using the given context and animation setup
		/// </summary>
		public void Render( IRenderContext context, AnimationLayer[] layers, ModelInstance.ReferencePoint[] refPoints )
		{
			//	Determine the current animation frame
			int currentFrame = DefaultFrame;
			if ( layers != null )
			{
				currentFrame = ( layers[ ( int )Part ].CurrentAnimationFrame );
			}

		//	DrawMeshBounds( layers );

			//	Assign texture sampler parameters
			//	TODO: This should be part of the render technique
			ITexture2d lastTexture = null;
			for ( int surfaceIndex = 0; surfaceIndex < m_Surfaces.Length; ++surfaceIndex )
			{
				Surface curSurface = m_Surfaces[ surfaceIndex ];

				if ( curSurface.Texture != lastTexture )
				{
					if ( lastTexture != null )
					{
						Graphics.Renderer.UnbindTexture( lastTexture );
					}
					Graphics.Renderer.BindTexture( curSurface.Texture );
					lastTexture = curSurface.Texture;
				}
				if ( m_TextureParameter != null )
				{
					m_TextureParameter.Set( curSurface.Texture );
				}

				m_SurfaceRenderer.CurrentSurface	= curSurface;
				m_SurfaceRenderer.CurrentFrame		= curSurface.SurfaceFrames[ currentFrame ];

				m_Technique.Apply( context, m_SurfaceRenderer );
			}
			if ( lastTexture != null )
			{
				Graphics.Renderer.UnbindTexture( lastTexture );
			}

			foreach ( NestedPart nestedPart in m_NestedParts )
			{
				FrameInfo curFrame = FrameInfoList[ currentFrame ];
				Tag transformTag = curFrame.Tags[ nestedPart.m_TransformTagIndex ];

				Matrix44 transform = transformTag.Transform;
				Graphics.Renderer.PushTransform( Transform.LocalToWorld, transform );

				ModelMesh nestedMesh = m_Model.GetPartMesh( nestedPart.m_Part );
				if ( nestedMesh != null )
				{
					nestedMesh.Render( context, layers, refPoints );
				}
				ModelInstance.ReferencePoint refPt = refPoints[ ( int )nestedPart.m_Part ];
				if ( refPt != null )
				{
					refPt.Render( context );
				}

				Graphics.Renderer.PopTransform( Transform.LocalToWorld );
			}
		}

		#endregion

		#region INamedObject Members

		/// <summary>
		/// The name of this mesh
		/// </summary>
		public string Name
		{
			get { return m_Name; }
			set { m_Name = value; }
		}

		#endregion	

		#region	Private stuff

		private readonly Model				m_Model;
		private readonly ModelPart			m_Part;
		private readonly TechniqueSelector	m_Technique = new TechniqueSelector( );
		private readonly SurfaceRenderer	m_SurfaceRenderer = new SurfaceRenderer( );
		private string						m_Name;
		private string[]					m_TagNames;
		private FrameInfo[]					m_FrameInfo;
		private int							m_Frame;
		private Surface[]					m_Surfaces;
		private IEffectParameter			m_TextureParameter;
		
		private readonly static Draw.IPen	m_BoundsDraw;

		static ModelMesh( )
		{
			m_BoundsDraw = Graphics.Draw.NewPen( Color.Red, 1.5f );
			m_BoundsDraw.State.DepthTest = true;
			m_BoundsDraw.State.DepthWrite = true;
		}

		#endregion
	}
}
