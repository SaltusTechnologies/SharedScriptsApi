using System.ComponentModel;


namespace SharedScriptsApi.Enums
{

    public enum DtCode
    {
        // 0 Success Code
        [Description("Successful Request")]

        // 1 - 99 Success Code with additional information
        Success = 0,
        [Description("Successful: Service is working")]
        ServiceWorking = 1,
        [Description("Successful: The password entered is expired for user")]
        PasswordExpired = 2,

        // 100-149 Request Errors
        [Description("Insufficient information to process request")]
        InsufficientInformation = 100,
        [Description("Credentials are invalid")]
        InvalidCredentials = 101,
        [Description("The custom headers were invalid")]
        InvalidRequestHeaders = 102,
        [Description("Invalid Operation")]
        InvalidOperation = 104,
        [Description("Header is missing the authorization cookie")]
        HeaderNoCookie = 105,
        [Description("Header time is out of sync with server")]
        HeaderTimeOutOfSync = 106,
        [Description("Header is missing a client version number")]
        HeaderNoVersion = 107,

        // 150-199 Sync Data Errors
        [Description("Incident Failed to Sync")]
        IncidentSyncFailure = 150,

        // 200-299 Generic errors
        [Description("An exception was thrown while processing the request")]
        RequestException = 200,
    }
}
