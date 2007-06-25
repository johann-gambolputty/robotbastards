using System;
using System.Reflection;
using Rb.AssemblySelector;

[assembly: AssemblyIdentifier( "PhysicsApi", "ODE", AddToIdMap = true )]
[assembly: AssemblyIdentifier( "Loader", "Collada" )]

namespace Rb.AssemblySelector.TestApp
{
    class Program
    {
        static void Main(string[] args)
        {
            IdentifierMap.AssemblySelector selector = IdentifierMap.BuildSelector( "name=Rb;Loader=Collada;PhysicsApi" );

            bool match = selector.MatchesFilename( Assembly.GetExecutingAssembly( ).FullName );
            match = selector.MatchesAssembly( Assembly.GetExecutingAssembly( ) );

        }
    }
}
