using System;
using System.Collections;
using System.Reflection;

namespace Rb.Core.CommandLine
{
	/// <summary>
	/// Builds objects from the command line
	/// </summary>
	/// <remarks>
	/// For every member in a class which has a <see cref="CmdArgAttribute"/> attribute, the parser reads
	/// a value from the command line into the member (setting the property or field, or calling the method
	/// with 1 argument).
	/// 
	/// The following example could parse the command line <c>MyApp.exe /logFile "path" /bias 0.1 /weight 100</c>
	/// <code>
	/// public class MyCommandLine
	/// {
	///		[CmdArg("logFile")]
	///		public string LogFile
	///		{ 
	///			get { return m_LogFile; } 
	///			set { m_LogFile = value; }
	///		}
	/// 
	///		[CmdArg("weight", Required = true)]
	///		public int m_Weight;
	/// 
	///		[CmdArg("bias")]
	///		public void SetBias(float bias)
	///		{
	///			m_Bias = bias;
	///		}
	/// 
	///		public static MyCommandLine Create()
	///		{
	///			return CommandLineParser.Build(new MyCommandLine());
	///		}
	/// 
	///		private float m_Bias;
	///		private string m_LogFile;
	///		private int m_Weight;
	/// }
	/// </code>
	/// </remarks>
	public static class CommandLineParser
	{
		/// <summary>
		/// Builds an object from specified command line arguments
		/// </summary>
		/// <param name="obj">Object to build</param>
		/// <param name="args">Command line arguments</param>
		/// <exception cref="CmdArgNotFoundException">
		/// Thrown if an argument is required (<see cref="CmdArgAttribute.Required"/>), but is not present
		///  </exception>
		/// <exception cref="CmdArgIncompleteException">
		/// Thrown if an argument is present but does not have a value following it.
		///  </exception>
		/// <exception cref="CustomAttributeFormatException">
		/// Thrown if a member 
		/// </exception>
		public static void Build(object obj, string[] args)
		{
			Type type = obj.GetType();
			bool orderedList = false;
			bool namedList = false;

			//	Iterate over all properties in the command line object
			foreach (MemberInfo member in type.GetMembers(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.FlattenHierarchy))
			{
				//	Get the CmdArg attribute attached to the current property
				CmdArgAttribute[] attributes = (CmdArgAttribute[])member.GetCustomAttributes(typeof(CmdArgAttribute), false);
				if (attributes.Length == 0)
				{
					continue;
				}
				CmdArgAttribute attribute = attributes[0];

				orderedList |= attribute.IsIndexed;
				namedList |= !string.IsNullOrEmpty(attribute.FlagName);
				if (orderedList && namedList)
				{
					throw new CustomAttributeFormatException(string.Format("Ordered and flagged command line arguments mixed in type \"{0}\"", type.Name));
				}
				Type setType = GetMemberSetType(member);
				if (orderedList)
				{
					//	Argument is part of an ordered list
					SetMemberValue(member, setType, obj, args[attribute.Index]);
					continue;
				}

				//	Argument is part of a named flag list
				//	Find the named flag
				int argIndex = FindArgument(args, attribute.FlagName);
				bool isBooleanMember = setType == typeof(bool);
				if (argIndex == -1)
				{
					//	Flag was not found. For boolean properties, this sets the property value to false (CmdArgAttribute.Required
					//	is ignored). Otherwise, and exception is thrown
					if (isBooleanMember)
					{
						SetMemberValue(member, obj, false);
					}
					else if (attribute.Required)
					{
						throw new CmdArgNotFoundException(attribute.FlagName);
					}
					continue;
				}

				//	Found the named flag
				if (!isBooleanMember)
				{
					//	Read the argument value following the flag into the property value
					if (argIndex == (args.Length - 1))
					{
						throw new CmdArgIncompleteException(attribute.FlagName);
					}
					SetMemberValue(member, setType, obj, args[++argIndex]);
				}
				else
				{
					//	Boolean property - the flag exists, so set the property value to true
					SetMemberValue(member, obj, true);
				}
			}
		}

		/// <summary>
		/// Builds an object from specified command line arguments
		/// </summary>
		/// <param name="type">Type of object to build</param>
		/// <param name="args">Command line arguments</param>
		/// <exception cref="CmdArgNotFoundException">
		/// Thrown if an argument is required (<see cref="CmdArgAttribute.Required"/>), but is not present
		///  </exception>
		/// <exception cref="CmdArgIncompleteException">
		/// Thrown if an argument is present but does not have a value following it.
		///  </exception>
		public static object Build( Type type, string[] args)
		{
			object obj = Activator.CreateInstance(type);
			Build(obj, args);
			return obj;
		}

		/// <summary>
		/// Builds an object from command line arguments
		/// </summary>
		/// <param name="type">Type of object to build</param>
		/// <exception cref="CmdArgNotFoundException">
		/// Thrown if an argument is required (<see cref="CmdArgAttribute.Required"/>), but is not present
		///  </exception>
		/// <exception cref="CmdArgIncompleteException">
		/// Thrown if an argument is present but does not have a value following it.
		///  </exception>
		public static object Build(Type type)
		{
			return Build(type, Environment.GetCommandLineArgs());
		}

		/// <summary>
		/// Builds an object from command line arguments
		/// </summary>
		/// <typeparam name="T">Type of object to build</typeparam>
		/// <exception cref="CmdArgNotFoundException">
		/// Thrown if an argument is required (<see cref="CmdArgAttribute.Required"/>), but is not present
		///  </exception>
		/// <exception cref="CmdArgIncompleteException">
		/// Thrown if an argument is present but does not have a value following it.
		///  </exception>
		public static T Build<T>( )
		{
			return ( T )Build( typeof( T ) );
		}

		/// <summary>
		/// Builds an object from specified command line arguments
		/// </summary>
		/// <typeparam name="T">Type of object to build</typeparam>
		/// <param name="args">Command line arguments</param>
		/// <exception cref="CmdArgNotFoundException">
		/// Thrown if an argument is required (<see cref="CmdArgAttribute.Required"/>), but is not present
		///  </exception>
		/// <exception cref="CmdArgIncompleteException">
		/// Thrown if an argument is present but does not have a value following it.
		///  </exception>
		public static T Build<T>( string[] args )
		{
			return ( T )Build( typeof( T ), args );
		}

		#region Private Members

		/// <summary>
		/// Finds a named argument in an array of command line arguments
		/// </summary>
		private static int FindArgument(string[] args, string name)
		{
			for (int argIndex = 0; argIndex < args.Length; ++argIndex)
			{
				char prefix = args[argIndex][0];
				if ((prefix != '-') && (prefix != '/') && (prefix != '\\'))
				{
					continue;
				}
				if (args[argIndex].Substring(1).Equals(name, StringComparison.InvariantCultureIgnoreCase))
				{
					return argIndex;
				}
			}
			return -1;
		}

		/// <summary>
		/// Gets the type of a member used when setting its value
		/// </summary>
		private static Type GetMemberSetType(MemberInfo member)
		{
			switch (member.MemberType)
			{
				case MemberTypes.Property	: return ((PropertyInfo)member).PropertyType;
				case MemberTypes.Field		: return ((FieldInfo)member).FieldType;
				case MemberTypes.Method		: return ((MethodInfo)member).GetParameters()[0].ParameterType;
				default:
					throw new CustomAttributeFormatException(member.MemberType + " class members cannot be set by command line arguments");
			}
		}

		/// <summary>
		/// Sets the value of a member, given a command line argument
		/// </summary>
		private static void SetMemberValue(MemberInfo member, Type convert, object obj, string arg)
		{
			arg = arg.Trim('"');
			object value = Convert.ChangeType(arg, convert);
			SetMemberValue(member, obj, value);
		}
		
		/// <summary>
		/// Sets the value of a member, given a command line argument
		/// </summary>
		private static void SetMemberValue(MemberInfo member, object obj, object value)
		{
			switch (member.MemberType)
			{
				case MemberTypes.Property	: SetPropertyValue((PropertyInfo)member, obj, value); break;
				case MemberTypes.Field		: SetFieldValue((FieldInfo)member, obj, value); break;
				case MemberTypes.Method		:
					((MethodInfo)member).Invoke(obj, new object[] {value});
					break;
				default:
					throw new CustomAttributeFormatException();
			}
		}

		/// <summary>
		/// Sets the value of a field
		/// </summary>
		private static void SetFieldValue(FieldInfo field, object obj, object value)
		{
			if (field.FieldType.GetInterface(typeof(IList).Name) != null)
			{
				((IList)field.GetValue(obj)).Add(value);
			}
			else
			{
				field.SetValue(obj, value);
			}
		}

		/// <summary>
		/// Sets the value of a property
		/// </summary>
		private static void SetPropertyValue(PropertyInfo property, object obj, object value)
		{
			if (property.PropertyType.GetInterface(typeof(IList).Name) != null)
			{
				((IList)property.GetValue(obj, null)).Add(value);
			}
			else
			{
				property.SetValue(obj, value, null);
			}
		}

		#endregion
	}
}
