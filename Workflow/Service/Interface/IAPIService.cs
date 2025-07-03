using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Workflow.Service.Interface
{
    public interface IAPIService
    {
        Task<T?> GetAsync<T>(string endpoint, Dictionary<string, string?>? queryParams = null);
        Task<TResponse?> PostAsync<TRequest, TResponse>(string endpoint, TRequest data);
    }
}
