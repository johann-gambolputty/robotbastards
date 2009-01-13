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
			m_QualifiedName = uniqueName;
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
			get { return m_QualifiedName; }
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
		/// Gets the group that owns this command
		/// </summary>
		public CommandGroup Group
		{
			get { return m_Group; }
			set
			{
				Arguments.CheckNotNull( value, "value" );
				if ( !value.Commands.Contains( this ) )
				{
					throw new InvalidOperationException( string.Format( "Command \"{0}\" was not contained by group \"{1}\" - can't set group as owner", NameId, value.NameId ) );
				}
				if ( m_Group != null )
				{
					throw new InvalidOperationException( string.Format( "Can't change group for command \"{0}\" (already set to group \"{1}\")", NameId, m_Group.NameId ) );
				}
				m_Group = value;
				m_QualifiedName = m_Group.NameId + '.' + m_NameId;
				m_Id = m_QualifiedName.GetHashCode( );
			}
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
			if ( triggerData.User != null )
			{
				triggerData.User.OnCommandTriggered( triggerData );
			}
		}

		#region Private Members

		private CommandGroup m_Group;
		private int m_Id;
		private string m_QualifiedName;
		private readonly string m_NameId;
		private readonly string m_Name;
		private readonly string m_Description;

		#endregion
	}
}
