using Rb.Interaction.Interfaces;
using Rb.Core.Maths;

namespace Rb.Interaction.Classes
{
	/// <summary>
	/// Default implementation of <see cref="ICommandInputStateFactory"/>
	/// </summary>
	public class CommandInputStateFactory : ICommandInputStateFactory
	{
		/// <summary>
		/// Returns null
		/// </summary>
		public virtual ICommandInputState NewInputState( object context )
		{
			return null;
		}

		/// <summary>
		/// Returns a new <see cref="CommandInputScalarState"/>
		/// </summary>
		public virtual ICommandInputState NewScalarInputState( object context, float lastValue, float value )
		{
			return new CommandScalarInputState( lastValue, value );
		}

		/// <summary>
		/// Returns a new <see cref="CommandInputPointState"/>
		/// </summary>
		public virtual ICommandInputState NewPointInputState( object context, float lastX, float lastY, float x, float y )
		{
			return new CommandPointInputState( new Point2( lastX, lastY ), new Point2( x, y ) );
		}

		/// <summary>
		/// Gets the default factory instance
		/// </summary>
		public static CommandInputStateFactory Default
		{
			get { return s_Default; }
		}


		#region Private Members

		private readonly static CommandInputStateFactory s_Default = new CommandInputStateFactory( );

		#endregion
	}
}
