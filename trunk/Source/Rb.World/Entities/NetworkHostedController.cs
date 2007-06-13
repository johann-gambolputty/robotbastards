using System;
using System.Collections.Generic;
using System.Text;

namespace Rb.World.Entities
{
    /// <summary>
    /// A controller that is hosted either locally or remotely
    /// </summary>
    public class NetworkHostedController : HostedController
    {
        /// <summary>
        /// If the current host's ID is equal to hostId, then the specified local controller is added
        /// </summary>
        /// <param name="hostId">ID of the local controller's host</param>
        /// <param name="path">Local controller path</param>
        public override void LoadLocalController( Guid hostId, string path )
        {
            if ( m_Scene.GetService< IHost >( ).Id == hostId )
            {
                //  The scene host is the local controller host - create away
                m_Parent.AddChild( ResourceManager.Instance.Load( path ) );

            /*
			    <object type="RbEngine.Interaction.CommandInputListener">
				    <string value="TestCommands" property="CommandListName"/>
			    </object>

			    <object type="RbEngine.Network.Runt.ChildUpdateProvider">
				    <type value="RbEngine.Interaction.CommandMessage" property="UpdateMessageType"/>
			    </object>

			    <object type="RbEngine.Network.Runt.ChildUpdateHandler"/>

			    <object type="RbEngine.Entities.TestUserEntityController"/>
            */
            }
            else
            {
                throw new ApplicationException( "Controllers can only be hosted locally" );
                /*
			        <object type="RbEngine.Network.Runt.ChildUpdateHandler"/>
			        <object type="RbEngine.Entities.TestUserEntityController"/>
                */
            }
        }
    }
}
