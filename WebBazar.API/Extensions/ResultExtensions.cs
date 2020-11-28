using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WebApp.API.Helpers;

namespace WebApp.API.Extensions
{
    public static class ResultExtensions
    {
        public static async Task<ActionResult<TData>> ToActionResult<TData>(this Task<TData> resultTask)
        {
            var result = await resultTask;

            if (result == null)
            {
                return new NotFoundResult();
            }

            return result;
        }
        
        public static async Task<ActionResult> ToActionResult(this Task<Result> resultTask)
        {
            var result = await resultTask;

            if (result.Failure)
            {
                return new BadRequestObjectResult(result.Error);
            }

            return new OkResult();
        }

        public static async Task<ActionResult<TData>> ToActionResult<TData>(this Task<Result<TData>> resultTask)
        {
            var result = await resultTask;

            if (result.Failure)
            {
                return new BadRequestObjectResult(result.Error);
            }

            return result.Data;
        }
    }
}