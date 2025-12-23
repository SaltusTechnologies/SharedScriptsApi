using Newtonsoft.Json.Linq;
using SharedScriptsApi.Enums;


namespace SharedScriptsApi.Interfaces
{
    public interface IServiceResponse
    {
        JToken? DataObject { get; set; }
        string? Type { get; set; }
        Exception? Exception { get; set; }
        DtCode DtCode { get; set; }
        string? Description { get; }
        Guid ResponseId { get; set; }
    }

    public interface IServiceResponse<T> : IServiceResponse
    {
        new T DataObject { get; set; }
    }
}
