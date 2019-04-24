using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TransactionWrapper;

namespace UnitTestProject1
{
    [TestClass]
    public class HashTableWrapperTest
    {
        
        [TestMethod]
        public void CommitTest()
        {
            HashTableWrapper wrapper = new HashTableWrapper();

            wrapper.BeginTransaction();
            
            int expectedNumberOfElements = 1000;
            for (int i = 0; i < 1000; i++)
            {
                wrapper.Add(i, i);
            }

            wrapper.CommitTransaction();

            Assert.AreEqual(expectedNumberOfElements, wrapper.Count);
        }
        
        [TestMethod]
        public void RollbackTest()
        {
            HashTableWrapper wrapper = new HashTableWrapper();

            wrapper.BeginTransaction();

            int expectedNumberOfElements = 0;
            for (int i = 0; i < 1000; i++)
            {
                wrapper.Add(i, i);
            }

            wrapper.RollbackTransaction();

            Assert.AreEqual(expectedNumberOfElements, wrapper.Count);
        }

        [TestMethod]
        public void CommitAndRollbackTest()
        {
            int[] toCommitData = new[] {1, 3, 5, 7};
            int[] toRollbackData = new[] { 2, 6, 8, 10 };

            HashTableWrapper wrapper = new HashTableWrapper();

            wrapper.BeginTransaction();
            for (int i = 0; i < toCommitData.Length; i++)
            {
                wrapper.Add(toCommitData[i], toCommitData[i]);
            }
            wrapper.CommitTransaction();

            wrapper.BeginTransaction();
            for (int i = 0; i < toRollbackData.Length; i++)
            {
                wrapper.Add(toRollbackData[i], toRollbackData[i]);
            }
            wrapper.RollbackTransaction();

            for (int i = 0; i < toCommitData.Length; i++)
            {
                Assert.IsTrue(wrapper.ContainsKey(toCommitData[i]), "Data committed successfully.");
            }

            for (int i = 0; i < toRollbackData.Length; i++)
            {
                Assert.IsFalse(wrapper.ContainsKey(toRollbackData[i]), "Data rollback performed successfully.");
            }
        }

        [TestMethod]
        public void HashTableBlockedByTransactionTest()
        {
            HashTableWrapper wrapper = new HashTableWrapper();

            wrapper.BeginTransaction();

            for (int i = 0; i < 1000; i++)
            {
                wrapper.Add(i, i);
            }

            Assert.IsTrue(wrapper.IsLocked, "Data locked by transaction successfully.");

            wrapper.RollbackTransaction();
        }
        
        
        [TestMethod]
        public void TestWhenHashTableBlockedByAnotherTransaction()
        {
            HashTableWrapper wrapper = new HashTableWrapper();
            Task.Factory.StartNew(() =>
            {
                wrapper.BeginTransaction();
                bool add1 = wrapper.Add(1,1);
                wrapper.CommitTransaction();
                Assert.IsTrue(add1, "May add record in current transaction.");
            });
            
            Task.Run(() =>
            {
                try
                {
                    wrapper.BeginTransaction();
                    Assert.Fail("Not blocked by another transaction.");
                }
                catch (AccessViolationException e)
                {
                    // nope.
                }
            });
        }
    }
}
