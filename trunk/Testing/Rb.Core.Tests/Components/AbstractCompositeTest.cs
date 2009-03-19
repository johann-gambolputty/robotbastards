using NUnit.Framework;
using Rb.Core.Components;

namespace Rb.Core.Tests.Components
{
	/// <summary>
	/// Base class for composite tests
	/// </summary>
	[TestFixture]
	public class AbstractCompositeTest
	{
		/// <summary>
		/// Ensures that changing the Owner of a component works
		/// </summary>
		[Test]
		public void TestComponentOwner( )
		{
			IComponent component = new Component( );
			IComposite composite0 = CreateComposite( );
			IComposite composite1 = CreateComposite( );

			//	Add the component to a composite
			component.Owner = composite0;
			Assert.AreEqual( composite0, component.Owner );
			Assert.IsTrue( composite0.Components.Contains( component ) );

			//	Change the owner of the component
			component.Owner = composite1;
			Assert.AreEqual( composite1, component.Owner );
			Assert.IsTrue( composite1.Components.Contains( component ) );
			Assert.IsFalse( composite0.Components.Contains( component ) );

			//	Remove the component from its current owner
			component.Owner = null;
			Assert.IsNull( component.Owner );
			Assert.IsFalse( composite0.Components.Contains( component ) );
			Assert.IsFalse( composite1.Components.Contains( component ) );
		}

		protected virtual IComposite CreateComposite( ) { return new Composite( ); }
	}
}
