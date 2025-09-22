using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Collections;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Model;
using Application.Pipelines;
using System.Reflection.Metadata.Ecma335;

namespace Infrastructure.Validations
{

    //public class AddressModel
    //{
    //    public string Street { get; set; }
    //    [SkipGlobalValidation]
    //    public string Landmark { get; set; } // Will be skipped
    //}

    /*
    public class SkipValidationFilter : IAsyncActionFilter
    {
        public async Task OnActionExecutionAsync(ActionExecutingContext ctx, ActionExecutionDelegate next)
        {
            var hasAttribute = ctx.ActionDescriptor
                .EndpointMetadata
                .OfType<SkipGlobalValidationAttribute>()
                .Any();
            if (!hasAttribute)
            {
                // Check the parameter types
                foreach (var arg in ctx.ActionArguments.Values)
                {
                    if (arg?.GetType()
                          .GetCustomAttribute<SkipGlobalValidationAttribute>() != null)
                    {
                        hasAttribute = true;
                        break;
                    }
                }
            }
            if (!hasAttribute && !ctx.ModelState.IsValid)
            {
                ctx.Result = new BadRequestObjectResult(ctx.ModelState);
                return;
            }
            await next();
        }
    }
    */

    


    public class EnforceGlobalStringLengthAttribute : ActionFilterAttribute
    {
        private const int DefaultMinLength = 1;
        private const int DefaultMaxLength = 50;

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            foreach (var argument in context.ActionArguments.Values)
            {
                Type type = argument.GetType();

                if (type.FullName == "System.String" || type.FullName == "System.Guid" || type.FullName == "System.Integer")
                {
                    return;
                }
                if (argument != null)
                {
                    var error = ValidateObject(argument);
                    if (error != null)
                    {
                        context.Result = new BadRequestObjectResult(error);
                        return;
                    }
                }
            }

            base.OnActionExecuting(context);
        }

        private string? ValidateObject(object obj)
        {
            if (obj.GetType().Name == "List`1")
                return null;
            
            var props = obj.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);

            foreach (var prop in props)
            {
                if (prop.GetCustomAttribute<SkipGlobalValidationAttribute>() != null)
                    continue;

                var value = prop.GetValue(obj);

                if (value == null)
                    continue;

                if (prop.PropertyType == typeof(string))
                {
                    var strValue = value as string;
                    if (strValue != null && (strValue.Length < DefaultMinLength || strValue.Length > DefaultMaxLength))
                    {
                        return $"The field '{prop.Name}' must be between {DefaultMinLength} and {DefaultMaxLength} characters.";
                    }
                }
                else if (!prop.PropertyType.IsPrimitive && !prop.PropertyType.IsEnum && prop.PropertyType != typeof(DateTime))
                {
                    // Recursively validate nested objects
                    var error = ValidateObject(value);
                    if (error != null)
                        return error;
                }
            }

            return null;
        }
    }

    

    /// Allow per-property custom length ranges (e.g., min=3, max=50).
    /// Collect all validation errors instead of stopping at the first one

    /*
    [AttributeUsage(AttributeTargets.Property)]
    public class GlobalStringLengthAttribute : Attribute
    {
        public int Min { get; set; } = 1;
        public int Max { get; set; } = 100;

        public GlobalStringLengthAttribute(int min = 1, int max = 100)
        {
            Min = min;
            Max = max;
        }
    }

    public class EnforceGlobalStringLengthAttribute1 : ActionFilterAttribute
    {
        private const int DefaultMinLength = 1;
        private const int DefaultMaxLength = 100;

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var errors = new List<string>();

            foreach (var argument in context.ActionArguments.Values)
            {
                if (argument != null)
                {
                    ValidateObject(argument, errors);
                }
            }

            if (errors.Any())
            {
                context.Result = new BadRequestObjectResult(new { Errors = errors });
                return;
            }

            base.OnActionExecuting(context);
        }

        private void ValidateObject(object obj, List<string> errors, string? parent = null)
        {
            var props = obj.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);

            foreach (var prop in props)
            {
                if (prop.GetCustomAttribute<SkipGlobalValidationAttribute>() != null)
                    continue;

                var value = prop.GetValue(obj);
                if (value == null)
                    continue;

                string fullName = parent != null ? $"{parent}.{prop.Name}" : prop.Name;

                if (prop.PropertyType == typeof(string))
                {
                    var str = value as string;
                    var attr = prop.GetCustomAttribute<GlobalStringLengthAttribute>();
                    int min = attr?.Min ?? DefaultMinLength;
                    int max = attr?.Max ?? DefaultMaxLength;

                    if (str!.Length < min || str.Length > max)
                    {
                        errors.Add($"The field '{fullName}' must be between {min} and {max} characters.");
                    }
                }
                else if (IsComplexType(prop.PropertyType))
                {
                    ValidateObject(value, errors, fullName);
                }
                else if (value is IEnumerable enumerable && !(value is string))
                {
                    foreach (var item in enumerable)
                    {
                        if (item != null && IsComplexType(item.GetType()))
                        {
                            ValidateObject(item, errors, fullName);
                        }
                    }
                }
            }
        }

        private bool IsComplexType(Type type)
        {
            return type.IsClass && type != typeof(string) && !type.IsPrimitive && type != typeof(DateTime);
        }
    }

    public class AddressModel
    {
        [GlobalStringLength(5, 50)]
        public string Street { get; set; }

        [SkipGlobalValidation]
        public string Landmark { get; set; }
    }
    */
}
