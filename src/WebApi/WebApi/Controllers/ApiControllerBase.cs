using Common.Services;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Runtime.CompilerServices;

namespace WebApi.Controllers
{
    public abstract class ApiControllerBase : ControllerBase
    {
        protected async Task<IActionResult> ExecuteAsync(
            Func<Task<IActionResult>> asyncAction)
        {
            IActionResult actionResult;

            try
            {
                actionResult = await asyncAction();
            }
            catch (Exception ex)
            {
                if (ex is DataAccessException dataExc)
                {
                    actionResult = StatusCode(
                        (int)dataExc.StatusCode);
                }
                else
                {
                    actionResult = StatusCode(
                        (int)HttpStatusCode.InternalServerError);
                }
            }

            return actionResult;
        }

        protected Task<IActionResult> ExecuteAsync<T>(
            Func<Task<T>> asyncAction,
            Func<T, IActionResult> actionResultFactory = null) => ExecuteAsync(async () =>
            {
                var value = await asyncAction();

                if (actionResultFactory == null)
                {
                    actionResultFactory = value => Ok(value);
                }

                var result = actionResultFactory(value);
                return result;
            });
    }
}
