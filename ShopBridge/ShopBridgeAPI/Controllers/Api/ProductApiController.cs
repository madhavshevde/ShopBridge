using ShopBridge.Domain.Abstract;
using ShopBridge.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace ShopBridgeAPI.Controllers.Api
{
    public class ProductApiController : ApiController
    {
		private IProductRepository _repository;
		public ProductApiController(IProductRepository productRepository)
		{
			_repository = productRepository;
		}

		[HttpGet]
		public IEnumerable<Product> List()
		{
			IEnumerable<Product> products = _repository.Products.ToList();

			return products;
		}

		[HttpGet]
		public Product Get(int id)
		{
			try
			{
				return _repository.GetProduct(id);
			}
			catch(Exception ex)
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
			catch(Exception ex)
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
			catch(Exception ex)
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
