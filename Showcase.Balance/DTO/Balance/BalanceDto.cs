using System;

namespace DTO.Balance
{
    public class BalanceDto
    {
        public Guid Id { get; set; }
        public Guid CorrelationId { get; set; }
        public int Balance { get; set; }
        public DateTimeOffset CreateTime { get; set; }
        public DateTimeOffset UpdateTime { get; set; }
    }
}