using System;
using Rb.Core.Components;
using Rb.Core.Resources;

namespace Rb.World.Entities
{
	/// <summary>
	/// This component allows an entity to be controlled by a remote or local host
	/// </summary>
	public class EntityRemoteControl : Component, ISceneObject
	{
		/// <summary>
		/// Setup constructor
		/// </summary>
		public EntityRemoteControl( Guid hostId, string controllerPath )
		{
			m_HostId = hostId;
			m_ControllerPath = controllerPath;
		}
		
		#region ISceneObject Members

		/// <summary>
		/// Sets the scene context
		/// </summary>
		public void SetSceneContext( Scene scene )
		{
            IHost host = scene.GetService< IHost >( );
            if ( ( host == null ) || ( host.HostType == HostType.Local ) )
            {
                //  Local hosts can't receive commands from remote controllers, so the controller
                //  must be created locally
				LoadParameters parameters = new ComponentLoadParameters( this );
                AddChild( ResourceManager.Instance.Load( m_ControllerPath, parameters ) );
            }
            else if ( host.Id == m_HostId )
            {
				//  The scene host is the local controller host - create away
				LoadParameters parameters = new ComponentLoadParameters( this );
                AddChild( ResourceManager.Instance.Load( m_ControllerPath, parameters ) );

                //  Also need listeners and so forth
            }
            else
            {
                //  Also need listeners and so forth
                throw new ApplicationException( "Unimplemented" );
            }
		}

		#endregion

		#region Private stuff

		private Guid	m_HostId;
		private string	m_ControllerPath;

		#endregion

	}
}
