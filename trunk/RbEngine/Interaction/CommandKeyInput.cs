using System;
using System.Windows.Forms;

namespace RbEngine.Interaction
{
	/// <summary>
	/// Input from keystrokes
	/// </summary>
	public class CommandKeyInput : CommandInput, Components.IXmlLoader
	{
		/// <summary>
		/// Setup constructor
		/// </summary>
		/// <param name="key">Key to check for</param>
		public CommandKeyInput( Keys key )
		{
			m_Key = key;
		}

		/// <summary>
		/// Default constructor. Assigns no key
		/// </summary>
		public CommandKeyInput( )
		{
			m_Key = Keys.None;
		}

		/// <summary>
		/// Creates a CommandKeyInputBinding associated with the specified view
		/// </summary>
		public override CommandInputBinding BindToView( Scene.SceneView view )
		{
			return new CommandKeyInputBinding( view, m_Key );
		}

		private Keys m_Key;

		#region IXmlLoader Members

		/// <summary>
		/// Parses the generating element of this object
		/// </summary>
		public void ParseGeneratingElement( System.Xml.XmlElement element )
		{
			m_Key = ( Keys )Enum.Parse( typeof( Keys ), element.GetAttribute( "key" ), true );
		}

		/// <summary>
		/// Parses an element in the definition of this object
		/// </summary>
		public bool ParseElement(System.Xml.XmlElement element)
		{
			return false;
		}

		#endregion
	}
}