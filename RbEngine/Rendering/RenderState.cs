using System;
using System.Drawing;

namespace RbEngine.Rendering
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
		kLighting					= 0x1,

		/// <summary>
		/// Depth testing flag
		/// </summary>
		kDepthTest					= 0x2,

		/// <summary>
		/// Depth writing flag
		/// </summary>
		kDepthWrite					= 0x4,

		/// <summary>
		/// Front face culling flag
		/// </summary>
		kCullFrontFaces				= 0x8,

		/// <summary>
		/// Back face culling flag
		/// </summary>
		kCullBackFaces				= 0x10,

		/// <summary>
		/// Backface winding flag
		/// </summary>
		kBackFacesAreClockwiseWound	= 0x20,

		/// <summary>
		/// 2D texture flag
		/// </summary>
		k2dTextures					= 0x40
	}

	/// <summary>
	/// Depth test pass values (pixel is rendered if the result of a depth test is the selected value)
	/// </summary>
	public enum DepthTestPass
	{
		/// <summary>
		/// Depth test will never be passed
		/// </summary>
		kNever,

		/// <summary>
		/// Depth test will be passed if the rendered depth is less than the fragment depth
		/// </summary>
		kLess,

		/// <summary>
		/// Depth test will be passed if the rendered depth is less than or equal to the fragment depth
		/// </summary>
		kLessOrEqual,
		
		/// <summary>
		/// Depth test will be passed if the rendered depth is equal to the fragment depth
		/// </summary>
		kEqual,
		
		/// <summary>
		/// Depth test will be passed if the rendered depth is not equal to the fragment depth
		/// </summary>
		kNotEqual,
		
		/// <summary>
		/// Depth test will be passed if the rendered depth is greater than or equal to the fragment depth
		/// </summary>
		kGreaterOrEqual,
		
		/// <summary>
		/// Depth test will be passed if the rendered depth is greater than the fragment depth
		/// </summary>
		kGreater,
		
		/// <summary>
		/// Depth test will always be passed
		/// </summary>
		kAlways
	}

	/// <summary>
	/// Polygon rendering modes
	/// </summary>
	public enum PolygonRenderMode
	{
		/// <summary>
		/// Polygons vertices are rendered as points
		/// </summary>
		kPoints,

		/// <summary>
		/// Polygon edges are rendered as edges
		/// </summary>
		kLines,

		/// <summary>
		/// Polygons are rendered filled
		/// </summary>
		kFill
	}

	/// <summary>
	/// Summary description for RenderState.
	/// </summary>
	public abstract class RenderState : IApplicable
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
		/// Enables lighting (equivalent to EnableCap( RenderStateFlag.kLighting ))
		/// </summary>
		/// <returns></returns>
		public RenderState EnableLighting( )
		{
			return SetCap( RenderStateFlag.kLighting, true );
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

		#endregion

		/// <summary>
		/// Applies this render state
		/// </summary>
		public abstract void Apply( );

		#region	Private stuff

		private Color				m_Colour		= Color.Black;
		private PolygonRenderMode	m_FrontPolyMode	= PolygonRenderMode.kFill;
		private PolygonRenderMode	m_BackPolyMode	= PolygonRenderMode.kFill;
		private DepthTestPass		m_DepthTest		= DepthTestPass.kLessOrEqual;
		private RenderStateFlag		m_CapFlags		= RenderStateFlag.kDepthWrite | RenderStateFlag.kDepthTest;
		private bool[]				m_Lights		= new bool[ 8 ];
		private float				m_PointSize		= 1;
		private float				m_LineWidth		= 1;

		#endregion

	}
}
