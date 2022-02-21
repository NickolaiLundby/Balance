using System;

namespace DTO.Balance
{
    public class BalanceInsertDto
    {
        public Guid Id { get; set; }
        public Guid CorrelationId { get; set; }
        public int Balance { get; set; }
    }
}