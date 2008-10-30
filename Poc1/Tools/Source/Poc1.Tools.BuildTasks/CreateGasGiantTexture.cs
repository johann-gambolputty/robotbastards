using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Xml.Serialization;
using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;
using Poc1.Tools.Atmosphere.GasGiant;

namespace Poc1.Tools.BuildTasks
{
	/// <summary>
	/// Creates gas giant textures
	/// </summary>
	public class CreateGasGiantTexture : Task
	{
		/// <summary>
		/// Task input files
		/// </summary>
		[Required]
		public ITaskItem SourceFile
		{
			get { return m_NoiseDefinitionFile; }
			set { m_NoiseDefinitionFile = value; }
		}

		/// <summary>
		/// Task output files
		/// </summary>
		[Required]
		public ITaskItem OutputFile
		{
			get { return m_OutputFile; }
			set { m_OutputFile = value; }
		}

		/// <summary>
		/// Executes this task
		/// </summary>
		public override bool Execute( )
		{
			string definitionFile = SourceFile.ItemSpec;

			//	Test serialization
			//StringWriter output = new StringWriter( );
			//s_BuildParametersSerializer.Serialize( output, new GasGiantModel( ) );

			GasGiantModel model;
			using ( FileStream stream = new FileStream( definitionFile, FileMode.Open, FileAccess.Read ) )
			{
				model = ( GasGiantModel )s_BuildParametersSerializer.Deserialize( stream );
			}

			string outputDir = Path.GetDirectoryName( OutputFile.ItemSpec );
			if ( !Directory.Exists( outputDir ) )
			{
				Directory.CreateDirectory( outputDir );
			}

			Bitmap bmp = GasGiantMarbleTextureBuilder.Generate( model );
			bmp.Save( OutputFile.ItemSpec, ImageFormat.Jpeg );

			return true;
		}

		#region Private Members

		private ITaskItem m_NoiseDefinitionFile;
		private ITaskItem m_OutputFile;

		private readonly static XmlSerializer s_BuildParametersSerializer = new XmlSerializer( typeof( GasGiantModel ) );

		#endregion
	}
}
