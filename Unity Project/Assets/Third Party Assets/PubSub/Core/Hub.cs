using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace PubSub
{
    public class Hub
    {
        internal List<Handler> _handlers = new List<Handler>();
        internal object _locker = new object();
        private static Hub _default;

        public static Hub Default => _default ?? (_default = new Hub());
        
        /// <summary>
        /// Modified Publish method - the generic version wouldn't work correctly when publishing an object cast
        /// as its base type - it would never invoke a subscription for the supertype.
        /// 
        /// This will inspect the supertype and get all handlers that match.
        /// </summary>
        /// <param name="data"></param>
        public void Publish(object data)
        {
            foreach (var handler in GetAliveHandlers(data.GetType()))
            {
                handler.Action.DynamicInvoke(data);
            }
        }

        public void PublishGeneric<T>(T data = default(T))
        {
            foreach (var handler in GetAliveHandlers<T>())
            //foreach (var handler in GetAliveHandlers(data.GetType()))
            {
                switch (handler.Action)
                {
                    case Action<T> action:
                        action(data);
                        break;
                    case Func<T, Task> func:
                        func(data);
                        break;
                }
            }
        }

        public async Task PublishAsync<T>(T data = default(T))
        {
            foreach (var handler in GetAliveHandlers<T>())
            {
                switch (handler.Action)
                {
                    case Action<T> action:
                        action(data);
                        break;
                    case Func<T, Task> func:
                        await func(data);
                        break;
                }
            }
        }

        /// <summary>
        ///     Allow subscribing directly to this Hub.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="handler"></param>
        public void Subscribe<T>(Action<T> handler)
        {
            Subscribe(this, handler);
        }

        public void Subscribe<T>(object subscriber, Action<T> handler)
        {
            SubscribeDelegate<T>(subscriber, handler);
        }

        public void Subscribe<T>(Func<T, Task> handler)
        {
            Subscribe(this, handler);
        }

        public void Subscribe<T>(object subscriber, Func<T, Task> handler)
        {
            SubscribeDelegate<T>(subscriber, handler);
        }

        /// <summary>
        ///     Allow unsubscribing directly to this Hub.
        /// </summary>
        public void Unsubscribe()
        {
            Unsubscribe(this);
        }

        public void Unsubscribe(object subscriber)
        {
            lock (_locker)
            {
                var query = _handlers.Where(a => !a.Sender.IsAlive ||
                                                a.Sender.Target.Equals(subscriber));

                foreach (var h in query.ToList())
                    _handlers.Remove(h);
            }
        }

        /// <summary>
        ///     Allow unsubscribing directly to this Hub.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        public void Unsubscribe<T>()
        {
            Unsubscribe<T>(this);
        }

        /// <summary>
        ///     Allow unsubscribing directly to this Hub.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="handler"></param>
        public void Unsubscribe<T>(Action<T> handler)
        {
            Unsubscribe(this, handler);
        }

        public void Unsubscribe<T>(object subscriber, Action<T> handler = null)
        {
            lock (_locker)
            {
                var query = _handlers.Where(a => !a.Sender.IsAlive ||
                                                a.Sender.Target.Equals(subscriber) && a.Type == typeof(T));

                if (handler != null)
                    query = query.Where(a => a.Action.Equals(handler));

                foreach (var h in query.ToList())
                    _handlers.Remove(h);
            }
        }

        public bool Exists<T>()
        {
            return Exists<T>(this);
        }

        public bool Exists<T>(object subscriber)
        {
            lock (_locker)
            {
                foreach (var h in _handlers)
                {
                    if (Equals(h.Sender.Target, subscriber) &&
                         typeof(T) == h.Type)
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        public bool Exists<T>(object subscriber, Action<T> handler)
        {
            lock (_locker)
            {
                foreach (var h in _handlers)
                {
                    if (Equals(h.Sender.Target, subscriber) &&
                         typeof(T) == h.Type &&
                         h.Action.Equals(handler))
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        private void SubscribeDelegate<T>(object subscriber, Delegate handler)
        {
            var item = new Handler
            {
                Action = handler,
                Sender = new WeakReference(subscriber),
                Type = typeof(T)
            };

            lock (_locker)
            {
                _handlers.Add(item);
            }
        }

        public List<Handler> GetAliveHandlers(Type type)
        {
            PruneHandlers();
            return _handlers.Where(h => h.Type.GetTypeInfo().IsAssignableFrom(type.GetTypeInfo())).ToList();
        }

        public List<Handler> GetAliveHandlers<T>()
        {
            PruneHandlers();
            return _handlers.Where(h => h.Type.GetTypeInfo().IsAssignableFrom(typeof(T).GetTypeInfo())).ToList();
        }

        private void PruneHandlers()
        {
            lock (_locker)
            {
                for (int i = _handlers.Count - 1; i >= 0; --i)
                {
                    if (!_handlers[i].Sender.IsAlive)
                        _handlers.RemoveAt(i);
                }
            }
        }

        public class Handler //internal
        {
            public Delegate Action { get; set; }
            public WeakReference Sender { get; set; }
            public Type Type { get; set; }
        }
    }
}