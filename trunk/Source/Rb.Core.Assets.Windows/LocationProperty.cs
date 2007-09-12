using System.Collections.Generic;

namespace Rb.Core.Assets.Windows
{
	public class LocationProperty : IComparer< LocationTreeNode >
	{
		public delegate int CompareDelegate( LocationTreeNode x, LocationTreeNode y );

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
			return m_Compare( x, y );
		}

		#endregion
	}
}
