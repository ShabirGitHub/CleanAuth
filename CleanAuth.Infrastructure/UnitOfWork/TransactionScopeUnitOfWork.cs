using System.Transactions;

namespace CleanAuth.Infrastructure.UnitOfWork
{
    public class TransactionScopeUnitOfWork : IUnitOfWork
    {
        private bool _disposed = false;
        private readonly TransactionScope _transactionScope;

        public TransactionScopeUnitOfWork()
        {
            _transactionScope = new TransactionScope(
                    TransactionScopeOption.Required,
                    new TransactionOptions { IsolationLevel = IsolationLevel.ReadCommitted, Timeout = TransactionManager.MaximumTimeout },
                    TransactionScopeAsyncFlowOption.Enabled
                    );
        }
        public void Commit()
        {
            _transactionScope.Complete();
        }
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    _transactionScope.Dispose();
                }

                _disposed = true;
            }
        }
    }
}
