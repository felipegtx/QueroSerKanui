using Kanui.IO.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.CSharp;
namespace Kanui.Tests.Fakes
{
    public class FakeSerializer : ISerializer
    {
        public Func<object> FakeDeserialize { get; set; }
        public Action<dynamic> FakeSerialize { get; set; }

        public void Serialize<SomeType>(SomeType data, string pathToFile)
        {
            FakeSerialize(data);
        }

        public SomeType DeserializeFrom<SomeType>(string pathToFile)
        {
            return (SomeType)FakeDeserialize();
        }
    }
}
