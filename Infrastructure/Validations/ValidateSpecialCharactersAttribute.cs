using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Application.Pipelines;
using System.Reflection;

namespace Infrastructure.Validations
{
    public class ValidateSpecialCharactersAttribute : ActionFilterAttribute
    {
        private static readonly Regex _invalidCharRegex = new Regex(@"^[a-zA-Z0-9 /\@.,%+!_:-]+$", RegexOptions.Compiled);

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            foreach (var argument in context.ActionArguments.Values)
            {
                if (argument == null) continue;

                var props = argument.GetType().GetProperties();
                foreach (var prop in props)
                {
                    if (prop.GetCustomAttribute<SkipGlobalValidationAttribute>() != null)
                        continue;

                    if (prop.PropertyType == typeof(string))
                    {
                        var value = prop.GetValue(argument) as string;
                        if (!string.IsNullOrEmpty(value) && !_invalidCharRegex.IsMatch(value))
                        {
                            context.Result = new BadRequestObjectResult($"Invalid character found in field '{prop.Name}'.");
                            return;
                        }
                    }
                }
            }

            base.OnActionExecuting(context);
        }
    }
}