using Rb.Core.Utils;

namespace Rb.Interaction
{
	/// <summary>
	/// Provides the heartbeat that raises the interaction update
	/// </summary>
	public class InteractionUpdateTimer
	{
		/// <summary>
		/// Update event
		/// </summary>
		public event ActionDelegates.Action Update;

		/// <summary>
		/// Returns the default interaction update timer
		/// </summary>
		public static InteractionUpdateTimer Instance
		{
			get { return s_Instance; }
		}

		/// <summary>
		/// Raises the update event
		/// </summary>
		public void OnUpdate( )
		{
			if ( Update != null )
			{
				Update( );
			}
		}

		#region Private Members

		private readonly static InteractionUpdateTimer s_Instance = new InteractionUpdateTimer( );

		//static InteractionUpdateTimer( )
		//{
		//    foreach ( Assembly assembly in AppDomain.CurrentDomain.GetAssemblies( ) )
		//    {
		//        if ( assembly.FullName.StartsWith( "System" ) )
		//        {
		//            continue;
		//        }
		//        foreach ( Type type in assembly.GetTypes( ) )
		//        {
		//            if ( type.IsSubclassOf( typeof( InteractionUpdateTimer ) ) )
		//            {
		//                s_Instance = ( InteractionUpdateTimer )Activator.CreateInstance( type );
		//                return;
		//            }
		//        }
		//    }

		//    throw new TypeLoadException( "Failed to find type derived from InteractionUpdateTimer" );
		//}
		
		#endregion
	}
}
