#pragma once


namespace Poc1
{
	namespace Fast
	{
		#pragma managed(push, off)

		///	\brief	Unmanaged cube map faces. MUST MATCH values in CubeMapFace
		enum UCubeMapFace
		{
			NegativeX,
			PositiveX,
			NegativeY,
			PositiveY,
			NegativeZ,
			PositiveZ
		};
		
		///	\brief	Unmanaged pixel formats
		enum UPixelFormat
		{
			FormatR8G8B8,
			FormatR8G8B8A8,
		};

		#pragma managed

		///	\brief	Converts a PixelFormat value to a UPixelFormat value
		inline UPixelFormat GetUPixelFormat( System::Drawing::Imaging::PixelFormat format )
		{
			switch ( format )
			{
				case System::Drawing::Imaging::PixelFormat::Format24bppRgb : return FormatR8G8B8;
				case System::Drawing::Imaging::PixelFormat::Format32bppArgb : return FormatR8G8B8A8;
			}
			throw gcnew System::ArgumentException( "Unhandled pixel format", "format" );
		}

		///	\brief	Converts a CubeMapFace value to a UCubeMapFace value
		inline UCubeMapFace GetUCubeMapFace( Rb::Rendering::Interfaces::Objects::CubeMapFace face )
		{
			switch ( face )
			{
				case Rb::Rendering::Interfaces::Objects::CubeMapFace::NegativeX : return NegativeX;
				case Rb::Rendering::Interfaces::Objects::CubeMapFace::PositiveX : return PositiveX;
				case Rb::Rendering::Interfaces::Objects::CubeMapFace::NegativeY : return NegativeY;
				case Rb::Rendering::Interfaces::Objects::CubeMapFace::PositiveY : return PositiveY;
				case Rb::Rendering::Interfaces::Objects::CubeMapFace::NegativeZ : return NegativeZ;
				case Rb::Rendering::Interfaces::Objects::CubeMapFace::PositiveZ : return PositiveZ;
			}
			return NegativeX;
		}
		
		#pragma managed(pop)
	};
};