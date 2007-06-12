using System;

namespace RbEngine.Rendering
{
	/// <summary>
	/// Applies a RenderTechnique
	/// </summary>
	public class AppliedTechnique : ITechnique
	{
		/// <summary>
		/// Default constructor
		/// </summary>
		/// <remarks>
		/// Effect and Technique are not set - nothing will happen if Apply() is called.
		/// </remarks>
		public AppliedTechnique( )
		{
		}

		/// <summary>
		/// Technique setup constructor
		/// </summary>
		public AppliedTechnique( RenderTechnique technique )
		{
			Effect		= technique.Effect;
			Technique	= technique;
		}

		/// <summary>
		/// Sets up the effect used
		/// </summary>
		/// <param name="effect"> Render effect to look up techniques in </param>
		/// <remarks>
		/// Selects the first technique in effect, if there is one.
		/// </remarks>
		public AppliedTechnique( RenderEffect effect )
		{
			Effect = effect;
		}

		/// <summary>
		/// Sets up the effect used, and the name of the technique
		/// </summary>
		/// <param name="effect"> Render effect to look up techniques in </param>
		/// <param name="techniqueName"> Name of the technique to select </param>
		/// <remarks>
		/// If effect does not contain a technique with the appropriate name, then the Technique property will return null. If Technique is null, then nothing
		/// will happen when Apply() is called.
		/// </remarks>
		public AppliedTechnique( RenderEffect effect, string techniqueName )
		{
			m_Effect	= effect;
			SelectTechnique( techniqueName );
		}

		#region		Public properties

		/// <summary>
		/// The render technique being used
		/// </summary>
		public RenderTechnique Technique
		{
			get
			{
				return m_Technique;
			}
			set
			{
				//	System.Diagnostics.Trace.Assert( m_Effect != null, String.Format( "Effect must be set before technique \"{0}\" is selected", value.Name ) );
				//	System.Diagnostics.Trace.Assert( m_Effect.GetTechniqueIndex( value ) != -1, String.Format( "Technique \"{0}\" did not exist in effect", value.Name ) );
				m_Technique = value;
				if ( m_Technique != null )
				{
					m_Effect = m_Technique.Effect;
				}
				else
				{
					m_Effect = null;
				}
			}
		}

		/// <summary>
		/// The render effect being applied. If this is set, then the technique is changed to the first technique in the new effect
		/// </summary>
		public RenderEffect	Effect
		{
			get
			{
				return m_Effect;
			}
			set
			{
				m_Effect = value;
				if ( m_Effect.TechniqueCount > 0 )
				{
					m_Technique = m_Effect.GetTechnique( 0 );
				}
			}
		}

		#endregion

		#region		Operations

		/// <summary>
		/// Wrapper around Technique.Apply( render ) (checks if Technique is valid)
		/// </summary>
		/// <param name="render"> Delegate used to render geometry between passes </param>
		public void Apply( TechniqueRenderDelegate render )
		{
			if ( Technique != null )
			{
				Technique.Apply( render );
			}
			else
			{
				render( );
			}
		}
 

		/// <summary>
		/// Selects a named technique from the current effect. Returns false if there is no technique with that name
		/// </summary>
		/// <param name="techniqueName"> Name of the technique to select </param>
		/// <returns> Returns false if the named technique was not found in the effect </returns>
		public bool	SelectTechnique( string techniqueName )
		{
			RenderTechnique technique = m_Effect.FindTechnique( techniqueName );
			if ( technique != null )
			{
				m_Technique = technique;
				return true;
			}
			else
			{
				Output.WriteLineCall( Output.RenderingWarning, "Could not find technique \"{0}\" in effect \"{1}\"", techniqueName, m_Effect.Name );
			}
			return false;
		}

		#endregion

		#region	Private stuff

		private RenderEffect		m_Effect;
		private RenderTechnique		m_Technique;

		#endregion

	}
}
