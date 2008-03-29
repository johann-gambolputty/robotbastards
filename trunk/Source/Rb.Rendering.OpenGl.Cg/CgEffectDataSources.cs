using System;
using Rb.Rendering;
using Rb.Rendering.Interfaces.Objects;

namespace Rb.Rendering.OpenGl.Cg
{
	/// <summary>
	/// CG shader parameter bindings
	/// </summary>
	public class CgEffectDataSources : EffectDataSourcesBase
	{
		public CgEffectDataSources( )
		{
			foreach ( object binding in Enum.GetValues( typeof( EffectRenderStateBinding ) ) )
			{
				IEffectDataSource dataSource = new CgEffectRenderStateDataSource( ( EffectRenderStateBinding )binding );
				ParameterNameDataSources.Add( binding.ToString( ), dataSource );
				SemanticNameDataSources.Add( binding.ToString( ), dataSource );
			}
		}

		protected override IEffectValueDataSource<T> CreateValueDataSource<T>( )
		{
			return new CgEffectValueDataSource<T>( );
		}
	}
}
