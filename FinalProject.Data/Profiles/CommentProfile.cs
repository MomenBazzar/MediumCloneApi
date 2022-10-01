using AutoMapper;
using FinalProject.Data.Entities;
using FinalProject.Data.Models;

namespace FinalProject.Data.Profiles;
public class CommentProfile : Profile
{
    public CommentProfile()
    {
        CreateMap<Comment, CommentDto>();
        CreateMap<CommentCreateDto, Comment>();
    }
}
