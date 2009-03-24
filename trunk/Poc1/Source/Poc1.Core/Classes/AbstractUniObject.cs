
using Poc1.Core.Interfaces;

namespace Poc1.Core.Classes
{
	/// <summary>
	/// Abstract universe object
	/// </summary>
	public class AbstractUniObject : IUniObject
	{
		#region IUniObject Members

		/// <summary>
		/// Gets the transform for this object
		/// </summary>
		public UniTransform Transform
		{
			get { return m_Transform; }
		}

		#endregion

		#region Private Members

		private readonly UniTransform m_Transform = new UniTransform( );

		#endregion
	}
}
