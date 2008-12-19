using System.Drawing;
using Rb.Interaction.Classes;

namespace Bob.Core.Projects
{
	/// <summary>
	/// Project command
	/// </summary>
	public class ProjectCommand : Command
	{
		/// <summary>
		/// Sets up the command
		/// </summary>
		/// <param name="icon">Command icon (can be null)</param>
		/// <param name="uniqueName">Globally unique name of the command. Used to generate the command identifier</param>
		/// <param name="locName">Localised command name. Includes menu hotkey qualifer ('&')</param>
		/// <param name="locDescription">Localised command description</param>
		public ProjectCommand( Icon icon, string uniqueName, string locName, string locDescription ) :
			base( uniqueName, locName.Replace( "&", "" ), locDescription )
		{
			m_MenuName = locName;
			m_Icon = icon;
		}

		/// <summary>
		/// Gets/sets the command icon
		/// </summary>
		public Icon Icon
		{
			get { return m_Icon; }
			set { m_Icon = value; }
		}

		/// <summary>
		/// Gets the menu name
		/// </summary>
		public string MenuName
		{
			get { return m_MenuName; }
		}

		#region Private Members

		private readonly string m_MenuName;
		private Icon m_Icon;
		
		#endregion
	}
}
