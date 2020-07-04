using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StandardLibrary.Console
{
    public class ConsoleArgumentEngine
    {
        public const string ParameterMemberIsInvalid = 
            "Parameter member is invalid";

        public const string ParameterIsNotHasAllKeysExcceptionMessage =
            "Parameter is not has all keys";

        public const string ParameterIsNotHasAllPositionParametersExcceptionMessage =
            "Parameter is not has all position parameters";

        public const string SomePositionParametersHasInvalidPosition =
            "Some position parameters has invalid position";

        public const string SomeKeyParametersHasInvalidKey =
            "Some key parameters has invalid key";
        
        public const string SomeFlagParametersHasInvalidKey =
            "Some flag parameters has invalid key";

        public const string SomeParametersHasInvalidType =
            "Some parameters has invalid type";


        private readonly PositionParameter[] positions;
        private readonly FlagParameter[] flags;
        private readonly KeyParameter[] keys;

        /// <summary>
        ///     
        ///     Creates new ArgumentEngine with given parameters
        /// 
        /// </summary>
        /// <param name="parameters">Arguments</param>
        public ConsoleArgumentEngine(params Parameter[] parameters)
        {
            positions = parameters.Where((s) => s.GetType() == typeof(PositionParameter))
                .Cast<PositionParameter>().ToArray();

            flags = parameters.Where((s) => s.GetType() == typeof(FlagParameter))
                .Cast<FlagParameter>().ToArray();

            keys = parameters.Where((s) => s.GetType() == typeof(KeyParameter))
                .Cast<KeyParameter>().ToArray();

            #region Validation

            if (positions.Length + flags.Length + keys.Length != parameters.Length)
                throw new ArgumentException(nameof(parameters),
                    ParameterMemberIsInvalid,
                    new Exception(SomeParametersHasInvalidType));

            if (positions != null)
            {
                var tmp = positions.Select((s) => s.Position);
                if (!Array.TrueForAll(positions, (s) => tmp.Count((q) => s.Position == q) == 1))
                {
                    throw new ArgumentException(nameof(parameters),
                        ParameterMemberIsInvalid,
                        new ArgumentException(SomePositionParametersHasInvalidPosition, "Position"));
                }
            }

            if (keys != null)
            {
                var tmp = keys.Select((s) => s.Key);
                if (!Array.TrueForAll(keys, (s) => tmp.Count((q) => s.Key == q) == 1))
                {
                    throw new ArgumentException(nameof(parameters),
                        ParameterMemberIsInvalid,
                        new ArgumentException(SomeKeyParametersHasInvalidKey, "Key"));
                }
            }
            
            if (flags != null)
            {
                var tmp = flags.Select((s) => s.Key);
                if (!Array.TrueForAll(flags, (s) => tmp.Count((q) => s.Key == q) == 1))
                {
                    throw new ArgumentException(nameof(parameters),
                        ParameterMemberIsInvalid,
                        new ArgumentException(SomeFlagParametersHasInvalidKey, "Key"));
                }
            }

            if(flags != null && keys != null)
            {
                var tmp1 = flags.Select((s) => s.Key).ToArray();
                var tmp2 = keys.Select((s) => s.Key).ToArray();

                if (!Array.TrueForAll(tmp1, (s) => !tmp2.Contains(s)))
                {
                    throw new ArgumentException(nameof(parameters),
                        ParameterMemberIsInvalid,
                        new ArgumentException(SomeFlagParametersHasInvalidKey, "Key"));
                }
            }

            #endregion
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
            var dic = new Dictionary<string, object>();
            List<int> requiredSupportList = new List<int>();

            List<int> usedIndexes = new List<int>();

            try
            {
                requiredSupportList.Clear();
                dic = dic.Concat(keys.Select((s, i) =>
                {
                    if (!args.Contains("--" + s.Key))
                    {
                        if (s.IsRequired)
                            throw new IndexOutOfRangeException("Trigger exception");
                        else
                        {
                            requiredSupportList.Add(i);
                            return default;
                        }
                    }
                    else
                    {
                        usedIndexes.Add(args.ToList().IndexOf("--" + s.Key));
                        usedIndexes.Add(usedIndexes.Last() + 1);

                        return new KeyValuePair<string, object>(s.Name, s.ParseFunc
                            (args[args.ToList().IndexOf("--" + s.Key) + 1]));
                    }

                })).Where((s, i) => !requiredSupportList.Contains(i))
                .ToDictionary((s) => s.Key, (s) => s.Value);
            }
            catch(IndexOutOfRangeException)
            {
                throw new ArgumentException(nameof(args), ParameterIsNotHasAllKeysExcceptionMessage);
            }

            try
            {
                requiredSupportList.Clear();
                dic = dic.Concat(positions.Select((s, i) =>
                {
                    if (args.Length - 1 >= s.Position && !usedIndexes.Contains(s.Position))
                    {
                        usedIndexes.Add(s.Position);

                        return new KeyValuePair<string, object>
                            (s.Name, s.ParseFunc(args[s.Position]));
                    }
                    else
                    {

                        if (s.IsRequired)
                            throw new IndexOutOfRangeException("Trigger exception");
                        else
                        {
                            requiredSupportList.Add(i);
                            return default;
                        }
                    }
                })).Where((s, i) => dic.Count - 1 >= i || (!requiredSupportList.Contains(i - dic.Count) &&
                    usedIndexes.Where((q) => q == positions[i - dic.Count].Position).Count() == 1))
                .ToDictionary((s) => s.Key, (s) => s.Value);
            }
            catch (IndexOutOfRangeException)
            {
                throw new ArgumentException(nameof(args),
                    ParameterIsNotHasAllPositionParametersExcceptionMessage);
            }


            requiredSupportList.Clear();
            dic = dic.Concat(flags.Select((s) =>
            {
                if (args.Contains("--" + s.Key))
                    usedIndexes.Add(args.ToList().IndexOf("--" + s.Key));
                

                return new KeyValuePair<string, object>
                        (s.Name, args.Contains("--" + s.Key));
            })
            .Where((s, i) => usedIndexes.Contains(i)))
            .ToDictionary((s) => s.Key, (s) => s.Value);

            return dic;
        }



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
    }
}
