
using Rb.Interaction.Classes;

namespace Bob.Core.Commands
{
	/// <summary>
	/// Workspace command group
	/// </summary>
	/// <remarks>
	/// Adds an ordinal to the group, to determine in what order a set of groups are
	/// displayed (e.g. in MenuCommandUiManager).
	/// </remarks>
	public class WorkspaceCommandGroup : CommandGroup
	{
		/// <summary>
		/// Setting the command group's ordinal to this value will make it
		/// </summary>
		public const int LastOrdinal = int.MaxValue;

		/// <summary>
		/// Setup constructor. Sets the registry used to keep track of commands added to this group
		/// </summary>
		public WorkspaceCommandGroup( string commandGroupName, string commandGroupLocName, CommandRegistry registry ) :
			base( commandGroupName, commandGroupLocName, registry )
		{
			m_Ordinal = LastOrdinal;
		}
		
		/// <summary>
		/// Setup constructor. Sets the registry used to keep track of commands added to this group
		/// </summary>
		public WorkspaceCommandGroup( int ordinal, string commandGroupName, string commandGroupLocName, CommandRegistry registry ) :
			base( commandGroupName, commandGroupLocName, registry )
		{
			m_Ordinal = ordinal;
		}

		/// <summary>
		/// Gets/sets the ordering of this command group
		/// </summary>
		public int Ordinal
		{
			get { return m_Ordinal; }
			set { m_Ordinal = value; }
		}

		#region Private Members

		private int m_Ordinal; 

		#endregion
	}
}
