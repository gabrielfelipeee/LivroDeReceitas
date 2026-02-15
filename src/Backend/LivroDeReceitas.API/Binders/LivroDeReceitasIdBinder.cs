using Microsoft.AspNetCore.Mvc.ModelBinding;
using Sqids;

namespace LivroDeReceitas.API.Binders
{
    public class LivroDeReceitasIdBinder : IModelBinder
    {
        private readonly SqidsEncoder<long> _idEncoder;
        public LivroDeReceitasIdBinder(SqidsEncoder<long> idEncoder) => _idEncoder = idEncoder;

        public Task BindModelAsync(ModelBindingContext bindingContext)
        {
            var modelName = bindingContext.ModelName;

            var valueProviderResult = bindingContext.ValueProvider.GetValue(modelName);

            if (valueProviderResult == ValueProviderResult.None)
                return Task.CompletedTask;

            bindingContext.ModelState.SetModelValue(modelName, valueProviderResult);

            var value = valueProviderResult.FirstValue; // Id recebido no endpoint (string)
            if (string.IsNullOrEmpty(value))
                return Task.CompletedTask;

            var id = _idEncoder.Decode(value).SingleOrDefault(); // id long
            bindingContext.Result = ModelBindingResult.Success(id);

            return Task.CompletedTask;
        }
    }
}
