using System;
using System.Data;
using System.Linq;
using Dapper;
using Dapper.Contrib.Extensions;
using DTO.Balance;

namespace DataAccess.Balances
{
    public class BalanceRepository : IBalanceRepository
    {
        private readonly IDbConnection _connection;

        public BalanceRepository(IDbConnection connection)
        {
            _connection = connection;
        }

        public void Create(BalanceInsertDto balance)
        {
            var row = Inserter(balance);
            _connection.Insert(row);
        }

        public BalanceDto Get(Guid id)
        {
            const string sql = @"SELECT * FROM Balances WHERE Id = @id;";
            return _connection.Query<BalanceDa>(sql, new {id})
                .Select(Converter)
                .SingleOrDefault();
        }

        public bool Exists(Guid id)
        {
            return _connection.Get<BalanceDa>(id) != default;
        }

        public void IncrementBalance(Guid id, Guid correlationId)
        {
            const string sql = @"UPDATE Balances SET Balance = Balance + 1 WHERE Id = @id;";
            _connection.Execute(sql, new {id});
        }
        
        public void DecrementBalance(Guid id, Guid correlationId)
        {
            const string sql = @"UPDATE Balances SET Balance = Balance - 1 WHERE Id = @id;";
            _connection.Execute(sql, new {id});
        }

        private static BalanceDa Inserter(BalanceInsertDto dto) => new()
        {
            Id = dto.Id,
            CorrelationId = dto.CorrelationId,
            Balance = dto.Balance
        };

        private static BalanceDto Converter(BalanceDa da) => new()
        {
            Id = da.Id,
            CorrelationId = da.CorrelationId,
            Balance = da.Balance,
            CreateTime = da.CreateTime,
            UpdateTime = da.UpdateTime
        };
    }
}