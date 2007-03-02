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

		NumAnimations
	}

	/// <summary>
	/// A set of MD3 animations that can be applied to a particular layer (MD3 surface)
	/// </summary>
	public class AnimationSet : IAnimationSet
	{
		/// <summary>
		/// Adds an animation to this set
		/// </summary>
		public void			Add( Animation anim )
		{
			m_Animations[ anim.Name ] = anim;
		}

		#region	IAnimationSet Members

		/// <summary>
		/// Finds an animation by name
		/// </summary>
		public IAnimation	Find( string animationName )
		{
			return ( IAnimation )m_Animations[ animationName ];
		}

		#endregion

		private Hashtable	m_Animations = new Hashtable( );
	}
}
