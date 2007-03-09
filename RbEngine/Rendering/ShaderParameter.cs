using System;

namespace RbEngine.Rendering
{

	/// <summary>
	/// Binding options for a shader parameter
	/// </summary>
	/// <remarks>
	/// When adding values to this enum, update ShaderParameter.CreateBindingParameterNames()
	/// </remarks>
	public enum ShaderParameterBinding
	{
		/// <summary>
		/// Unbound parameter
		/// </summary>
		NoBinding,

		/// <summary>
		/// Parameter bound to the current model matrix
		/// </summary>
		ModelMatrix,

		/// <summary>
		/// Parameter bound to the current model matrix
		/// </summary>
		InverseTransposeModelMatrix,

		/// <summary>
		/// Parameter bound to the current view matrix
		/// </summary>
		ViewMatrix,

		/// <summary>
		/// Parameter bound to the current projection matrix
		/// </summary>
		ProjectionMatrix,

		/// <summary>
		/// Parameter bound to current texture matrix
		/// </summary>
		TextureMatrix,

		/// <summary>
		/// Parameter bound to current model view matrix
		/// </summary>
		ModelViewMatrix,

		/// <summary>
		/// Parameter bound to the inverse of the current model view matrix
		/// </summary>
		InverseModelViewMatrix,

		/// <summary>
		/// Parameter bound to the inverse transpose of the current model view matrix
		/// </summary>
		InverseTransposeModelViewMatrix,
		
		/// <summary>
		/// Parameter bound to current model view projection matrix
		/// </summary>
		ModelViewProjectionMatrix,
		
		/// <summary>
		/// Parameter bound to current eye position (world space)
		/// </summary>
		EyePosition,
		
		/// <summary>
		/// Parameter bound to current eye x axis (world space)
		/// </summary>
		EyeXAxis,
		
		/// <summary>
		/// Parameter bound to current eye y axis(world space)
		/// </summary>
		EyeYAxis,
		
		/// <summary>
		/// Parameter bound to current eye z axis (world space)
		/// </summary>
		EyeZAxis,

		//	TODO: This is very specific - requires a certain data structure to be present in the shader... 
		/// <summary>
		/// Data about the active point lights in the renderer
		/// </summary>
		/// <remarks>
		/// Binds to a struct like this:
		/// <code>
		/// struct PointLights
		/// {
		///    int m_NumLights;
		///    float4 m_Positions[ 4 ];
		/// }
		/// </code>
		/// </remarks>
		PointLights,


		//	TODO: This is very specific - requires a certain data structure to be present in the shader... 
		/// <summary>
		/// Data about the active spotlights in the renderer
		/// </summary>
		/// <remarks>
		/// Binds to a struct like this:
		/// <code>
		/// struct SpotLights
		/// {
		///    int m_NumLights;
		///    float4 m_Positions[ 4 ];
		///    float4 m_Directions[ 4 ];
		///    float  m_ArcRadians[ 4 ];
		/// }
		/// </code>
		/// </remarks>
		SpotLights,


		/// <summary>
		/// Binds the parameter to the first texture in the renderer. Parameter must be a texture sampler
		/// </summary>
		Texture0,

		/// <summary>
		/// Total number of bindings
		/// </summary>
		NumBindings
	}

	/// <summary>
	/// Summary description for ShaderParameter.
	/// </summary>
	public abstract class ShaderParameter
	{
		#region	Binding parameter names

		/// <summary>
		/// Gets the name of a bound parameter name
		/// </summary>
		public static string	GetBindingParameterName( ShaderParameterBinding param )
		{
			return ms_BindingParameterNames[ ( int )param ];
		}

		/// <summary>
		/// Returns the parameter binding from a parameter name
		/// </summary>
		/// <param name="name">Parameter name</param>
		/// <returns>The binding implied by the parameter name, or ShaderParameterBinding.NumBindings if no binding is implied</returns>
		public static ShaderParameterBinding	GetBindingFromParameterName( string name )
		{
			for ( int nameIndex = 0; nameIndex < ( int )ShaderParameterBinding.NumBindings; ++nameIndex )
			{
				if ( string.Compare( ms_BindingParameterNames[ nameIndex ], name, true ) == 0 )
				{
					return ( ShaderParameterBinding )nameIndex;
				}
			}
			return ShaderParameterBinding.NumBindings;
		}

		/// <summary>
		/// Creates binding parameter names
		/// </summary>
		/// <returns>Array of parameter names that get bound to various render state values</returns>
		private static string[]	CreateBindingParameterNames( )
		{
			string[] names = new string[ ( int )ShaderParameterBinding.NumBindings ];

			names[ ( int )ShaderParameterBinding.NoBinding ]						= "";
			names[ ( int )ShaderParameterBinding.ModelMatrix ]						= "ModelMatrix";
			names[ ( int )ShaderParameterBinding.InverseTransposeModelMatrix ]		= "InverseTransposeModelMatrix";
			names[ ( int )ShaderParameterBinding.ViewMatrix ]						= "ViewMatrix";
			names[ ( int )ShaderParameterBinding.ProjectionMatrix ]					= "ProjectionMatrix";
			names[ ( int )ShaderParameterBinding.TextureMatrix ]					= "TextureMatrix";
			names[ ( int )ShaderParameterBinding.ModelViewMatrix ]					= "ModelViewMatrix";
			names[ ( int )ShaderParameterBinding.InverseModelViewMatrix ]			= "InverseModelViewMatrix";
			names[ ( int )ShaderParameterBinding.InverseTransposeModelViewMatrix ]	= "InverseTransposeModelViewMatrix";
			names[ ( int )ShaderParameterBinding.ModelViewProjectionMatrix ]		= "ModelViewProjectionMatrix";
			names[ ( int )ShaderParameterBinding.EyePosition ]						= "EyePosition";
			names[ ( int )ShaderParameterBinding.EyeXAxis ] 						= "EyeXAxis";
			names[ ( int )ShaderParameterBinding.EyeYAxis ] 						= "EyeYAxis";
			names[ ( int )ShaderParameterBinding.EyeZAxis ] 						= "EyeZAxis";
			names[ ( int )ShaderParameterBinding.PointLights ]						= "PointLights";
			names[ ( int )ShaderParameterBinding.SpotLights ]						= "SpotLights";
			names[ ( int )ShaderParameterBinding.Texture0 ]							= "Texture0";

			//	Make sure that all names are present and correct
			for ( int nameIndex = 0; nameIndex < ( int )ShaderParameterBinding.NumBindings; ++nameIndex )
			{
				if ( names[ nameIndex ] == null )
				{
					throw new ApplicationException( string.Format( "Missing parameter name for shader binding \"{0}\"", ( ( ShaderParameterBinding )nameIndex ).ToString( ) ) );
				}
			}

			return names;
		}

		private static string[]	ms_BindingParameterNames = CreateBindingParameterNames( );

		#endregion



		/// <summary>
		/// Sets the shader parameter to a texture
		/// </summary>
		/// <param name="val">New parameter value</param>
		/// <remarks>
		/// Assumes that this parameter is a texture sampler of some description
		/// </remarks>
		public abstract void	Set( Texture2d val );

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
	}
}
