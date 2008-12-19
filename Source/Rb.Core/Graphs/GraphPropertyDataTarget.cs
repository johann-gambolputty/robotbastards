
using System.ComponentModel;

namespace Rb.Core.Graphs
{
	/// <summary>
	/// A data target defined by a property descriptor
	/// </summary>
	public class GraphPropertyDataTarget : IGraphDataTarget
	{
		/// <summary>
		/// Setup constructor
		/// </summary>
		/// <param name="instance">Object instance, owner of the property</param>
		/// <param name="property">Target property to set/get</param>
		public GraphPropertyDataTarget( object instance, PropertyDescriptor property )
		{
			m_Instance = instance;
			m_Property = property;
		}

		#region IGraphDataTarget Members

		/// <summary>
		/// Gets the name of this target
		/// </summary>
		public string Name
		{
			get { return m_Property.Name; }
		}

		/// <summary>
		/// Gets/sets the value of the target
		/// </summary>
		public object Value
		{
			get { return m_Property.GetValue( m_Instance ); }
			set { m_Property.SetValue( m_Instance, value ); }
		}

		#endregion
		
		#region Private Members

		private readonly object m_Instance;
		private readonly PropertyDescriptor m_Property;

		#endregion


	}
}
