using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace TransactionWrapper
{
    public static class Transaction
    {
      
        public static bool BeginTransaction(this HashTableWrapper hashTable)
        {
            if (!Monitor.TryEnter(hashTable))
            {
                throw new AccessViolationException("The same table already locked!");
            }

            return true;
        }

        public static void CommitTransaction(this HashTableWrapper hashTable)
        {
            hashTable.TransactionJournalAdd.Clear();
            hashTable.TransactionJournalRemove.Clear();
            Monitor.Exit(hashTable);
        }

        public static void RollbackTransaction(this HashTableWrapper hashTable)
        {
            foreach (var journalItem in hashTable.TransactionJournalAdd)
            {
                hashTable.Remove(journalItem);
            }

            foreach (var journalItem in hashTable.TransactionJournalRemove)
            {
                hashTable.Add(journalItem);
            }
            Monitor.Exit(hashTable);
        }
    }
}
