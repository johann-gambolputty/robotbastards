using System;

namespace RbEngine.Interaction
{
	/// <summary>
	/// Description attribute that can be attached to an enum value.
	/// </summary>
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
