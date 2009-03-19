using System;

namespace Poc1.Core.Interfaces.Astronomical.Planets
{

	/// <summary>
	/// Event arguments passed by <see cref="IPlanetModel.ModelChanged"/>
	/// </summary>
	public class ModelChangedEventArgs : EventArgs
	{
		/// <summary>
		/// Gets a default empty arguments object
		/// </summary>
		public static new ModelChangedEventArgs Empty
		{
			get { return s_EmptyArgs; }
		}

		#region Private Members

		private static readonly ModelChangedEventArgs s_EmptyArgs = new ModelChangedEventArgs( );

		#endregion
	}
}
