using Rb.Tools.LevelEditor.Core.Actions;

namespace Rb.Tools.LevelEditor.Core.Selection
{
	/// <summary>
	/// Interface, used by <see cref="DeleteObjectAction"/> 
	/// </summary>
	public interface IDelete
	{
		/// <summary>
		/// Adds this object back
		/// </summary>
		void UnDelete( );

		/// <summary>
		/// Deletes this object
		/// </summary>
		void Delete( );
	}
}
