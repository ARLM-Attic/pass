using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace pass.Commands
{
    public class DelPass : IPassCommand
    {
        private readonly string _fileName;
        public DelPass(string fileName)
        {
            _fileName = fileName;
        }

        public string GetName()
        {
            return "del";
        }

        public void Execute(string[] args)
        {
            if (!args.Any())
                throw new Exception("Invalid key");

            var passFile = new PassFile();
            passFile.Load(_fileName);
            if (passFile.Remove(args[0].ToLower()))
            {
                passFile.Save(_fileName);
                Console.WriteLine("Removed.");
            }
            else
            {
                Console.WriteLine("Not Found.");
            }
        }
    }
}
