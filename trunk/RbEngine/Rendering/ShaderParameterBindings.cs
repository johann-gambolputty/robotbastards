using System;
using RbEngine.Maths;

namespace RbEngine.Rendering
{
	/// <summary>
	/// Default bindings for shader parameters
	/// </summary>
	public enum ShaderParameterDefaultBinding
	{
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
		/// Total number of default bindings
		/// </summary>
		NumDefaults
	}

	/// <summary>
	/// Shader binding
	/// </summary>
	public abstract class ShaderParameterBinding
	{
		/*
		#region	Default parameter names

		/// <summary>
		/// Gets the name of a default binding parameter
		/// </summary>
		public static string	GetDefaultBindingParameterName( Default binding )
		{
			return ms_DefaultParameterNames[ ( int )binding ];
		}

		/// <summary>
		/// Returns the parameter binding from a parameter name
		/// </summary>
		/// <param name="name">Parameter name</param>
		/// <returns>The binding implied by the parameter name, or ShaderParameterBinding.NumBindings if no binding is implied</returns>
		public static Default	GetDefaultBindingFromParameterName( string name )
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
			names[ ( int )ShaderParameterBinding.ShadowMatrix ]						= "ShadowMatrix";
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
		*/
		#region	Shader parameter binding

		/// <summary>
		/// Adds a parameter to this binding
		/// </summary>
		public abstract void	Bind( ShaderParameter parameter );

		/// <summary>
		/// Removes a parameter from this binding
		/// </summary>
		public abstract void	Unbind( ShaderParameter parameter );

		/// <summary>
		/// Applies this binding to a particular shader parameter
		/// </summary>
		/// <remarks>
		/// Shouldn't be necessary to do this - just Bind() and Unbind()
		/// </remarks>
		public abstract void	ApplyTo( ShaderParameter parameter );

		#endregion

	}

	/// <summary>
	/// An application-defined shader parameter binding
	/// </summary>
	public abstract class ShaderParameterCustomBinding : ShaderParameterBinding
	{
		#region Binding value type

		/// <summary>
		/// Binding value type
		/// </summary>
		public enum ValueType
		{
			Int,
			Float,
			Vector2,
			Vector3,
			Matrix
		}

		#endregion

		#region	Single value setters

		/// <summary>
		/// Sets the value of this binding to an integer
		/// </summary>
		public abstract void Set( int val );

		/// <summary>
		/// Sets the value of this binding to a floating point value
		/// </summary>
		public abstract void Set( float val );

		/// <summary>
		/// Sets the value of this binding to a 2d vector
		/// </summary>
		public abstract void Set( Vector2 val );

		/// <summary>
		/// Sets the value of this binding to a 3d point
		/// </summary>
		public abstract void Set( Point3 val );

		/// <summary>
		/// Sets the value of this binding to a 3d vector
		/// </summary>
		public abstract void Set( Vector3 val );

		/// <summary>
		/// Sets the value of this binding to a matrix
		/// </summary>
		public abstract void Set( Matrix44 val );

		#endregion

		#region	Array value setters

		/// <summary>
		/// Sets the value at the specified index to an integer
		/// </summary>
		public abstract void SetAt( int index, int val );
		
		/// <summary>
		/// Sets the value at the specified index to a floating point value
		/// </summary>
		public abstract void SetAt( int index, float val );
		
		/// <summary>
		/// Sets the value at the specified index to a 2d vector
		/// </summary>
		public abstract void SetAt( int index, Vector2 val );
		
		/// <summary>
		/// Sets the value at the specified index to a 3d point
		/// </summary>
		public abstract void SetAt( int index, Point3 val );
		
		/// <summary>
		/// Sets the value at the specified index to a 3d vector
		/// </summary>
		public abstract void SetAt( int index, Vector3 val );

		#endregion
	}

	/// <summary>
	/// Stores a set of shader parameter bindings
	/// </summary>
	public abstract class ShaderParameterBindings
	{
		/// <summary>
		/// Singleton
		/// </summary>
		public static ShaderParameterBindings			Inst
		{
			get
			{
				return ms_Singleton;
			}
		}

		/// <summary>
		/// Delegate type used by AddNewBindingListener
		/// </summary>
		public delegate void							NewBindingDelegate( ShaderParameterBinding binding );


		/// <summary>
		/// Adds a delegate to an event that gets called when CreateBinding(), CreateArrayBinding() is called. Also, called for all existing default and custom bindings
		/// </summary>
		public abstract void							AddNewBindingListener( NewBindingDelegate newBinding );

		/// <summary>
		/// Creates a binding
		/// </summary>
		/// <returns></returns>
		public abstract ShaderParameterCustomBinding	CreateBinding( string name, ValueType type );

		/// <summary>
		/// Creates a binding to an array
		/// </summary>
		public abstract ShaderParameterCustomBinding	CreateBinding( string name, ValueType type, int arraySize );

		/// <summary>
		/// Gets a default binding
		/// </summary>
		public abstract ShaderParameterBinding			GetBinding( ShaderParameterDefaultBinding binding );

		/// <summary>
		///	Finds a binding by its name
		/// </summary>
		public abstract ShaderParameterBinding			GetBinding( string name );


		#region	Singleton setup

		/// <summary>
		/// Sets up the singleton
		/// </summary>
		protected ShaderParameterBindings( )
		{
			ms_Singleton = this;
		}

		/// <summary>
		/// Singleton instance
		/// </summary>
		private static ShaderParameterBindings			ms_Singleton;

		#endregion

	}
}
