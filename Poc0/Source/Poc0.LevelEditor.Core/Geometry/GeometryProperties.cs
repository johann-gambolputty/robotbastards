
using System;
using System.ComponentModel;

namespace Poc0.LevelEditor.Core.Geometry
{
	/// <summary>
	/// Information about a piece of static geometry (rendering properties, sound properties, etc.)
	/// </summary>
	[Serializable]
	public class GeometryProperties
	{
		/// <summary>
		/// Gets/sets the name of this material
		/// </summary>        
		[Editor( typeof( MaterialUITypeEditor ), typeof( System.Drawing.Design.UITypeEditor ) )]        
		public Material Material
		{
			get { return m_Material; }
			set { m_Material = value; }
		}

		#region Private members

		private Material m_Material;

		#endregion
	}
}
