using AutoMapper;
using Itinero.Exceptions;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using RoutingAssistant.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace RoutingAssistant.Infrastructure
{
    public class ErrorHandlingMiddleware
    {
        private readonly RequestDelegate next;

        public ErrorHandlingMiddleware(RequestDelegate next)
        {
            this.next = next;
        }

        public async Task InvokeAsync(HttpContext context /* other scoped dependencies */)
        {
            try
            {
                await this.next(context);
            }
            catch (ResolveFailedException ex)
            {
                await HandleExceptionAsync(context, HttpStatusCode.BadRequest, ex.Message);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        private static Task HandleExceptionAsync(HttpContext context, HttpStatusCode code, string message)
        {

            var result = JsonConvert.SerializeObject(new ExceptionRespDto { Message = message});
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)code;
            return context.Response.WriteAsync(result);
        }
    }
}
