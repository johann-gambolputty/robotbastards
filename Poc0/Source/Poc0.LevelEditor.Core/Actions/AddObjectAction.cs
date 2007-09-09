using System;
using Rb.World;

namespace Poc0.LevelEditor.Core.Actions
{
	class AddObjectAction : IAction
	{
		/// <summary>
		/// Action setup constructor
		/// </summary>
		public AddObjectAction( Scene scene, object template, float x, float y, Guid id )
		{
			m_Id = id;
			m_Scene = scene;
			//m_Instance = new ObjectEditState( scene, x, y, builder.CreateInstance( Builder.Instance ) );
			m_Instance = new ObjectEditState( scene, x, y, ( ( ICloneable )template ).Clone( ) );

			Redo( );
		}

		#region IAction Members

		public void Undo( )
		{
			m_Scene.Objects.Remove( m_Id );
		}

		public void Redo( )
		{
			m_Scene.Objects.Add( m_Id, m_Instance );
		}

		#endregion

		#region Private members

		private readonly Scene m_Scene;
		private readonly Guid m_Id;
		private readonly object m_Instance;

		#endregion
	}
}
