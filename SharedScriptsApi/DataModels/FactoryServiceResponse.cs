using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SharedScriptsApi.Enums;
using SharedScriptsApi.Extensions;
using SharedScriptsApi.Interfaces;
using System.Net;

namespace SharedScriptsApi.DataModels
{
    public class FactoryServiceResponse : IServiceResponse
    {
        [JsonProperty(PropertyName = "Id")]
        public string Id { get => ResponseId.ToString(); }
        [JsonIgnore]
        public Guid ResponseId { get; set; }
        public string? Type { get; set; }
        [JsonProperty(PropertyName = "Data")]
        public JToken? DataObject { get; set; }
        public Exception? Exception { get; set; }

        public DtCode DtCode { get; set; } = default;
        [JsonIgnore]
        public string Description { get => DtCodeExtensions.GetDescription(this.DtCode); }

        public FactoryServiceResponse(Guid responseId, HttpStatusCode statusCode, DtCode dtCode)
        {
            this.ResponseId = responseId;
            this.DtCode = dtCode;
        }
        public FactoryServiceResponse(Guid responseId, string type, DtCode dtCode, JToken data = null)
            : this(responseId, HttpStatusCode.OK, dtCode)
        {
            this.DataObject = data;
            this.DtCode = dtCode;
            this.Type = type;
        }
        public FactoryServiceResponse(Guid responseId, Exception exception, HttpStatusCode statusCode, DtCode dtCode)
            : this(responseId, statusCode, dtCode)
        {
            this.Exception = exception;
            this.Type = exception.GetType().Name;
        }
        public FactoryServiceResponse(Guid responseId, string type, JToken data = null)
            : this(responseId, HttpStatusCode.OK, DtCode.Success)
        {
            this.DataObject = data;
            this.Type = type;
        }
    }

    public class FactoryServiceResponse<T> : FactoryServiceResponse, IServiceResponse<T>
    {
        [JsonProperty(PropertyName = "Data")]
        public new T DataObject { get; set; }
        public FactoryServiceResponse(Guid responseId, HttpStatusCode statusCode, DtCode dtCode)
            : base(responseId, statusCode, dtCode) { }
        public FactoryServiceResponse(Guid responseId, string typeName, T data = default)
            : base(responseId, HttpStatusCode.OK, DtCode.Success)
        {
            this.DataObject = data;
            this.Type = typeName;
        }
    }
}