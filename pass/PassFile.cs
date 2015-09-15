using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace pass
{
    public class PassFile
    {
        private string _encryptedCRC;


        public PassFile()
        {
            Passwords = new Dictionary<string, Tuple<string, string>>();
        }


        //IV / ENCRYPTED
        public Dictionary<string, Tuple<string, string>> Passwords { get; set; }


        public void Load(string fileName)
        {
            using (StreamReader sr = new StreamReader(fileName, Encoding.Default))
            {
                Load(sr);
            }
        }
        public void Load(StreamReader sr)
        {
            this._encryptedCRC = sr.ReadLine();
            Passwords.Clear();
            while (!sr.EndOfStream)
            {
                var r = sr.ReadLine().Split('\t');
                Passwords.Add(r[0], Tuple.Create(r[1], r[2]));
            };
        }

        public void Save(StreamWriter sw)
        {
            sw.WriteLine(_encryptedCRC);
            foreach (var pass in Passwords)
                sw.WriteLine("{0}\t{1}\t{2}", pass.Key, pass.Value.Item1, pass.Value.Item2);
        }


        public bool ValidateMaster(string masterKey)
        {
            var sha = new SHA256Managed();
            var kHash = Convert.ToBase64String(sha.ComputeHash(Encoding.ASCII.GetBytes(masterKey+"invariable#%!")));
            if (kHash != _encryptedCRC)
                return false;

            return true;

        }


        public void SetPassword(string name, string masterKey, string password)
        {
            if (!ValidateMaster(masterKey))
                throw new ArgumentException("Invalid Master Key.");

            var crypto = new Crypto();
            var iv = crypto.CreateIV();
            var encrypted = crypto.EncryptStringToBytes(password, crypto.ConvertStringKey(masterKey), iv);
            Passwords[name.ToLower()] = Tuple.Create(Convert.ToBase64String(iv), Convert.ToBase64String(encrypted));
        }

        public void ResetMaster(string oldKey, string newKey)
        {
            if (!string.IsNullOrEmpty(oldKey) && !string.IsNullOrEmpty(_encryptedCRC))
            {
                if (!ValidateMaster(oldKey))
                    throw new ArgumentException("Invalid old master key");
            }

            var sha = new SHA256Managed();
            this._encryptedCRC = Convert.ToBase64String(sha.ComputeHash(Encoding.ASCII.GetBytes(newKey + "invariable#%!")));
            
            if (!Passwords.Any())
                return;

            var crypto = new Crypto();

            var pass = Passwords.ToArray();
            var b_oldKey = crypto.ConvertStringKey(oldKey);
            var b_newKey = crypto.ConvertStringKey(newKey);
            Passwords.Clear();
            foreach (var p in pass)
            {
                var password = crypto.DecryptStringFromBytes(Convert.FromBase64String(p.Value.Item2), b_oldKey, Convert.FromBase64String(p.Value.Item1));
                var iv = crypto.CreateIV();
                var encrypted = crypto.EncryptStringToBytes(password, b_newKey, iv);
                Passwords[p.Key] = Tuple.Create(Convert.ToBase64String(iv), Convert.ToBase64String(encrypted));
            }

        }


        public string GetPassword(string name, string masterKey)
        {
            if (!ValidateMaster(masterKey))
                throw new ArgumentException("Invalid Master Key.");

            var val = Passwords[name.ToLower()];
            var crypto = new Crypto();

            var result = crypto.DecryptStringFromBytes(Convert.FromBase64String(val.Item2),
                crypto.ConvertStringKey(masterKey), Convert.FromBase64String(val.Item1));
           
            return result;

        }


        public void Save(string fileName)
        {
            using (StreamWriter sw = new StreamWriter(fileName, false, Encoding.Default))
            {
                Save(sw);
            }
        }

        public string[] GetNames()
        {
            return Passwords.Keys.ToArray();
        }

        public bool Remove(string key)
        {
            return Passwords.Remove(key);
        }
    }
}
