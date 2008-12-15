using System;
using Rb.Core.Utils;

namespace Rb.Interaction.Classes
{
	/// <summary>
	/// Command class
	/// </summary>
	public class Command
	{
		/// <summary>
		/// Event raised when the command is triggered by a specified user
		/// </summary>
		public event Action<CommandTriggerData> CommandTriggered;

		/// <summary>
		/// Sets up the command
		/// </summary>
		/// <param name="uniqueName">Globally unique name of the command. Used to generate the command identifier</param>
		/// <param name="locName">Localised command name</param>
		/// <param name="locDescription">Localised command description</param>
		public Command( string uniqueName, string locName, string locDescription )
		{
			m_NameId = uniqueName;
			m_Name = locName;
			m_Description = locDescription;
			m_Id = m_NameId.GetHashCode( );
		}

		/// <summary>
		/// Gets the identifying name of the command
		/// </summary>
		public string NameId
		{
			get { return m_NameId; }
		}

		/// <summary>
		/// Gets the localised name of this command
		/// </summary>
		public string NameUi
		{
			get { return m_Name; }
		}

		/// <summary>
		/// Gets the description of the command
		/// </summary>
		public string DescriptionUi
		{
			get { return m_Description; }
		}

		/// <summary>
		/// Gets the identifier of this command
		/// </summary>
		public int Id
		{
			get { return m_Id; }
		}

		/// <summary>
		/// Returns the unique name of this command
		/// </summary>
		public override string ToString( )
		{
			return NameUi;
		}

		/// <summary>
		/// Triggers the command (raises the command triggered event)
		/// </summary>
		public virtual void Trigger( CommandTriggerData triggerData )
		{
			Arguments.CheckNotNull( triggerData, "triggerData" );
			if ( CommandTriggered != null )
			{
				CommandTriggered( triggerData );
			}
		}

		#region Private Members

		private readonly int m_Id;
		private readonly string m_NameId;
		private readonly string m_Name;
		private readonly string m_Description;

		#endregion
	}
}
