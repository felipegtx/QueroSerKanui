using Kanui.IO.Abstractions;
using System.Collections.Generic;
using System.IO;

namespace Kanui.IO
{
    internal sealed class FSController : IFSController
    {
        public bool TryRead(string pathToFile, out IEnumerable<string> fileContent)
        {
            fileContent = null;
            if (!Exists(pathToFile)) { return false; }

            using (var fs = new FileStream(pathToFile, System.IO.FileMode.Open, System.IO.FileAccess.Read, System.IO.FileShare.ReadWrite))
            {
                using (var sr = new System.IO.StreamReader(fs))
                {
                    var lst = new List<string>();
                    while (!sr.EndOfStream) { lst.Add(sr.ReadLine()); }
                    fileContent = lst;
                }

            }
            return true;
        }

        public bool Exists(string pathToFile)
        {
            return File.Exists(pathToFile);
        }
    }
}
