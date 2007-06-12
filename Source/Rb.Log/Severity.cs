using System;
using System.Collections.Generic;
using System.Text;

namespace Rb.Log
{
    /// <summary>
    /// Severity levels for a source
    /// </summary>
    public enum Severity
    {
        /// <summary>
        /// Verbiage
        /// </summary>
        Verbose,

        /// <summary>
        /// Information
        /// </summary>
        Info,

        /// <summary>
        /// Warnings
        /// </summary>
        Warning,

        /// <summary>
        /// Errors
        /// </summary>
        Error,

		/// <summary>
		/// Total number of values
		/// </summary>
		/// <remarks>
		/// Must remain at the end of the enum
		/// </remarks>
		Count
    }
}
