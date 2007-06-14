using Rb.Core.Components;

namespace Rb.World
{
    /// <summary>
    /// Host types
    /// </summary>
    public enum HostType
    {
        Local,
        Client,
        Server
    }

    /// <summary>
    /// Scene host interface
    /// </summary>
    public interface IHost : IUnique
    {
        /// <summary>
        /// Gets the type of this host
        /// </summary>
        HostType HostType
        {
            get;
        }
    }
}
