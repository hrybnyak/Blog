using BLL.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace BLL.Mappers
{ 
    public abstract class MapperBase<TFirst, TSecond> : IMapper<TFirst, TSecond>
    {
        public abstract TFirst Map(TSecond element);
        public abstract TSecond Map(TFirst element);
        public IEnumerable<TFirst> Map(IEnumerable<TSecond> elements, Action<TFirst> callback = null)
        {
            var objectCollection = new List<TFirst>();
            if (elements != null)
            {
                foreach (TSecond element in elements)
                {
                    TFirst newObject = Map(element);
                    if (newObject != null)
                    {
                        if (callback != null)
                            callback(newObject);
                        objectCollection.Add(newObject);
                    }
                }
            }
            return objectCollection;
        }
        public IEnumerable<TSecond> Map(IEnumerable<TFirst> elements, Action<TSecond> callback = null)
        {
            var objectCollection = new List<TSecond>();

            if (elements != null)
            {
                foreach (TFirst element in elements)
                {
                    TSecond newObject = Map(element);
                    if (newObject != null)
                    {
                        if (callback != null)
                            callback(newObject);
                        objectCollection.Add(newObject);
                    }
                }
            }
            return objectCollection;
        }
    }
}
