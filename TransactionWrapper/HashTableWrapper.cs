using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace TransactionWrapper
{
    public class HashTableWrapper : IDictionary<int, int>
    {
        private Dictionary<int, int> HashTable { get; set; }

        public Dictionary<int, int> TransactionJournalAdd { get; set; }

        public Dictionary<int, int> TransactionJournalRemove { get; set; }


        public HashTableWrapper()
        {
            HashTable = new Dictionary<int, int>();
            TransactionJournalAdd = new Dictionary<int, int>();
            TransactionJournalRemove = new Dictionary<int, int>();
        }

        public bool ContainsKey(int key)
        {
            return HashTable.ContainsKey(key);
        }

        void IDictionary<int, int>.Add(int key, int value)
        {
            Add(key, value);
        }

        bool IDictionary<int, int>.Remove(int key)
        {
            if (!HashTable.ContainsKey(key))
                return false;

            TransactionJournalRemove.Add(key, HashTable[key]);
            return HashTable.Remove(key);
        }

        public bool TryGetValue(int key, out int value)
        {
            throw new NotImplementedException();
        }

        public int this[int key]
        {
            get => HashTable[key];
            set => throw new NotImplementedException();
        }

        public ICollection<int> Keys => HashTable.Keys;
        public ICollection<int> Values => HashTable.Values;

        public bool Add(int key, int value)
        {
            HashTable.Add(key, value);
            TransactionJournalAdd.Add(key, value);
            return true;
        }

        public int Get(int key)
        {
            return HashTable[key];
        }

        public bool Remove(int key)
        {
            return HashTable.Remove(key);
        }

        public IEnumerator<KeyValuePair<int, int>> GetEnumerator()
        {
            throw new NotImplementedException();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public void Add(KeyValuePair<int, int> item)
        {
            Add(item.Key, item.Value);
        }

        public void Clear()
        {
            throw new NotImplementedException();
        }

        public bool Contains(KeyValuePair<int, int> item)
        {
            throw new NotImplementedException();
        }

        public void CopyTo(KeyValuePair<int, int>[] array, int arrayIndex)
        {
            throw new NotImplementedException();
        }

        public bool Remove(KeyValuePair<int, int> item)
        {
            return Remove(item.Key);
        }

        public int Count => HashTable.Count;
        public bool IsReadOnly => Monitor.IsEntered(this);

        public bool IsLocked => Monitor.IsEntered(this);
    }
}
