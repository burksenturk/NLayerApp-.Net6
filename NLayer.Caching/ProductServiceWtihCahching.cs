﻿using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using NLayer.Core.DTOs;
using NLayer.Core.Models;
using NLayer.Core.Repositories;
using NLayer.Core.Services;
using NLayer.Core.UnitOfWorks;
using NLayer.Service.Exceptions;
using System.Linq.Expressions;

namespace NLayer.Caching
{
    public class ProductServiceWtihCahching : IProductService
    {
        //önce tüm productları tutacagım bir key tutuyorum. inmemory cashde key-value şeklinde cashlerimizi tutabiliyoruz.
        private const string CacheProductKey = "productsCache";
        private readonly IProductRepository _repository;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMemoryCache _memoryCache; //InmemoryCache için IMemoryCache e ihtiyacımız var.

        public ProductServiceWtihCahching(IProductRepository repository, IMapper mapper, IUnitOfWork unitOfWork, IMemoryCache memoryCache)
        {
            _repository = repository;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _memoryCache = memoryCache;

            //uygulama ilk ayağa kalktığında Cache yooksa oluşturcak.
            if (!_memoryCache.TryGetValue(CacheProductKey, out _))  //boolean döner
            {
                _memoryCache.Set(CacheProductKey, _repository.GetProductWithCategory().Result); //ctor içerisinde async kullanamam Result alıp sync yaptım
            }

        }

        public async Task<Product> AddAsync(Product entity)
        {
            await _repository.AddAsync(entity);
            await _unitOfWork.CommitAsync();
            await CachAllProductsAsync();
            return entity;
        }

        public async Task<IEnumerable<Product>> AddRangeAsync(IEnumerable<Product> entities)
        {
            await _repository.AddRangeAsync(entities);
            await _unitOfWork.CommitAsync();
            await CachAllProductsAsync();
            return entities;
        }

        public Task<bool> AnyAsync(Expression<Func<Product, bool>> expression)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Product>> GetAllAsync()
        {
            return Task.FromResult(_memoryCache.Get<IEnumerable<Product>>(CacheProductKey));
        }

        public Task<Product> GetByIdAsync(int id)
        {
            var product = _memoryCache.Get<List<Product>>(CacheProductKey).FirstOrDefault(x => x.Id == id);

            if (product == null)
            {
                throw new NotFoundException($"{typeof(Product).Name}({id}) not found");
            }

            return Task.FromResult(product); // geri dönüş tipi Task ve async kullanmadgımız için FromResult kullanıyoruz.
        }

        public Task<CustomResponseDto<List<ProductWithCategoryDto>>> GetProductWithCategory()
        {
            var products = _memoryCache.Get<IEnumerable<Product>>(CacheProductKey);

            var productsWithCategoryDto = _mapper.Map<List<ProductWithCategoryDto>>(products);

            return Task.FromResult(CustomResponseDto<List<ProductWithCategoryDto>>.Success(200, productsWithCategoryDto));
        }

        public async Task RemoveAsync(Product entity)
        {
            _repository.Remove(entity);
            await _unitOfWork.CommitAsync();
            await CachAllProductsAsync();
        }

        public async Task RemoveRangeAsync(IEnumerable<Product> entities)
        {
            _repository.RemoveRange(entities);
            await _unitOfWork.CommitAsync();
            await CachAllProductsAsync();
        }

        public async Task UpdateAsync(Product entity)
        {
            _repository.Update(entity);
            await _unitOfWork.CommitAsync();
            await CachAllProductsAsync();
        }

        public IQueryable<Product> Where(Expression<Func<Product, bool>> expression)
        {
            return _memoryCache.Get<List<Product>>(CacheProductKey).Where(expression.Compile()).AsQueryable();
        }


        public async Task CachAllProductsAsync() //bu metot her cagırdıgımda sıfırdan datayı çekip Cachliyor
        {
            _memoryCache.Set(CacheProductKey, await _repository.GetAll().ToListAsync());
        }
    }
}
