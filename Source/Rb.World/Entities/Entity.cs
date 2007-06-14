using System;
using Rb.Core.Components;
using Rb.Core.Resources;

namespace Rb.World.Entities
{
    /// <summary>
    /// Entity class
    /// </summary>
    public class Entity : Component, ISceneObject
    {
        #region Controller

        /// <summary>
        /// If the current host's ID is equal to hostId, then the specified local controller is added
        /// </summary>
        /// <param name="hostId">ID of the local controller's host</param>
        /// <param name="controllerPath">Path to the resource that describes the controller</param>
        public void SetupController( Guid hostId, string controllerPath )
        {
            IHost host = m_Scene.GetService< IHost >( );
            if ( ( host == null ) || ( host.HostType == HostType.Local ) )
            {
                //  Local hosts can't receive commands from remote controllers, so the controller
                //  must be created locally
                AddChild( ResourceManager.Instance.Load( controllerPath ) );
            }
            else if ( host.Id == hostId )
            {
                //  The scene host is the local controller host - create away
                AddChild( ResourceManager.Instance.Load( controllerPath ) );

                //  Also need listeners and so forth
            }
            else
            {
                //  Also need listeners and so forth
                throw new ApplicationException( "Unimplemented" );
            }
        }

        #endregion

        #region ISceneObject Members

        /// <summary>
        /// Sets the scene context that this object has been created in
        /// </summary>
        public void SetSceneContext( Scene scene )
        {
            m_Scene = scene;
        }

        #endregion

        #region Protected stuff

        protected Scene m_Scene;

        #endregion
    }
}
