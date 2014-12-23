using System.Collections.Generic;

namespace Kanui.IO.Abstractions
{
    public interface IFSController
    {
        bool TryRead(string pathToFile, out IEnumerable<string> fileContent);
        bool Exists(string pathToFile);
    }
}
