using System;
using NUnit.Framework;
using Rb.Core.Components;

namespace Rb.World.Tests
{
	[TestFixture]
	public class TestScene
	{
		private class SceneObject : Node, ISceneObject, IUnique
		{
			public SceneObject( params SceneObject[] children )
			{
				foreach ( SceneObject child in children )
				{
					AddChild( child );
				}
			}

			public void CheckAddCount( int expected )
			{
				Assert.AreEqual( expected, m_AddCount );
				foreach ( SceneObject child in Children )
				{
					child.CheckAddCount( expected );
				}
			}

			#region ISceneObject Members

			public void AddedToScene( Scene scene )
			{
				Assert.IsTrue( ++m_AddCount == 1 );
			}

			public void RemovedFromScene( Scene scene )
			{
				Assert.IsTrue( --m_AddCount == 0 );
			}

			#endregion

			#region IUnique Members

			public Guid Id
			{
				get { return m_Id; }
				set { m_Id = value; }
			}

			#endregion

			private int m_AddCount = 0;
			private Guid m_Id = Guid.NewGuid( );
		}

		[Test]
		public void TestSceneGraph( )
		{
			Scene scene = new Scene( );

			SceneObject subTree = new SceneObject( new SceneObject( ), new SceneObject( ) );

			SceneObject root =
				new SceneObject
				(
					new SceneObject( ),
					subTree,
					new SceneObject( )
				);

			scene.Objects.Add( root );
			root.CheckAddCount( 1 );

			root.RemoveChild( subTree );
			subTree.CheckAddCount( 0 );
		}
	}
}
