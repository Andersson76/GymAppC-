using GymAppC.Application.Features.Auth.Commands.Register;
using GymAppC.Domain.Entities;

namespace GymAppC.Application.Tests.Architecture;

public sealed class LayerDependencyTests
{
    [Fact]
    public void Domain_DoesNotReferenceOuterLayers()
    {
        var referencedProjects = GetGymAppReferences(typeof(User).Assembly);

        Assert.DoesNotContain("GymAppC.Application", referencedProjects);
        Assert.DoesNotContain("GymAppC.Infrastructure", referencedProjects);
        Assert.DoesNotContain("GymAppC.Api", referencedProjects);
    }

    [Fact]
    public void Application_DoesNotReferenceInfrastructureOrApi()
    {
        var referencedProjects = GetGymAppReferences(typeof(RegisterUserCommand).Assembly);

        Assert.DoesNotContain("GymAppC.Infrastructure", referencedProjects);
        Assert.DoesNotContain("GymAppC.Api", referencedProjects);
    }

    private static string[] GetGymAppReferences(System.Reflection.Assembly assembly)
    {
        return assembly.GetReferencedAssemblies()
            .Select(reference => reference.Name)
            .Where(name => name is not null && name.StartsWith("GymAppC", StringComparison.Ordinal))
            .Cast<string>()
            .ToArray();
    }
}
