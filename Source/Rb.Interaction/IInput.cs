
namespace Rb.Interaction
{
    /// <summary>
    /// Input interface for commands
    /// </summary>
    public interface IInput
    {
        /// <summary>
        /// Gets the context that this input was created for
        /// </summary>
        InputContext Context
        {
            get;
        }

        /// <summary>
        /// True if the input is active
        /// </summary>
        bool IsActive
        {
            get;
            set;
        }

        /// <summary>
        /// Flag that determines if the input should be disabled (IsActive = false) after each update
        /// </summary>
        bool DeactivateOnUpdate
        {
            get;
        }

        /// <summary>
        /// Creates a new command message
        /// </summary>
        CommandMessage CreateCommandMessage( Command cmd );
    }
}
