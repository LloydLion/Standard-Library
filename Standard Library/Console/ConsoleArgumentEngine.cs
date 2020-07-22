using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;

namespace StandardLibrary.Console
{
	public class ConsoleArgumentEngine
	{
		[Obsolete]
		public const string ParameterMemberIsInvalid = 
			"Parameter member is invalid";

		[Obsolete]
		public const string ParameterIsNotHasAllKeysExcceptionMessage =
			"Parameter is not has all keys";

		[Obsolete]
		public const string ParameterIsNotHasAllPositionParametersExcceptionMessage =
			"Parameter is not has all position parameters";

		[Obsolete]
		public const string SomePositionParametersHasInvalidPosition =
			"Some position parameters has invalid position";

		[Obsolete]
		public const string SomeKeyParametersHasInvalidKey =
			"Some key parameters has invalid key";
		
		[Obsolete]
		public const string SomeFlagParametersHasInvalidKey =
			"Some flag parameters has invalid key";

		[Obsolete]
		public const string SomeParametersHasInvalidType =
			"Some parameters has invalid type";


		public static ParserRegistrator Registrator { get; } = new ParserRegistrator();

		private readonly ParameterParser[] parsers;

		/// <summary>
		///     
		///     Creates new ArgumentEngine with given parameters
		/// 
		/// </summary>
		/// <param name="parameters">Arguments</param>
		public ConsoleArgumentEngine(params Parameter[] parameters)
		{
			parsers = Registrator.Parsers.Select((s) => (ParameterParser)s.Clone()).ToArray();

			foreach (var item in parsers)
			{
				item.Parameters = parameters.Where((h) => h.GetType() == item.TargetParameterType).ToArray();
			}
		}


		/// <summary>
		/// 
		///     Calculets arguments dictionary from args
		///     Exemple:
		///     
		///     static Main(string[] args)
		///     {
		///         ...
		///         var arguments = engine.Calculate(args);
		///         ...
		///     }
		/// 
		/// </summary>
		/// <param name="args">Input string array</param>
		/// <returns>Parsed arguments dictionary</returns>
		public Dictionary<string, object> Calculate(string[] args)
		{
			List<int> usedIndexes = new List<int>();
			Dictionary<string, object> output = new Dictionary<string, object>();

			StandardParameterParserOII oii = new StandardParameterParserOII(usedIndexes, args, output);

			foreach (var s in parsers) s.OnStart(oii);

			for (int i = 0; i < args.Length; i++)
			{
				if (usedIndexes.Contains(i) == true) continue;

				oii.CurrentIndex = i;

				//Priority system in Registrator. See ConsoleArgumentEngine.ParserRegistrator.Parsers [Class.Object.Property]
				for (int g = 0; g < parsers.Length; g++)
				{
					if(parsers[g].Parse(args[i], new AdvancedParameterInfo() { Position = i }, oii, out var parsedObject, out var targetParameter))
					{
						output.Add(targetParameter.Name, parsedObject);
						usedIndexes.Add(i);
						break;
					}
				}
			}

			foreach (var s in parsers) s.OnEnd(oii);
			return output;
		}


		#region ParametersClasses

		/// <summary>
		/// 
		///     Base class for any parameter
		/// 
		/// </summary>
		public abstract class Parameter
		{
			/// <summary>
			/// 
			///     Parsing function
			/// 
			/// </summary>
			public virtual Func<string, object> ParseFunc { get; set; }

			/// <summary>
			/// 
			///     Parameter name in output dictionary
			/// 
			/// </summary>
			public string Name { get; set; }

			/// <summary>
			/// 
			///     Set in false to make the parameter isn't required
			///     Default: required (true value)
			/// 
			/// </summary>
			public virtual bool IsRequired { get; set; }
		}

		/// <summary>
		/// 
		///     Class for Postion parameter
		/// 
		/// </summary>
		public class PositionParameter : Parameter
		{
			/// <summary>
			/// 
			///     Position of parameter
			///     Exemple: program.exe 321    ASd    true
			///                          ^pos:0 ^pos:1 ^pos:2
			/// 
			/// </summary>
			public int Position { get; set; }
		}

		/// <summary>
		/// 
		///     Class for parameters with key
		///     Exemple: program.exe --key value --key2 --value2
		/// 
		/// </summary>
		public class KeyParameter : Parameter
		{
			/// <summary>
			/// 
			///     Parameter key
			/// 
			/// </summary>
			public string Key { get; set; }
		}

		/// <summary>
		/// 
		///     Class for flag parameter
		///     Exemple: program.exe --flag1 --flag2
		///     Analog with KeyParameter: program.exe --flag1 true --flag2 true
		///     *If flag not exist false value will be inserted 
		///         into the output dictionary on Calculate(string[])
		/// 
		/// </summary>
		public class FlagParameter : Parameter
		{
			public const string ReadOnlyPropertyExceptionMessage = 
				"It is readonly property";

			public const string UnreadeblePropertyExceptionMessage =
				"It is unreadeble property";

			/// <summary>
			/// 
			///     Unsuported Property
			/// 
			/// </summary>
			public override Func<string, object> ParseFunc 
			{ 
				get => throw new InvalidOperationException(UnreadeblePropertyExceptionMessage); 
				set => throw new InvalidOperationException(ReadOnlyPropertyExceptionMessage); 
			}

			/// <summary>
			/// 
			///     Unsuported Property
			/// 
			/// </summary>
			public override bool IsRequired 
			{ 
				get => throw new InvalidOperationException(UnreadeblePropertyExceptionMessage); 
				set => throw new InvalidOperationException(ReadOnlyPropertyExceptionMessage); 
			}

			/// <summary>
			/// 
			///     Parameter flag
			/// 
			/// </summary>
			public string Key { get; set; }
		}

		#endregion

		#region ParametersParsers
		
		public abstract class ParameterParser : ICloneable
		{
			private readonly Dictionary<Type, Parameter[]> getCastedParametersMethodCache = new Dictionary<Type, Parameter[]>();

			public Parameter[] Parameters { get; set; }
			public abstract Type TargetParameterType { get; }
			public abstract ParsePriorityLevel Priority { get; }

			public abstract object Clone();
			public abstract bool Parse(string argument, AdvancedParameterInfo info, IParameterParserOII oii, out object parsedObject, out Parameter targetParameter);
			public virtual void OnEnd(IParameterParserOIIEnder oiiEnd) { }
			public virtual void OnStart(IParameterParserOIIStarter oiiStart) { } 
			
			protected T[] GetCastedParameters<T>() where T : Parameter
			{
				if(getCastedParametersMethodCache.ContainsKey(typeof(T)))
				{
					return (T[])getCastedParametersMethodCache[typeof(T)];
				}


				getCastedParametersMethodCache.Add(typeof(T), Parameters.Cast<T>().ToArray());
				return GetCastedParameters<T>();
			}
		}

		public class PositionParameterParser  : ParameterParser
		{
			public override Type TargetParameterType => typeof(PositionParameter);
			public override ParsePriorityLevel Priority => ParsePriorityLevel.High;

			private List<PositionParameter> parameters;

			public override bool Parse(string forParse, AdvancedParameterInfo info, IParameterParserOII oii, out object parsedObject, out Parameter targetParameter)
			{
				parsedObject = default;
				targetParameter = default;

				var index = parameters.Select((s) => s.Position).ToList().IndexOf(info.Position);
				if (index == -1) return false;

				parsedObject = parameters[index].ParseFunc.Invoke(forParse);
				targetParameter = parameters[index];
				parameters.RemoveAt(index);

				return true;
			}

			public override object Clone()
			{
				return new PositionParameterParser();
			}

			public override void OnStart(IParameterParserOIIStarter oiiStart)
			{
				parameters = GetCastedParameters<PositionParameter>().ToList();
			}

			public override void OnEnd(IParameterParserOIIEnder oiiEnd)
			{
				if (parameters.Where((s) => s.IsRequired == true).Count() > 0)
				{
					throw new RequiredParametersNotFoundException(parameters.Where((s) => s.IsRequired == true).ToArray());
				}
			}
		}

		public class KeyParameterParser : ParameterParser
		{
			public override Type TargetParameterType => typeof(KeyParameter);
			public override ParsePriorityLevel Priority => ParsePriorityLevel.Standard;

			private List<KeyParameter> parameters;

			public override bool Parse(string argument, AdvancedParameterInfo info, IParameterParserOII oii, out object parsedObject, out Parameter targetParameter)
			{
				parsedObject = default;
				targetParameter = default;

				if (argument.StartsWith("--"))
				{
					argument = argument.Substring(2);

					var index = parameters.Select((s) => s.Key).ToList().IndexOf(argument);
					if (index == -1)
						return false;
					else
					{
						argument = oii.UseArgument(offest: 1);
						parsedObject = parameters[index].ParseFunc(argument);
						targetParameter = parameters[index];
						parameters.RemoveAt(index);

						return true;
					}
				}
				else
				{
					return false;
				}
			}

			public override object Clone()
			{
				return new KeyParameterParser();
			}

			public override void OnStart(IParameterParserOIIStarter oiiStart)
			{
				parameters = GetCastedParameters<KeyParameter>().ToList();
			}

			public override void OnEnd(IParameterParserOIIEnder oiiEnd)
			{
				if(parameters.Where((s) => s.IsRequired == true).Count() > 0)
				{
					throw new RequiredParametersNotFoundException(parameters.Where((s) => s.IsRequired == true).ToArray());
				}
			}
		}

		public class FlagParameterParser : ParameterParser
		{
			public override Type TargetParameterType => typeof(FlagParameter);
			public override ParsePriorityLevel Priority => ParsePriorityLevel.High;

			private List<FlagParameter> parameters;

			public override bool Parse(string argument, AdvancedParameterInfo info, IParameterParserOII oii, out object parsedObject, out Parameter targetParameter)
			{
				parsedObject = default;
				targetParameter = default;

				if (argument.StartsWith("--"))
				{
					argument = argument.Substring(2);

					int indexToRemove;
					if ((indexToRemove = parameters.Select((s) => s.Key).ToList().IndexOf(argument)) != -1)
					{
						targetParameter = parameters[indexToRemove];
						parameters.RemoveAt(indexToRemove);

						parsedObject = true;
						return true;
					}

					return false;
				}
				else
				{
					return false;
				}
			}

			public override void OnEnd(IParameterParserOIIEnder oiiEnd)
			{
				foreach (var item in parameters)
				{
					oiiEnd.AddElementToOutput(false, item.Name);
				}
			}

			public override void OnStart(IParameterParserOIIStarter oiiStart)
			{
				parameters = GetCastedParameters<FlagParameter>().ToList();
			}

			public override object Clone()
			{
				return new FlagParameterParser();
			}
		}

		#endregion

		#region SubClases

		public enum ParsePriorityLevel
		{
			Low = -1,
			Standard = 0,
			High = 1,
			Instanly = 2
		}

		public interface IParameterParserOII
		{
			string UseArgument(int offest);
		}

		public interface IParameterParserOIIEngineSide
		{
			int CurrentIndex { get; set; }
		}

		public interface IParameterParserOIIEnder
		{
			void AddElementToOutput(object value, string key);
		}

		public interface IParameterParserOIIStarter
		{

		}

		public class StandardParameterParserOII : IParameterParserOII, IParameterParserOIIEngineSide, IParameterParserOIIEnder, IParameterParserOIIStarter
		{
			private readonly List<int> usedIndexesList;
			private readonly string[] args;
			private readonly Dictionary<string, object> output;

			public StandardParameterParserOII(List<int> usedIndexesList, string[] args, Dictionary<string, object> output)
			{
				this.usedIndexesList = usedIndexesList;
				this.args = args;
				this.output = output;
			}

			public int CurrentIndex { get; set; }

			public void AddElementToOutput(object value, string key)
			{
				output.Add(key, value);
			}

			public string UseArgument(int offest)
			{
				usedIndexesList.Add(CurrentIndex + offest);
				return args[CurrentIndex + offest];
			}
		}

		public class ParserRegistrator
		{
			private readonly List<ParameterParser> parsers = new List<ParameterParser>();

			public ParserRegistrator()
			{
				parsers.AddRange(new ParameterParser[] { new FlagParameterParser(), new KeyParameterParser(), new PositionParameterParser() });
			}

			public void Rigister(ParameterParser parser)
			{
				parsers.Add(parser);
			}

			public ParameterParser[] Parsers { get => parsers.OrderBy((s) => s.Priority).ToArray(); }
		}

		public class AdvancedParameterInfo
		{
			public int Position { get; set; }
		}

		public class RequiredParametersNotFoundException : Exception
		{
			public const string BaseExceptionMessage = "Required parameter(s) not found";

			public Parameter[] Parameters { get; }
			public Type ParametersType { get; }
			public Type ThrowerParserType { get; }

			public RequiredParametersNotFoundException(Parameter[] parameters) : base(BaseExceptionMessage)
			{
				Parameters = parameters;


				var stack = new StackTrace();

				var frame = stack.GetFrame(stack.FrameCount - 2); //Invoker
				ThrowerParserType = frame.GetMethod().ReflectedType;


				var parametersType = parameters[0].GetType();
				if(parameters.Where((s) => s.GetType() != parametersType).Count() != 0) throw new ArgumentException("Type of all elements in array must be equals", nameof(parameters));
				ParametersType = parametersType;
			}
		}

		#endregion
	}
}
