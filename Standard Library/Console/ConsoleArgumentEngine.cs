using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Standard_Library.Console
{
    public class ConsoleArgumentEngine
    {
        private Parameter[] parameters;
        private PositionParameter[] positions;
        private FlagParameter[] flags;
        private KeyParameter[] keys;

        public ConsoleArgumentEngine(Parameter[] parameters)
        {
            this.parameters = parameters;

            positions = parameters.Where((s) => s.GetType() == typeof(PositionParameter))
                .Cast<PositionParameter>().ToArray();

            flags = parameters.Where((s) => s.GetType() == typeof(FlagParameter))
                .Cast<FlagParameter>().ToArray();

            keys = parameters.Where((s) => s.GetType() == typeof(KeyParameter))
                .Cast<KeyParameter>().ToArray();
        }

        public Dictionary<string, object> Calculate(string[] args)
        {
            var dic = new Dictionary<string, object>();

            try
            {
                dic = dic.Concat
                (positions.Select((s) => new KeyValuePair<string, object>(s.Name, s.ParseFunc(args[s.Position]))))
                .ToDictionary((s) => s.Key, (s) => s.Value);
            }
            catch (IndexOutOfRangeException)
            {
                throw new ArgumentException(nameof(args), "Args is not has all postion parameters");
            }

            try
            {
                dic = dic.Concat
                (keys.Select((s) => new KeyValuePair<string, object>(s.Name, s.ParseFunc
                    (args[args.ToList().IndexOf("--" + s.Key) + 1])))).ToDictionary((s) => s.Key, (s) => s.Value);
            }
            catch(IndexOutOfRangeException)
            {
                throw new ArgumentException(nameof(args), "Args is not has all keys");
            }

            dic = dic.Concat
            (flags.Select((s) => new KeyValuePair<string, object>(s.Name, args.Contains("--" + s.Key))))
            .ToDictionary((s) => s.Key, (s) => s.Value);

            return dic;
        }



        public abstract class Parameter
        {
            public virtual Func<string, object> ParseFunc { get; set; }
            public string Name { get; set; }
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
            public override Func<string, object> ParseFunc 
            { 
                get => (s) => bool.Parse(s); 
                set => throw new InvalidOperationException("It is readonly property"); 
            }

            public string Key { get; set; }
        }
    }
}
