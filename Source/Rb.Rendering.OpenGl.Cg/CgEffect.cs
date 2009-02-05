using System;
using System.Collections;
using System.IO;
using Rb.Assets.Interfaces;
using Rb.Rendering;
using Rb.Rendering.Interfaces.Objects;
using TaoCg = Tao.Cg.Cg;

namespace Rb.Rendering.OpenGl.Cg
{
	/// <summary>
	/// A CG effect
	/// </summary>
	public class CgEffect : Effect, IDisposable
	{
		#region Construction

		/// <summary>
		/// Creates the effect
		/// </summary>
		/// <param name="context"> Handle to the CG context that created this effect </param>
		public CgEffect( IntPtr context )
		{
			m_Context = context;
		}

		/// <summary>
		/// Creates the effect, loading it from a .cgfx file
		/// </summary>
		/// <param name="context"> Handle to the CG context that created this effect </param>
		/// <param name="path"> Path to the effect file </param>
		public CgEffect( IntPtr context, string path )
		{
			m_Context = context;
			Load( path );
		}

		/// <summary>
		/// Creates the effect, loading it from a .cgfx stream
		/// </summary>
		/// <param name="context">CG context</param>
		/// <param name="source">Asset source</param>
		public CgEffect( IntPtr context, IStreamSource source )
		{
			m_Context = context;
			Load( source );
		}

		#endregion

		#region	Effect application

		/// <summary>
		/// Applies this effect
		/// </summary>
		public override void Begin( )
		{
			foreach ( CgEffectParameter curParam in m_Parameters )
			{
				if ( curParam.DataSource != null )
				{
					curParam.DataSource.Apply( curParam );
				}
			}
		}

		/// <summary>
		/// Stops applying this effect
		/// </summary>
		public override void End( )
		{
		}

		#endregion

		#region	Effect loading and creation

		/// <summary>
		/// Loads this effect from a .cgfx file
		/// </summary>
		/// <param name="path"> Path to the effect file </param>
		/// <remarks>
		/// Included files must be in the current working directory!
		/// </remarks>
		public void	Load( string path )
		{
			if ( !CreateFromHandle( TaoCg.cgCreateEffectFromFile( m_Context, path, null ) ) )
			{
				throw new ApplicationException( string.Format( "Unable to create CG effect from path \"{0}\"\n{1}", path, TaoCg.cgGetLastListing( m_Context ) ) );
			}
		}

		/// <summary>
		/// Loads this effect from a .cgfx stream
		/// </summary>
		/// <param name="source">Asset source</param>
		public void Load( IStreamSource source )
		{
			using ( Stream streamSource = source.Open( ) )
			{
				using ( StreamReader reader = new StreamReader( streamSource ) )
				{
					string str = reader.ReadToEnd( );
					if ( !CreateFromHandle( TaoCg.cgCreateEffect( m_Context, str, null ) ) )
					{
						throw new ApplicationException( string.Format( "Unable to create CG effect from stream \"{0}\"\n{1}", source.Path, TaoCg.cgGetLastListing( m_Context ) ) );
					}
				}
			}
		}

		/// <summary>
		/// Creates this effect from an existing CGeffect handle
		/// </summary>
		/// <param name="effectHandle"> Handle to the CG effect. If this is null, nothing happens </param>
		private bool CreateFromHandle( IntPtr effectHandle )
		{
			if ( effectHandle == IntPtr.Zero )
			{
				return false;
			}

			m_EffectHandle = effectHandle;

			//	Run through all the techniques in the effect
			for ( IntPtr curTechnique = TaoCg.cgGetFirstTechnique( m_EffectHandle ); curTechnique != IntPtr.Zero; curTechnique = TaoCg.cgGetNextTechnique( curTechnique ) )
			{
				if ( TaoCg.cgValidateTechnique( curTechnique ) == TaoCg.CG_FALSE )
				{
					GraphicsLog.Warning( "Unable to validate technique \"{0}\" - {1}", TaoCg.cgGetTechniqueName( curTechnique ), TaoCg.cgGetLastListing( m_Context ) );
					continue;
				}
				string techniqueName = TaoCg.cgGetTechniqueName( curTechnique );
				GraphicsLog.Verbose( "Validating technique \"{0}\"", techniqueName );

				//	Create a Technique wrapper around the current technique
				Technique newTechnique = new Technique( techniqueName );

				//	Run through all the CG passes in the current technique
				for ( IntPtr curPass = TaoCg.cgGetFirstPass( curTechnique ); curPass != IntPtr.Zero; curPass = TaoCg.cgGetNextPass( curPass ) )
				{
					//	Create a CgRenderPass wrapper around the current pass, and add it to the current technique
					newTechnique.Add( new CgPass( curPass ) );
				}
				
				Add( newTechnique );
			}

			//	Run through all the parameters in the effect, creating CgShaderParameter objects for each
			for ( IntPtr curParam = TaoCg.cgGetFirstEffectParameter( m_EffectHandle ); curParam != IntPtr.Zero; curParam = TaoCg.cgGetNextParameter( curParam ) )
			{
				CgEffectParameter newParam = new CgEffectParameter( this, m_Context, curParam );
				m_Parameters.Add( newParam );
				Parameters.Add( newParam.Name, newParam );

				Graphics.EffectDataSources.BindParameter( newParam );
			}

			//	Add a listener to the shader binding collection
			Graphics.EffectDataSources.DataSourceAdded += OnDataSourceAdded;

			return true;
		}

		/// <summary>
		/// Determines if the new data source can be applied to any of this effect's parameters
		/// </summary>
		private void OnDataSourceAdded( IEffectDataSource dataSource )
		{
			foreach ( CgEffectParameter curParam in m_Parameters )
			{
				if ( curParam.DataSource == null )
				{
					Graphics.EffectDataSources.BindParameter( curParam );
				}
			}
		}

		#endregion

		#region Private stuff

		private readonly IntPtr		m_Context;
		private IntPtr				m_EffectHandle;
		private readonly ArrayList	m_Parameters = new ArrayList( );

		#endregion

		#region IDisposable Members

		public void Dispose( )
		{
			if ( m_EffectHandle != IntPtr.Zero )
			{
				TaoCg.cgDestroyEffect( m_EffectHandle );
				m_EffectHandle = IntPtr.Zero;
			}
		}

		#endregion
	}
}
