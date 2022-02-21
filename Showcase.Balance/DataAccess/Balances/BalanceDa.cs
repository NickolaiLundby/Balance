using System;
using Dapper.Contrib.Extensions;

namespace DataAccess.Balances
{
    public class BalanceDa
    {
        [ExplicitKey]
        public Guid Id { get; set; }
        public Guid CorrelationId { get; set; }
        public int Balance { get; set; }
        [Write(false)]
        public DateTimeOffset CreateTime { get; set; }
        [Write(false)]
        public DateTimeOffset UpdateTime { get; set; }
    }
}