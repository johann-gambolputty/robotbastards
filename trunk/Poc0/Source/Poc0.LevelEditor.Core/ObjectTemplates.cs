using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Rb.Core.Components;
using Rb.Core.Resources;

namespace Poc0.LevelEditor.Core
{
	/// <summary>
	/// Stores a list of <see cref="ObjectTemplate"/> objects
	/// </summary>
	public class ObjectTemplates
	{
		/// <summary>
		/// Clears all templates
		/// </summary>
		public void Clear( )
		{
			m_Templates.Clear( );
		}

		/// <summary>
		/// Loads a resource containing a list of object templates
		/// </summary>
		/// <param name="resourcePath">Path to the resource</param>
		public void Append( string resourcePath )
		{
			IList result = ( IList )ResourceManager.Instance.Load( resourcePath );
			foreach ( ObjectTemplate template in result )
			{
				m_Templates.Add( template );
			}
		}

		/// <summary>
		/// Gets the collection of templates
		/// </summary>
		public IEnumerable< ObjectTemplate > Templates
		{
			get { return m_Templates; }
		}

		private readonly List< ObjectTemplate > m_Templates = new List< ObjectTemplate >( );
	}
}
