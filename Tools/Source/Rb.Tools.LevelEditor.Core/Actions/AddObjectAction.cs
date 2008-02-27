using System;
using Rb.Core.Components;
using Rb.Core.Maths;
using Rb.Tools.LevelEditor.Core.Selection;

namespace Rb.Tools.LevelEditor.Core.Actions
{
	/// <summary>
	/// Adds an object to the scene
	/// </summary>
	public class AddObjectAction : IAction
	{
		/// <summary>
		/// Action setup constructor
		/// </summary>
		public AddObjectAction( object template, ILineIntersection pick, Guid id )
		{
			m_Id = id;
			m_Instance = CreateInstance( template, pick );

			Redo( );
		}

		#region IAction Members

		/// <summary>
		/// Undoes this action, by removing the object edit state from the level editor scene, and the actual object
		/// from the runtime scene
		/// </summary>
		public void Undo( )
		{
			EditorState.Instance.CurrentScene.Objects.Remove( m_Id );
		}

		/// <summary>
		/// Redoes this action, by adding the object edit state to the level editor scene, and the actual object
		/// to the runtime scene
		/// </summary>
		public void Redo( )
		{
			EditorState.Instance.CurrentScene.Objects.Add( m_Id, m_Instance );
		}

		#endregion

		#region Private members

		private readonly Guid m_Id;
		private readonly object m_Instance;

		private static object CreateInstance( object template, ILineIntersection pick )
		{
			object instance = CreateInstance( template );
			IPlaceableObjectEditor placeable = instance as IPlaceableObjectEditor;
			if ( placeable != null )
			{
				placeable.Place( pick );
			}
			return instance;
		}
		
		private static object CreateInstance( object template )
		{
			if ( template is IInstanceBuilder )
			{
				return ( ( IInstanceBuilder )template ).CreateInstance( Builder.Instance );
			}

			if ( template is ICloneable )
			{
				return ( ( ICloneable )template ).Clone( );
			}

			throw new ArgumentException( "Template must implement ICloneable or IInstanceBuilder", "template" );
		}

		#endregion
	}
}
