using System;
using System.Collections;
using System.Collections.Generic;
namespace Spider.Data
{
    //非线程安全的线性表
    public class LineList<T> : IList<T>
    {
        private class Node<D>
        {
            public Node(D data, Node<D> next)
            {
                Data = data;
                Next = next;
            }
            public Node(D data)
                : this(data, null)
            {
            }
            public Node()
                : this(default(D))
            {
            }
            public D Data { set; get; }
            public Node<D> Next { set; get; }
        }        
        private int _count;
        private Node<T> _head;
        private Node<T> Head
        {
            get { return _head; }
            set { _head = value; }
        }
        private Node<T> _current;
        private Node<T> Current
        {
            get { return _current; }
            set { _current = value; }
        }
        public LineList()
        {
            Count = 0;
        }
        public LineList(IEnumerable<T> nodes)
            : this()
        {
            if (nodes != null)
            {
                foreach (T item in nodes)
                {
                    Add(item);
                }
            }
        }
        public int IndexOf(T item)
        {
            int index = -1;
            bool flag = false;
            Node<T> node = Head;
            while (node != null)
            {
                index++;
                if (node.Data.Equals(item))
                {
                    flag = true;
                    break;
                }
                node = node.Next;
            }
            if (flag)
            {
                return index;
            }
            else
            {
                return -1;
            }
        }

        public void Insert(int index, T item)
        {
            if (IsReadOnly)
            {
                throw new InvalidOperationException("The object is readonly.");
            }
            else if (index < 0)
            {
                throw new ArgumentOutOfRangeException("index");
            }
            else
            {
                int count = 0;
                bool success = false;
                Node<T> node = Head;
                if(Head == null)
                {
                   Add(item);
                   success = true;
                }
                else{
                while (node != null)
                {
                    if (index == count)
                    {
                        Node<T> itemNode = new Node<T>(item);
                        itemNode.Next = node;
                        node = itemNode;
                        if(index == 0)
                        {
                           Head=itemNode;
                        }
                        Count++;
                        success = true;
                        break;
                    }
                    node = node.Next;
                    count++;
                }
                }
                if (!success)
                {
                    throw new ArgumentOutOfRangeException("index");
                }
            }
        }

        public void RemoveAt(int index)
        {
            if (IsReadOnly)
            {
                throw new InvalidOperationException("The object is readonly.");
            }
            else
            {
                int count = 0;
                bool success = false;
                Node<T> node = Head;
                while (node != null)
                {
                    if (index == count)
                    { 
                        if(node.Next!=null)
                        {           
                            node.Next = node.Next.Next;
                        }
                        else
                        {
                            node.Next=null;
                        }
                        Count--;
                        success = true;
                        break;
                    }
                    node = node.Next;
                    if(index == 0)
                    {
                        Head=node;
                    }
                    count++;
                }
                if (!success)
                {
                    throw new IndexOutOfRangeException();
                }
            }
        }

        public T this[int index]
        {
            get
            {
                if (index < 0)
                {
                    throw new IndexOutOfRangeException();
                }
                Node<T> node = Head;
                int count = 0;
                while (node != null)
                {
                    if (index == count)
                    {
                        return node.Data;
                    }
                    node = node.Next;
                    count++;
                }
                throw new IndexOutOfRangeException();
            }
            set
            {
                if (index < 0)
                {
                    throw new IndexOutOfRangeException();
                }
                Node<T> node = Head;
                bool flag = false;
                int count = 0;
                while (node != null)
                {
                    if (index == count)
                    {
                        node.Data = value;
                        flag = true;
                        break;
                    }
                    node = node.Next;
                    count++;
                }
                if (!flag)
                {
                    throw new IndexOutOfRangeException();
                }
            }
        }

        public void Add(T item)
        {
            if (IsReadOnly)
            {
                throw new InvalidOperationException("The object is readonly.");
            }
            else
            {
                Node<T> node = Current;
                if (Current == null)
                {
                    Head = new Node<T>(item);
                    Current = Head;
                }
                else
                {
                    Current.Next = new Node<T>(item);
                    Current = Current.Next;
                }
                Count++;
            }
        }

        public void Clear()
        {
            if (IsReadOnly)
            {
                throw new InvalidOperationException("The object is readonly.");
            }
            else
            {
                Node<T> node = Head;
                while (node != null)
                {
                    var temp=node;                    
                    node = node.Next;
                    temp.Next=null;
                }
                Head = null;
                Current = null;
                Count = 0;
            }
        }

        public bool Contains(T item)
        {
            Node<T> node = Head;
            while (node != null)
            {
                if (node.Data.Equals(item))
                {
                    return true;
                }
                node = node.Next;
            }
            return false;
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            if (arrayIndex < 0 || arrayIndex > array.Length)
            {
                throw new ArgumentOutOfRangeException("arrayIndex");
            }
            else
            {
                Array.Copy(this.ToArray(), 0, array, arrayIndex, Count);
            }
        }
        protected T[] ToArray()
        {
            T[] array = new T[Count];
            Node<T> node = Head;
            int i = 0;
            while (node != null)
            {
                array[i] = node.Data;
                node = node.Next;                
                i++;
            }
            return array;
        }
        public int Count
        {
            get
            {
                return _count;
            }
            private set
            {
                _count = value;
            }
        }

        public bool IsReadOnly
        {
            set;
            get;
        }

        public bool Remove(T item)
        {
            Node<T> node = Head;
            while (node != null)
            {
                if (node.Data.Equals(item))
                {
                    node.Next = node.Next == null?null : node.Next.Next;
                    Count--;
                    return true;
                }
                node = node.Next;
            }
            return false;
        }

        public IEnumerator<T> GetEnumerator()
        {
            Node<T> node = Head;
            while (node != null)
            {
                var data = node.Data;
                node = node.Next;
                yield return data;
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            Node<T> node = Head;
            while (node != null)
            {
                var data = node.Data;
                node = node.Next;
                yield return data;
            }
        }
    }
}
