using System;
using System.Collections;
using RbEngine.Animation;

namespace RbOpenGlMd3Loader
{
	/// <summary>
	/// MD3s have a fixed animation set, which this enumeration describes
	/// </summary>
	public enum AnimationType
	{
		BothDeath1,
		BothDead1,
		BothDeath2,
		BothDead2,
		BothDeath3,
		BothDead3,

		TorsoGesture,

		TorsoAttack,
		TorsoAttack2,

		TorsoDrop,
		TorsoRaise,

		TorsoStand,
		TorsoStand2,

		LegsWalkCrouch,
		LegsWalk,
		LegsRun,
		LegsBack,
		LegsSwim,

		LegsJump,
		LegsLand,

		LegsJumpBack,
		LegsLandBack,

		LegsIdle,
		LegsIdleCrouch,

		LegsTurn,

		NumAnimations,

		FirstTorsoAnim	= TorsoGesture,
		FirstLegAnim	= LegsWalkCrouch
	}

	/// <summary>
	/// A set of MD3 animations that can be applied to a particular layer (MD3 surface)
	/// </summary>
	public class AnimationSet : IAnimationSet
	{
		/// <summary>
		/// Gets the animation frame array
		/// </summary>
		public Animation[]	Animations
		{
			get
			{
				return m_Animations;
			}
		}

		/// <summary>
		/// Gets an animation
		/// </summary>
		public Animation	GetAnimation( AnimationType animType )
		{
			return m_Animations[ ( int )animType ];
		}

		#region	IAnimationSet Members

		/// <summary>
		/// Finds an animation by name
		/// </summary>
		public IAnimation	Find( string animationName )
		{
			return m_Animations[ ( int )Enum.Parse( typeof( AnimationType ), animationName, true ) ];
		}

		#endregion

		private Animation[]			m_Animations	= new Animation[ ( int )AnimationType.NumAnimations ];
	}
}
