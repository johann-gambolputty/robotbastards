namespace Poc0.Core.Objects
{
	/// <summary>
	/// Enumerates body states
	/// </summary>
	public enum BodyState
	{
		Alive,
		Disabled,
		Dead
	}

	/// <summary>
	/// Delegate for <see cref="IBody.HealthChanged"/>
	/// </summary>
	public delegate void BodyHealthChangedDelegate( Body body, HealthChange change );

	/// <summary>
	/// Delegate for <see cref="IBody.StateChanged"/>
	/// </summary>
	public delegate void BodyStateChangedDelegate( Body body, BodyState oldState, BodyState newState );

	/// <summary>
	/// Body interface
	/// </summary>
	public interface IBody
	{
		#region State

		/// <summary>
		/// Current health
		/// </summary>
		int Health
		{
			get;
		}

		/// <summary>
		/// Gets the state of the body
		/// </summary>
		BodyState State
		{
			get;
			set;
		}

		#endregion

		#region Events

		/// <summary>
		/// Event, called when this body's health changes
		/// </summary>
		event BodyHealthChangedDelegate HealthChanged;

		/// <summary>
		/// Event, called when this body's state is changed
		/// </summary>
		event BodyStateChangedDelegate StateChanged;

		//event BodyStaticCollisionDelegate StaticCollision;
		//event BodyDynamicCollisionDelegate DynamicCollision;

		#endregion

		#region Operations

		/// <summary>
		/// Applies damage or healing to the body
		/// </summary>
		void ApplyHealthChange( HealthChange change );

		#endregion
	}
}
