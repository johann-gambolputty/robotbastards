using System;
using System.Collections;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using Rb.Core.Assets;
using TaoCg = Tao.Cg.Cg;

namespace Rb.Rendering.OpenGl.Cg
{
	/// <summary>
	/// A CG effect
	/// </summary>
	public class CgEffect : Effect
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
		/// <param name="source">Asset source</param>
		public CgEffect( IntPtr context, ISource source )
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
			foreach ( CgShaderParameter curParam in m_Parameters )
			{
				if ( curParam.Binding == null )
				{
				}
				else
				{
					curParam.Binding.ApplyTo( curParam );
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

		private static readonly Regex ms_IncludeRegex = new Regex
			(
				@"\#include ""(?<path>.*)"""
			);

		private static string OpenIncludePath( ISource source, string path )
		{
			ISource includeSource = source.GetRelativeSource( path );
			if ( includeSource == null )
			{
				throw new FileNotFoundException( "Could not find include file", path );
			}
			using ( Stream stream = includeSource.Open( ) )
			{
				StreamReader reader = new StreamReader( stream );
				string contents = reader.ReadToEnd( );

				//	Recursively inline all includes
				contents = InlineAllIncludes( includeSource, contents );
				return contents;
			}
		}

		private static int CountLines( string str, int start, int end )
		{
			int lines = 0;
			for ( int index = start; index < end; ++index )
			{
				if ( str[ index ] == '\n' )
				{
					++lines;
				}
			}
			return lines;
		}

		public static string InlineAllIncludes(ISource source, string cgCode)
		{
			Match match = ms_IncludeRegex.Match( cgCode );
			if ( !match.Success )
			{
				return cgCode;
			}

			string sourcePath = source.Path.Replace( @"\", @"\\" );

			StringBuilder sb = new StringBuilder( cgCode.Length );
			int lastPos = 0;
			int orgLine = 1;
			for ( ; match.Success; match = match.NextMatch( ) )
			{
				Capture cap = match.Captures[ 0 ];
				sb.Append( cgCode, lastPos, cap.Index - lastPos );
				orgLine += CountLines( cgCode, lastPos, cap.Index );

				string path = match.Groups[ "path" ].Value;
				sb.AppendFormat( "#line 1 \"{0}\"\n", path );

				string includeContents = OpenIncludePath( source, path );
				sb.Append( includeContents );

				sb.AppendFormat( "#line {0} \"{1}\"\n", orgLine, sourcePath );

				lastPos = cap.Index + cap.Length;
			}

			sb.Append( cgCode, lastPos, cgCode.Length - lastPos );

			return sb.ToString( );
		}

		/// <summary>
		/// Loads this effect from a .cgfx stream
		/// </summary>
		/// <param name="source">Asset source</param>
		public void Load( ISource source )
		{
			using ( Stream streamSource = source.Open( ) )
			{
				StreamReader reader = new StreamReader( streamSource );
				string str = reader.ReadToEnd( );

				//	Replace any instances of "#include" with the actual file contents
				//	(this is because CG doesn't appear to have a search path, and wouldn't be able
				//	to handle, say, compressed file systems, databases, or other methods of retrieving assets)
				str = InlineAllIncludes( source, str );

				File.WriteAllText("c:\\temp\\effectDump.cgfx", str);

				if ( !CreateFromHandle( TaoCg.cgCreateEffect( m_Context, str, null ) ) )
				{
					throw new ApplicationException( string.Format( "Unable to create CG effect from stream \"{0}\"\n{1}", source.Path, TaoCg.cgGetLastListing( m_Context ) ) );
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
				string techniqueName = TaoCg.cgGetTechniqueName( curTechnique );
				if ( TaoCg.cgValidateTechnique( curTechnique ) == 0 )
				{
					GraphicsLog.Warning( "Unable to validate technique \"{0}\" - {1}", techniqueName, TaoCg.cgGetLastListing( m_Context ) );
					continue;
				}

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
				CgShaderParameter newParam = new CgShaderParameter( m_Context, this, curParam );
				m_Parameters.Add( newParam );

				newParam.Binding = Graphics.ShaderParameterBindings.GetBinding( newParam.Name );
			}
			//	Add a listener to the shader binding collection (this initialis
			Graphics.ShaderParameterBindings.OnNewBinding += OnNewBinding;

			return true;
		}

		/// <summary>
		/// Determines if the new binding can be applied to any of this effect's parameters
		/// </summary>
		/// <param name="binding"></param>
		private void OnNewBinding( ShaderParameterBinding binding )
		{
			foreach ( CgShaderParameter curParam in m_Parameters )
			{
				if ( curParam.Name == binding.Name )
				{
					curParam.Binding = binding;
					break;
				}
			}
		}

		#endregion

		#region	Effect parameters

		/// <summary>
		/// Finds a shader parameter by name
		/// </summary>
		/// <param name="name"> Name of the parameter to search for </param>
		/// <returns> Returns the named parameter, or null if the parameter could not be found </returns>
		public override ShaderParameter	GetParameter( string name )
		{
			foreach ( CgShaderParameter curParam in m_Parameters )
			{
				if ( TaoCg.cgGetParameterName( curParam.Parameter ) == name )
				{
					return curParam;
				}
			}

			return null;
		}

		#endregion

		#region Private stuff

		private readonly IntPtr		m_Context;
		private IntPtr				m_EffectHandle;
		private readonly ArrayList	m_Parameters = new ArrayList( );

		#endregion
	}
}
