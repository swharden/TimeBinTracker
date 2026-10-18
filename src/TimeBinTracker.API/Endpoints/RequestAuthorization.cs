using System.Diagnostics;
using Amazon.SimpleSystemsManagement;
using Amazon.SimpleSystemsManagement.Model;

namespace TimeBinTracker.API.Endpoints;

public static class RequestAuthorization
{
    const string LOCAL_DEV_PASSWORD = "<local-testing>";
    const string SSM_NAME = "/time-bin-tracker/bearer-token";

    /// <summary>
    /// Permit "<local-testing>" for a password when developing locally,
    /// otherwise get the password from an AWS parameter store.
    /// </summary>
    public static async Task<bool> IsAuthorized(
        HttpContext context,
        IAmazonSimpleSystemsManagement parameterStore)
    {
        string? authorization = context.Request.Headers.Authorization;

        string? token = authorization?
            .StartsWith("Bearer ", StringComparison.OrdinalIgnoreCase) == true
                ? authorization["Bearer ".Length..].Trim()
                : null;

        if (string.IsNullOrEmpty(token))
            return false;

        if (Debugger.IsAttached)
            return token == LOCAL_DEV_PASSWORD;

        var ssmRequest = new GetParameterRequest
        {
            Name = SSM_NAME,
            WithDecryption = true
        };

        var ssmParameter = await parameterStore.GetParameterAsync(ssmRequest);
        return token == ssmParameter.Parameter.Value;
    }
}
