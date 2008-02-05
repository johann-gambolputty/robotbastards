using System;
using Rb.Core.Utils;
using Rb.Core.Maths;
using Rb.Rendering;

namespace Rb.World.Entities
{
    /// <summary>
    /// Entity occupying 3d space
    /// </summary>
    public class Entity3d : Entity
    {
        #region Position

		/// <summary>
		/// The current position of the entity
		/// </summary>
        public Point3Interpolator CurrentPosition
        {
            get { return m_Position;  }
            set { m_Position = value; }
        }

		/// <summary>
		/// The next position of the entity
		/// </summary>
        public Point3 NextPosition
        {
            get { return m_Position.End; }
            set { m_Position.End = value; }
        }

        #endregion

		#region Orientation

		/// <summary>
		/// Ahead facing vector
		/// </summary>
		public Vector3 Ahead
		{
			get { return m_ZAxis; }
		}

		/// <summary>
		/// Back facing vector
		/// </summary>
		public Vector3 Back
		{
			get { return m_ZAxis * -1.0f; }
		}

		/// <summary>
		/// Left facing vector
		/// </summary>
		public Vector3 Left
		{
			get { return m_XAxis; }
		}

		/// <summary>
		/// Right facing vector
		/// </summary>
		public Vector3 Right
		{
			get { return m_XAxis * -1.0f; }
		}

		/// <summary>
		/// Up facing vector
		/// </summary>
		public Vector3 Up
		{
			get { return m_YAxis; }
		}

		/// <summary>
		/// Down facing vector
		/// </summary>
		public Vector3 Down
		{
			get { return m_YAxis * -1.0f; }
		}

		/// <summary>
		/// Sets the left, up and ahead vectors
		/// </summary>
		public void SetFrame( Vector3 left, Vector3 up, Vector3 ahead )
		{
			m_XAxis = left;
			m_YAxis = up;
			m_ZAxis = ahead;
		}

		#endregion

		#region Updates

		/// <summary>
		/// Updates the entity
		/// </summary>
		/// <param name="updateClock">Clock causing updates</param>
		public override void Update( Clock updateClock )
		{
			m_Position.Step( updateClock.CurrentTickTime );
		}

		#endregion

		#region Rendering

		/// <summary>
		/// Renders the entity
		/// </summary>
		/// <param name="context">Rendering context</param>
		public override void Render( IRenderContext context )
		{
			//	Get the interpolated position of the entity
			float t = ( float )( context.RenderTime - m_Position.LastStepTime ) / m_Position.LastStepInterval;
			Point3 curPos = m_Position.UpdateCurrent( t );

			//	TODO: Get the interpolated rotation of the entity

			//	Push the entity transform
			Matrix44 mat = new Matrix44( curPos, Left, Up, Ahead );

			Rb.Rendering.Graphics.Renderer.PushTransform( Transform.LocalToWorld, mat );

			base.Render( context );

			//	Pop the entity transform
			Rb.Rendering.Graphics.Renderer.PopTransform(Transform.LocalToWorld);
		}

		#endregion

		#region Message handling

		//	TODO: AP: Movement message handling should be in a separate object?

		/// <summary>
		/// Handles a movement request message
		/// </summary>
		/// <param name="movement">Movement request message</param>
		[Dispatch]
		public void HandleMovement( MovementXzRequest movement )
		{
			NextPosition += new Vector3( movement.DeltaX, 0, movement.DeltaZ );
		}

		/// <summary>
		/// Handles rotation request message
		/// </summary>
		/// <param name="rotation">Rotation request message</param>
		[Dispatch]
		public void HandleRotation( TurnRequest rotation )
		{
			float x = Functions.Cos( rotation.Rotation );
			float z = Functions.Sin( rotation.Rotation );

			m_ZAxis = new Vector3( x, 0, z );
			m_XAxis = Vector3.Cross( m_YAxis, m_ZAxis ).MakeNormal( );
		}

		#endregion

		#region Private stuff

		private Point3Interpolator	m_Position	= new Point3Interpolator( );
		private Vector3 			m_XAxis 	= Vector3.XAxis;
		private Vector3 			m_YAxis 	= Vector3.YAxis;
		private Vector3 			m_ZAxis 	= Vector3.ZAxis;

		#endregion
	}
}
