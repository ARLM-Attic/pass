using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace pass.Commands
{
    public class GenereatePass : IPassCommand
    {
        public string GetName() => "generate";

        public void Execute(string[] args)
        {
            var set = ArrayRange('a', 'z').Union(
                                ArrayRange('A', 'Z')).Union(
                                    ArrayRange('0', '9')).Union(
                                        ".!@#$*&%_".ToCharArray()).ToArray();

            int len = 16;

            var rnd = new Random();
            var charArray = Enumerable.Range(1, len).Select(_ => rnd.Next(0, set.Length - 1)).Select(i => set[i]).ToArray();

            var passwrd = new string(charArray);
            //Clipboard.SetText(passwrd);
            Console.Write(passwrd);
        }

        private char[] ArrayRange(char from, char to)
        {
            return Enumerable.Range(@from, to - @from + 1).Select(s => (char)s).ToArray();
        }
    }
}
