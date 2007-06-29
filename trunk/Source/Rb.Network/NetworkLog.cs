
using Rb.Log;

namespace Rb.Network
{
    /// <summary>
    /// Network logging
    /// </summary>
    /// <example>
    /// NetworkLog.Error( "No {0}", "pie" );
    /// </example>
    public class NetworkLog : StaticTag< NetworkLog >
    {
        public override string TagName
        {
            get { return "Network"; }
        }

        /// <summary>
        /// Static log class for RUNT networking
        /// </summary>
        /// <example>
        /// NetworkLog.RuntLog.Info( "No tea" );
        /// </example>
        public class RuntLog : StaticTag< RuntLog >
        {
            public override Tag ParentTag
            {
                get { return NetworkLog.Tag; }
            }

            public override string TagName
            {
                get { return "Runt"; }
            }
        }
    }
}
