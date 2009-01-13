using System;
using System.Drawing;
using System.IO;
using Bob.Core.Ui.Interfaces;

namespace Poc1.Bob.Core.Interfaces.Templates
{
	/// <summary>
	/// Project type
	/// </summary>
	public abstract class Template : TemplateBase
	{
		/// <summary>
		/// Setup constructor
		/// </summary>
		/// <param name="name">Name of this template</param>
		/// <param name="description">Description of this template</param>
		public Template( string name, string description ) :
			this( null, name, description )
		{
		}

		/// <summary>
		/// Setup constructor
		/// </summary>
		/// <param name="name">Name of this template</param>
		/// <param name="description">Description of this template</param>
		/// <param name="icon">Icon for this template (if null, default icon is used)</param>
		public Template( Icon icon, string name, string description ) :
			base( name, description, icon )
		{
		}

		/// <summary>
		/// Gets the views associated with this template
		/// </summary>
		public virtual IViewInfo[] Views
		{
			get { return new IViewInfo[ 0 ]; }
		}

		#region Instances

		/// <summary>
		/// Gets the file extension used to store serialized instances of this template
		/// </summary>
		/// <remarks>
		/// Should not return initial '.' (e.g. return "xml", not ".xml")
		/// </remarks>
		public abstract string SupportedExtension
		{
			get;
		}

		/// <summary>
		/// Opens an instance from a file
		/// </summary>
		/// <param name="location">File location</param>
		/// <returns>Returns the new instance</returns>
		public virtual TemplateInstance OpenInstance( string location )
		{
			using ( FileStream stream = new FileStream( location, FileMode.Open, FileAccess.Read ) )
			{
				return OpenInstance( location, stream );
			}
		}

		/// <summary>
		/// Opens an instance from a stream
		/// </summary>
		/// <param name="streamLocation">Location of the stream. Used to identify the stream in exceptions</param>
		/// <param name="stream">Stream containing serialized instance</param>
		/// <returns>Returns the new template instance</returns>
		/// <exception cref="ArgumentNullException">Thrown if stream is null</exception>
		public abstract TemplateInstance OpenInstance( string streamLocation, Stream stream );

		/// <summary>
		/// Creates an instance of this template
		/// </summary>
		/// <returns>Returns the new instance</returns>
		public abstract TemplateInstance CreateInstance( string name );

		#endregion
	}
}
