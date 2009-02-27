
namespace Rb.Core.Components
{
	/// <summary>
	/// Simple implementation of <see cref="IComponent"/>
	/// </summary>
	public class Component : IComponent
	{
		/// <summary>
		/// Changes the owner of any IComponent
		/// </summary>
		public static void ChangeOwner( IComponent component, ref IComposite currentOwner, IComposite newOwner )
		{
			if ( currentOwner == newOwner )
			{
				return;
			}
			IComposite oldOwner = currentOwner;
			currentOwner = null;
			if ( oldOwner != null )
			{
				oldOwner.Remove( component );
			}
			if ( newOwner != null )
			{
				currentOwner = newOwner;
				newOwner.Add( component );
			}
		}

		#region IComponent Members

		/// <summary>
		/// Gets/sets the owner of this component
		/// </summary>
		/// <remarks>
		/// If the owner is set, the component is added to the component
		/// list of the specified composite, and removed from the component list
		/// of the previous owner.
		/// </remarks>
		public virtual IComposite Owner
		{
			get { return m_Owner; }
			set { ChangeOwner( this, ref m_Owner, value ); }
		}

		#endregion

		#region Private Members

		private IComposite m_Owner;

		#endregion
	}
}
