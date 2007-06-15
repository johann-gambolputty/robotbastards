using Tao.OpenGl;

namespace Rb.Rendering.OpenGl
{
	/// <summary>
	/// Summary description for OpenGlMaterial.
	/// </summary>
	public class OpenGlMaterial : Material
	{
		/// <summary>
		/// Applies this material
		/// </summary>
		public override void	Begin( )
		{
			Gl.glMaterialfv( Gl.GL_FRONT_AND_BACK, Gl.GL_AMBIENT,	m_Ambient );
			Gl.glMaterialfv( Gl.GL_FRONT_AND_BACK, Gl.GL_DIFFUSE,	m_Diffuse );
			Gl.glMaterialfv( Gl.GL_FRONT_AND_BACK, Gl.GL_SPECULAR,	m_Specular );
		}

		/// <summary>
		/// Stops applying this material
		/// </summary>
		public override void End( )
		{
		}
	}
}
