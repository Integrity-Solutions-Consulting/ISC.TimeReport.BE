using System.Text.Json;
using Azure;
using isc.time.report.be.domain.Exceptions;
using isc.time.report.be.domain.Models.Dto;
using isc.time.report.be.domain.Models.Response.Shared;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.IdentityModel.Tokens;
using Serilog;

namespace isc.time.report.be.api.Extentions
{
    public static class ApplicationExtentions
    {

        public static IApplicationBuilder ConfigureExcepcionHandler(this IApplicationBuilder app)
        {


            app.UseExceptionHandler(erroprApp =>
            {
                erroprApp.Run(async context =>
                {

                    context.Response.StatusCode = 500;
                    context.Response.ContentType = "application/json";

                    var exceptionHandlerPathFeature = context.Features.Get<IExceptionHandlerFeature>();

                    int _code = context.Response.StatusCode;
                    int _codeApp = 500;
                    string _message = exceptionHandlerPathFeature.Error.Message;
                    string _stackTrace = null;

                    try
                    {
                        _codeApp = ((BaseCustomException)((ExceptionHandlerFeature)exceptionHandlerPathFeature).Error).Code;
                        _message = ((BaseCustomException)((ExceptionHandlerFeature)exceptionHandlerPathFeature).Error).Message;
                        _stackTrace = ((BaseCustomException)((ExceptionHandlerFeature)exceptionHandlerPathFeature).Error).StackTrace;

                        switch (_codeApp)
                        {
                            case 500:
                                context.Response.StatusCode = 500;
                                _code = _codeApp;
                                break;
                            case 401:

                                switch (_stackTrace)
                                {
                                    case "SecurityTokenExpiredException":
                                        context.Response.Headers.Add("token-Expired", "true");
                                        break;
                                    case "ArgumentException":
                                    case "invalid_token":
                                        context.Response.Headers.Add("token-valid", "false");
                                        break;
                                }

                                context.Response.StatusCode = _codeApp;
                                _code = _codeApp;
                                break;
                            case 204:
                                context.Response.StatusCode = _codeApp;
                                _code = _codeApp;
                                break;
                            default:
                                context.Response.StatusCode = 500;
                                _code = 500;
                                break;

                        }
                        if (exceptionHandlerPathFeature.Error is ClientFaultException clientFaultException)
                        {
                            context.Response.StatusCode = 400;
                            _code = 400;
                        }

                    }
                    catch (InvalidCastException ex)
                    {
                        Log.Error("{Proceso} {errorCode} {errorMessage}", "ExceptionHandler", 0, ex.Message);
                    }





                    ErrorResponseDto _response = new ErrorResponseDto
                    {
                        Code = _code,
                        Message = _message,
                        Error = new List<ErrorData> {
                            new ErrorData
                        {
                            Code = _codeApp == 0 ? _code : _codeApp,
                            Message = _stackTrace ??= _message
                            }
                        },
                        TraceId = context?.TraceIdentifier == null ? "no-traceid" : context?.TraceIdentifier.Split(":")[0].ToLower()    
                    };




                    Log.Error("{Proceso} {errorCode} {errorMessage}", "ExceptionHandler", context.Response.StatusCode, _message);


                    string sjson = JsonSerializer.Serialize(_response);



                    await context.Response.WriteAsync(sjson);
                    await context.Response.Body.FlushAsync();

                });
            });

            return app;

        }
    }
}
