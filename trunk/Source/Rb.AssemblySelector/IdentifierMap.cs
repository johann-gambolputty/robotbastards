using System;
using System.Collections.Generic;
using System.Reflection;
using System.IO;
using System.Text.RegularExpressions;

namespace Rb.AssemblySelector
{
    public class IdentifierMap
    {
        public static IdentifierMap Instance
        {
            get { return ms_Singleton; }
        }
        private static IdentifierMap ms_Singleton = new IdentifierMap( );
        
        private IdentifierMap( )
        {
            foreach ( Assembly assembly in AppDomain.CurrentDomain.GetAssemblies())
            {
                AddAssemblyToMap( assembly );
            }

            AppDomain.CurrentDomain.AssemblyLoad += new AssemblyLoadEventHandler( OnAssemblyLoad );

            AddAssemblyIdentifiers( ".", SearchOption.TopDirectoryOnly );
        }

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

        public void AddAssemblyIdentifiers( string directory, SearchOption option )
        {
            foreach ( string file in System.IO.Directory.GetFiles( directory, "*.dll", option ) )
            {
                string assemblyName = System.IO.Path.GetFileNameWithoutExtension( file );
                Assembly curAssembly = Assembly.ReflectionOnlyLoad(assemblyName);

                AddAssemblyToMap( curAssembly );
            }
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

        private AssemblyMap m_Map = new AssemblyMap( );
        private IdMap m_IdMap = new IdMap( );

        private static AssemblyIdentifiers CreateAssemblyIdentifiers( Assembly assembly )
        {
            AssemblyIdentifiers identifiers = null;
			foreach ( CustomAttributeData idAttribute in CustomAttributeData.GetCustomAttributes( assembly ) )
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

        private void AddAssemblyToMap( Assembly assembly )
        {
            string assemblyKey = assembly.GetName().Name;
            if ( m_Map.ContainsKey( assemblyKey ) )
            {
                return;
            }

            AssemblyIdentifiers identifiers = CreateAssemblyIdentifiers( assembly );
            if ( identifiers == null )
            {
                return;
            }
            m_Map[ assemblyKey ] = identifiers;

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
            return curSelector == null ? new AssemblySelector( null ) : curSelector;
        }

        private static Regex SelectRegex = new Regex(@"(?<Named>name=(?<Name>\w+))|(?<Matches>(?<Id>\w+)=(?<Value>\w+))|(?<Exists>\w+)");

        /*
        public void AddAssembly( Assembly assembly )
        {
            object[] idAttributes = assembly.GetCustomAttributes( typeof( AssemblyIdentifierAttribute ), true );
            foreach ( AssemblyIdentifierAttribute idAttribute in idAttributes )
            {
                if ( !idAttribute.AddToIdMap )
                {
                    continue;
                }

                IdList identifiers;
                if ( !m_Map.TryGetValue( idAttribute.Identifier, out identifiers ) )
                {
                    identifiers = new IdList( );
                    m_Map.Add( idAttribute.Identifier, identifiers );
                }

                if ( identifiers.IndexOf( idAttribute.Value ) == -1 )
                {
                    identifiers.Add( idAttribute.Value );
                }
            }
        }

        public Assembly Load( string searchString, string selectionString, string directory, System.IO.SearchOption option )
        {
            AssemblySelector selector = BuildSelector( selectionString );

            foreach ( string file in System.IO.Directory.GetFiles( directory, searchString, option ) )
            {
                if ( selector.MatchesFilename( file ) )
                {
                    string assembly = System.IO.Path.GetFileNameWithoutExtension( file );
                    Assembly curAssembly = Assembly.ReflectionOnlyLoad( assembly );

                    if ( selector.MatchesIdentifiers( curAssembly ) )
                    {
                        return Assembly.Load( assembly );
                    }
                }
            }

            return null;
        }
        */

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

            private AssemblySelector m_Next;
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
                foreach ( KeyValuePair< string, string > kvp in identifiers )
                {
                    if ( ( kvp.Key == m_Id ) && ( Instance.IsMatchingId( kvp.Key, kvp.Value ) ) )
                    {
                        return base.MatchesIdentifiers( identifiers );
                    }
                }
                return false;
            }

            private string m_Id;
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

            private string m_Name;
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
                foreach ( KeyValuePair< string, string > kvp in identifiers )
                {
                    if ( ( kvp.Key == m_Id ) && ( kvp.Value == m_Value ) )
                    {
                        return base.MatchesIdentifiers( identifiers );
                    }
                }

                return false;
            }

            private string m_Id;
            private string m_Value;
        }

    }
}
