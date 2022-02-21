using System;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using DataAccess.Balances;
using DTO.Balance;

namespace BusinessLogic.Handlers
{
    public class BalanceHandler
    {
        public BalanceHandler()
        {
        }

        public void CreateNewBalance(IBalanceRepository balanceRepository, HttpClient httpClient, BalanceDto balance, Guid correlationId)
        {
            if (balanceRepository.Exists(balance.Id)) return;
            
            balanceRepository.Create(new BalanceInsertDto
            {
                Id = balance.Id,
                CorrelationId = correlationId,
                Balance = balance.Balance
            });
            
            var result = balanceRepository.Get(balance.Id);
            SendRequest(httpClient, result, correlationId);
        }
        
        public void IncrementBalance(IBalanceRepository balanceRepository, HttpClient httpClient, Guid id, Guid correlationId)
        {
            if (!balanceRepository.Exists(id)) return;
            
            balanceRepository.IncrementBalance(id, correlationId);

            var result = balanceRepository.Get(id);
            
            SendRequest(httpClient, result, correlationId);
        }
        
        public void DecrementBalance(IBalanceRepository balanceRepository, HttpClient httpClient, Guid id, Guid correlationId)
        {
            if (!balanceRepository.Exists(id)) return;
            
            balanceRepository.DecrementBalance(id, correlationId);

            var result = balanceRepository.Get(id);
            
            SendRequest(httpClient, result, correlationId);
        }
        
        private void SendRequest(HttpClient httpClient, BalanceDto balance, Guid correlationId)
        {
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Put,
                RequestUri = new Uri($"api/v1/notify/{balance.Id}"),
                Content = new StringContent(JsonSerializer.Serialize(balance), Encoding.UTF8, "application/json")
            };
            request.Headers.Add("CorrelationId", correlationId.ToString());
            
            httpClient.Send(request);
        }
    }
}