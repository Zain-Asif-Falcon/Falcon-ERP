using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace YoumeioAPI.ActionFilters
{
    public class ValidationFilterModelAttribute : IActionFilter
    {
        public void OnActionExecuted(ActionExecutedContext context)
        {
            Microsoft.AspNetCore.Mvc.Controller controller = context.Controller as Controller;

            if (controller != null)
            {
                var model = controller.ViewData.Model;
                if (model == null)
                {
                    context.Result = new BadRequestObjectResult("Could not find the object");
                    return;
                }
                else
                {
                    context.Result = new OkObjectResult(model);
                }
            }
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            if (!context.ModelState.IsValid)
            {
                context.Result = new BadRequestObjectResult(context.ModelState);
            }
        }
    }
}
