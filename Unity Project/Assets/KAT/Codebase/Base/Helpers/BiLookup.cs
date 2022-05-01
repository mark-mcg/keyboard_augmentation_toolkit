using MiscUtil.Collections.Extensions;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace BiLookup
{


    public class BiLookup<TForwardKey, TReverseKey> : IEnumerable<KeyValuePair<TForwardKey, TReverseKey>>
    {
        private Lookup<TForwardKey, TReverseKey> ForwardLookup;
        private Lookup<TReverseKey, TForwardKey> ReverseLookup;
        private IEnumerable<KeyValuePair<TForwardKey, TReverseKey>> data;

        public BiLookup(IEnumerable<KeyValuePair<TForwardKey, TReverseKey>> oneWayMap)
        {
            data = oneWayMap;
            // Create a Lookup to organize the packages. Use the first character of Company as the key value.
            // Select Company appended to TrackingNumber for each element value in the Lookup.
            ForwardLookup = (Lookup<TForwardKey, TReverseKey>)oneWayMap.ToLookup(p => p.Key,
                                                            p => p.Value);

            ReverseLookup = (Lookup<TReverseKey, TForwardKey>)oneWayMap.ToLookup(p => p.Value,
                                                p => p.Key);
        }

        public IEnumerable<TReverseKey> Forward(TForwardKey key)
        {
            return ForwardLookup[key];
        }

        public IEnumerable<TForwardKey> Reverse(TReverseKey key)
        {
            return ReverseLookup[key];
        }

        public TReverseKey ForwardFirst(TForwardKey key)
        {
            if (ForwardLookup[key].Count() == 0)
                UnityEngine.Debug.LogError("Key " + key + " not found, exception...");
            return ForwardLookup[key].First();
        }

        public TForwardKey ReverseFirst(TReverseKey key)
        {
            if (ReverseLookup[key].Count() == 0)
                UnityEngine.Debug.LogError("Key " + key + " not found, exception...");
            return ReverseLookup[key].First();
        }

        public int Count()
        {
            return ForwardLookup.Count();
        }

        IEnumerator<KeyValuePair<TForwardKey, TReverseKey>> IEnumerable<KeyValuePair<TForwardKey, TReverseKey>>.GetEnumerator()
        {
            return data.GetEnumerator();
        }

        public IEnumerator GetEnumerator()
        {
            return data.GetEnumerator();
        }
    }
}