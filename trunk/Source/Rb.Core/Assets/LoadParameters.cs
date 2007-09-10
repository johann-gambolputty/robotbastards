using System;
using Rb.Core.Components;

namespace Rb.Core.Assets
{
	/// <summary>
	/// Parameters, passed to <see cref="IAssetLoader.Load"/>
	/// </summary>
	public class LoadParameters : ISupportsDynamicProperties, ICloneable
	{
		/// <summary>
		/// Default constructor. No target, and results are not added to the cache
		/// </summary>
		public LoadParameters( )
		{
		}

		/// <summary>
		/// Sets an object that is loaded into
		/// </summary>
		/// <param name="target">Target object</param>
		public LoadParameters( Object target )
		{
			m_Target = target;
		}

		/// <summary>
		/// Access to the target object
		/// </summary>
		public Object Target
		{
			get { return m_Target; }
			set { m_Target = value; }
		}

		/// <summary>
		/// Flag to determine if the loaded asset should be added to the asset cache
		/// </summary>
		public bool CanCache
		{
			get { return m_CanCache; }
			set { m_CanCache = value; }
		}

		#region ISupportsDynamicProperties Members

		/// <summary>
		/// Gets loader dynamic properties
		/// </summary>
		public IDynamicProperties Properties
		{
			get { return m_Properties; }
		}

		#endregion

		#region Private stuff

		private bool m_CanCache;
		private object m_Target;
		private IDynamicProperties m_Properties = new DynamicProperties( );

		#endregion

		#region ICloneable Members

		/// <summary>
		/// Clones this object
		/// </summary>
		/// <returns>Deep copy clone</returns>
		public virtual object Clone( )
		{
			LoadParameters clone = new LoadParameters( );
			DeepCopy( clone );
			return clone;
		}

		/// <summary>
		/// Copies members from this LoadParameters to parameters
		/// </summary>
		protected void DeepCopy( LoadParameters parameters )
		{
			parameters.m_Target = m_Target;
			parameters.m_Properties = m_Properties;
		}

		#endregion
	}
}
