using System.Data;
using DataAccess.Balances;
using DataAccess.Messages;

namespace DataAccess.DaRunner
{
    public class DaFactory : IDaFactory
    {
        private readonly IDbConnection _connection;

        public DaFactory(IDbConnection connection)
        {
            _connection = connection;
        }

        public IMessageRepository MessageRepository()
        {
            return new MessageRepository(_connection);
        }

        public IBalanceRepository BalanceRepository()
        {
            return new BalanceRepository(_connection);
        }
    }
}