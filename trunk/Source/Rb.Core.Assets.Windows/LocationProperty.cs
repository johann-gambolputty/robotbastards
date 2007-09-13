using System.Collections.Generic;

namespace Rb.Core.Assets.Windows
{
	public class LocationProperty : IComparer< LocationTreeNode >
	{
		public delegate int CompareDelegate( object x, object y );

		public LocationProperty( string name, CompareDelegate compare, int size )
		{
			m_Name = name;
			m_Compare = compare;
			m_Size = size;
		}
		
		public int DefaultSize
		{
			get { return m_Size; }
		}

		public string Name
		{
			get { return m_Name; }
		}

		private readonly string m_Name;
		private readonly CompareDelegate m_Compare;
		private readonly int m_Size;

		#region IComparer<LocationTreeNode> Members

		public int Compare( LocationTreeNode x, LocationTreeNode y )
		{
			bool xIsFolder = x is LocationTreeFolder;
			bool yIsFolder = y is LocationTreeFolder;

			if ( xIsFolder && !yIsFolder )
			{
				return 1;
			}
			if ( !xIsFolder && yIsFolder )
			{
				return -1;
			}

			object xValue = x[ this ];
			object yValue = y[ this ];

			if ( xValue == null )
			{
				return ( yValue == null ) ? 0 : -1;
			}

			return yValue == null ? 1 : m_Compare( xValue, yValue );
		}

		#endregion

	}
}
