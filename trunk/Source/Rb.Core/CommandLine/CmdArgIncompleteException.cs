using System;

namespace Rb.Core.CommandLine
{
	/// <summary>
	/// Exception thrown by <see cref="CommandLineParser.Build(object,string[])"/> when the flag for an argument
	/// is found in the command argument list, but it has no following value
	/// </summary>
	public class CmdArgIncompleteException : Exception
	{
		/// <summary>
		/// Setup constructor
		/// </summary>
		/// <param name="argumentName">Argument name</param>
		public CmdArgIncompleteException(string argumentName)
		{
			m_ArgumentName = argumentName;
		}

		/// <summary>
		/// Gets the argument name
		/// </summary>
		public string ArgumentName
		{
			get { return m_ArgumentName; }
		}

		#region Private Members

		private readonly string m_ArgumentName;

		#endregion
	}
}
