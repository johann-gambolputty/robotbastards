using System;
using System.Collections.Generic;
using System.Reflection;
using System.IO;
using System.Text.RegularExpressions;

namespace Rb.AssemblySelector
{
    /// <summary>
    /// Stores information about assemblies
    /// </summary>
    /// <remarks>
    /// Assemblies are categorised by this class using the AssemblyIdentifierAttribute assembly attribute, which is a key/value pair
    /// describing some aspect of the assembly.
    /// For example,
    /// <code>
    /// [assembly: AssemblyIdentifier( "GraphicsApi", "DirectX" )]
    /// </code>
    /// will assign an identifier with key "GraphicsApi" and value "DirectX" to the assembly when read by the identifier map.
    /// After categorisation (all loaded assemblies and assemblies in the current working directory are categorised on map
    /// construction), the map can be queried with selection strings.
    /// A selection string is a semi-colon delimited array of queries, all of which must evaluate to true to select an assembly
    /// from the map.
    /// Selection strings can be formed of name substring queries ("name=..."), explicit value queries ("key=value"), or value
    /// match to existing key queries ("key").
    /// For example, "name=blah;myKey0;myKey1=pie" will load an assembly with "blah" in its name, has a key "myKey0" with a value
    /// that matches the existing value of "myKey0" (set by a previously loaded assembly), and has a key "myKey1" with a value
    /// "pie".
	/// Assemblies add their key/value identifiers to the map if the identifier attribute property <see cref="AssemblyIdentifierAttribute.AddToIdMap"/>
    /// is true.
    /// A practical example 
    /// </remarks>
    public class IdentifierMap
    {
        /// <summary>
        /// IdentifierMap singleton
        /// </summary>
        public static IdentifierMap Instance
        {
            get { return ms_Singleton; }
        }

        /// <summary>
        /// Loads an assembly based on evidence queried by a selection string
        /// </summary>
        /// <param name="selectionString">Selection string</param>
        /// <returns>Returns the loaded assembly, or null if no assemblies matched the selection string</returns>
        public Assembly Load( string selectionString )
        {
            AssemblySelector selector = BuildSelector( selectionString );

            foreach ( KeyValuePair< string, AssemblyIdentifiers > kvp in m_Map )
            {
                if ( ( selector.MatchesFilename( kvp.Key ) ) && ( selector.MatchesIdentifiers( kvp.Value ) ) )
                {
                    return Assembly.Load( kvp.Key );
                }
            }
            return null;
        }

		/// <summary>
		/// Loads all assemblies based on evidence queried by a selection string
		/// </summary>
		/// <param name="selectionString">Selection string</param>
		public void LoadAll( string selectionString )
		{
            AssemblySelector selector = BuildSelector( selectionString );

            foreach ( KeyValuePair< string, AssemblyIdentifiers > kvp in m_Map )
            {
                if ( ( selector.MatchesFilename( kvp.Key ) ) && ( selector.MatchesIdentifiers( kvp.Value ) ) )
                {
					try
					{
						Assembly.Load( kvp.Key );
					}
					catch ( Exception ex )
					{
						System.Diagnostics.Trace.WriteLine( string.Format( "Failed to load assembly \"{0}\":\n{1}", kvp.Key, ex ) );
					}
                }
            }
		}

        /// <summary>
        /// Searches a directory for assemblies, storing information about each in this map
        /// </summary>
        /// <param name="directory">Directory to search</param>
        /// <param name="option">Directory search options</param>
        public void AddAssemblyIdentifiers( string directory, SearchOption option )
        {
			string[] files = Directory.GetFiles( directory, "*.dll", option );
			AppDomain.CurrentDomain.ReflectionOnlyAssemblyResolve += new ResolveEventHandler( ResolveAssembly );
            foreach ( string file in files )
            {
				try
				{
					string assemblyName = Path.GetFileNameWithoutExtension( file );

					if ( m_Map.ContainsKey( assemblyName ) )
					{
						continue;
					}


					Assembly.ReflectionOnlyLoad( assemblyName );
					//	NOTE: ReflectionOnlyLoad will trigger the "OnAssemblyLoad" event handler
				}
				catch ( Exception ex )
				{
					//	Can't really do anything here... doh. At least dump the exceptign to debug output
					System.Diagnostics.Trace.WriteLine( string.Format( "Failed to add assembly \"{0}\" to map:\n{1}", file, ex ) );
				}
			}
			AppDomain.CurrentDomain.ReflectionOnlyAssemblyResolve -= ResolveAssembly;
        }

		/// <summary>
		/// Resolves an assembly required by the reflection only load
		/// </summary>
		private static Assembly ResolveAssembly( object sender, ResolveEventArgs args )
		{
			//	This only seems to occur when trying to resolve the assembly selector assembly itself...
		//	Assembly result = Assembly.Load( args.Name );
			Assembly result = Assembly.ReflectionOnlyLoad( args.Name );
			return result;
		   // return Assembly.ReflectionOnlyLoad( args.Name );
		}
        
        /// <summary>
        /// Builds an AssemblySelector object from a selection string
        /// </summary>
        /// <param name="selectionString">Selection string</param>
        /// <returns>Returns a new AssemblySelector based on the selection string</returns>
        public static AssemblySelector BuildSelector( string selectionString )
        {
            Match match = SelectRegex.Match( selectionString );

            AssemblySelector curSelector = null;
            for ( ; match.Success; match = match.NextMatch( ) )
            {
                if ( match.Groups[ "Named" ].Success )
                {
                    string name = match.Groups[ "Name" ].Value;
                    curSelector = new NameSelector( name, curSelector );
                }
                else if ( match.Groups[ "Exists" ].Success )
                {
                    curSelector = new IdExistsSelector( match.Groups[ "Exists" ].Value, curSelector );
                }
                else if ( match.Groups[ "Matches" ].Success )
                {
                    string id       = match.Groups[ "Id" ].Value;
                    string value    = match.Groups[ "Value" ].Value;

                    curSelector = new HasIdOfValueSelector( id, value, curSelector );
                }
            }
            return curSelector ?? new AssemblySelector( null );
        }

        public class AssemblyIdentifiers : Dictionary< string, string >
        {
        }

        private class AssemblyMap : Dictionary< string, AssemblyIdentifiers >
        {
        }

        private class ValueList : List< string >
        {
        }

        private class IdMap : Dictionary< string, ValueList >
        {
        }

        private readonly AssemblyMap             m_Map           = new AssemblyMap( );
        private readonly IdMap                   m_IdMap         = new IdMap( );
        private readonly static IdentifierMap    ms_Singleton    = new IdentifierMap( );
        private readonly static Regex            SelectRegex     = new Regex( @"(?<Named>name=(?<Name>\w+))|(?<Matches>(?<Id>\w+)=(?<Value>\w+))|(?<Exists>\w+)" );


        /// <summary>
        /// Stores information about currently loaded assemblies, and assemblies in the current working directory
        /// </summary>
        private IdentifierMap( )
        {
            foreach ( Assembly assembly in AppDomain.CurrentDomain.GetAssemblies( ) )
            {
                AddAssemblyToMap( assembly );
            }

            AppDomain.CurrentDomain.AssemblyLoad += OnAssemblyLoad;

            AddAssemblyIdentifiers( ".", SearchOption.TopDirectoryOnly );
        }

        private static AssemblyIdentifiers CreateAssemblyIdentifiers( Assembly assembly )
        {
            AssemblyIdentifiers identifiers = null;

			IList<CustomAttributeData> attributes;
			try
			{
				attributes = CustomAttributeData.GetCustomAttributes( assembly );
			}
			catch ( Exception ex )
			{
				System.Diagnostics.Trace.WriteLine( "Failed to read custom attributes" );
				throw ex;
			}

			foreach ( CustomAttributeData idAttribute in attributes )
			{
				if ( idAttribute.Constructor.DeclaringType.GUID == typeof( AssemblyIdentifierAttribute ).GUID )
				{
                    if ( identifiers == null )
                    {
                        identifiers = new AssemblyIdentifiers( );
                    }
                    string id = idAttribute.ConstructorArguments[ 0 ].Value.ToString( );
                    string val = idAttribute.ConstructorArguments[ 1 ].Value.ToString( );

                    identifiers[ id ] = val;
				}
			}
            return identifiers;
        }

		private static string GetAssemblyKey( Assembly assembly )
		{
			return assembly.GetName( ).Name;
		}

        private void AddAssemblyToMap( Assembly assembly )
        {
            string assemblyKey = GetAssemblyKey( assembly );
            if ( m_Map.ContainsKey( assemblyKey ) )
            {
                return;
            }
			//	Only add the assembly if it's not already loaded
			if ( !assembly.ReflectionOnly )
			{
				return;
			}

            AssemblyIdentifiers identifiers = CreateAssemblyIdentifiers( assembly );
            m_Map[ assemblyKey ] = identifiers;
			if ( identifiers == null )
			{
				return;
			}

            foreach ( KeyValuePair< string, string > identifier in identifiers )
            {
			    ValueList values;
                if ( !m_IdMap.TryGetValue( identifier.Key, out values ) )
                {
                    values = new ValueList( );
                    m_IdMap.Add( identifier.Key, values );
                }
                if ( values.IndexOf( identifier.Value ) == -1 )
                {
                    values.Add( identifier.Value );
                }
            }
        }

        private void OnAssemblyLoad( object sender, AssemblyLoadEventArgs args )
        {
            AddAssemblyToMap( args.LoadedAssembly );
        }
        
        public bool IsMatchingId( string id, string value )
        {
            ValueList values;
            if ( !m_IdMap.TryGetValue( id, out values ) )
            {
                return false;
            }

            return values.IndexOf( value ) != -1;
        }

        public class AssemblySelector
        {
            public AssemblySelector( AssemblySelector nextSelector )
            {
                m_Next = nextSelector;
            }

            public virtual bool MatchesFilename( string assemblyName )
            {
                return ( m_Next == null ) ? true : m_Next.MatchesFilename( assemblyName );
            }

			public virtual bool MatchesIdentifiers( AssemblyIdentifiers identifiers )
            {
                return ( m_Next == null ) ? true : m_Next.MatchesIdentifiers( identifiers );
            }

			private readonly AssemblySelector m_Next;
        }

        private class IdExistsSelector : AssemblySelector
        {
            public IdExistsSelector( string id, AssemblySelector nextSelector ) :
                base( nextSelector )
            {
                m_Id = id;
            }

			public override bool MatchesIdentifiers( AssemblyIdentifiers identifiers )
            {
				if ( identifiers == null )
				{
					return false;
				}

                foreach ( KeyValuePair< string, string > kvp in identifiers )
                {
                    if ( ( kvp.Key == m_Id ) && ( Instance.IsMatchingId( kvp.Key, kvp.Value ) ) )
                    {
                        return base.MatchesIdentifiers( identifiers );
                    }
                }
                return false;
            }

			private readonly string m_Id;
        }

        private class NameSelector : AssemblySelector
        {
            public NameSelector( string name, AssemblySelector nextSelector ) :
                base( nextSelector )
            {
                m_Name = name;
            }

            public override bool MatchesFilename( string path )
            {
                return path.Contains( m_Name ) && base.MatchesFilename( path );
            }

			private readonly string m_Name;
        }
        
        private class HasIdOfValueSelector : AssemblySelector
        {
            public HasIdOfValueSelector( string id, string value, AssemblySelector nextSelector ) :
                base( nextSelector )
            {
                m_Id = id;
                m_Value = value;
            }

			public override bool MatchesIdentifiers( AssemblyIdentifiers identifiers )
			{
				if ( identifiers == null )
				{
					return false;
				}
                foreach ( KeyValuePair< string, string > kvp in identifiers )
                {
                    if ( ( kvp.Key == m_Id ) && ( kvp.Value == m_Value ) )
                    {
                        return base.MatchesIdentifiers( identifiers );
                    }
                }

                return false;
            }

            private readonly string m_Id;
			private readonly string m_Value;
        }

    }
}
