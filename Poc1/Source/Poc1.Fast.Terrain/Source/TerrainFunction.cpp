#include "stdafx.h"
#include "TerrainFunction.h"
#include "FractalTerrainParameters.h"
#include "Mem.h"
#include "Sse\SseSphereTerrainGenerator.h"
#include "Sse\SsePlaneTerrainGenerator.h"

///	\page	Adding new terrain function types
///
///	1) Implement the function in a function class (see e.g SseSimpleFractal)
///	2) Add to the TerrainFunctionType enum
///	3) If the function requires a parameters class, create on (see e.g. FractalTerrainParameters)
///		3.1) Be sure to add a Setup() call that sets up the function class (see e.g. FractalTerrainParameters::Setup(SseSimpleFractal&))
///	4) Associate the function class and the parameters by overloading the FunctionTypes class (see TerrainFunction.cpp for details)
///	5) Add support for the new function in the switch statement of TerrainGeneratorFactory<>::Create()
///	6) Add support for the new function in the switch statement of CreateTerrainGenerator()
///	Done!
///

///	\page	Adding new terrain geometry types
///
///	1) Implement the geometry generator class (see e.g. SseSphereTerrainGeneratorT)
///	2) Add displacer classes (see e.g. SseFlatSphereTerrainDisplacer). There should be a flat displacer, a
///		height function displacer, and a ground function displacer.
///	3) Add to the TerrainGeometry enum
///	4) Associate the new geometry enum value with the displacer types using the GeometryTypes class (see TerrainFunction.cpp for details)
///	5) Add support for the new geometry type in the switch statements of CreateTerrainGenerator()
///	Done!
///

namespace Poc1
{
	namespace Fast
	{
		namespace Terrain
		{	
			
			//	---------------------------------------------------------------------------------------------

			template < TerrainGeometry >
			struct GeometryTypes
			{
			};

			template < >
			struct GeometryTypes< TerrainGeometry::Sphere >
			{
				typedef SseFlatSphereTerrainDisplacer FlatDisplacer;

				template < typename FunctionClass >
				struct HeightDisplacer
				{
					typedef SseSphereFunction3dDisplacer< FunctionClass > Type;
				};
				
				template < typename FunctionClass, typename BaseDisplacer >
				struct GroundDisplacer
				{
					typedef SseSphereFunction3dGroundDisplacer< BaseDisplacer, FunctionClass > Type;
				};

				template < typename Displacer >
				struct TerrainGenerator
				{
					typedef SseSphereTerrainGeneratorT< Displacer > Type;
				};
			};
			
			template < >
			struct GeometryTypes< TerrainGeometry::Plane >
			{
				typedef SseFlatPlaneTerrainDisplacer FlatDisplacer;

				template < typename FunctionClass >
				struct HeightDisplacer
				{
					typedef SsePlaneFunction3dDisplacer< FunctionClass > Type;
				};
				
				template < typename FunctionClass, typename BaseDisplacer >
				struct GroundDisplacer
				{
					typedef SsePlaneFunction3dGroundDisplacer< BaseDisplacer, FunctionClass > Type;
				};

				template < typename Displacer >
				struct TerrainGenerator
				{
					typedef SsePlaneTerrainGeneratorT< Displacer > Type;
				};
			};

			template < TerrainFunctionType FunctionType >
			struct FunctionTypes
			{
			};
			
			template < >
			struct FunctionTypes< TerrainFunctionType::Flat >
			{
			};
			
			template < >
			struct FunctionTypes< TerrainFunctionType::SimpleFractal >
			{
				typedef SseSimpleFractal			ClassType;
				typedef FractalTerrainParameters	ParametersType;
			};
			
			template < >
			struct FunctionTypes< TerrainFunctionType::RidgedFractal >
			{
				typedef SseRidgedFractal			ClassType;
				typedef FractalTerrainParameters	ParametersType;
			};

			template < TerrainGeometry Geometry >
			struct TerrainGeneratorFactory : public GeometryTypes< Geometry >
			{
				static UTerrainGenerator* Create( )
				{
					return new ( Aligned( 16 ) ) TerrainGenerator< FlatDisplacer >::Type( );
				}

				template < TerrainFunctionType HeightFunctionType >
				static UTerrainGenerator* Create( TerrainFunctionParameters^ heightParams )
				{
					typedef FunctionTypes< HeightFunctionType >::ClassType HClass;
					typedef FunctionTypes< HeightFunctionType >::ParametersType HParamsType;
					typedef TerrainGenerator< HeightDisplacer< HClass >::Type >::Type GeneratorType;

					GeneratorType* generator = new ( Aligned( 16 ) ) GeneratorType( );
					
					heightParams->Setup( generator->GetDisplacer( ) );
					( ( HParamsType^ )heightParams )->Setup( generator->GetDisplacer( ).GetFunction( ) );

					return generator;
				}
				
				template < TerrainFunctionType HeightFunctionType, TerrainFunctionType GroundFunctionType >
				static UTerrainGenerator* Create( TerrainFunctionParameters^ heightParams, TerrainFunctionParameters^ groundParams )
				{
					typedef FunctionTypes< HeightFunctionType >::ClassType HClass;
					typedef FunctionTypes< HeightFunctionType >::ParametersType HParamsType;
					
					typedef FunctionTypes< GroundFunctionType >::ClassType GClass;
					typedef FunctionTypes< GroundFunctionType >::ParametersType GParamsType;

					typedef HeightDisplacer< HClass >::Type HeightDisplacerType;
					typedef GroundDisplacer< GClass, HeightDisplacerType >::Type GroundDisplacerType;
					typedef TerrainGenerator< GroundDisplacerType >::Type GeneratorType;

					GeneratorType* generator = new ( Aligned( 16 ) ) GeneratorType( );

					heightParams->Setup( generator->GetDisplacer( ).GetBaseDisplacer( ) );
					groundParams->Setup( generator->GetDisplacer( ) );

					( ( HParamsType^ )heightParams )->Setup( generator->GetDisplacer( ).GetBaseDisplacer( ).GetFunction( ) );
					( ( GParamsType^ )groundParams )->Setup( generator->GetDisplacer( ).GetFunction( ) );

					return generator;
				}
				
				template < TerrainFunctionType HeightFunctionType >
				static UTerrainGenerator* Create( TerrainFunctionParameters^ heightParams, TerrainFunction^ groundFunction )
				{
					if ( groundFunction == nullptr )
					{
						return Create< HeightFunctionType >( heightParams );
					}
					switch ( groundFunction->FunctionType )
					{
						case TerrainFunctionType::Flat			: return Create< HeightFunctionType >( heightParams );
						case TerrainFunctionType::SimpleFractal	: return Create< HeightFunctionType, TerrainFunctionType::SimpleFractal >( heightParams, groundFunction->Parameters );
						case TerrainFunctionType::RidgedFractal	: return Create< HeightFunctionType, TerrainFunctionType::RidgedFractal >( heightParams, groundFunction->Parameters );
					}

					throw gcnew System::NotSupportedException( "Unsupported ground function type" );
				}
			};

			
			//	---------------------------------------------------------------------------------------------

			UTerrainGenerator* CreateTerrainGenerator( TerrainGeometry geometry, TerrainFunction^ heightFunction, TerrainFunction^ groundFunction )
			{
				TerrainFunctionType functionType = ( heightFunction == nullptr ) ? TerrainFunctionType::Flat : heightFunction->FunctionType;
				switch ( functionType )
				{
					case TerrainFunctionType::Flat :
						switch ( geometry )
						{
							case TerrainGeometry::Sphere	: return TerrainGeneratorFactory< TerrainGeometry::Sphere >::Create( );
							case TerrainGeometry::Plane		: return TerrainGeneratorFactory< TerrainGeometry::Plane >::Create( );
						}
						throw gcnew System::NotSupportedException( "Geometry type not supported for flat terrain" );

					case TerrainFunctionType::SimpleFractal :
						switch ( geometry )
						{
							case TerrainGeometry::Sphere	: return TerrainGeneratorFactory< TerrainGeometry::Sphere >::Create< TerrainFunctionType::SimpleFractal >( heightFunction->Parameters, groundFunction );
							case TerrainGeometry::Plane		: return TerrainGeneratorFactory< TerrainGeometry::Plane >::Create< TerrainFunctionType::SimpleFractal >( heightFunction->Parameters, groundFunction );
						}
						throw gcnew System::NotSupportedException( "Geometry type not supported for simple fractals" );

					case TerrainFunctionType::RidgedFractal	:
						switch ( geometry )
						{
							case TerrainGeometry::Sphere	: return TerrainGeneratorFactory< TerrainGeometry::Sphere >::Create< TerrainFunctionType::RidgedFractal >( heightFunction->Parameters, groundFunction );
							case TerrainGeometry::Plane		: return TerrainGeneratorFactory< TerrainGeometry::Plane >::Create< TerrainFunctionType::RidgedFractal >( heightFunction->Parameters, groundFunction );
						}
						throw gcnew System::NotSupportedException( "Geometry type not supported for ridged fractals" );
				}
				throw gcnew System::NotSupportedException( "Height function type not supported" );
			}

			//	---------------------------------------------------------------------------------------------

			//	---------------------------------------------------------------------------------------------

			void TerrainFunctionParameters::Setup( SseTerrainDisplacer& displacer )
			{
				displacer.SetFunctionScale( FunctionScale );
				displacer.SetOutputScale( OutputScale );
			}

			//	---------------------------------------------------------------------------------------------

			//	---------------------------------------------------------------------------------------------


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

			TerrainFunctionParameters^ TerrainFunction::CreateParameters( TerrainFunctionType functionType )
			{
				switch ( functionType )
				{
					case TerrainFunctionType::Flat			: return nullptr;
					case TerrainFunctionType::SimpleFractal	: return gcnew FractalTerrainParameters( );
					case TerrainFunctionType::RidgedFractal	: return gcnew FractalTerrainParameters( );
				}
				throw gcnew System::NotImplementedException( );
			}

			UTerrainGenerator* TerrainFunction::CreateGenerator( TerrainGeometry geometry, TerrainFunction^ heightFunction )
			{
				if ( heightFunction == nullptr )
				{
					throw gcnew System::ArgumentNullException( "heightFunction" );
				}
				return CreateTerrainGenerator( geometry, heightFunction, nullptr );
			}

			UTerrainGenerator* TerrainFunction::CreateGenerator( TerrainGeometry geometry, TerrainFunction^ heightFunction, TerrainFunction^ groundFunction )
			{
				if ( groundFunction == nullptr )
				{
					return CreateGenerator( geometry, heightFunction );
				}
				if ( heightFunction == nullptr )
				{
					throw gcnew System::ArgumentNullException( "heightFunction" );
				}
				if ( groundFunction == nullptr )
				{
					return CreateTerrainGenerator( geometry, heightFunction, nullptr );
				}
				return CreateTerrainGenerator( geometry, heightFunction, groundFunction );
			}
			
			//	---------------------------------------------------------------------------------------------
		}; //Terrain
	}; //Fast
}; //Poc1