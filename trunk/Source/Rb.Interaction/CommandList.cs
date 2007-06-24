using System;
using System.Reflection;
using System.Collections.Generic;

namespace Rb.Interaction
{
    /// <summary>
    /// List of commands
    /// </summary>
    public class CommandList : List< Command >
    {
        /// <summary>
        /// The name of this command list
        /// </summary>
        public string Name
        {
            set { m_Name = value; }
            get { return m_Name; }
        }

        /// <summary>
        /// Updates all the commands in the list (calling <see cref="Command.Update"/>)
        /// </summary>
        public void Update( )
        {
            foreach ( Command curCommand in this )
            {
                curCommand.Update( );
            }
        }

        /// <summary>
        /// Builds this command list from an enum type
        /// </summary>
        /// <param name="enumType">enum's Type</param>
        public void BuildFromEnum( Type enumType )
        {
            InteractionLog.Info( "Creating command list from enum \"{0}\"", enumType.Name );
            m_Name = enumType.Name;

            string[] commandNames = Enum.GetNames( enumType );
            int[] commandValues = ( int[] )Enum.GetValues( enumType );

            for ( int commandIndex = 0; commandIndex < commandNames.Length; ++commandIndex )
            {
                string name         = commandNames[ commandIndex ];
                string description  = string.Empty;

                MemberInfo[] info = enumType.GetMember( name );
                object[] attribs = info[ 0 ].GetCustomAttributes( typeof( CommandDescriptionAttribute ), false);
                if (attribs.Length > 0)
                {
                    name = ( ( CommandDescriptionAttribute )attribs[ 0 ] ).Name;
                    description = ( ( CommandDescriptionAttribute )attribs[ 0 ] ).Description;
                }

                InteractionLog.Verbose( "Adding enum command \"{0}\"", name );

                Command newCommand = new Command( name, description, checked( ( byte )commandValues[ commandIndex ] ) );

                Add( newCommand );
            }
        }

        private string m_Name;
    }
}
