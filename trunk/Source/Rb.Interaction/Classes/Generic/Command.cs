
using System;
using Rb.Core.Utils;

namespace Rb.Interaction.Classes.Generic
{
	/// <summary>
	/// Extends Command with a typed trigger event handlder
	/// </summary>
	/// <typeparam name="TriggerDataType">Trigger data type</typeparam>
	public class Command<TriggerDataType> : Command
		where TriggerDataType : CommandTriggerData
	{
		/// <summary>
		/// Event raised when the command is triggered by a specified user
		/// </summary>
		public new event Action<TriggerDataType> CommandTriggered;

		/// <summary>
		/// Setup constructor
		/// </summary>
		public Command( string uniqueName, string locName, string locDescription ) :
			base( uniqueName, locName, locDescription )
		{
		}

		/// <summary>
		/// Raises the <see cref="CommandTriggered"/> event
		/// </summary>
		public override void Trigger( CommandTriggerData triggerData )
		{
			base.Trigger( triggerData );

			TriggerDataType triggerDataT = Arguments.CheckedNonNullCast<TriggerDataType>( triggerData, "triggerData" );
			if ( CommandTriggered != null )
			{
				CommandTriggered( triggerDataT );
			}
		}
	}
}
