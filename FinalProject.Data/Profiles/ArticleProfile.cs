using AutoMapper;
using FinalProject.Data.Entities;
using FinalProject.Data.Models;

namespace FinalProject.Data.Profiles;
public class ArticleProfile : Profile
{
    public ArticleProfile()
    {
        CreateMap<Article, ArticleDto>();
        CreateMap<ArticleCreateDto, Article>();
        CreateMap<ArticleUpdateDto, Article>();
    }
}
