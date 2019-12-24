using System;
using System.Collections.Generic;
using System.Text;

namespace BLL.Interfaces
{
    public interface IMapper<TFirst, TSecond>
    {
        public TFirst Map(TSecond element);
        public TSecond Map(TFirst element);
        public IEnumerable<TFirst> Map(IEnumerable<TSecond> elements, Action<TFirst> callback = null);
        public IEnumerable<TSecond> Map(IEnumerable<TFirst> elements, Action<TSecond> callback = null);
    }
}
