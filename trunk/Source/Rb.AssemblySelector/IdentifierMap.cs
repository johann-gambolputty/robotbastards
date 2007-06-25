using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text.RegularExpressions;

namespace Rb.AssemblySelector
{
    public class IdentifierMap
    {
        public static IdentifierMap Instance
        {
            get { return ms_Singleton; }
        }

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

        private static IdentifierMap ms_Singleton = new IdentifierMap( );

        private IdentifierMap( )
        {
            foreach ( Assembly assembly in AppDomain.CurrentDomain.GetAssemblies())
            {
                AddAssembly( assembly );
            }
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

        public Assembly Load( string searchString, string selectionString, string directory, System.IO.SearchOption option )
        {
            AssemblySelector selector = BuildSelector( selectionString );

            foreach ( string file in System.IO.Directory.GetFiles( directory, searchString, option ) )
            {
                if ( selector.MatchesFilename( file ) )
                {
                    string assembly = file.Substring( 0, file.LastIndexOf( ".dll" ) );
                    Assembly curAssembly = AppDomain.CurrentDomain.Load( assembly );

                    if ( selector.MatchesAssembly( curAssembly ) )
                    {
                        return curAssembly;
                    }
                }
            }

            return null;
        }

        public class AssemblySelector
        {
            public AssemblySelector( AssemblySelector nextSelector )
            {
                m_Next = nextSelector;
            }

            public bool MatchesAssembly( Assembly assembly )
            {
                AssemblyIdentifierAttribute[] idAttributes = ( AssemblyIdentifierAttribute[] )assembly.GetCustomAttributes( typeof( AssemblyIdentifierAttribute ), true );
                return MatchesAssembly( assembly, idAttributes );
            }

            public virtual bool MatchesFilename( string path )
            {
                return ( m_Next == null ) ? true : m_Next.MatchesFilename( path );
            }

            public virtual bool MatchesAssembly(Assembly assembly, AssemblyIdentifierAttribute[] idAttributes)
            {
                return ( m_Next == null ) ? true : m_Next.MatchesAssembly( assembly, idAttributes );
            }

            private AssemblySelector m_Next;
        }

        public bool IsMatchingId( string id, string value )
        {
            IdList identifiers;
            if ( !m_Map.TryGetValue( id, out identifiers ) )
            {
                return false;
            }

            return identifiers.IndexOf( value ) != -1;
        }

        private class IdExistsSelector : AssemblySelector
        {
            public IdExistsSelector( string id, AssemblySelector nextSelector ) :
                base( nextSelector )
            {
                m_Id = id;
            }
            
            public override bool MatchesAssembly( Assembly assembly, AssemblyIdentifierAttribute[] idAttributes )
            {
                foreach ( AssemblyIdentifierAttribute idAttribute in idAttributes )
                {
                    if ( ( idAttribute.Identifier == m_Id ) && ( Instance.IsMatchingId( idAttribute.Identifier, idAttribute.Value ) ) )
                    {
                        return base.MatchesAssembly( assembly, idAttributes );
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

            public override bool MatchesAssembly( Assembly assembly, AssemblyIdentifierAttribute[] idAttributes )
            {
                foreach ( AssemblyIdentifierAttribute idAttribute in idAttributes )
                {
                    if ( ( idAttribute.Identifier == m_Id ) && ( idAttribute.Value == m_Value ) )
                    {
                        return base.MatchesAssembly( assembly, idAttributes );
                    }
                }
                return false;
            }

            private string m_Id;
            private string m_Value;
        }

        private class IdList : List< string >
        {
        }

        private class IdMap : Dictionary< string, IdList >
        {
        }

        private IdMap m_Map = new IdMap( );
    }
}
