using System;

namespace DTO.Messages
{
    public class MessageDto
    {
        public Guid Id { get; set; }
        public Guid CorrelationId { get; set; }
        public string Status { get; set; }
        public string Type { get; set; }
        public string Content { get; set; }
        public DateTimeOffset CreateTime { get; set; }
    }
}