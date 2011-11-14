using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EasySec.Hashing;

namespace NBlog.Specs.Infrastructure
{
    public class DumbHashGenerator : IHashGenerator
    {
        public string GenerateHash(string inputText)
        {
            return inputText;
        }

        public bool CompareHash(string hashedText, string compareText)
        {
            return compareText == hashedText;
        }
    }
}
