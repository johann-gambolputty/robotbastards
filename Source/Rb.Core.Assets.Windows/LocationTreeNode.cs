using Rb.Core.Components;

namespace Rb.Core.Assets.Windows
{
	public class LocationTreeNode : ISupportsDynamicProperties
	{
		public LocationTreeNode( LocationTreeFolder parent, ISource source ) :
			this( parent, source, GetSourceName( source ) )
		{
		}

		public LocationTreeNode( LocationTreeFolder parent, ISource source, string name )
		{
			m_Parent = parent;
			m_Source = source;
			m_Name = name;

			m_Parent.Add( this );
		}

		public string Name
		{
			get { return m_Name; }
		}

		public string Path
		{
			get { return m_Source.ToString( ); }
		}

		public LocationTreeFolder Parent
		{
			get { return m_Parent; }
		}

		public ISource Source
		{
			get { return m_Source; }
		}

		#region ISupportsDynamicProperties Members

		public IDynamicProperties Properties
		{
			get { return m_Properties; }
		}

		#endregion

		private readonly LocationTreeFolder m_Parent;
		private readonly IDynamicProperties m_Properties = new DynamicProperties( );
		private readonly ISource m_Source;
		private readonly string m_Name;
		
		private static string GetSourceName( ISource source )
		{
			string path = source.ToString( );

			char lastChar = path[ path.Length - 1 ];
			if ( ( lastChar == '/' ) || ( lastChar == '\\' ) )
			{
				path = path.Remove( path.Length - 1 );
			}

			return path.Substring( path.LastIndexOfAny( new char[] { '\\', '/' } ) + 1 );
		}
	}

}
