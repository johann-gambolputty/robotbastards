using System;

namespace Rb.Core.Maths
{
	/// <summary>
	/// Base class for numerical integration
	/// </summary>
	public class Integral
	{
		/// <summary>
		/// Delegate type, passed to derived-class integration functions (e.g. GaussianQuadrature.Integrate)
		/// </summary>
		public delegate float FunctionDelegate( float input );
	}
}
