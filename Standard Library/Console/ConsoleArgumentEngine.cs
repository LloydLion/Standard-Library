using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StandardLibrary.Console
{
    public class ConsoleArgumentEngine
    {
        public const string ParameterMemberHasInvalidTypeExcceptionMessage = 
            "Parameter member has invalid type";


        public const string ParameterIsNotHasAllKeysExcceptionMessage =
            "Parameter is not has all keys";

        public const string ParameterIsNotHasAllPositionParametersExcceptionMessage =
            "Parameter is not has all position parameters";



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

            if (positions.Length + flags.Length + keys.Length != parameters.Length)
                throw new ArgumentException(nameof(parameters),
                    ParameterMemberHasInvalidTypeExcceptionMessage);
        }

        public Dictionary<string, object> Calculate(string[] args)
        {
            var dic = new Dictionary<string, object>();
            List<int> indexL = new List<int>();

            try
            {    
                dic = dic.Concat(positions.Select((s, i) =>
                {
                    if (args.Length - 1 >= s.Position)
                    {

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
                })).Where((s, i) => !indexL.Contains(i))
                .ToDictionary((s) => s.Key, (s) => s.Value);
            }
            catch (IndexOutOfRangeException)
            {
                throw new ArgumentException(nameof(args), 
                    ParameterIsNotHasAllPositionParametersExcceptionMessage);
            }

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

            dic = dic.Concat
                (flags.Select((s) => new KeyValuePair<string, object>
                (s.Name, args.Contains("--" + s.Key))))
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
