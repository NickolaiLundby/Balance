using System;
using DTO.Balance;

namespace DataAccess.Balances
{
    public interface IBalanceRepository
    {
        void Create(BalanceInsertDto dto);
        BalanceDto Get(Guid id);
        void IncrementBalance(Guid id, Guid correlationId);
        void DecrementBalance(Guid id, Guid correlationId);
        bool Exists(Guid id);
    }
}