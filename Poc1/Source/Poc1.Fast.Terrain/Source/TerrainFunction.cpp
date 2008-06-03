#include "stdafx.h"
#include "TerrainFunction.h"
#include "FractalTerrainParameters.h"
#include "Mem.h"
#include "Sse\SseSphereTerrainGenerator.h"

namespace Poc1
{
	namespace Fast
	{
		namespace Terrain
		{
			template < typename HeightFunction, typename ParametersType >
			SseSphereTerrainGeneratorT< SseSphereFunction3dDisplacer< HeightFunction > >* CreateSphereFunction( ParametersType^ params )
			{
				SseSphereTerrainGeneratorT< SseSphereFunction3dDisplacer< HeightFunction > >* result;
				result = new ( Aligned( 16 ) ) SseSphereTerrainGeneratorT< SseSphereFunction3dDisplacer< HeightFunction > >( );
				params->Setup( result->GetDisplacer( ).GetFunction( ) );
				return result;
			}
			
			template < typename HeightFunction, typename HeightParametersType, typename GroundFunction, typename GroundParametersType >
			SseSphereTerrainGeneratorT< SseSphereFull3dDisplacer< GroundFunction, HeightFunction > >* CreateSphereFunction( HeightParametersType^ heightParams, GroundParametersType^ groundParams )
			{
				SseSphereTerrainGeneratorT< SseSphereFull3dDisplacer< GroundFunction, HeightFunction > >* result;
				result = new ( Aligned( 16 ) ) SseSphereTerrainGeneratorT< SseSphereFull3dDisplacer< GroundFunction, HeightFunction > >( );
				groundParams->Setup( result->GetDisplacer( ).GetFunction( ) );
				heightParams->Setup( result->GetDisplacer( ).GetBaseDisplacer( ).GetFunction( ) );
				return result;
			}

			template < typename HeightFunction, typename HeightParametersType >
			TerrainGenerator* CreateSphereFunction( HeightParametersType^ heightParameters, TerrainFunctionType groundFunction, System::Object^ groundParameters )
			{
				switch ( groundFunction )
				{
					case TerrainFunctionType::SimpleFractal :
						return CreateSphereFunction< HeightFunction, HeightParametersType, SseSimpleFractal >( heightParameters, ( FractalTerrainParameters^ )groundParameters );
						
					case TerrainFunctionType::RidgedFractal :
						return CreateSphereFunction< HeightFunction, HeightParametersType, SseRidgedFractal >( heightParameters, ( FractalTerrainParameters^ )groundParameters );
				}
				throw gcnew System::NotImplementedException( );
			//	SseSphereTerrainGeneratorT< SseFull3dDisplacer< GroundFunction, HeightFunction > >* result;
			//	result = new ( Aligned( 16 ) ) SseSphereTerrainGeneratorT< SseFull3dDisplacer< GroundFunction, HeightFunction > >( );
			//	groundParams->Setup( result->GetDisplacer( ).GetFunction( ) );
			//	heightParams->Setup( result->GetDisplacer( ).GetBaseDisplacer( ).GetFunction( ) );
			//	return result;
			}


			TerrainFunction::TerrainFunction( TerrainFunctionType functionType )
			{
				m_FunctionType = functionType;
				m_Parameters = CreateParameters( functionType );
			}

			System::String^ TerrainFunction::Name( TerrainFunctionType functionType )
			{
				switch ( functionType )
				{
					case TerrainFunctionType::SimpleFractal : return "Simple Fractal";
					case TerrainFunctionType::RidgedFractal : return "Ridged Fractal";
				}
				throw gcnew System::NotImplementedException( );
			}

			System::Object^ TerrainFunction::CreateParameters( TerrainFunctionType functionType )
			{
				switch ( functionType )
				{
					case TerrainFunctionType::SimpleFractal : return gcnew FractalTerrainParameters( );
					case TerrainFunctionType::RidgedFractal : return gcnew FractalTerrainParameters( );
				}
				throw gcnew System::NotImplementedException( );
			}

			TerrainGenerator* TerrainFunction::CreateGenerator( TerrainGeometry geometry, TerrainFunction^ heightFunction )
			{
				switch ( geometry )
				{
					case TerrainGeometry::Plane :
						{
							switch ( heightFunction->FunctionType )
							{
								case TerrainFunctionType::Flat :
								case TerrainFunctionType::SimpleFractal : 
								case TerrainFunctionType::RidgedFractal : break;
							}
							throw gcnew System::NotImplementedException( );
						};
					case TerrainGeometry::Sphere :
						{
							switch ( heightFunction->FunctionType )
							{
								case TerrainFunctionType::Flat :
									return new ( Aligned( 16 ) ) SseSphereTerrainGeneratorT< SseFlatSphereTerrainDisplacer >( );
								case TerrainFunctionType::SimpleFractal : return CreateSphereFunction< SseSimpleFractal >( ( FractalTerrainParameters^ )heightFunction->Parameters );
								case TerrainFunctionType::RidgedFractal : return CreateSphereFunction< SseRidgedFractal >( ( FractalTerrainParameters^ )heightFunction->Parameters );
							}
						};
				}

				return 0;
			}

			TerrainGenerator* TerrainFunction::CreateGenerator( TerrainGeometry geometry, TerrainFunction^ heightFunction, TerrainFunction^ groundFunction )
			{
				switch ( geometry )
				{
					case TerrainGeometry::Plane :
						{
							switch ( heightFunction->FunctionType )
							{
								case TerrainFunctionType::SimpleFractal : 
								case TerrainFunctionType::RidgedFractal : break;
							}
							throw gcnew System::NotImplementedException( );
						};
					case TerrainGeometry::Sphere :
						{
							switch ( heightFunction->FunctionType )
							{
								case TerrainFunctionType::SimpleFractal :
									return CreateSphereFunction< SseSimpleFractal >( ( FractalTerrainParameters^ )heightFunction->Parameters, groundFunction->FunctionType, groundFunction->Parameters );
								case TerrainFunctionType::RidgedFractal :
									return CreateSphereFunction< SseRidgedFractal >( ( FractalTerrainParameters^ )heightFunction->Parameters, groundFunction->FunctionType, groundFunction->Parameters );
							}
							throw gcnew System::NotImplementedException( );
						};
				}

				return 0;
			}
		};
	};
};