using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace pass.Commands
{
    public class SetPass : IPassCommand
    {
        private readonly string _fileName;

        public SetPass(string fileName)
        {
            _fileName = fileName;
        }

        public string GetName()
        {
            return "set";
        }

        public void Execute(string[] args)
        {
            if (args.Length < 1)
                throw new ArgumentException("set [name] [password?]\r\n");

            var name = args[0].ToLower();

            //Console.WriteLine("Set password for " + name);

            Console.Write("\r\nMaster Password:");
            var master = ConsoleExt.ReadPassword();
            var passFile = new PassFile();
            passFile.Load(_fileName);
            if (!passFile.ValidateMaster(master))
                throw new Exception("Invalid master password.");

            string pass1;
            if (args.Length > 1)
            {
                pass1 = args[1];
            }
            else
            {
                Console.Write("Password: ");
                pass1 = ConsoleExt.ReadPassword("*");
                Console.Write(" Confirm: ");
                var pass2 = ConsoleExt.ReadPassword("*");
                if (pass1 != pass2)
                    throw new Exception("Passwords do not match.");
            }

            passFile.SetPassword(name, master, pass1);
            passFile.Save(_fileName);
        }
    }

    public interface IPassCommand
    {
        string GetName();
        void Execute(string[] args);
    }
}
