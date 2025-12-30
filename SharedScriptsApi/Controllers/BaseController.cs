using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json.Linq;
using SharedScriptsApi.Helpers;
using SharedScriptsApi.Interfaces;

namespace SharedScriptsApi.Controllers
{
    /// <summary>
    /// Base class for all 2.0 controllers
    /// </summary>
    [Produces("application/json")]
    [Consumes("application/json")]
    [Route("api/{version:apiVersion}")]
    [ApiController]
    [Authorize]
    public class BaseController : Controller
    {
        protected readonly ILogger<BaseController> _logger;
        protected readonly IHttpContextAccessor _httpContextAccessor;
        protected readonly IServiceProvider _serviceProvider;
        private const string API_BASE = "api";
        private const string API_VERSION_ROUTE_CONSTRAINT_NAME = "version";
        private const string DEFAULT_API_VERSION = "2.0";

        public BaseController(ILogger<BaseController> logger, IHttpContextAccessor httpContextAccessor, IServiceProvider serviceProvider)
        {
            _logger = logger;
            _httpContextAccessor = httpContextAccessor;
            _serviceProvider = serviceProvider;
        }

        /// <summary>
        /// Gets the route value dictionary for the controller context
        /// </summary>
        /// <returns></returns>
        protected RouteValueDictionary GetRouteValueDictionary() => ControllerContext.RouteData.Values;
        /// <summary>
        /// Gets the base request segment
        /// </summary>
        /// <returns></returns>
        protected string GetBaseRequestSegment()
        {
            if (GetRouteValueDictionary().TryGetValue(API_VERSION_ROUTE_CONSTRAINT_NAME, out object? version))
            {
                return $"/{API_BASE}/{(version?.ToString() ?? DEFAULT_API_VERSION)}/";
            }
            return $"/{API_BASE}/{version}/";
        }

        /// <summary>
        /// Tests the service
        /// </summary>
        /// <param name="token"></param>
        /// <param name="id"></param>
        /// <param name="httpContext"></param>
        /// <returns></returns>
        //    protected Task<IServiceResponse> Test(JToken token, Guid id, HttpContext httpContext)
        //    {
        //        return Task.FromResult(ServiceResponseFactory.Test(id, "Service Available", _appConfigProvider));
        //    }

        //    /// <summary>
        //    /// Sends back an Exception ServiceResponse based on the name of the Exception
        //    /// </summary>
        //    /// <returns>IServiceResponse</returns>
        //    protected Task<IServiceResponse> Exception(JToken token, Guid id, HttpContext httpContext)
        //    {
        //        IServiceResponse response;

        //        try
        //        {
        //            JObject json = (JObject)token;

        //            if (json.TryGetValue("name", StringComparison.OrdinalIgnoreCase, out string name))
        //            {
        //                response = ServiceResponseFactory.Error(id, name);
        //            }
        //            else
        //            {
        //                response = ServiceResponseFactory.Error(id, "Cannot get exception name");
        //                // log the exception
        //                _logger.LogError(response.Exception, response.Description);
        //            }
        //        }
        //        catch (Exception ex)
        //        {
        //            _logger.LogError(ex, "Test failed");

        //            response = ServiceResponseFactory.Error(id, ex);
        //        }

        //        return Task.FromResult(response);
        //    }

        /// <summary>
        /// Processes the request and returns a JsonResult
        /// </summary>
        /// <param name="description"></param>
        /// <param name="isAnonymous"></param>
        /// <param name="process"></param>
        /// <returns></returns>
        protected async Task<IActionResult> ProcessRequest(string description, bool isAnonymous, Func<JToken, Guid, Task<IServiceResponse>> process)
        {
            IServiceResponse response;
            Guid id = Guid.Empty;
            //LogDetailLevel logLevel = default;
            //DateTime? endDate = null;
            //int? maxcharacters = null;
            var filePath = string.Empty;

            //if (TryGetLoggingConfiguration(out ILoggingConfiguration2_0? configuration))
            //{
            //    logLevel = configuration!.DefaultLevel ?? default;
            //    maxcharacters = configuration!.MaxCharacters;
            //}

            //if (TryGetEndpointOverride(configuration!, out IEndpointOverride? methodOverride))
            //{
            //    logLevel = methodOverride!.Level;
            //    endDate = methodOverride?.EndDate;
            //}

            //bool isDetailed = logLevel == LogDetailLevel.Detailed && endDate.HasValue && endDate.Value.ToUniversalTime() > DateTime.UtcNow;

            _logger.LogInformation(description);

            try
            {
                // [Jason] not sure we need to grab the threshold for the configurator
                TimeSpan threshold = await _twoPointOSettingService.GetServerThreshold();

                if (HttpHelper.TryGetRequestToken(HttpContext.Request, threshold, isAnonymous, out response, out id, out JToken requestBody, out DtCode dtCode))
                {
                    var requestToken = DateTimeHelper.ConvertDates(requestBody, HttpType.Request, _appConfigProvider);

                    response = await process(requestToken, id);

                    //if (isDetailed)
                    //{
                    //    if (methodOverride != null && !string.IsNullOrWhiteSpace(configuration?.DetailedLogLocation))
                    //    {
                    //        var environment = _httpItemsProvider.GetEnvironment();
                    //        var customer = _httpItemsProvider.GetCustomerName();
                    //        var requestDate = _httpItemsProvider.GetRequestDate()?.ToString("yyyy-MM-dd-HH-mm-ss") ?? string.Empty;
                    //        _logger.LogInformation($"ENVIRONMENT: {environment} CUSTOMER: {customer} REQUEST_DATE_TIME: {requestDate}");

                    //        if (!string.IsNullOrWhiteSpace(environment) && !string.IsNullOrWhiteSpace(customer) && !string.IsNullOrWhiteSpace(requestDate))
                    //        {
                    //            filePath = Path.Combine(string.Format(configuration.DetailedLogLocation, customer, environment), $"{methodOverride.RequestPath.Replace(GetBaseRequestSegment(), "").Replace("\\", "-")}_{id}_{requestDate}.json");
                    //            _logger.LogInformation($"FILE_PATH: {filePath}");
                    //            // wrap the request and response in a JObject
                    //            var jobject = new JObject { { "Request", requestToken }, { "Response", JToken.FromObject(response) } };
                    //            await _loggingService.WriteJsonLog(jobject, filePath, "LogDetails");
                    //        }
                    //    }
                    //}

                    //_logger.LogTruncatedResponse(LogLevel.Information, response, logLevel, maxcharacters, endDate);
                    _logger.LogInformation(description + " - Success");
                }
                else
                {
                    // log the exception
                    //if (isDetailed)
                    //{
                    //    await _loggingService.WriteJsonLog(response, filePath, "Response");
                    //}

                    //_logger.LogTruncatedResponse(LogLevel.Error, response, logLevel, maxcharacters, endDate);
                    _logger.LogError(response.Exception, response.Description);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, description + " - Failed");

                response = ServiceResponseFactory.Error(id, ex);
            }

            return Json(response, JsonHelper.Settings);

            //    }
            //    /// <summary>
            //    /// Gets the logging configuration
            //    /// </summary>
            //    /// <param name="logConfig"></param>
            //    /// <returns></returns>
            //    private bool TryGetLoggingConfiguration(out ILoggingConfiguration2_0? logConfig)
            //    {
            //        logConfig = _appConfigProvider.GetConfiguration2_0();
            //        return logConfig != null;
            //    }
            //    /// <summary>
            //    /// Gets the endpoint override
            //    /// </summary>
            //    /// <param name="logConfiguration"></param>
            //    /// <param name="endpointOverride"></param>
            //    /// <returns></returns>
            //    private bool TryGetEndpointOverride(ILoggingConfiguration2_0 logConfiguration, out IEndpointOverride? endpointOverride)
            //    {
            //        endpointOverride = null;
            //        if (logConfiguration == null)
            //        {
            //            return false;
            //        }
            //        var endpointOverrides = logConfiguration.EndpointOverrides;

            //        if (endpointOverrides != null && endpointOverrides.Any())
            //        {
            //            var path = _httpItemsProvider.GetRequestPath();
            //            _logger.LogInformation($"REQUEST_PATH: {path}");

            //            if (!string.IsNullOrWhiteSpace(path))
            //            {
            //                path = path.Replace(GetBaseRequestSegment(), string.Empty);
            //                endpointOverride = endpointOverrides
            //                    .FirstOrDefault(x => !string.IsNullOrWhiteSpace(x.RequestPath) &&
            //                                        x.RequestPath.Equals(path, StringComparison.OrdinalIgnoreCase));
            //            }
            //        }
            //        return endpointOverride != null;
            //    }
            //}
        }
}
