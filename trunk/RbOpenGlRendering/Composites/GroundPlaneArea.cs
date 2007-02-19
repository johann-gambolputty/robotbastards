using System;
using RbEngine.Rendering;

using Tao.OpenGl;


namespace RbOpenGlRendering.Composites
{
	/// <summary>
	/// OpenGL implementation of RbEngine.Rendering.Composites.GroundPlaneArea
	/// </summary>
	public class GroundPlaneArea : RbEngine.Rendering.Composites.GroundPlaneArea, IDisposable
	{
		/// <summary>
		/// Sets up the render technique
		/// </summary>
		public GroundPlaneArea( )
		{
			//	TODO: This is overkill - just set up the appropriate states in Render()
			RenderPass gridRenderPass = new RenderPass( );
			gridRenderPass.Add
			(
				RenderFactory.Inst.NewRenderState( )
					.SetPolygonRenderingMode( PolygonRenderMode.kLines )
					.DisableCap( RenderStateFlag.kCullFrontFaces )
					.DisableCap( RenderStateFlag.kCullBackFaces )
					.SetDepthOffset( -1.0f )
			);

			RenderPass filledRenderPass = new RenderPass( );
			filledRenderPass.Add
			(
				RenderFactory.Inst.NewRenderState( )
					.SetColour( System.Drawing.Color.Gainsboro )
				//	.EnableLighting( )
				//	.SetLight( 0, true )
			);
            filledRenderPass.Add
			(
				RenderFactory.Inst.NewMaterial( ).Setup( System.Drawing.Color.DarkGray, System.Drawing.Color.LightCyan )
			);

			RenderTechnique technique = new RenderTechnique( "default" );
			technique.Add( filledRenderPass );
			technique.Add( gridRenderPass );

			m_Effect = new RenderEffect( technique );

			m_Technique = new SelectedTechnique( m_Effect );
			m_Technique.RenderCallback = new RenderTechnique.RenderDelegate( RenderPlane );
		}

		/// <summary>
		/// Height of the ground plane (Z)
		/// </summary>
		public override float Height
		{
			set
			{
				base.Height = value;
				DestroyCallList( );
			}
		}

		/// <summary>
		/// Width of the ground plane (X)
		/// </summary>
		public override float Width
		{
			set
			{
				base.Width = value;
				DestroyCallList( );
			}
		}

		/// <summary>
		/// Renders the ground plane
		/// </summary>
		public override void Render( )
		{
			if ( m_CallList == -1 )
			{
				MakeCallList( );
			}

			m_Technique.Apply( );
		}

		private void RenderPlane( )
		{
			Gl.glCallList( m_CallList );
		}

		#region	Private stuff

		private int					m_CallList	= -1;
		private RenderEffect		m_Effect;
		private SelectedTechnique	m_Technique;

		/// <summary>
		/// Destroys the call list that renders the ground plane
		/// </summary>
		private void DestroyCallList( )
		{
			if ( m_CallList != -1 )
			{
				//	Warning: GroundPlane must be Dispose()d before GL shutdown
				Gl.glDeleteLists( m_CallList, 1 );
				m_CallList = -1;
			}
		}

		/// <summary>
		/// Makes a call list that renders the ground plane
		/// </summary>
		private void MakeCallList( )
		{
			m_CallList = Gl.glGenLists( 1 );
			Gl.glNewList( m_CallList, Gl.GL_COMPILE );

			Gl.glBegin( Gl.GL_QUADS );
			GenerateVertices( Width, Height );
			Gl.glEnd( );

			Gl.glEndList( );
		}

		/// <summary>
		/// Generates vertices for a grid of quads on the ground plane, centered on the origin
		/// </summary>
		/// <param name="width"> Width of the area covered by the quads (Width property) </param>
		/// <param name="height"> Height of the area covered by the quads (Height property) </param>
		private void GenerateVertices( float width, float height )
		{
			int			numRows		= ( int )width / 10;
			int			numCols		= ( int )height / 10;
			float		xIncrement	= ( float )width / ( float )numCols;
			float		zIncrement	= ( float )height / ( float )numRows;

			float		y			= 0.0f;
			float		z			= -height / 2;
			float		nextZ		= z + zIncrement;

			for ( int row = 0; row < numRows; ++row )
			{
				float	x			= -width / 2;
				float	nextX		= x + xIncrement;

				for ( int col = 0; col < numCols; ++col )
				{
					Gl.glNormal3f( 0, 1, 0 );
					Gl.glVertex3f( x, y, z );
					Gl.glNormal3f( 0, 1, 0 );
					Gl.glVertex3f( x, y, nextZ );
					Gl.glNormal3f( 0, 1, 0 );
					Gl.glVertex3f( nextX, y, nextZ );
					Gl.glNormal3f( 0, 1, 0 );
					Gl.glVertex3f( nextX, y, z );

					x = nextX;
					nextX += xIncrement;
				}
				z = nextZ;
				nextZ += zIncrement;
			}
		}

		#endregion

		#region IDisposable Members

		public void Dispose()
		{
			DestroyCallList( );
		}

		#endregion
	}
}
