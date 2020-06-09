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

        public Dictionary<string, object> Calculate(string[] args)
        {
            var dic = new Dictionary<string, object>();
            List<int> indexL = new List<int>();

            List<int> usedIndexes = new List<int>();

            try
            {
                indexL.Clear();
                dic = dic.Concat(keys.Select((s, i) =>
                {
                    if (!args.Contains("--" + s.Key))
                    {
                        if (s.IsRequired)
                            throw new IndexOutOfRangeException("Trigger exception");
                        else
                        {
                            indexL.Add(i);
                            return default;
                        }
                    }
                    else
                    {
                        usedIndexes.Add(i);

                        return new KeyValuePair<string, object>(s.Name, s.ParseFunc
                            (args[args.ToList().IndexOf("--" + s.Key) + 1]));
                    }

                })).Where((s, i) => !indexL.Contains(i))
                .ToDictionary((s) => s.Key, (s) => s.Value);
            }
            catch(IndexOutOfRangeException)
            {
                throw new ArgumentException(nameof(args), ParameterIsNotHasAllKeysExcceptionMessage);
            }

            try
            {
                dic = dic.Concat(positions.Select((s, i) =>
                {
                    if (args.Length - 1 >= s.Position)
                    {
                        usedIndexes.Add(i);

                        return new KeyValuePair<string, object>
                            (s.Name, s.ParseFunc(args[s.Position]));
                    }
                    else
                    {

                        if (s.IsRequired)
                            throw new IndexOutOfRangeException("Trigger exception");
                        else
                        {
                            indexL.Add(i);
                            return default;
                        }
                    }
                })).Where((s, i) => !indexL.Contains(i) && !usedIndexes.Contains(i))
                .ToDictionary((s) => s.Key, (s) => s.Value);
            }
            catch (IndexOutOfRangeException)
            {
                throw new ArgumentException(nameof(args),
                    ParameterIsNotHasAllPositionParametersExcceptionMessage);
            }

            dic = dic.Concat
                (flags.Select((s) => new KeyValuePair<string, object>
                (s.Name, args.Contains("--" + s.Key)))
                .Where((s, i) => usedIndexes.Contains(i)))
                .ToDictionary((s) => s.Key, (s) => s.Value);

            return dic;
        }



        public abstract class Parameter
        {
            public virtual Func<string, object> ParseFunc { get; set; }
            public string Name { get; set; }
            public virtual bool IsRequired { get; set; }
        }

        public class PositionParameter : Parameter
        { 
            public int Position { get; set; }
        }

        public class KeyParameter : Parameter
        {
            public string Key { get; set; }
        }

        public class FlagParameter : Parameter
        {
            public const string ReadOnlyPropertyExceptionMessage = 
                "It is readonly property";

            public override Func<string, object> ParseFunc 
            { 
                get => (s) => bool.Parse(s); 
                set => throw new InvalidOperationException(ReadOnlyPropertyExceptionMessage); 
            }

            public override bool IsRequired 
            { 
                get => false; 
                set => throw new InvalidOperationException(ReadOnlyPropertyExceptionMessage); 
            }

            public string Key { get; set; }
        }
    }
}
