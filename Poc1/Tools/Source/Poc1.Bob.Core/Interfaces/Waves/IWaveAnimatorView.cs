
using Poc1.Tools.Waves;
using Rb.Core.Utils;

namespace Poc1.Bob.Core.Interfaces.Waves
{
	/// <summary>
	/// Wave animator view interface
	/// </summary>
	public interface IWaveAnimatorView
	{
		/// <summary>
		/// Event raised when the generate button is clicked
		/// </summary>
		event ActionDelegates.Action GenerateAnimation;

		/// <summary>
		/// Gets/sets the generation enabled flag
		/// </summary>
		bool GenerationEnabled
		{
			get; set;
		}

		/// <summary>
		/// Sets the progress of the wave animation gneerator
		/// </summary>
		float WaveAnimationGenerationProgress
		{
			set;
		}

		/// <summary>
		/// Gets/sets the wave animation model
		/// </summary>
		WaveAnimationParameters Model
		{
			get; set;
		}

		/// <summary>
		/// Shows the specified wave animation
		/// </summary>
		/// <param name="animation">Wave animation to show</param>
		void ShowAnimation( WaveAnimation animation );

	}
}
