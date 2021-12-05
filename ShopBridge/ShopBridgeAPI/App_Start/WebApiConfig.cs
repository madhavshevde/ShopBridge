using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace ShopBridgeAPI
{
	public static class WebApiConfig
	{
		public static void Register(HttpConfiguration config)
		{
			config.MapHttpAttributeRoutes();
			// Web API configuration and services

			// Web API routes
			// Route for searching product
			config.Routes.MapHttpRoute(
				name: "",
				routeTemplate: "api/{controller}/product/{productName}/Page{page}",
				defaults: new
				{
					action = "Search"
				}
			);

			// Search category
			config.Routes.MapHttpRoute(
				name: "",
				routeTemplate: "api/{controller}/category/{category}/Page{page}"
			);

			// Get page without filtering by catgory
			config.Routes.MapHttpRoute(
				name: "",
				routeTemplate: "api/{controller}/Page{page}",
				defaults: new
				{
					category = (string)null,
				}
			);


			config.Routes.MapHttpRoute(
				name: "",
				routeTemplate: "api/{controller}",
				defaults: new
				{
					category = (string)null,
					page = 1
				}
			);

			// Get all categories
			config.Routes.MapHttpRoute(
				name: "",
				routeTemplate: "api/{controller}/Categories",
				defaults: new
				{
					action = "GetCategories"
				}
			);

		config.Routes.MapHttpRoute(
				name: "",
				routeTemplate: "api/{controller}/{id}",
				defaults: new { id = RouteParameter.Optional }
			);
		}
	}
}
