using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mime;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using pass.Commands;

namespace pass
{
    class Program
    {
        static void Main(string[] args)
        {
            string fileName = GetDefaultFile();
            Registra(new Reset(fileName));
            Registra(new SetPass(fileName));
            Registra(new GetPass(fileName));
            Registra(new ListPass(fileName));
            Registra(new DelPass(fileName));
            Registra(new GenereatePass());



            IPassCommand cmd;
            if ((!args.Any())
                || !_commands.TryGetValue(args[0], out cmd))
            {
                Console.WriteLine("\t\npass [command] [parameters...]");
                Console.WriteLine("\t\nAvaliable Commands:");
                foreach (var c in _commands)
                    Console.WriteLine(c.Key);

                Console.WriteLine();
                return;
            }


            try
            {
                cmd.Execute(args.Skip(1).ToArray());
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

        }

        private static void Registra(IPassCommand cmd)
        {
            _commands.Add(cmd.GetName(), cmd);
        }

        static Dictionary<string, IPassCommand> _commands = new Dictionary<string, IPassCommand>(); 

        private static string GetDefaultFile()
        {
            var dir = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            string file = Path.Combine(dir, "pass_data.dat");
            file = Path.GetFullPath(file);
            return file;
        }
    }
}
