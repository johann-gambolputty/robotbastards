using System;
using System.Collections;
using System.Text;
using System.Windows.Forms;
using Poc0.LevelEditor.EditModes.Controls;
using Poc0.LevelEditor.Properties;
using Rb.Assets;
using Rb.Assets.Interfaces;
using Rb.Core.Components;
using Rb.Core.Maths;
using Rb.Log;
using Rb.Tools.LevelEditor.Core;
using Rb.Tools.LevelEditor.Core.Actions;
using Rb.Tools.LevelEditor.Core.Controls.Forms;
using Rb.Tools.LevelEditor.Core.EditModes;
using Rb.Tools.LevelEditor.Core.Selection;
using Rb.World;

namespace Poc0.LevelEditor.EditModes
{
	/// <summary>
	/// Edit mode used for adding and removing objects
	/// </summary>
	public class ObjectEditMode : EditMode
	{
		/// <summary>
		/// Loads all object templates from the "Editor/Templates" directory
		/// </summary>
		public ObjectEditMode( RayCastOptions pickOptions )
		{
			m_PickOptions = pickOptions;
			m_Templates = BuildObjectTemplates( );
			if ( m_Templates.Count > 0 )
			{
				m_Template = m_Templates[ 0 ];
			}
		}

		/// <summary>
		/// Gets/sets the template object used to create new objects in the scene
		/// </summary>
		public object CurrentTemplate
		{
			get { return m_Template; }
			set { m_Template = value; }
		}

		/// <summary>
		/// Gets the list of available templates
		/// </summary>
		public IEnumerable Templates
		{
			get { return m_Templates; }
		}

		/// <summary>
		/// Gets the display name of this edit mode
		/// </summary>
		public override string DisplayName
		{
			get { return Resources.ObjectEditModeName; }
		}

		/// <summary>
		/// Creates a control for this edit mode
		/// </summary>
		public override Control CreateControl( )
		{
			return new ObjectEditModeControl( this );
		}

		/// <summary>
		/// Gets the mouse buttons used by this edit mode
		/// </summary>
		public override MouseButtons Buttons
		{
			get { return MouseButtons.Right; }
		}
		
		/// <summary>
		/// Returns a description of the edit mode inputs
		/// </summary>
		public override string InputDescription
		{
			get
			{
				return string.Format(  Resources.AddObjectInputs, ResourceHelper.MouseButtonName( Buttons ) );
			}
		}
		
		/// <summary>
		/// Binds to the specified control
		/// </summary>
		/// <param name="control">Control to bind to</param>
		protected override void BindToControl( Control control )
		{
			control.MouseClick += OnMouseClick;
		}

		/// <summary>
		/// Unbinds to the specified control
		/// </summary>
		/// <param name="control">Control to unbind from</param>
		protected override void UnbindFromControl( Control control )
		{
			control.MouseClick -= OnMouseClick;
		}

		#region Control event handlers

		/// <summary>
		/// Handles mouse click events. Adds an object to the tile under the cursor
		/// </summary>
		private void OnMouseClick( object sender, MouseEventArgs args )
		{
			if ( args.Button != MouseButtons.Right )
			{
				return;
			}
			IPicker picker = ( IPicker )sender;

			ILineIntersection pick = picker.FirstPick( args.X, args.Y, m_PickOptions );
			if ( pick != null )
			{
				Guid id = Guid.NewGuid( );

				try
				{
					IAction addAction = new AddObjectAction( m_Template, pick, id );
					EditorState.Instance.CurrentUndoStack.Push( addAction );
				}
				catch ( Exception ex )
				{
					AppLog.Exception( ex, "Failed to add new object" );
					ErrorMessageBox.Show( Resources.AddObjectFailed );
				}
			}
		}

		#endregion
		#region Private members

		private object m_Template;
		private readonly ArrayList m_Templates = new ArrayList( );
		private readonly RayCastOptions m_PickOptions;
		
		/// <summary>
		/// Builds a list of object templates from the contents of the "Editor/Templates" directory
		/// </summary>
		/// <returns></returns>
		private static ArrayList BuildObjectTemplates( )
		{
			ArrayList templates = new ArrayList( );

			const string templatesLocation = "Editor/Templates";
			StringBuilder invalidTemplates = new StringBuilder( );

			//	TODO: AP: Make directory traversal part of the location manager
			IFolder folder = Locations.Instance.GetFolder( templatesLocation );
			foreach ( IFile file in folder.Files )
			{
				object loadResult = AssetManager.Instance.Load( file );
				if ( loadResult is ObjectTemplate )
				{
					templates.Add( loadResult );
				}
				else if ( loadResult is IList )
				{
					foreach ( ObjectTemplate template in ( IList )loadResult )
					{
						templates.Add( template );
					}
				}
				else
				{
					invalidTemplates.AppendLine( file.Name );
				}
			}

			if ( invalidTemplates.Length > 0 )
			{
				AppLog.Error( "\"{0}\" directory contained file(s) that did not contain a templates:\n", invalidTemplates );
				ErrorMessageBox.Show( EditorForm.Instance, string.Format( Resources.FoundInvalidTemplates, invalidTemplates ) );
			}

			return templates;
		}

		#endregion
	}
}
