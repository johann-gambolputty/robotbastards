using System;

namespace Rb.Interaction
{
	/// <summary>
	/// Description attribute that can be attached to an enum value.
	/// </summary>
	/// <remarks>
	/// this should be attached to enum values that represent commands, to associate descriptions with those values.
	/// CommandList objects can be generated by passing an enum's type into CommandListManager.CreateFromEnum()
	/// </remarks>
	/// <example>
	/// public enum MyCommands
	/// {
	///		[ Interaction.CommandDescription( "Self Destruct", "Destructs self" ) ]
	///		SelfDestruct
	/// }
	/// </example>
	public class CommandDescriptionAttribute : Attribute
	{
		/// <summary>
		/// Stores the description
		/// </summary>
        public CommandDescriptionAttribute( string name, string description )
		{
		    m_Name = name;
			m_Description = description;
		}

		/// <summary>
		/// Gets the command description
		/// </summary>
		public string Description
		{
			get { return m_Description; }
		}

        /// <summary>
        /// Gets the command name
        /// </summary>
	    public string Name
	    {
	        get { return m_Name; }
	    }

	    private string m_Name;
		private string m_Description;
	}
}