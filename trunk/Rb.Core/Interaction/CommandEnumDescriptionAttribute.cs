using System;

namespace Rb.Core.Interaction
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
	///		[ Interaction.CommandEnumDescription( "Self Destructs" ) ]
	///		[ Interaction.CommandEnumInputInterpreter( typeof( MyCommandInputInterpreter ) ) ]
	///		SelfDestruct
	/// }
	/// </example>
	public class CommandEnumDescriptionAttribute : Attribute
	{
		/// <summary>
		/// Stores the description
		/// </summary>
		public CommandEnumDescriptionAttribute( string description )
		{
			m_Description = description;
		}

		/// <summary>
		/// Gets the command description
		/// </summary>
		public string	Description
		{
			get
			{
				return m_Description;
			}
		}

		private string m_Description;
	}
}