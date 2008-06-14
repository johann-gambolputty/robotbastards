
using System;
using System.Windows.Forms;
using Rb.Core.Maths;

namespace Rb.NiceControls
{
	/// <summary>
	/// Base class describing a function
	/// </summary>
	public abstract class FunctionDescriptor
	{
		/// <summary>
		/// Sets up the descriptor
		/// </summary>
		/// <param name="name">Name of the function</param>
		/// <param name="defaultFunction">The default function used by the descriptor</param>
		public FunctionDescriptor( string name, IFunction1d defaultFunction )
		{
			m_Name = name;
			m_Function = defaultFunction;
		}

		/// <summary>
		/// Gets/sets the associated function
		/// </summary>
		public IFunction1d Function
		{
			get { return m_Function; }
			set
			{
				if ( value == null )
				{
					throw new ArgumentNullException( "value" );
				}
				if ( !SupportsFunction( value ) )
				{
					throw new ArgumentException( "Unsupported function type " + value.GetType( ), "value" );
				}
				m_Function = value;
			}
		}

		/// <summary>
		/// Gets the name of this function
		/// </summary>
		public string Name
		{
			get { return m_Name; }
		}

		/// <summary>
		/// Returns the name of the function
		/// </summary>
		public override string ToString( )
		{
			return m_Name;
		}

		/// <summary>
		/// Returns true if the specified function is supported
		/// </summary>
		public abstract bool SupportsFunction( IFunction1d function );

		/// <summary>
		/// Adds this function to an existing control (created by a function of the same type)
		/// </summary>
		public virtual void AddToControl( Control control )
		{
		}

		/// <summary>
		/// Removes this function from an existing control (created by a function of the same type)
		/// </summary>
		public virtual void RemoveFromControl( Control control )
		{
		}

		/// <summary>
		/// Creates a control for this function
		/// </summary>
		public virtual Control CreateControl( )
		{
			return null;
		}

		#region Private Members

		private IFunction1d m_Function;
		private readonly string m_Name;

		#endregion
	}
}
