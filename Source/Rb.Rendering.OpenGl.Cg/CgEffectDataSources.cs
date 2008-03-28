using System;
using Rb.Rendering.Base;
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
			//foreach ( object renderStateValue in Enum.GetValues( typeof( CgEffectRenderStateDataSource.RenderStateSource ) ) )
			//{
			//    ParameterNameDataSources.Add( renderStateValue.ToString( ), new CgEffectRenderStateDataSource( ( );
			//}
		}

		protected override IEffectValueDataSource<T> CreateValueDataSource<T>( )
		{
			return new CgEffectValueDataSource<T>( );
		}
	}
}
