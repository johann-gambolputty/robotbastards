using System;
using System.ComponentModel;

namespace Poc0.LevelEditor.Core.Geometry
{
	/// <summary>
	/// Floor information
	/// </summary>
	[Serializable]
	public class FloorProperties : GeometryProperties
	{
		/// <summary>
		/// Gets the default floor data object
		/// </summary>
		[Browsable( false )]
		public static FloorProperties Default
		{
			get { return ms_Default; }
		}

		#region Private members

		private readonly static FloorProperties ms_Default;

		static FloorProperties( )
		{
			ms_Default = new FloorProperties( );
		}

		#endregion
	}
}
