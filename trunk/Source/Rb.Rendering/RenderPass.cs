using System.Collections.Generic;

namespace Rb.Rendering
{

	/// <summary>
	/// A render pass sets up the state of the renderer for rendering
	/// </summary>
	public class RenderPass : IPass
	{
		/// <summary>
		/// Default constructor (no child passes)
		/// </summary>
		public RenderPass( )
		{
		}

		/// <summary>
		/// Setup constructor
		/// </summary>
		/// <param name="passes">Passes to add to this render pass</param>
		public RenderPass( params IPass[] passes )
		{
			Add( passes );
		}
        
        /// <summary>
        /// Adds all enumerable IPass objects in passes
        /// </summary>
        /// <param name="passes">Passes enumeration</param>
        public RenderPass(IEnumerable<IPass> passes)
        {
            Add( passes );
        }

		/// <summary>
		/// Adds an object to this render pass (must implement the Rendering.IPass interface)
		/// </summary>
		public void Add( IPass pass )
		{
			m_Passes.Add( pass );
		}

		/// <summary>
		/// Adds an object to this render pass (must implement the Rendering.IPass interface)
		/// </summary>
		public void Add( params IPass[] passes )
		{
			for ( int index = 0; index < passes.Length; ++index )
			{
				Add( passes[ index ] );
			}
        }

        /// <summary>
        /// Adds all enumerable IPass objects in passes
        /// </summary>
        /// <param name="passes">Passes enumeration</param>
        public void Add( IEnumerable< IPass > passes )
        {
            foreach ( IPass pass in passes )
            {
                Add( pass );
            }
        }

        /// <summary>
        /// Access to the set of passes making up this render pass
        /// </summary>
	    public IList< IPass > Passes
	    {
	        get { return m_Passes; }
	    }


        #region IPass methods

        /// <summary>
		/// Applies this render pass
		/// </summary>
		public virtual void Begin( )
		{
			foreach ( IPass pass in m_Passes )
			{
				pass.Begin( );
			}
		}

		/// <summary>
		/// Cleans up the application of this render pass
		/// </summary>
		public virtual void End( )
		{
			foreach ( IPass pass in m_Passes )
			{
				pass.End( );
			}
        }

        #endregion

		#region	Private stuff

		private List< IPass > m_Passes = new List< IPass >( );

		#endregion

	}
}
