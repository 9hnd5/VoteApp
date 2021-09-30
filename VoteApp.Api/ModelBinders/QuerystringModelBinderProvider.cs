using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ModelBinding.Binders;
using VoteApp.Application.Commons.ExtensionMethods;

namespace VoteApp.Api.ModelBinders
{
    public class QuerystringModelBinderProvider : IModelBinderProvider
    {
        public IModelBinder GetBinder(ModelBinderProviderContext context)
        {
            if (!context.Metadata.ModelType.IsGenericType) return null;

            var genericType = context.Metadata.ModelType.GetGenericTypeDefinition();
            if (genericType == typeof(FilterOperator<>)) return new BinderTypeModelBinder(typeof(QuerystringModelBinder));

            return null;
        }
    }
}
