using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace DailyApp.WPF.HttpClients
{
    /// <summary>
    /// 调用Api工具类
    /// </summary>
    public class HttpRestClient
    {
        private readonly RestClient Client;
        private readonly string baseUrl = "http://localhost:81/api/";

        public HttpRestClient()
        {
            Client = new RestClient();
        }

        /// <summary>
        /// 请求
        /// </summary>
        /// <param name="request">请求数据</param>
        /// <returns>接收的数据</returns>
        public ApiResponse Execute(ApiRequest apiRequest)
        {
            RestRequest request = new RestRequest(apiRequest.Method); // 请求方式
            request.AddHeader("Content-Type", apiRequest.ContentType); // 内容类型
            if (apiRequest.Parameters != null) // 参数
            {
                // 序列化参数
                request.AddParameter("param", JsonConvert.SerializeObject(apiRequest.Parameters), ParameterType.RequestBody);
            }

            Client.BaseUrl = new Uri(baseUrl + apiRequest.Route);
            var response = Client.Execute(request);
            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                // 反序列化返回的数据
                return JsonConvert.DeserializeObject<ApiResponse>(response.Content);
            }
            else
            {
                return new ApiResponse() { ResultCode = -99, Msg = "服务器忙,请稍等..." };
            }
        }

        /// <summary>
        /// 异步请求
        /// </summary>
        /// <param name="apiRequest"></param>
        /// <returns></returns>
        public async Task<ApiResponse> ExecuteAsync(ApiRequest apiRequest)
        {
            RestRequest request = new RestRequest(apiRequest.Method); // 请求方式
            request.AddHeader("Content-Type", apiRequest.ContentType); // 内容类型
            if (apiRequest.Parameters != null) // 参数
            {
                // 序列化参数
                request.AddParameter("param", JsonConvert.SerializeObject(apiRequest.Parameters), ParameterType.RequestBody);
            }

            Client.BaseUrl = new Uri(baseUrl + apiRequest.Route);
            var response = await Client.ExecuteAsync(request);
            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                // 反序列化返回的数据
                return JsonConvert.DeserializeObject<ApiResponse>(response.Content);
            }
            else
            {
                return new ApiResponse() { ResultCode = -99, Msg = "服务器忙,请稍等..." };
            }
        }

    }
}
