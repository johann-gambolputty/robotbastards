using System;
using System.Collections;

namespace RbEngine.Rendering
{
	/// <summary>
	/// Stores a set of techniques (RenderTechnique objects) that can be used for rendering geometry
	/// </summary>
	/// <seealso>Shader</seealso>
	/// <seealso>RenderTechnique</seealso>
	/// <seealso>SelectedTechnique</seealso>
	public class RenderEffect : Shader, Components.IParentObject
	{
		#region	Construction

		/// <summary>
		/// Creates a render effect with no techniques
		/// </summary>
		public RenderEffect( )
		{
		}

		/// <summary>
		/// Creates a render effect with one technique
		/// </summary>
		/// <param name="technique"> Technique to add </param>
		public RenderEffect( RenderTechnique technique )
		{
			m_Techniques.Add( technique );
		}

		#endregion

		#region	Techniques

		/// <summary>
		/// Finds a named technique
		/// </summary>
		/// <param name="name"> Name of the technique to look for (case-sensitive match to RenderTechnique.Name)</param>
		/// <returns> Returns the named technique. Returns null if no technique with the specified name can be found </returns>
		public RenderTechnique	FindTechnique( string name )
		{
			foreach ( RenderTechnique technique in m_Techniques )
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
		public void				Add( RenderTechnique technique )
		{
			m_Techniques.Add( technique );
		}

		/// <summary>
		/// Returns the indexed technique
		/// </summary>
		/// <param name="index"> Technique index, in range [0..GetNumTechniques()] </param>
		/// <returns> The indexed technique </returns>
		public RenderTechnique	GetTechnique( int index )
		{
			return ( RenderTechnique )m_Techniques[ index ];
		}

		/// <summary>
		/// Returns the index of a technique in this effect
		/// </summary>
		/// <param name="technique"> Technique to look for </param>
		/// <returns> Returns -1 if the technique can't be found </returns>
		public int				GetTechniqueIndex( RenderTechnique technique )
		{
			return m_Techniques.IndexOf( technique );
		}


		/// <summary>
		/// The number of techniques stored in this effect
		/// </summary>
		public int				TechniqueCount
		{
			get
			{
				return m_Techniques.Count;
			}
		}

		#endregion

		#region	Private stuff

		private ArrayList		m_Techniques	= new ArrayList( );

		#endregion

		#region IParentObject Members

		/// <summary>
		/// Adds a child object, but only if it's of type RenderTechnique
		/// </summary>
		/// <param name="childObject"> Child render technique</param>
		public void AddChild(Object childObject )
		{
			Add( ( RenderTechnique )childObject );
		}

		/// <summary>
		/// Visits all child techniques, calling visitor() for each
		/// </summary>
		/// <param name="visitor">Visitor function</param>
		public void VisitChildren( Components.ChildVisitorDelegate visitor )
		{
			for ( int childIndex = 0; childIndex < m_Techniques.Count; ++childIndex )
			{
				if ( !visitor( m_Techniques[ childIndex ] ) )
				{
					return;
				}
			}
		}

		#endregion
	}

}
