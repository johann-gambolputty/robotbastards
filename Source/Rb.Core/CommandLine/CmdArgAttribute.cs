using System;

namespace Rb.Core.CommandLine
{
	/// <summary>
	/// Attribute used to mark a property in a command line class
	/// </summary>
	public class CmdArgAttribute : Attribute
	{
		/// <summary>
		/// Setup constructor for an argument whose value is specified by its order in the command list
		/// </summary>
		/// <param name="argumentIndex">Index of the argument in the command list</param>
		/// <remarks>
		/// Ordered arguments cannot be mixed with flagged arguments in the command list
		/// </remarks>
		public CmdArgAttribute(int argumentIndex)
		{
			m_Index = argumentIndex;
		}

		/// <summary>
		/// Setup constructor for an argument whose value is specified with a flag (e.g. "MyApp.exe -logfile c:\log.txt
		/// </summary>
		/// <param name="flagName">Name of the command line flag (without prefix)</param>
		/// <remarks>
		/// Flagged arguments cannot be mixed with ordered arguments in the command list
		/// </remarks>
		public CmdArgAttribute(string flagName)
		{
			m_FlagName = flagName;
		}

		/// <summary>
		/// Gets the name of the command line flag
		/// </summary>
		public string FlagName
		{
			get { return m_FlagName; }
		}

		/// <summary>
		/// Gets/sets the required argument flag
		/// </summary>
		public bool Required
		{
			get { return m_Required; }
			set { m_Required = value; }
		}

		/// <summary>
		/// Gets the index of the argument in the command list
		/// </summary>
		public int Index
		{
			get { return m_Index; }
		}

		/// <summary>
		/// Returns true if this argument is indexed
		/// </summary>
		public bool IsIndexed
		{
			get { return m_Index != -1; }
		}

		#region Private Members

		private bool m_Required;
		private readonly string m_FlagName;
		private readonly int m_Index = -1;

		#endregion
	}
}
