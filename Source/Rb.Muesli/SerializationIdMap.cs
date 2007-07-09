using System;
using System.Collections.Generic;
using System.Reflection;

namespace Rb.Muesli
{
	/// <summary>
	/// A map of types that have the SerializationIdAttribute
	/// </summary>
	public class SerializationIdMap
	{
		/// <summary>
		/// Gets the map singleton
		/// </summary>
		public static SerializationIdMap Instance
		{
			get { return ms_Instance; }
		}

		/// <summary>
		/// Gets the type associated with a given identifier
		/// </summary>
		public Type GetType( uint id )
		{
			return m_Map[ id ];
		}

		#region Private stuff

		/// <summary>
		/// Scans existing assemblies for types with the SerializationIdAttribute
		/// </summary>
		private SerializationIdMap( )
		{
			foreach ( Assembly assembly in AppDomain.CurrentDomain.GetAssemblies( ) )
			{
				try
				{
					ScanAssemblyForTypes( assembly );
				}
				catch
				{	
				}
			}
			AppDomain.CurrentDomain.AssemblyLoad += OnAssemblyLoad;
		}

		/// <summary>
		/// Called when an assembly is loaded. Scans the assembly for types with the SerializationIdAttribute
		/// </summary>
		private void OnAssemblyLoad( object sender, AssemblyLoadEventArgs args )
		{
			ScanAssemblyForTypes( args.LoadedAssembly );
		}

		/// <summary>
		/// Scans an assemblyfor types with the SerializationIdAttribute
		/// </summary>
		private void ScanAssemblyForTypes( Assembly assembly )
		{
			foreach ( Type type in assembly.GetTypes( ) )
			{
				SerializationIdAttribute[] attribs = ( SerializationIdAttribute[] )type.GetCustomAttributes( typeof( SerializationIdAttribute ), false );
				if ( attribs.Length > 0 )
				{
					System.Diagnostics.Trace.Assert( attribs[ 0 ].Id != SerializationIdAttribute.NoId );
					m_Map[ attribs[ 0 ].Id ] = type;
				}
			}
		}

		private Dictionary< uint, Type > m_Map = new Dictionary< uint, Type >( );
		private static SerializationIdMap ms_Instance = new SerializationIdMap( );

		#endregion
	}
}
