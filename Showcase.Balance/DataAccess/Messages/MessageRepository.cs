using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Dapper;
using Dapper.Contrib.Extensions;
using DTO.Constants;
using DTO.Messages;

namespace DataAccess.Messages
{
    public class MessageRepository : IMessageRepository
    {
        private readonly IDbConnection _connection;

        public MessageRepository(IDbConnection connection)
        {
            _connection = connection;
        }

        public void Create(MessageInsertDto message)
        {
            var row = Inserter(message);
            _connection.Insert(row);
        }

        public MessageDto Get(Guid id)
        {
            var row = _connection.Get<MessageDa>(id);
            return Converter(row);
        }

        public IEnumerable<MessageDto> GetPending()
        {
            const string sql = "SELECT * FROM Messages WHERE Status = 'Pending' ORDER BY CreateTime DESC;";
            var rows = _connection.Query<MessageDa>(sql);
            return rows?.Select(Converter);
        }

        public bool Exists(Guid id)
        {
            var row = _connection.Get<MessageDa>(id);
            return row != null;
        }

        public void UpdateStatus(Guid id, string status)
        {
            const string sql = "UPDATE Messages SET Status = @status WHERE Id = @id;";
            _connection.Execute(sql, new {id, status});
        }

        private static MessageDa Inserter(MessageInsertDto dto) => new()
        {
            Id = dto.Id,
            CorrelationId = dto.CorrelationId,
            Status = Status.Pending,
            Type = dto.Type,
            Content = dto.Content
        };

        private static MessageDto Converter(MessageDa da) => new()
        {
            Id = da.Id,
            CorrelationId = da.CorrelationId,
            Status = da.Status,
            Type = da.Type,
            Content = da.Content,
            CreateTime = da.CreateTime
        };
    }
}