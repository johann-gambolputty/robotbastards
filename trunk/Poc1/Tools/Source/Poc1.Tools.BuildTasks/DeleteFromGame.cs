using System.IO;
using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;

namespace Poc1.Tools.BuildTasks
{
	public class DeleteFromGame : Task
	{
		[Required]
		public ITaskItem[] Files
		{
			get { return m_Files; }
			set { m_Files = value; }
		}

		public override bool Execute( )
		{
			foreach ( ITaskItem item in m_Files )
			{
				bool isDesign;
				string dstPath = CopyToGame.CreateDestinationPath( item.ItemSpec, out isDesign );
				if ( isDesign )
				{
					File.Delete( dstPath );
				}
			}
			return true;
		}

		private ITaskItem[] m_Files;
	}
}
