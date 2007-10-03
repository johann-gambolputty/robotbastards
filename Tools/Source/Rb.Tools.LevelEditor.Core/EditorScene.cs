using System;
using System.Collections.Generic;
using Rb.Tools.LevelEditor.Core.Selection;
using Rb.World;

namespace Rb.Tools.LevelEditor.Core
{
	/// <summary>
	/// The editor scene encapsulates the runtime scene, and stores extra editor state
	/// </summary>
	[Serializable]
	public class EditorScene : Scene
	{
		/// <summary>
		/// Setup constructor
		/// </summary>
		/// <param name="runtimeScene">The encapsulated runtime scene</param>
		public EditorScene( Scene runtimeScene )
		{
			m_RuntimeScene = runtimeScene;
		}

		/// <summary>
		/// Picks an object from the scene
		/// </summary>
		/// <param name="pick">Pick information</param>
		/// <returns>Returns picked object</returns>
		public object PickObject( IPickInfo pick )
		{
			foreach ( IPickable pickable in Objects.GetAllOfType< IPickable >( ) )
			{
				IPickable testResult = pickable.TestPick( pick );
				if ( testResult != null )
				{
					return testResult;
				}
			}
			return null;
		}
		
		/// <summary>
		/// Picks objects from the scene
		/// </summary>
		/// <param name="pick">Pick information</param>
		/// <returns>Returns picked objects</returns>
		public object[] PickObjects( IPickInfo pick )
		{
			List< object > objects = new List< object >( );
			foreach ( IPickable pickable in Objects.GetAllOfType< IPickable >( ) )
			{
				IPickable testResult = pickable.TestPick( pick );
				if ( testResult != null )
				{
					objects.Add( testResult );
				}
			}
			return objects.ToArray( );
		}

		/// <summary>
		/// Runtime scene
		/// </summary>
		public Scene RuntimeScene
		{
			get { return m_RuntimeScene; }
		}

		private readonly Scene m_RuntimeScene;
	}
}
