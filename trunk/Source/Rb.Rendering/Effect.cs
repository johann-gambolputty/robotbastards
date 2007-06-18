using System;
using System.Collections.Generic;

namespace Rb.Rendering
{
	/// <summary>
	/// Stores a set of techniques that can be used for rendering stuff
	/// </summary>
	public class Effect : IEffect
	{
		#region	Construction

		/// <summary>
		/// Creates a render effect with no techniques
		/// </summary>
		public Effect( )
		{
		}

		/// <summary>
		/// Creates a render effect with one or more techniques
		/// </summary>
		/// <param name="techniques">Techniques to add</param>
		public Effect( params ITechnique[] techniques )
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
		/// <param name="name"> Name of the technique to look for (case-sensitive match to Technique.Name)</param>
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
		    technique.Effect = this;
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
        /// Technique collection getter
        /// </summary>
        /// <remarks>
        /// Can only add techniques via Add()
        /// </remarks>
	    public ICollection< ITechnique > Techniques
	    {
	        get { return m_Techniques; }
	    }

		#endregion

        #region IEffect Members

        /// <summary>
        /// Gets a shader parameter by its name
        /// </summary>
        /// <param name="name">Parameter name</param>
        /// <returns>Returns the named shader parameter, or null if it doesn't exist</returns>
        public virtual ShaderParameter GetParameter( string name )
        {
            return null;
        }

        /// <summary>
        /// Gets a technique from its name
        /// </summary>
        public virtual ITechnique GetTechnique( string name )
        {
            return FindTechnique( name );
        }


        /// <summary>
        /// Finds a technique in this effect that can substitute the specified technique
        /// </summary>
        /// <param name="technique">Technique to substitute</param>
        /// <returns>
        /// Returns an ITechnique from this effect that can substitute technique. If none
        /// can be found, technique is returned.
        /// </returns>
        public virtual ITechnique SubstituteTechnique( ITechnique technique )
        {
            foreach ( ITechnique substituteTechnique in m_Techniques )
            {
                if ( substituteTechnique.IsSubstituteFor( technique ) )
                {
                    return substituteTechnique;
                }
            }
            return technique;
        }

        #endregion

        #region	IPass Members

        /// <summary>
		/// Starts using this shader
		/// </summary>
		/// <remarks>
		/// Called from Technique.Begin()
		/// </remarks>
		public virtual void Begin( )
		{
		}

		/// <summary>
		/// Stops using this shader
		/// </summary>
		/// <remarks>
		/// Called from Technique.End()
		/// </remarks>
		public virtual void End( )
		{
		}

		#endregion

        #region	Private stuff

        private List< ITechnique > m_Techniques = new List< ITechnique >( );

        #endregion
	}

}
