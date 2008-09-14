using System;
using Poc1.Universe.Interfaces;

namespace Poc1.Universe.Classes.Cameras
{
	/// <summary>
	/// A camera that follows an entity
	/// </summary>
	public class EntityTrackingCamera : PointTrackingCamera
	{
		/// <summary>
		/// Can't modify the look at position (TODO: AP: Make pan change an offset vector rather than the look-at point itself)
		/// </summary>
		public override bool CanModifyLookAtPoint
		{
			get { return false; }
		}

		/// <summary>
		/// Gets the position of the tracked entity. Throws on set
		/// </summary>
		public override UniPoint3 LookAtPoint
		{
			get
			{
				return Entity.Transform.Position;
			}
			set
			{
				throw new InvalidOperationException( "Can't override look-at point in entity tracking camera" );
			}
		}

		/// <summary>
		/// Gets/sets the entity that is followed by this camera (forces update of camera frame)
		/// </summary>
		/// <remarks>
		/// If the entity changes position, the camera's frame will not update until the
		/// next <see cref="PointTrackingCamera.UpdateFrame"/> or <see cref="PointTrackingCamera.Begin"/> call.
		/// </remarks>
		public IBody Entity
		{
			get { return m_Entity; }
			set
			{
				DirtyFrame( m_Entity != value );
				m_Entity = value;
			}
		}

		#region Private Members

		private IBody m_Entity;

		#endregion
	}
}
