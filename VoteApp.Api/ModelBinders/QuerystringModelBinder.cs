using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace VoteApp.Api.ModelBinders
{
    public class QuerystringModelBinder : IModelBinder
    {
        public Task BindModelAsync(ModelBindingContext bindingContext)
        {
            var operators = new string[] { "EQ", "NE", "GT", "GTE", "LT", "LTE", "S", "NIN", "IN" };
            var modelName = bindingContext.ModelName;
            var modelType = bindingContext.ModelType;

            var properties = new Dictionary<string, string>();
            foreach (var op in operators)
            {
                properties.Add(op, $"{modelName}[{op}]");
            }

            var instance = Activator.CreateInstance(modelType);
            foreach (var property in properties)
            {
                var value = bindingContext.ValueProvider.GetValue(property.Value).FirstValue;
                if (value == null) continue;
                var method = modelType.GetMethod("SetOperator");
                method.Invoke(instance, new[] { property.Key, value });
                bindingContext.Result = ModelBindingResult.Success(instance);
            }
            return Task.CompletedTask;
        }
    }
}
