using AutoMapper;
using GymAppC.Application.Dtos;
using GymAppC.Application.DTOs.Workouts;
using GymAppC.Application.Features.Workouts.Commands.CreateWorkout;
using GymAppC.Application.Features.Workouts.Commands.UpdateWorkout;
using GymAppC.Domain.Entities;

namespace GymAppC.Application.Mappings;

public sealed class ApplicationMappingProfile : Profile
{
    public ApplicationMappingProfile()
    {
        CreateMap<User, AuthResponseDto>()
            .ForMember(destination => destination.Token, options => options.Ignore());
        CreateMap<User, CurrentUserDto>();

        CreateMap<Workout, WorkoutDto>();
        CreateMap<CreateWorkoutCommand, Workout>()
            .ForMember(destination => destination.Id, options => options.Ignore())
            .ForMember(destination => destination.User, options => options.Ignore())
            .ForMember(destination => destination.Exercises, options => options.Ignore());
        CreateMap<UpdateWorkoutCommand, Workout>()
            .ForMember(destination => destination.Id, options => options.Ignore())
            .ForMember(destination => destination.UserId, options => options.Ignore())
            .ForMember(destination => destination.User, options => options.Ignore())
            .ForMember(destination => destination.Exercises, options => options.Ignore());
    }
}
