using System;

///https://gist.github.com/felipegtx/596dfc55131359ec354f
namespace Kanui.DI
{
    public abstract class InstanceResolverFor<SomeType>
    {
        public static Func<SomeType> InstanceBuilder = () =>
        {
            throw new Exception(string.Format("The type '{0}' does not have a valid factory.", typeof(SomeType).FullName));
        };

        public static SomeType Instance { get { return InstanceBuilder(); } }
    }
}
