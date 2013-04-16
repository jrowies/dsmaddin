using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using DocumentSessionManager;
using Moq;
using System.Collections;

namespace DocumentSessionManagerTests
{
  [TestFixture]
  public class UnitOfWorkTests
  {
    [TestCase]
    public void TestSimpleCommit()
    {
      var mockRepository = new Mock<IRepository>();

      int savedCount = 0;
      int removedCount = 0;
      mockRepository.Setup(r => r.Persist(It.IsAny<IList<object>>(), It.IsAny<IList<object>>()))
        .Callback(delegate(IList<object> saved, IList<object> removed)
                  {
                    savedCount = saved.Count;
                    removedCount = removed.Count;
                  });

      var uow = new UnitOfWork(mockRepository.Object);

      uow.RegisterSavedOrUpdated(new object());
      uow.RegisterRemoved(new object());
      uow.Commit();

      Assert.AreEqual(1, savedCount);
      Assert.AreEqual(1, removedCount);

      //mockRepository.Verify(r => r.Persist(It.Is<IDictionary<string, object>>(arg => arg.Count == 1), It.Is<IDictionary<string, object>>(arg => arg.Count == 1)), Times.Exactly(1));
      //mockRepository.Verify(r => r.Persist(It.IsAny<IDictionary<string, object>>(), It.IsAny<IDictionary<string, object>>()), Times.Once());
    }

    [TestCase]
    public void TestRollback()
    {
      var mockRepository = new Mock<IRepository>();

      var uow = new UnitOfWork(mockRepository.Object);

      uow.RegisterSavedOrUpdated(new object());
      uow.RegisterRemoved(new object());
      uow.Rollback();

      mockRepository.Verify(r => r.Persist(It.IsAny<IList<object>>(), It.IsAny<IList<object>>()), Times.Never());
    }

    [TestCase]
    public void TestRetryCommit()
    {
      var mockRepository = new Mock<IRepository>();

      bool firstCallToCommit = true;
      int savedCount = 0;

      mockRepository.Setup(r => r.Persist(It.IsAny<IList<object>>(), It.IsAny<IList<object>>()))
        .Callback(delegate(IList<object> saved, IList<object> removed)
        {
          if (firstCallToCommit)
            throw new Exception("First call throws exception for testing purposes");
          else
            savedCount = saved.Count;
        });

      var uow = new UnitOfWork(mockRepository.Object);

      uow.RegisterSavedOrUpdated(new object());

      Assert.Catch(typeof(Exception),
        delegate()
        {
          uow.Commit();
        });

      firstCallToCommit = false;
      uow.Commit();
      Assert.AreEqual(1, savedCount);
    }

    [TestCase]
    public void TestDoubleCommit()
    {
      var mockRepository = new Mock<IRepository>();

      int savedCount = 0;
      mockRepository.Setup(r => r.Persist(It.IsAny<IList<object>>(), It.IsAny<IList<object>>()))
        .Callback(delegate(IList<object> saved, IList<object> removed)
        {
          savedCount = saved.Count;
        });

      var uow = new UnitOfWork(mockRepository.Object);

      uow.RegisterSavedOrUpdated(new object());
      uow.Commit();

      Assert.AreEqual(1, savedCount);

      uow.Commit();
      Assert.AreEqual(0, savedCount);
    }

    [TestCase]
    public void TestActionOnAlreadyRegisteredObject()
    {
      var uow = new UnitOfWork(null);
      object obj = new object();

      uow.RegisterRemoved(obj);
      Assert.Catch(typeof(Exception),
        delegate()
        {
          uow.RegisterRemoved(obj);
        });

      uow = new UnitOfWork(null);
      uow.RegisterRemoved(obj);
      Assert.Catch(typeof(Exception),
        delegate()
        {
          uow.RegisterSavedOrUpdated(obj);
        });

      uow = new UnitOfWork(null);
      uow.RegisterSavedOrUpdated(obj);
      Assert.Catch(typeof(Exception),
        delegate()
        {
          uow.RegisterRemoved(obj);
        });

      uow = new UnitOfWork(null);
      uow.RegisterSavedOrUpdated(obj);
      Assert.Catch(typeof(Exception),
        delegate()
        {
          uow.RegisterSavedOrUpdated(obj);
        });
    }

  }

}
