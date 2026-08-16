using FluentValidation;
using GymAppC.Application.Common.Behaviors;
using MediatR;

namespace GymAppC.Application.Tests.Common;

public sealed class ValidationBehaviorTests
{
    [Fact]
    public async Task Handle_WhenRequestIsInvalid_ThrowsAndDoesNotCallNext()
    {
        var nextWasCalled = false;
        var behavior = new ValidationBehavior<PipelineRequest, string>(
            [new PipelineRequestValidator()]);
        RequestHandlerDelegate<string> next = () =>
        {
            nextWasCalled = true;
            return Task.FromResult("handled");
        };

        var exception = await Assert.ThrowsAsync<ValidationException>(() =>
            behavior.Handle(new PipelineRequest(0), next, CancellationToken.None));

        Assert.False(nextWasCalled);
        Assert.Contains(
            exception.Errors,
            error => error.PropertyName == nameof(PipelineRequest.Value));
    }

    [Fact]
    public async Task Handle_WhenRequestIsValid_CallsNextAndReturnsItsResponse()
    {
        var nextCalls = 0;
        var behavior = new ValidationBehavior<PipelineRequest, string>(
            [new PipelineRequestValidator()]);
        RequestHandlerDelegate<string> next = () =>
        {
            nextCalls++;
            return Task.FromResult("handled");
        };

        var result = await behavior.Handle(
            new PipelineRequest(1),
            next,
            CancellationToken.None);

        Assert.Equal("handled", result);
        Assert.Equal(1, nextCalls);
    }

    private sealed record PipelineRequest(int Value);

    private sealed class PipelineRequestValidator : AbstractValidator<PipelineRequest>
    {
        public PipelineRequestValidator()
        {
            RuleFor(request => request.Value).GreaterThan(0);
        }
    }
}
