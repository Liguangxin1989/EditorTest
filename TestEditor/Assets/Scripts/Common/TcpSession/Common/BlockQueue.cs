using System.Collections.Generic;
using System.Threading;

namespace NetWrapper.Network
{
    public class BlockQueue<T> where T : class
    {
        private bool isClose = false;

        readonly Queue<T> q = new Queue<T>();

        public int Count
        {
            get { return this.q.Count; }
        }

        public bool Closed
        {
            get { return isClose; }
        }

        public bool Empty()
        {
            lock(q)
            {
                return q.Count == 0;
            }
        }

        public void Clear()
        {
            lock(q)
            {
                q.Clear();
            }
        }

        public void Close()
        {
            isClose = true;
            lock (q)
            {
                q.Clear();
                Monitor.PulseAll(q);
            }
        }

        public void Enqueue(T item)
        {
            lock (q)
            {
                q.Enqueue(item);
                if(q.Count == 1)
                {
                    Monitor.PulseAll(q);
                }
            }
        }

        public void Enqueue(IEnumerable<T> items)
        {
            bool needNotify = false;
            lock (q)
            {
                if (q.Count == 0)
                {
                    needNotify = true;
                }
                foreach (T item in items)
                {
                    q.Enqueue(item);
                }
            }
            if (needNotify)
            {
                Monitor.Pulse(q);
            }
        }

        public T Dequeue()
        {
            return Dequeue(true);
        }

        public T Peek()
        {
            return Dequeue(false);
        }

        public T TryDequeue()
        {
            return TryDequeue(true);
        }

        public T TryPeek()
        {
            return TryDequeue(false);
        }

        public int DequeueAll(IList<T> addTo)
        {
            int count = 0;
            lock (q)
            {
                while (q.Count != 0)
                {
                    T t = q.Dequeue();
                    addTo.Add(t);
                    ++count;
                }
            }
            return count;
        }

        protected T Dequeue(bool isPop)
        {
            T t;
            lock (q)
            {
                while (!isClose && q.Count == 0)
                {
                    Monitor.Wait(q);
                }
                if(isClose)
                {
                    return null;
                }
                if (isPop)
                {
                    t = q.Dequeue();
                }
                else
                {
                    t = q.Peek();
                }
            }
            return t;
        }

        protected T TryDequeue(bool isPop)
        {
            T t = null;
            lock (q)
            {
                if (q.Count == 0)
                {
                    return t;
                }
                if (isPop)
                {
                    t = q.Dequeue();
                }
                else
                {
                    t = q.Peek();
                }
            }
            return t;
        }
    }
}
