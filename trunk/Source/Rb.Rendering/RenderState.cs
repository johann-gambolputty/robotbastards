using System;
using System.Drawing;

namespace Rb.Rendering
{
	/// <summary>
	/// Flagged rendering capabilities
	/// </summary>
	[ Flags ]
	public enum RenderStateFlag
	{
		/// <summary>
		/// Lighting flag
		/// </summary>
		Lighting					= 0x1,

		/// <summary>
		/// Depth testing flag
		/// </summary>
		DepthTest					= 0x2,

		/// <summary>
		/// Depth writing flag
		/// </summary>
		DepthWrite					= 0x4,

		/// <summary>
		/// Front face culling flag
		/// </summary>
		CullFrontFaces				= 0x8,

		/// <summary>
		/// Back face culling flag
		/// </summary>
		CullBackFaces				= 0x10,

		/// <summary>
		/// Backface winding flag
		/// </summary>
		BackFacesAreClockwiseWound	= 0x20,

		/// <summary>
		/// 2D texture flag
		/// </summary>
		Texture2d					= 0x40,

		/// <summary>
		/// Blending flag
		/// </summary>
		Blend						= 0x80
	}

	/// <summary>
	/// Blending factors
	/// </summary>
	public enum BlendFactor
	{
		/// <summary>
		/// Zero contribution from input
		/// </summary>
		Zero,

		/// <summary>
		/// Input blended by destination colour
		/// </summary>
		DstColour,

		/// <summary>
		/// Input blended by one minus the destination colour
		/// </summary>
		OneMinusDstColour,
		
		/// <summary>
		/// Input blended by destination alpha
		/// </summary>
		DstAlpha,
		
		/// <summary>
		/// Input blended by one minus the destination alpha
		/// </summary>
		OneMinusDstAlpha,

		/// <summary>
		/// Input blended by source colour
		/// </summary>
		SrcColour,
		
		/// <summary>
		/// Input blended by one minus the source colour
		/// </summary>
		OneMinusSrcColour,
		
		/// <summary>
		/// Input blended by source alpha
		/// </summary>
		SrcAlpha,
		
		/// <summary>
		/// Input blended by one minus the source alpha
		/// </summary>
		OneMinusSrcAlpha,
		
		
		/// <summary>
		/// Input blended by saturated source alpha
		/// </summary>
		SrcAlphaSaturate,

		/// <summary>
		/// Input only
		/// </summary>
		One
	}

	/// <summary>
	/// Depth test pass values (pixel is rendered if the result of a depth test is the selected value)
	/// </summary>
	public enum DepthTestPass
	{
		/// <summary>
		/// Depth test will never be passed
		/// </summary>
		Never,

		/// <summary>
		/// Depth test will be passed if the rendered depth is less than the fragment depth
		/// </summary>
		Less,

		/// <summary>
		/// Depth test will be passed if the rendered depth is less than or equal to the fragment depth
		/// </summary>
		LessOrEqual,
		
		/// <summary>
		/// Depth test will be passed if the rendered depth is equal to the fragment depth
		/// </summary>
		Equal,
		
		/// <summary>
		/// Depth test will be passed if the rendered depth is not equal to the fragment depth
		/// </summary>
		NotEqual,
		
		/// <summary>
		/// Depth test will be passed if the rendered depth is greater than or equal to the fragment depth
		/// </summary>
		GreaterOrEqual,
		
		/// <summary>
		/// Depth test will be passed if the rendered depth is greater than the fragment depth
		/// </summary>
		Greater,
		
		/// <summary>
		/// Depth test will always be passed
		/// </summary>
		Always
	}

	/// <summary>
	/// Polygon rendering modes
	/// </summary>
	public enum PolygonRenderMode
	{
		/// <summary>
		/// Polygons vertices are rendered as points
		/// </summary>
		Points,

		/// <summary>
		/// Polygon edges are rendered as edges
		/// </summary>
		Lines,

		/// <summary>
		/// Polygons are rendered filled
		/// </summary>
		Fill
	}

	/// <summary>
	/// Polygon shading modes
	/// </summary>
	public enum PolygonShadeMode
	{
		/// <summary>
		/// Flat shading
		/// </summary>
		Flat,

		/// <summary>
		/// Smooth shading
		/// </summary>
		Smooth
	}

	/// <summary>
	/// Summary description for RenderState.
	/// </summary>
	public abstract class RenderState : IPass
	{
		#region	Setup

		/// <summary>
		/// Sets the current colour
		/// </summary>
		public RenderState SetColour( Color colour )
		{
			m_Colour = colour;
			return this;
		}

		/// <summary>
		/// Sets the depth test pass criteria
		/// </summary>
		public RenderState SetDepthTest( DepthTestPass depthTest )
		{
			m_DepthTest = depthTest;
			return this;
		}

		/// <summary>
		/// Sets up the source and destination blend factors. Automatically enables blending
		/// </summary>
		public RenderState SetBlendMode( BlendFactor srcBlend, BlendFactor dstBlend )
		{
			m_SrcBlend = srcBlend;
			m_DstBlend = dstBlend;
			EnableCap( RenderStateFlag.Blend );
			return this;
		}

		/// <summary>
		/// Enables lighting (equivalent to SetCap( RenderStateFlag.kLighting, true ) )
		/// </summary>
		/// <returns></returns>
		public RenderState EnableLighting( )
		{
			return SetCap( RenderStateFlag.Lighting, true );
		}

		/// <summary>
		/// Disables lighting (equivalent to SetCap( RenderStateFlag.kLighting  false ) )
		/// </summary>
		/// <returns></returns>
		public RenderState DisableLighting( )
		{
			return SetCap( RenderStateFlag.Lighting, false );
		}

		/// <summary>
		/// Enables a flag capability
		/// </summary>
		public RenderState EnableCap( RenderStateFlag cap )
		{
			return SetCap( cap, true );
		}

		/// <summary>
		/// Disables a flag capability
		/// </summary>
		public RenderState DisableCap( RenderStateFlag cap )
		{
			return SetCap( cap, false );
		}

		/// <summary>
		/// Sets a flag capability
		/// </summary>
		public RenderState SetCap( RenderStateFlag cap, bool enable )
		{
			if ( enable )
			{
				m_CapFlags |= cap;
			}
			else
			{
				m_CapFlags &= ~cap;
			}
			return this;
		}

		/// <summary>
		/// Enables or disables a given light
		/// </summary>
		public RenderState SetLight( int light, bool enable )
		{
			m_Lights[ light ] = enable;
			return this;
		}

		/// <summary>
		/// Sets the point size
		/// </summary>
		public RenderState SetPointSize( float size )
		{
			m_PointSize = size;
			return this;
		}

		/// <summary>
		/// Sets the line width
		/// </summary>
		public RenderState SetLineWidth( float width )
		{
			m_LineWidth = width;
			return this;
		}


		/// <summary>
		/// Sets the polygon shade mode
		/// </summary>
		public RenderState SetShadeMode( PolygonShadeMode mode )
		{
			m_ShadeMode = mode;
			return this;
		}

		/// <summary>
		/// Sets the polygon rendering mode for both front and back facing polygons
		/// </summary>
		/// <param name="mode"></param>
		public RenderState SetPolygonRenderingMode( PolygonRenderMode mode )
		{
			m_FrontPolyMode = mode;
			m_BackPolyMode = mode;
			return this;
		}
		
		/// <summary>
		/// Sets the polygon rendering mode for front facing polygons
		/// </summary>
		/// <param name="mode"></param>
		public RenderState SetFrontPolygonRenderingMode( PolygonRenderMode mode )
		{
			m_FrontPolyMode = mode;
			return this;
		}

		/// <summary>
		/// Sets the polygon rendering mode for back facing polygons
		/// </summary>
		/// <param name="mode"></param>
		public RenderState SetBackPolygonRenderingMode( PolygonRenderMode mode )
		{
			m_BackPolyMode = mode;
			return this;
		}

		/// <summary>
		/// Sets the depth offset
		/// </summary>
		public RenderState	SetDepthOffset( float offset )
		{
			m_DepthOffset = offset;
			return this;
		}

		#endregion

		#region	IPass Members

		/// <summary>
		/// Starts applying this render state
		/// </summary>
		public abstract void Begin( );

		/// <summary>
		/// Stops applying this render state
		/// </summary>
		public abstract void End( );

		#endregion

		#region	Protected stuff

		protected BlendFactor		m_SrcBlend		= BlendFactor.One;
		protected BlendFactor		m_DstBlend		= BlendFactor.Zero;
		protected Color				m_Colour		= Color.Black;
		protected PolygonShadeMode	m_ShadeMode		= PolygonShadeMode.Smooth;
		protected PolygonRenderMode	m_FrontPolyMode	= PolygonRenderMode.Fill;
		protected PolygonRenderMode	m_BackPolyMode	= PolygonRenderMode.Fill;
		protected DepthTestPass		m_DepthTest		= DepthTestPass.LessOrEqual;
		protected RenderStateFlag	m_CapFlags		= RenderStateFlag.DepthWrite | RenderStateFlag.DepthTest;
		protected bool[]			m_Lights		= new bool[ 8 ];
		protected float				m_PointSize		= 1;
		protected float				m_LineWidth		= 1;
		protected float				m_DepthOffset	= 0;

		#endregion

	}
}
