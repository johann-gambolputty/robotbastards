using System;
using System.ComponentModel;
using System.Drawing.Design;
using Rb.Core.Utils;

namespace Rb.Core.Maths
{
	/// <summary>
	/// Function. Rename!
	/// </summary>
	[Editor( typeof( CustomUITypeEditor<IFunction1d> ), typeof( UITypeEditor ) )]
	public interface IFunction1d
	{
		/// <summary>
		/// Event, invoked when the parameters of this function are changed
		/// </summary>
		event Action<IFunction1d> ParametersChanged;

		/// <summary>
		/// Gets a value for this function at a specified point
		/// </summary>
		float GetValue( float t );
	}
}
