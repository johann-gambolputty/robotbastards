using System;

namespace Rb.Core.CommandLine
{
	/// <summary>
	/// Exception thrown by <see cref="CommandLineParser.Build(object,string[])"/> when a required argument
	/// is not found in the command argument list
	/// </summary>
	public class CmdArgNotFoundException : Exception
	{
		/// <summary>
		/// Setup constructor
		/// </summary>
		/// <param name="argumentName">Argument name</param>
		public CmdArgNotFoundException(string argumentName)
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
