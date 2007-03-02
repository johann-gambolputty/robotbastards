using System;


namespace RbOpenGlMd3Loader
{
	/// <summary>
	/// An MD3 animation
	/// </summary>
	public class Animation : RbEngine.Animation.IAnimation
	{
		#region	INamedObject Members

		/// <summary>
		/// Event, invoked when the name is changed
		/// </summary>
		public event RbEngine.Components.NameChangedDelegate NameChanged;

		/// <summary>
		/// Sets the name of this object
		/// </summary>
		public string	Name
		{
			get
			{
				return m_Name;
			}
			set
			{
				m_Name = value;
				if ( NameChanged != null )
				{
					NameChanged( this );
				}
			}
		}

		#endregion

		#region	IAnimation Members

		#endregion

		#region	Private stuff

		private string	m_Name;

		#endregion
	}
}
