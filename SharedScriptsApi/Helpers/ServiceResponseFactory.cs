using Microsoft.Data.SqlClient;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SharedScriptsApi.Attributes;
using SharedScriptsApi.DataModels;
using SharedScriptsApi.Enums;
using SharedScriptsApi.Extensions;
using SharedScriptsApi.Interfaces;
using System.Collections;
using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Security.Authentication;

namespace SharedScriptsApi.Helpers
{
    public class ServiceResponseFactory
    {
        public static IServiceResponse Success<T>(Guid id, T data)
        {
            if (data == null)
            {
                return Error(new FormatException("Unable to serialize null response data"));
            }
            else if (data is not object || data is string)
            {
                return new FactoryServiceResponse<T>(id, GetTypeName(typeof(T)), data);
            }
            else if (data is object)
            {
                if (TrySerializeDataObject(data, out JToken token, out string type))
                {
                    return new FactoryServiceResponse(id, type, token);
                }
                else
                {
                    return Error(new FormatException($"Unable to serialize response data of type '${data.GetType()}'"));
                }
            }
            else
            {
                return Error(new FormatException($"Unable to serialize response data of type '${data.GetType()}'"));
            }
        }

        public static IServiceResponse Test<T>(Guid id, T data)
        {
            return new FactoryServiceResponse(id, "String", data.ToString());
        }

        public static IServiceResponse NoContent<T>(Guid id, T data)
        {
            if (TrySerializeDataObject(data, out JToken token, out string type))
            {
                return new FactoryServiceResponse(id, type, null);
            }
            else
            {
                return Error(new FormatException($"Unable to serialize response data of type '${data.GetType()}'"));
            }
        }

        public static IServiceResponse NoContent(Guid id, string typeName)
        {
            return new FactoryServiceResponse(id, typeName, null);
        }

        public static IServiceResponse NoContent(Guid id, string typeName, DtCode dtCode)
        {
            return new FactoryServiceResponse(id, typeName, dtCode, null);
        }

        public static IServiceResponse SuccessWithInfo<T>(Guid id, T data, DtCode dtCode)
        {
            if (TrySerializeDataObject(data, out JToken token, out string type))
            {
                return new FactoryServiceResponse(id, type, dtCode, token);
            }
            else
            {
                return Error(new FormatException($"Unable to serialize response data of type '${data.GetType()}'"));
            }
        }

        public static IServiceResponse InsufficientInformation(Guid id, string message = null) =>
            new FactoryServiceResponse(id, new ArgumentException($"{DtCode.InsufficientInformation.GetDescription()}{(message != null ? $": {message}" : "")}"),
                HttpStatusCode.BadRequest, DtCode.InsufficientInformation);

        public static IServiceResponse InsufficientInformation(Guid? id, string message = null) => 
            new FactoryServiceResponse(id ?? Guid.Empty, new ArgumentException($"{DtCode.InsufficientInformation.GetDescription()}{(message != null ? $": {message}" : "")}"), 
                HttpStatusCode.BadRequest, DtCode.InsufficientInformation);

        public static IServiceResponse InvalidCredentials(Guid id, string message = null) => 
            new FactoryServiceResponse(id, new AuthenticationException($"{DtCode.InvalidCredentials.GetDescription()}{(message != null ? $": {message}" : "")}"), 
                HttpStatusCode.Unauthorized, DtCode.InvalidCredentials);

        public static IServiceResponse InvalidOperation(Guid id, string message = null) => 
            new FactoryServiceResponse(id, new InvalidOperationException($"{DtCode.InvalidOperation.GetDescription()}{(message != null ? $": {message}" : "")}"), 
                HttpStatusCode.BadRequest, DtCode.InvalidOperation);

        public static IServiceResponse Error(Exception error) => 
            new FactoryServiceResponse(Guid.Empty, error, HttpStatusCode.InternalServerError, DtCode.RequestException);

        public static IServiceResponse Error(Guid id, Exception error) => 
            new FactoryServiceResponse(id, error, HttpStatusCode.InternalServerError, DtCode.RequestException);

        public static IServiceResponse Error(Guid id, string error) => 
            new FactoryServiceResponse(id, CreateException(error), HttpStatusCode.InternalServerError, DtCode.RequestException);

        public static IServiceResponse InvalidRequestHeaders(DtCode dtCode) => 
            new FactoryServiceResponse(Guid.Empty, new ArgumentException(dtCode.GetDescription()), HttpStatusCode.BadRequest, dtCode);

        public static IServiceResponse IncidentSyncFailure(Guid id, Exception error) =>
            new FactoryServiceResponse(id, error, HttpStatusCode.InternalServerError, DtCode.IncidentSyncFailure);

        public static IServiceResponse Void(Guid id) => 
            new FactoryServiceResponse(id, "Void");

        private static Exception CreateException(string name)
        {
            Exception retVal;
            switch (name)
            {
                case "argument_null_exception":
                    retVal = new ArgumentNullException();
                    break;
                case "argument_out_of_range_exception":
                    retVal = new ArgumentOutOfRangeException();
                    break;
                case "directory_not_found_exception":
                    retVal = new DirectoryNotFoundException();
                    break;
                case "divide_by_zero_exceptionn":
                    retVal = new DivideByZeroException();
                    break;
                case "dot_net_exception":
                    retVal = new Exception();
                    break;
                case "drive_not_found_exception":
                    retVal = new DriveNotFoundException();
                    break;
                case "file_not_found_exception":
                    retVal = new FileNotFoundException();
                    break;
                case "format_exception":
                    retVal = new FormatException();
                    break;
                case "index_out_of_range_exception":
                    retVal = new IndexOutOfRangeException();
                    break;
                case "invalid_operation_exception":
                    retVal = new InvalidOperationException();
                    break;
                case "json_reader_exception":
                    retVal = new JsonReaderException();
                    break;
                case "key_not_found_exception":
                    retVal = new KeyNotFoundException();
                    break;
                case "not_implemented_exception":
                    retVal = new NotImplementedException();
                    break;
                case "not_supported_exception":
                    retVal = new NotSupportedException();
                    break;
                case "object_disposed_exception":
                    retVal = new ObjectDisposedException("Test");
                    break;
                case "overflow_exception":
                    retVal = new OverflowException();
                    break;
                case "path_too_long_exception":
                    retVal = new PathTooLongException();
                    break;
                case "platform_not_supported_exception":
                    retVal = new PlatformNotSupportedException();
                    break;
                case "rank_exception":
                    retVal = new RankException();
                    break;
                case "sql_exception":
                    try
                    {
                        ThrowSqlException("Data Source={0};Initial Catalog={1};user id={2};password={3};");
                        retVal = null;
                    }
                    catch (Exception ex)
                    {
                        retVal = ex;
                    }
                    break;
                case "timeout_exception":
                    retVal = new TimeoutException();
                    break;
                case "uri_format_exception":
                    retVal = new UriFormatException();
                    break;
                case "validation_exception":
                    retVal = new ValidationException();
                    break;
                default:
                    retVal = new Exception(name);
                    break;
            }
            return retVal;
        }

        /// <summary>
        /// This method is used to mock a SqlException
        /// </summary>
        /// <param name="connectionString"></param>
        private static void ThrowSqlException(string connectionString)
        {
            string queryString = "EXECUTE NonExistantStoredProcedure";

            using (var connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(queryString, connection);

                command.Connection.Open();
                command.ExecuteNonQuery();
            }
        }

        private static bool TrySerializeDataObject<T>(T data, out JToken result, out string typeName)
        {
            typeName = GetTypeName(typeof(T));

            result = data != null ? JToken.FromObject(data, JsonSerializer.Create(JsonHelper.Settings)) : null;

            if (result != null)
            {
                result = DateTimeHelper.ConvertDates(result, HttpType.Reponse);
            }

            return typeName != null;
        }

        private static string GetTypeName(Type type)
        {
            Type appliedType;
            string appliedTypeName = null;
            bool isGenericCollection = false;

            if (type != typeof(string) && typeof(IEnumerable).IsAssignableFrom(type))
            {
                if (type.IsInterface && type.IsGenericType && type.GetGenericTypeDefinition() == typeof(IEnumerable<>))
                {
                    appliedType = type.GetGenericArguments()[0];
                    isGenericCollection = true;
                }
                else
                {
                    var enumerableType = type.GetInterfaces().FirstOrDefault(x => x.IsGenericType && x.GetGenericTypeDefinition() == typeof(IEnumerable<>));
                    appliedType = enumerableType?.GetGenericArguments()[0];
                }
            }
            else
            {
                appliedType = type;
            }

            if (appliedType == null)
            {
                throw new InvalidOperationException();
            }

            TypeNameAttribute attribute = (TypeNameAttribute)appliedType.GetCustomAttributes(typeof(TypeNameAttribute), false).FirstOrDefault();
            if (attribute != null)
            {
                appliedTypeName = attribute.TypeName;

                if (isGenericCollection)
                {
                    appliedTypeName = attribute.CollectionName;
                }

            }
            else if (appliedType.IsInterface)
            {
                string interfaceName = appliedType.Name;
                if (appliedType.IsGenericType)
                {
                    interfaceName = interfaceName.Remove(interfaceName.IndexOf('<'));
                }

                appliedTypeName = interfaceName.Substring(1);

            }
            else
            {
                appliedTypeName = appliedType.Name;
            }
            return appliedTypeName;
        }
    }
}
