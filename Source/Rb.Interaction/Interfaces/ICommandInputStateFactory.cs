
namespace Rb.Interaction.Interfaces
{
	/// <summary>
	/// Factory used by input monitors (<see cref="ICommandInputBindingMonitor"/>) for creating input state
	/// </summary>
	public interface ICommandInputStateFactory
	{
		/// <summary>
		/// Creates a new input state
		/// </summary>
		ICommandInputState NewInputState( object context );

		/// <summary>
		/// Creates a new input state from an input parameterised by a single scalar value (e.g. mouse wheel)
		/// </summary>
		ICommandInputState NewScalarInputState( object context, float lastValue, float value );

		/// <summary>
		/// Creates a new input state from an input parameterised by a position value (e.g. mouse cursor)
		/// </summary>
		ICommandInputState NewPointInputState( object context, float lastX, float lastY, float x, float y );
	}

}
