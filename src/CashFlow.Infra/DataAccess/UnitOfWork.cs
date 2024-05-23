﻿using CashFlow.Domain.Repositories;

namespace CashFlow.Infra.DataAccess;
internal class UnitOfWork : IUnitOfWork
{
    private readonly CashFlowDbContext _dbContext;

    public UnitOfWork(CashFlowDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    /*public void Commit()
    {
        _dbContext.SaveChanges();
    }*/
    public async Task Commit() => await _dbContext.SaveChangesAsync();
}
