using ShopBridge.Domain.Abstract;
using ShopBridge.Domain.Entities;
using ShopBridgeAPI.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace ShopBridgeAPI.Controllers.Api
{
	public class ProductApiController : ApiController
	{
		private IProductRepository _repository;
		public int PageSize = 10;
		public ProductApiController(IProductRepository productRepository)
		{
			_repository = productRepository;
		}

		[HttpGet]
	    public IEnumerable<ProductCategoryInfo> GetCategories()
		{
			return _repository.ProductCategories;
		}

		[HttpGet]
		public ProductsListViewModel List(string category, int page)
		{
			ProductsListViewModel model = new ProductsListViewModel
			{
				Products = _repository.Products
						.Where(p => category == null || p.ProductCategoryName == category)
						.OrderBy(p => p.ProductID)
						.Skip((page - 1) * PageSize)
						.Take(PageSize)
						,
				PagingInfo = new PagingInfo
				{
					CurrentPage = page,
					ItemsPerPage = PageSize,
					TotalItems = category == null ? _repository.Products.Count() : _repository.Products.Where(p => p.ProductCategoryName == category).Count()
				},
				CurrentCategory = category,
				CurrentProductName = null
			};

			return model;
		}


		[HttpGet]
		public ProductsListViewModel Search(string productName, int page)
		{
			ProductsListViewModel model = new ProductsListViewModel
			{
				Products = _repository.Products
						.Where(p => p.ProductName.Contains(productName))
						.OrderBy(p => p.ProductID)
						.Skip((page - 1) * PageSize)
						.Take(PageSize)
						,
				PagingInfo = new PagingInfo
				{
					CurrentPage = page,
					ItemsPerPage = PageSize,
					TotalItems = _repository.Products.Where(p => p.ProductName.Contains(productName)).Count()
				},
				CurrentCategory = null,
				CurrentProductName = productName
			};

			return model;
		}

		[HttpGet]
		public Product Get(int id)
		{
			try
			{
				return _repository.GetProduct(id);
			}
			catch (Exception ex)
			{
				throw new HttpResponseException(CreateErrorResponse(ex));
			}
		}

		[HttpDelete]
		public Product Delete(int id)
		{
			try
			{
				return _repository.DeleteProduct(id);
			}
			catch (Exception ex)
			{
				throw new HttpResponseException(CreateErrorResponse(ex));
			}
		}

		[HttpPost]
		public Product Insert(Product product)
		{
			try
			{
				return _repository.SaveProduct(product);
			}
			catch (Exception ex)
			{
				throw new HttpResponseException(CreateErrorResponse(ex));
			}
		}

		[HttpPut]
		public Product Update(Product product)
		{
			try
			{
				return _repository.SaveProduct(product);
			}
			catch (Exception ex)
			{
				throw new HttpResponseException(CreateErrorResponse(ex));
			}
		}

		private HttpResponseMessage CreateErrorResponse(Exception ex)
		{
			var response = new HttpResponseMessage(HttpStatusCode.NotFound)
			{
				Content = new StringContent(ex.Message),
				ReasonPhrase = "Product ID Not Found"
			};

			return response;
		}
	}
}
