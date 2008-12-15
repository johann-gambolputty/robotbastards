using NUnit.Framework;
using Rb.Interaction.Classes;
using Rb.Interaction.Interfaces;

namespace Rb.Interaction.Tests
{
	/// <summary>
	/// Implements <see cref="TestICommandUserFactory"/> to test <see cref="LocalCommandUserFactory"/>
	/// </summary>
	[TestFixture]
	public class TestLocalCommandUserFactory : TestICommandUserFactory
	{
		/// <summary>
		/// Creates a LocalCommandUserFactory instance
		/// </summary>
		protected override ICommandUserFactory CreateFactory( )
		{
			return new LocalCommandUserFactory( );
		}
	}

}
