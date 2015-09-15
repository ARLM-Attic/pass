using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pass.Commands
{
    class Reset : IPassCommand
    {
        private readonly string _fileName;

        public Reset(string fileName)
        {
            _fileName = fileName;
        }

        public string GetName()
        {
            return "reset";
        }

        public void Execute(string[] args)
        {
            var passFile = new PassFile();
            string master = string.Empty;
            if (File.Exists(_fileName))
            {
                Console.Write("\r\nOld Password: ");
                master = ConsoleExt.ReadPassword();
                passFile.Load(_fileName);
                if (!passFile.ValidateMaster(master))
                    throw new Exception("Invalid old password.");
            }

            Console.Write("New Master Password: ");
            var pass1 = ConsoleExt.ReadPassword();

            Console.Write("Confirm: ");
            var pass2 = ConsoleExt.ReadPassword();
            if (pass1 != pass2)
                throw new Exception("Passwords do not match.");

            passFile.ResetMaster(master, pass1);
            passFile.Save(_fileName);

        }
    }
}
