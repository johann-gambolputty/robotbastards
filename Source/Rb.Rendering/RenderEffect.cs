using System;
using System.Collections.Generic;

namespace Rb.Rendering
{
	/// <summary>
	/// Stores a set of techniques that can be used for rendering stuff
	/// </summary>
	public class RenderEffect : Shader, IPass
	{
		#region	Construction

		/// <summary>
		/// Creates a render effect with no techniques
		/// </summary>
		public RenderEffect( )
		{
		}

		/// <summary>
		/// Creates a render effect with one or more techniques
		/// </summary>
		/// <param name="techniques">Techniques to add</param>
		public RenderEffect( params ITechnique[] techniques )
		{
            foreach ( ITechnique technique in techniques )
            {
			    Add( technique );
            }
		}

		#endregion

		#region	Techniques

		/// <summary>
		/// Finds a named technique
		/// </summary>
		/// <param name="name"> Name of the technique to look for (case-sensitive match to RenderTechnique.Name)</param>
		/// <returns> Returns the named technique. Returns null if no technique with the specified name can be found </returns>
		public ITechnique FindTechnique( string name )
		{
            foreach ( ITechnique technique in m_Techniques )
			{
				if ( technique.Name == name )
				{
					return technique;
				}
			}

			return null;
		}

		/// <summary>
		/// Adds a generic render technique to the effect
		/// </summary>
		public void Add( ITechnique technique )
		{
			m_Techniques.Add( technique );

            //  hmmm....
		    RenderTechnique renderTechnique = technique as RenderTechnique;
            if ( renderTechnique != null )
            {
			    renderTechnique.Effect = this;
            }
		}

		/// <summary>
		/// Returns the indexed technique
		/// </summary>
        /// <param name="index"> Technique index, in range [0..TechniqueCount-1] </param>
		/// <returns> The indexed technique </returns>
		public ITechnique GetTechnique( int index )
		{
			return m_Techniques[ index ];
		}

		/// <summary>
		/// Returns the index of a technique in this effect
		/// </summary>
		/// <param name="technique"> Technique to look for </param>
		/// <returns> Returns -1 if the technique can't be found </returns>
		public int GetTechniqueIndex( ITechnique technique )
		{
			return m_Techniques.IndexOf( technique );
		}


		/// <summary>
		/// The number of techniques stored in this effect
		/// </summary>
		public int TechniqueCount
		{
			get { return m_Techniques.Count; }
		}

        /// <summary>
        /// Technique list getter
        /// </summary>
        // TODO: AP: Make a proper TechniqueList class, that handles RenderTechnique.Add()
	    public IList< ITechnique > Techniques
	    {
	        get { return m_Techniques; }
	    }

		#endregion

		#region	Private stuff

		private List< ITechnique > m_Techniques	= new List< ITechnique >( );

		#endregion

		#region	IPass Members

		/// <summary>
		/// Starts using this shader
		/// </summary>
		/// <remarks>
		/// Called from RenderTechnique.Begin()
		/// </remarks>
		public virtual void Begin( )
		{
		}

		/// <summary>
		/// Stops using this shader
		/// </summary>
		/// <remarks>
		/// Called from RenderTechnique.End()
		/// </remarks>
		public virtual void End( )
		{
		}

		#endregion
	}

}
