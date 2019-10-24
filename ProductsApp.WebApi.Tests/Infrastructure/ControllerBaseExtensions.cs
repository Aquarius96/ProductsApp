using System.Linq;
using FluentAssertions.Execution;
using FluentAssertions.Primitives;

namespace Microsoft.AspNetCore.Mvc
{
    public static class ControllerBaseExtensions
    {
        public static ControllerBaseAssertion Should(this ControllerBase instance)
        {
            return new ControllerBaseAssertion(instance);
        }
    }

    public class ControllerBaseAssertion : ReferenceTypeAssertions<ControllerBase, ControllerBaseAssertion>
    {
        public ControllerBaseAssertion(ControllerBase controller)
        {
            Subject = controller;
        }

        protected override string Identifier => "Controller";

        public ControllerBaseAssertion HasError(string property,
            string message,
            string because = "",
            params object[] becauseArgs)
        {
            var state = Subject.ModelState.FirstOrDefault(m => m.Key == property);

            Execute.Assertion
                .BecauseOf(because, becauseArgs)
                .ForCondition(state.Key == property)
                .FailWith($"The controller should have error for property: {property}");

            Execute.Assertion
                .BecauseOf(because, becauseArgs)
                .ForCondition(state.Value.Errors.Any(e => e.ErrorMessage == message))
                .FailWith($"The controller should have error with message: {message}");

            return this;
        }
    }
}