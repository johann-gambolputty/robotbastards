using System;
using Rb.Core.Components;

namespace Rb.World.Entities
{
    /// <summary>
    /// Entity class
    /// </summary>
    public class Entity : Component, ISceneObject
    {
        /// <summary>
        /// If the current host's ID is equal to hostId, then the specified local controller is added
        /// </summary>
        /// <param name="hostId">ID of the local controller's host</param>
        /// <param name="path">Local controller path</param>
        public void LoadLocalController( Guid hostId, string path )
        {
            IHost host = m_Scene.GetService< IHost >( );
            if ( ( host == null ) || ( host.Id == Guid.Empty ) )
            {
                //  The scene host is the local controller host - create away
                ( ( IParent )Parent ).AddChild( ResourceManager.Instance.Load( path ) );
            }
            else if ( host.Id == hostId )
            {
                //  The scene host is the local controller host - create away
                ( ( IParent )Parent ).AddChild( ResourceManager.Instance.Load( path ) );
            }
            else
            {
                throw new ApplicationException( "Controllers can only be hosted locally" );
            }
        }

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
