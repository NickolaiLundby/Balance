using DataAccess.Balances;
using DataAccess.Messages;

namespace DataAccess.DaRunner
{
    public interface IDaFactory
    {
        public IMessageRepository MessageRepository();
        public IBalanceRepository BalanceRepository();
    }
}