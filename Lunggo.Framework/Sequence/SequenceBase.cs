using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;
using System.Web.Razor;
using Lunggo.Framework.SnowMaker;

namespace Lunggo.Framework.Sequence
{
    public abstract class SequenceBase
    {
        protected long GetNextNumber(SequenceProperties properties)
        {
            IUniqueIdGenerator generator = UniqueIdGenerator.GetInstance();
            return generator.NextId(properties.Name);
        }

        protected void Init(SequenceProperties properties)
        {
            IUniqueIdGenerator generator = UniqueIdGenerator.GetInstance();
            generator.SetIdInitialValue(properties.Name,properties.InitialValue);
        }

        public abstract long GetNext();
    }
}
