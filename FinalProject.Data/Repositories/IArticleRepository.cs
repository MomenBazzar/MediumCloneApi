﻿using FinalProject.Data.Entities;

namespace FinalProject.Data.Repositories;
public interface IArticleRepository : IGenericRepository<Article>,IUpdatableRepository<Article>, IRemovableRepository<Article>
{
}
