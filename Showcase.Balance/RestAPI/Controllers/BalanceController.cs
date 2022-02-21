using System;
using System.Net.Http;
using System.Text.Json;
using BusinessLogic.Handlers;
using DataAccess.DaRunner;
using DTO.Balance;
using DTO.Constants;
using DTO.Messages;
using Microsoft.AspNetCore.Mvc;

namespace RestAPI.Controllers
{
    [Route("api/v1/balance")]
    [ApiController]
    public class BalanceController : ControllerBase
    {
        private readonly DaRunner _daRunner;
        private readonly HttpClient _httpClient;

        public BalanceController(DaRunner daRunner, HttpClient httpClient)
        {
            _daRunner = daRunner;
            _httpClient = httpClient;
        }
        
        [HttpPut]
        [Route("new/{idempotencyKey}")]
        public ActionResult Create([FromRoute] Guid idempotencyKey, [FromBody] BalanceDto dto)
        {
            var correlationId = FetchCorrelationIdFromQuest();

            _daRunner.Run(da =>
            {
                if (da.MessageRepository().Exists(idempotencyKey)) return;

                da.MessageRepository().Create(new MessageInsertDto
                {
                    Id = idempotencyKey,
                    CorrelationId = correlationId,
                    Type = "CreateBalance",
                    Direction = Direction.Inbound,
                    Content = JsonSerializer.Serialize(dto)
                });
                
                new BalanceHandler().CreateNewBalance(
                    da.BalanceRepository(), 
                    _httpClient, 
                    dto, 
                    correlationId);
            });

            return Ok();
        }
        
        [HttpPut]
        [Route("update/{idempotencyKey}")]
        public ActionResult Update([FromRoute] Guid idempotencyKey, [FromBody] BalanceUpdateDto dto)
        {
            var correlationId = FetchCorrelationIdFromQuest();

            _daRunner.Run(da =>
            {
                if (da.MessageRepository().Exists(idempotencyKey)) return;

                da.MessageRepository().Create(new MessageInsertDto
                {
                    Id = idempotencyKey,
                    CorrelationId = correlationId,
                    Type = "UpdateBalance",
                    Direction = Direction.Inbound,
                    Content = JsonSerializer.Serialize(dto)
                });

                switch (dto.Action)
                {
                    case BalanceActions.Increment:
                        new BalanceHandler().IncrementBalance(da.BalanceRepository(), _httpClient, dto.Id, correlationId);
                        break;
                    case BalanceActions.Decrement:
                        new BalanceHandler().DecrementBalance(da.BalanceRepository(), _httpClient, dto.Id, correlationId);
                        break;
                    default:
                        return;
                }
            });

            return Ok();
        }

        private Guid FetchCorrelationIdFromQuest()
        {
            var correlationId = Request.Headers.TryGetValue("CorrelationId", out var id)
                ? Guid.Parse(id)
                : Guid.NewGuid();
            return correlationId;
        }
    }
}