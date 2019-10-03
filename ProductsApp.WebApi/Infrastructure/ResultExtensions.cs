using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace ProductsApp.Logic
{
    public static class ResultExtensions
    {
        public static void AddErrorToModelState(this Result result, ModelStateDictionary modelState)
        {
            if (result.Success)
            {
                return;
            }

            foreach(var error in result.Errors)
            {
                modelState.AddModelError(error.PropertyName, error.Message);
            }
        }
    }
}
