using System.Linq;
using FluentAssertions;
using FluentAssertions.Execution;
using FluentAssertions.Primitives;

namespace ProductsApp.Logic
{
    public static class ResultExtensions
    {
        public static ResultAssertions<T> Should<T>(this Result<T> instance)
        {
            return new ResultAssertions<T>(instance);
        }

        public static ResultAssertions Should(this Result instance)
        {
            return new ResultAssertions(instance);
        }

        public class ResultAssertions<T> : ReferenceTypeAssertions<Result<T>, ResultAssertions<T>>
        {
            protected override string Identifier => "Result";

            public ResultAssertions(Result<T> result)
            {
                Subject = result;
            }

            public ResultAssertions<T> BeSuccess(T value, 
                string because = "",
                params object[] becauseArgs)
            {
                Execute.Assertion
                    .BecauseOf(because, becauseArgs)
                    .ForCondition(Subject != null)
                    .FailWith("The result cannot be null");

                Execute.Assertion
                    .BecauseOf(because, becauseArgs)
                    .ForCondition(Subject.Success)
                    .FailWith("The Success should be true");

                Execute.Assertion
                    .BecauseOf(because, becauseArgs)
                    .ForCondition(Subject.Value != null)
                    .FailWith("Value cannot be null");

                value.Should().BeEquivalentTo(Subject.Value);

                Execute.Assertion
                    .BecauseOf(because, becauseArgs)
                    .ForCondition(Subject.Errors == null)
                    .FailWith("The Errors should be null");

                return this;
            }

            public ResultAssertions<T> BeFailure(string property,
                string message,
                string because = "",
                params object[] becauseArgs)
            {
                Execute.Assertion
                    .BecauseOf(because, becauseArgs)
                    .ForCondition(Subject != null)
                    .FailWith("The result cannot be null");

                Execute.Assertion
                    .BecauseOf(because, becauseArgs)
                    .ForCondition(Subject.Success == false)
                    .FailWith("The Subject should be false");

                Execute.Assertion
                    .BecauseOf(because, becauseArgs)
                    .ForCondition(Subject.Value == null)
                    .FailWith("The Value should be null");

                Execute.Assertion
                    .BecauseOf(because, becauseArgs)
                    .ForCondition(Subject.Errors != null)
                    .FailWith("The Errors should have errors");

                var error = Subject.Errors.FirstOrDefault(e => e.PropertyName == property);

                Execute.Assertion
                    .BecauseOf(because, becauseArgs)
                    .ForCondition(error != null)
                    .FailWith($"The Errors should contain error for property '{property}'");

                Execute.Assertion
                    .BecauseOf(because, becauseArgs)
                    .ForCondition(error.Message == message)
                    .FailWith($"The Message for property '{property}' should be '{message}'");

                return this;
            }

            public ResultAssertions<T> BeFailure(string message, 
                string because = "", 
                params object[] becauseArgs)
            {
                BeFailure(string.Empty, message, because, becauseArgs);

                return this;
            }
        }

        public class ResultAssertions : ReferenceTypeAssertions<Result, ResultAssertions>
        {
            protected override string Identifier => "Result";

            public ResultAssertions(Result result)
            {
                Subject = result;
            }

            public ResultAssertions BeSuccess(string because = "",
                params object[] becauseArgs)
            {
                Execute.Assertion
                    .BecauseOf(because, becauseArgs)
                    .ForCondition(Subject != null)
                    .FailWith("The result can't be null");

                Execute.Assertion
                    .BecauseOf(because, becauseArgs)
                    .ForCondition(Subject.Success)
                    .FailWith("The Success should be true");

                Execute.Assertion
                    .BecauseOf(because, becauseArgs)
                    .ForCondition(Subject.Errors == null)
                    .FailWith("The Errors should be null");

                return this;
            }

            public ResultAssertions BeFailure(string because = "", 
                params object[] becauseArgs)
            {
                Execute.Assertion
                    .BecauseOf(because, becauseArgs)
                    .ForCondition(Subject != null)
                    .FailWith("The result cannot be null");

                Execute.Assertion
                    .BecauseOf(because, becauseArgs)
                    .ForCondition(Subject.Success == false)
                    .FailWith("The Subject should be false");

                Execute.Assertion
                    .BecauseOf(because, becauseArgs)
                    .ForCondition(Subject.Errors != null)
                    .FailWith("The Errors should have errors");
                return this;
            }
        }
    }
}