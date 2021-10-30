using Domain.Contracts.V1.IV1;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ERPSystemAPI.ActionFilters
{
    public class ValidationFilterAttribute : IActionFilter
    {
        public void OnActionExecuted(ActionExecutedContext context)
        {
            Controller controller = context.Controller as Controller;
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
            var param = context.ActionArguments.SingleOrDefault(p => p.Value is int);
            if(param.Value == null)
            {
                context.Result = new BadRequestObjectResult("Please provide id");
                return;
            }
            if (param.Value.Equals(0))
            {
                context.Result = new BadRequestObjectResult("Id cannot be zero");
                return;
            }
            if (!context.ModelState.IsValid)
            {
                context.Result = new BadRequestObjectResult(context.ModelState);
            }
        }
    }
}
