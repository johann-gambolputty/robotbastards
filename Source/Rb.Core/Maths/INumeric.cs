
namespace Rb.Core.Maths
{
	/// <summary>
	/// Interface used for classes with the characteristics of real numbers
	/// </summary>
	/// <remarks>
	/// Generics don't support a "numeric" constraint, so can't infer that type parameters
	/// can be added, subtracted, etc.
	/// </remarks>
	public interface INumeric<T>
	{
		/// <summary>
		/// Adds this value to another and returns the result
		/// </summary>
		T Add( T addend );

		/// <summary>
		/// Subtracts another value from this and returns the result
		/// </summary>
		T Subtract( T subtrahend );

		/// <summary>
		/// Divides this value by another and returns the result
		/// </summary>
		T Divide( double divisor );

		/// <summary>
		/// Multiplies this value by another and returns the result
		/// </summary>
		T Multiply( double multiplier );
	}
}
