using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.IO;
using Microsoft.CSharp;
using Rb.Core;
using Rb.Core.Assets;
using Rb.Core.Components;
using Rb.Log;

namespace Rb.CodeLoader
{
	public class Loader : AssetLoader
	{
		/// <summary>
		/// Gets the name of this loader
		/// </summary>
		public override string Name
		{
			get { return "CodeLoader"; }
		}

		/// <summary>
		/// Returns the extensions that this loader supports
		/// </summary>
		public override string[] Extensions
		{
			get { return new string[] { "cs" }; }
		}

		/// <summary>
		/// Creates default loading parameters
		/// </summary>
		/// <param name="addAllProperties">If true, then the parameters object gets all relevant dynamic properties with their default values added</param>
		/// <returns>Returns default loading parameters</returns>
		public override LoadParameters CreateDefaultParameters( bool addAllProperties )
		{
			LoadParameters parameters = new LoadParameters( );
			if ( addAllProperties )
			{
				parameters.Properties.Add( InstanceTypeName, "" );
				parameters.Properties.Add( ReferencesName, new string[] { } );
			}
			return parameters;
		}

		/// <summary>
		/// Loads code from a source
		/// </summary>
		public override object Load( ISource source, LoadParameters parameters )
		{
			string typeToInstance = DynamicProperties.GetProperty< string >( parameters.Properties, InstanceTypeName, null );
			IEnumerable< string > assemblies = DynamicProperties.GetProperty< IEnumerable< string > >( parameters.Properties, ReferencesName, new string[] { } );

			using ( Stream stream = source.Open( ) )
			{
				StreamReader reader = new StreamReader( stream );
				string allLines = reader.ReadToEnd( );

				CodeDomProvider provider = new CSharpCodeProvider( );
				CompilerParameters compilerParams = new CompilerParameters( );
				compilerParams.GenerateInMemory = true;

#if DEBUG
				compilerParams.IncludeDebugInformation = true;
#endif
				//	Add referenced assemblies to compiler parameters
				foreach ( string assembly in assemblies )
				{
					compilerParams.ReferencedAssemblies.Add( assembly );
				}
				CompilerResults results = provider.CompileAssemblyFromSource( compilerParams, allLines );
				if ( results.Errors.Count > 0 )
				{
					//	Failed to compile assembly - dump
					foreach ( CompilerError error in results.Errors )
					{
						Entry entry = new Entry( AssetsLog.GetSource( error.IsWarning ? Severity.Warning : Severity.Error ), error.ErrorText );
						entry.Locate( error.FileName, error.Line, error.Column, "" );
						Source.HandleEntry( entry );
					}
					return null;
				}

				//	If the caller requested the instance of a specific type, then find that type in the
				//	compiled assembly, and create an instance of it
				if ( typeToInstance != null )
				{
					Type instanceType = results.CompiledAssembly.GetType( typeToInstance );
					return Activator.CreateInstance( instanceType );
				}

				//	Find a type that implements IBuilder, instance it, then use the IBuilder.CreateInstance()
				//	to create our required type
				foreach ( Type type in results.CompiledAssembly.GetTypes( ) )
				{
					if ( typeof( IBuilder ).IsAssignableFrom( type ) )
					{
						IBuilder builder = ( IBuilder )Activator.CreateInstance( type );
						return builder.CreateInstance( type );
					}
				}
			}

			return null;
		}


		private const string InstanceTypeName = "InstanceType";
		private const string ReferencesName = "References";

	}
}
