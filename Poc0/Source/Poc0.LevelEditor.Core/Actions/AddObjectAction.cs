using System;
using System.Collections.Generic;
using System.Text;
using Poc0.Core;
using Rb.Core.Components;
using Rb.World;
using Rb.Core.Maths;

namespace Poc0.LevelEditor.Core.Actions
{
	class AddObjectAction : IAction
	{
		public AddObjectAction( Scene scene, ICloneable builder, float x, float y, Guid id )
		{
			m_Id = id;
			m_Scene = scene;
			m_Instance = builder.Clone( );

			ObjectPattern pattern = m_Instance as ObjectPattern;
			if ( pattern != null )
			{
				ObjectPattern framePattern = pattern.FindPatternImplementingInterface< IHasWorldFrame >( );
				if ( framePattern != null )
				{
					framePattern.AddChild( new HasWorldFrame( x, y ) );
					framePattern.AddChild( new ObjectEditState( scene, framePattern ) );
				}

				//	TODO: AP: This is RUBBISH
				pattern[ "Id" ] = id;
			}

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
