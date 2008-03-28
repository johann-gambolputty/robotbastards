using System.Collections.Generic;
using Rb.Rendering.Interfaces.Objects;

namespace Rb.Rendering.Base
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

		/// <summary>
		/// Helper method for adding a technique to the technique dictionary
		/// </summary>
		public void Add( ITechnique technique )
		{
			Techniques.Add( technique.Name, technique );
		}

		#region IEffect Members

		/// <summary>
		/// Gets the techniques supported by this effect
		/// </summary>
		public IDictionary<string, ITechnique> Techniques
		{
			get { return m_Techniques; }
		}

		/// <summary>
		/// Gets the parameters supported by this effect
		/// </summary>
		public IDictionary<string, IEffectParameter> Parameters
		{
			get { return m_Parameters; }
		}

		/// <summary>
		/// Finds a technique in this effect that can substitute the specified technique
		/// </summary>
		/// <param name="technique">Technique to substitute</param>
		/// <returns>
		/// Returns an ITechnique from this effect that can substitute technique. If none
		/// can be found, technique is returned.
		/// </returns>
		public ITechnique SubstituteTechnique( ITechnique technique )
		{
            foreach ( ITechnique substituteTechnique in m_Techniques.Values )
            {
                if ( substituteTechnique.IsSubstituteFor( technique ) )
                {
                    return substituteTechnique;
                }
            }
            return technique;
		}

		#endregion

		#region IPass Members

		/// <summary>
		/// Starts using this effect
		/// </summary>
		/// <remarks>
		/// Called from Technique.Begin()
		/// </remarks>
		public virtual void Begin( )
		{
		}

		/// <summary>
		/// Stops using this effect
		/// </summary>
		/// <remarks>
		/// Called from Technique.End()
		/// </remarks>
		public virtual void End( )
		{
		}

		#endregion
		
		#region Private Members

		private readonly IDictionary<string, ITechnique> m_Techniques = new Dictionary<string, ITechnique>( );
		private readonly IDictionary<string, IEffectParameter> m_Parameters = new Dictionary<string, IEffectParameter>( );

		#endregion
	}

}
