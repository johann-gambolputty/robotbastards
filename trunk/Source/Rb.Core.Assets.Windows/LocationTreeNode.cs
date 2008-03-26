using System.Collections.Generic;
using Rb.Assets.Interfaces;

namespace Rb.Core.Assets.Windows
{
	public class LocationTreeNode
	{
		public LocationTreeNode( LocationTreeFolder parent, ISource source, int image, int selectedImage ) :
			this( parent, source, GetSourceName( source ), image, selectedImage )
		{
		}

		public LocationTreeNode( LocationTreeFolder parent, ISource source, string name, int image, int selectedImage )
		{
			m_Parent = parent;
			m_Source = source;
			m_Name = name;
			m_Image = image;
			m_SelectedImage = selectedImage;
		}

		public override string ToString( )
		{
			return Name;
		}

		public object Tag
		{
			get { return m_Tag; }
			set { m_Tag = value; }
		}

		public string Name
		{
			get { return m_Name; }
		}

		public string Path
		{
			get { return m_Source.ToString( ); }
		}

		public int Image
		{
			get { return m_Image; }
		}

		public int SelectedImage
		{
			get { return m_SelectedImage; }
		}

		public LocationTreeFolder Parent
		{
			get { return m_Parent; }
		}

		public ISource Source
		{
			get { return m_Source; }
		}

		public bool HasProperty( LocationProperty property )
		{
			return m_Properties.ContainsKey( property );
		}

		public object this[ LocationProperty property ]
		{
			get
			{
				object result;
				return m_Properties.TryGetValue( property, out result ) ? result : null;
			}
			set
			{
				if ( !m_Properties.ContainsKey( property ) )
				{
					m_Properties.Add( property, value );
				}
				else
				{
					m_Properties[ property ] = value;
				}
			}
		}


		private readonly int m_Image;
		private readonly int m_SelectedImage;
		private readonly LocationTreeFolder m_Parent;
		private readonly ISource m_Source;
		private readonly string m_Name;
		private object m_Tag;
		private readonly Dictionary< LocationProperty, object > m_Properties = new Dictionary< LocationProperty, object >( );
		
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
