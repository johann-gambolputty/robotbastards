
using System;

namespace Poc1.Tools.StarField
{
	/// <summary>
	/// Parameters used by <see cref="StarFieldBuilder"/>
	/// </summary>
	public class StarFieldBuildParameters
	{
		/// <summary>
		/// Gets/sets the width/height of the generated textures
		/// </summary>
		public int Resolution
		{
			get { return m_Resolution; }
			set
			{
				if ( value <= 0 )
				{
					throw new ArgumentException( string.Format( "Resolution cannot be less than or equal to zero ({0})", value ), "value" );
				}
				m_Resolution = value;
			}
		}

		/// <summary>
		/// Gets/sets the function radius
		/// </summary>
		public float FunctionRadius
		{
			get { return m_FunctionRadius; }
			set { m_FunctionRadius = value; }
		}

		#region Private Members

		private float m_FunctionRadius = 16;
		private int m_Resolution = 1024;

		#endregion
	}
}
