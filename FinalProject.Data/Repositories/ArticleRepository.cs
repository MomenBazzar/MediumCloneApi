﻿using FinalProject.Data.Entities;

namespace FinalProject.Data.Repositories;
public class ArticleRepository : GenericRepository<Article>, IArticleRepository
{
    public ArticleRepository(MediumDbContext context) : base(context)
    {
    }

    public async Task<Article> GetByIdAsync(int id)
    {
        return await _context.Articles.FindAsync(id);
    }

    public void Remove(Article article)
    {
        _context.Articles.Remove(article);
    }

    public void Update(Article article)
    {
        _context.Update(article);
    }
}
