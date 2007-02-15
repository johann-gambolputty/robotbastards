using System;
using RbEngine.Rendering;
using Tao.OpenGl;

namespace RbOpenGlRendering
{
	/// <summary>
	/// Summary description for OpenGlMaterial.
	/// </summary>
	public class OpenGlMaterial : Material
	{
		/// <summary>
		/// Applies this material
		/// </summary>
		public override void	Apply( )
		{
			Gl.glMaterialfv( Gl.GL_FRONT_AND_BACK, Gl.GL_AMBIENT,	m_Ambient );
			Gl.glMaterialfv( Gl.GL_FRONT_AND_BACK, Gl.GL_DIFFUSE,	m_Diffuse );
			Gl.glMaterialfv( Gl.GL_FRONT_AND_BACK, Gl.GL_SPECULAR,	m_Specular );
		}
	}
}
