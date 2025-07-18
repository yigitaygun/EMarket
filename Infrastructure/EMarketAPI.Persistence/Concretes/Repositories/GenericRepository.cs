﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EMarketAPI.Application.Abstractions.Repositories;
using EMarketAPI.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace EMarketAPI.Persistence.Concretes.Repositories
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {


        protected readonly AppDbContext _context;
        protected readonly DbSet<T> _dbSet;

        public GenericRepository(AppDbContext context)
        {
            _context = context;
            _dbSet = _context.Set<T>();
        }

        public async Task AddAsync(T entity)
            =>await _dbSet.AddAsync(entity);

        public void Delete(T entity)
            =>_dbSet.Remove(entity);

        public IQueryable<T> GetAll()
        {
            return _dbSet.AsQueryable();
        }

        public async Task<List<T>> GetAllAsync()
            =>await _dbSet.ToListAsync();

        public async Task<T?> GetByIdAsync(int id)
            =>await _dbSet.FindAsync(id);

        public void Update(T entity)
            =>_dbSet.Update(entity);
    }
}
