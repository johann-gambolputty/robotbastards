
using Rb.Rendering.Textures;

namespace Rb.Rendering
{

	/// <summary>
	/// A ShaderParameter represents a parameter in an effect
	/// </summary>
	public abstract class ShaderParameter
    {
        #region Construction

        /// <summary>
        /// Sets the source of this parameter
        /// </summary>
        public ShaderParameter( IEffect source )
        {
            m_Source = source;
        }

        #endregion

        #region Public properties

        /// <summary>
		/// Gets the shader that this parameter came from
		/// </summary>
		public IEffect	Source
		{
			get { return m_Source; }
        }

        #endregion

        #region Parameter setters

        /// <summary>
		/// Sets the shader parameter to a texture
		/// </summary>
		/// <param name="val">New parameter value</param>
		/// <remarks>
		/// Assumes that this parameter is a texture sampler of some description. Assumes that the texture
		/// is already set up in the renderer
		/// </remarks>
		public abstract void	Set( ITexture2d val );

		/// <summary>
		/// Sets the shader parameter to a single float value
		/// </summary>
		/// <param name="val"> New parameter value </param>
		public abstract void	Set( float val );

		/// <summary>
		/// Sets the shader parameter to a single integer value
		/// </summary>
		/// <param name="val"> New parameter value </param>
		public abstract void	Set( int val );

		/// <summary>
		/// Sets the shader parameter to an (x,y) float vector
		/// </summary>
		/// <param name="x"> Vector x component </param>
		/// <param name="y"> Vector y component </param>
		public abstract void	Set( float x, float y );

		/// <summary>
		/// Sets the shader parameter to an (x,y) integer vector
		/// </summary>
		/// <param name="x"> Vector x component </param>
		/// <param name="y"> Vector y component </param>
		public abstract void	Set( int x, int y );

		/// <summary>
		/// Sets the shader parameter to an (x,y,z) float vector
		/// </summary>
		/// <param name="x"> Vector x component </param>
		/// <param name="y"> Vector y component </param>
		/// <param name="z"> Vector z component </param>
		public abstract void	Set( float x, float y, float z );

		/// <summary>
		/// Sets the shader parameter to an (x,y,z) integer vector
		/// </summary>
		/// <param name="x"> Vector x component </param>
		/// <param name="y"> Vector y component </param>
		/// <param name="z"> Vector z component </param>
		public abstract void	Set( int x, int y, int z );

		/// <summary>
		/// Sets the shader parameter to a float array
		/// </summary>
		/// <param name="val"> New parameter value </param>
		public abstract void	Set( float[] val );

		/// <summary>
		/// Sets the shader parameter to an integer array
		/// </summary>
		/// <param name="val"> New parameter value </param>
		public abstract void	Set( int[] val );

        #endregion

        #region Private stuff

        private IEffect			m_Source;

        #endregion
    }
}
