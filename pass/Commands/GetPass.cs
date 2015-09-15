using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pass.Commands
{
    public class GetPass  : IPassCommand
    {
        private readonly string _fileName;

        public GetPass(string fileName)
        {
            _fileName = fileName;
        }

        public string GetName()
        {
            return "get";
        }

        public void Execute(string[] args)
        {
            if (args.Length <= 0)
                throw new ArgumentException("get [name]\r\n");

            var name = args[0].ToLower();
            Console.Write("\r\nMaster Password: ");
            var master = ConsoleExt.ReadPassword();
            var passFile = new PassFile();
            passFile.Load(_fileName);

            string passwrd = passFile.GetPassword(name, master);
            Console.WriteLine("Password: " + passwrd + "\n\n");
        }
    }
}
