using System;

namespace Poc1.Tools.Atmosphere
{
	/// <summary>
	/// Atmosphere build progress class. <see cref="Atmosphere.BuildLookupTexture"/>
	/// </summary>
	public class AtmosphereBuildProgress
	{
		/// <summary>
		/// Event, invoked when a slice of the lookup texture is completed by <see cref="Atmosphere.BuildLookupTexture"/>
		/// </summary>
		public event Action<float> SliceCompleted;

		/// <summary>
		/// Gets/sets the cancellation flag
		/// </summary>
		public bool Cancel
		{
			get { return m_Cancel; }
			set { m_Cancel = value; }
		}

		/// <summary>
		/// Invokes the SliceCompleted event
		/// </summary>
		public void OnSliceCompleted( float progress )
		{
			if ( SliceCompleted != null )
			{
				SliceCompleted( progress );
			}
		}

		private bool m_Cancel;
	}
}
