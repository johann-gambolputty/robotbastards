using System.Collections.Generic;
using Rb.Rendering.Interfaces.Objects;

namespace Rb.Rendering
{
	/// <summary>
	/// Stores a collection of <see cref="IPass"/> objects
	/// </summary>
	public class PassList : List< IPass >, IPass
	{
		#region IPass Members

		/// <summary>
		/// Calls <see cref="IPass.Begin"/> for all stored passes
		/// </summary>
		public void Begin( )
		{
			foreach ( IPass pass in this )
			{
				pass.Begin( );
			}
		}

		/// <summary>
		/// Calls <see cref="IPass.End"/> for all stored passes in reverse order
		/// </summary>
		public void End( )
		{
			for ( int index = Count - 1; index >= 0; --index )
			{
				this[ index ].End( );
			}
		}

		#endregion

	}
}
