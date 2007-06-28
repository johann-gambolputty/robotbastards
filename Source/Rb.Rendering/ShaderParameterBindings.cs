using System;
using Rb.Core.Maths;

namespace Rb.Rendering
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
        /// Binds the parameter to the second texture in the renderer. Parameter must be a texture sampler
        /// </summary>
        Texture1,

        /// <summary>
        /// Binds the parameter to the third texture in the renderer. Parameter must be a texture sampler
        /// </summary>
        Texture2,

        /// <summary>
        /// Binds the parameter to the fourth texture in the renderer. Parameter must be a texture sampler
        /// </summary>
        Texture3,

        /// <summary>
        /// Binds the parameter to the fifth texture in the renderer. Parameter must be a texture sampler
        /// </summary>
        Texture4,

        /// <summary>
        /// Binds the parameter to the sixth texture in the renderer. Parameter must be a texture sampler
        /// </summary>
        Texture5,

        /// <summary>
        /// Binds the parameter to the seventh texture in the renderer. Parameter must be a texture sampler
        /// </summary>
        Texture6,

        /// <summary>
        /// Binds the parameter to the eighth texture in the renderer. Parameter must be a texture sampler
        /// </summary>
        Texture7,

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
		#region	Shader parameter binding

		/// <summary>
		/// Name of the binding
		/// </summary>
		public string			Name
		{
			get { return m_Name; }
		}

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

		/// <summary>
		/// Sets the name of this binding
		/// </summary>
		protected				ShaderParameterBinding( string name )
		{
			m_Name = name;
		}

		private string			m_Name;
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
			/// <summary>
			/// 32-bit integer
			/// </summary>
			Int32,

			/// <summary>
			/// 32-bit floating point
			/// </summary>
			Float32,

			/// <summary>
			/// 2 element 32-bit floating point vector
			/// </summary>
			Vector2,
			
			/// <summary>
			/// 3 element 32-bit floating point vector
			/// </summary>
			Vector3,
			
			/// <summary>
			/// 4x4 32-bit floating point matrix
			/// </summary>
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

        /// <summary>
        /// Sets the value at the specified index to a matrix
        /// </summary>
        public abstract void SetAt( int index, Matrix44 val );

		#endregion

		/// <summary>
		/// Sets the name of this binding
		/// </summary>
		protected ShaderParameterCustomBinding( string name ) :
			base( name )
		{
		}
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
		/// Event, called whenever a new binding is added to the set
		/// </summary>
		public event NewBindingDelegate					OnNewBinding;

		/// <summary>
		/// Invokes the OnNewBinding event
		/// </summary>
		protected void AddNewBinding( ShaderParameterBinding binding )
		{
			OnNewBinding( binding );
		}

		/// <summary>
		/// Creates a binding
		/// </summary>
		/// <returns></returns>
		public abstract ShaderParameterCustomBinding	CreateBinding( string name, ShaderParameterCustomBinding.ValueType type );

		/// <summary>
		/// Creates a binding to an array
		/// </summary>
		public abstract ShaderParameterCustomBinding	CreateBinding( string name, ShaderParameterCustomBinding.ValueType type, int arraySize );

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
