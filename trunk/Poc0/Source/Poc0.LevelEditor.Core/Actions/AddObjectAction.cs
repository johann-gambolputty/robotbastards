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
		/// <summary>
		/// Action setup constructor
		/// </summary>
		public AddObjectAction( Scene scene, ICloneable builder, float x, float y, Guid id )
		{
			m_Id = id;
			m_Scene = scene;
			m_Instance = builder.Clone( );

			ObjectTemplate template = m_Instance as ObjectTemplate;
			if ( template != null )
			{
				ObjectTemplate frameTemplate = template.FindTemplateImplementingInterface< IHasWorldFrame >( );
				if ( frameTemplate != null )
				{
					frameTemplate.AddChild( new HasWorldFrame( x, y ) );
					frameTemplate.AddChild( new ObjectEditState( scene, frameTemplate ) );
				}

				//	TODO: AP: This is RUBBISH
				template[ "Id" ] = id;
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
