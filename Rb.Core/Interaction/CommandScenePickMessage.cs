using System;

namespace Rb.Core.Interaction
{
	/// <summary>
	/// Picks a point in the scene
	/// </summary>
	public class CommandScenePickMessage : CommandMessage
	{
		/// <summary>
		/// Setup constructor
		/// </summary>
		public CommandScenePickMessage( Command cmd, Maths.Ray3Intersection intersection ) :
			base( cmd )
		{
			m_Intersection = intersection;
		}

		/// <summary>
		/// Gets the pick intersection
		/// </summary>
		public Maths.Ray3Intersection Intersection
		{
			get
			{
				return m_Intersection;
			}
		}

		/// <summary>
		/// Reads this message
		/// </summary>
		protected override void Read( System.IO.BinaryReader input )
		{
			base.Read( input );

			//	TODO: ...
		}

		/// <summary>
		/// Writes this message
		/// </summary>
		public override void Write( System.IO.BinaryWriter output )
		{
			base.Write( output );
			//	TODO: ...
		}

		private Maths.Ray3Intersection m_Intersection;
	}
}
