using Microsoft.AspNetCore.Authorization;
using ProductManagement.Categories;
using ProductManagement.Permissions;
using ProductManagement.Products;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Domain.Repositories;

namespace ProductManagement
{
    [Authorize(ProductManagementPermissions.Products.Default)]
    public class ProductAppService :
    ProductManagementAppService, IProductAppService
    {
        private readonly IRepository<Product, Guid>
       _productRepository;
        private readonly IRepository<Category, Guid>
       _categoryRepository;
        public ProductAppService(
        IRepository<Product, Guid> productRepository,IRepository<Category,Guid> categoryRepositary)
        {
            _productRepository = productRepository;
            _categoryRepository = categoryRepositary;
        }

      public async Task<PagedResultDto<ProductDto>> GetListAsync(PagedAndSortedResultRequestDto input)
        {
            var queryable = await _productRepository.WithDetailsAsync(x => x.Category);
            queryable = queryable.Skip(input.SkipCount).Take(input.MaxResultCount).OrderBy((Product p) => input.Sorting ?? p.Name);
            var products = await AsyncExecuter.ToListAsync(queryable);
            var count = await _productRepository.GetCountAsync();
            return new PagedResultDto<ProductDto>(
            count,
            ObjectMapper.Map<List<Product>, List<ProductDto>>
           (products)
            );
        }
        [Authorize(ProductManagementPermissions.Products.Create)]
        public async Task CreateAsync(CreateUpdateProductDto input)
        {
            await _productRepository.InsertAsync(
            ObjectMapper.Map<CreateUpdateProductDto, Product>
           (input)
            );
        }
        public async Task<ListResultDto<CategoryLookupDto>>GetCategoriesAsync()
        {
            var categories = await _categoryRepository.GetListAsync();
            return new ListResultDto<CategoryLookupDto>(
            ObjectMapper
            .Map<List<Category>, List<CategoryLookupDto>>
           (categories)
            );
        }
        public async Task<ProductDto> GetAsync(Guid id)
        {
            return ObjectMapper.Map<Product, ProductDto>(
            await _productRepository.GetAsync(id)
            );
        }
        [Authorize(ProductManagementPermissions.Products.Update)]
        public async Task UpdateAsync(Guid id, CreateUpdateProductDto
        input)
        {
            var product = await _productRepository.GetAsync(id);
            ObjectMapper.Map(input, product);
        }
        [Authorize("ProductManagement.Products.Delete")]
        public async Task DeleteAsync(Guid id)
        {
            await _productRepository.DeleteAsync(id);
        }
    }
}
