using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace pass.Commands
{
    public class ListPass : IPassCommand
    {
        private readonly string _fileName;

        public ListPass(string fileName)
        {
            _fileName = fileName;
        }

        public string GetName()
        {
            return "list";
        }

        public void Execute(string[] args)
        {
            var pattern = (!args.Any()) ? ".*" : args[0];
            var passFile = new PassFile();
            passFile.Load(_fileName);
            string[] names = passFile.GetNames();
            
            var reg = new Regex(pattern, RegexOptions.Compiled | RegexOptions.IgnoreCase);
            Console.WriteLine();
            foreach (var n in names)
            {
                if (reg.Match(n).Success)
                    Console.WriteLine(n);
            }

            Console.WriteLine();

        }
    }
}
