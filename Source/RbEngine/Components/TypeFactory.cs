using System;

namespace RbEngine.Components
{
	/// <summary>
	/// Simple implementation of ITypeFactory that uses reflection
	/// </summary>
	public class TypeFactory : ITypeFactory
	{
		#region ITypeFactory Members

		/// <summary>
		/// Creates a single instance of objectType
		/// </summary>
		public object Create( Type objectType )
		{
			return System.Activator.CreateInstance( objectType );
		}

		#endregion
	}
}
