
1. Download code from public repository https://github.com/madhavshevde/ShopBridge.git
2. Run Products.sql script located in database folder. This script will create ShopBridge database and populate Products table.
3. Open the shopbridge solution
4. There are three projects
	ShopBridge.Domain => contains domain model
	ShopBridge.UnitTests => contains unit tests
	ShopBridgeAPI => Contains Webapi backend + front end solution developed in MVC5 and angular JS to consume
			the webapi.
		WebApi is located in Controllers\api\ProductsApiController.cs 
		
	Please edit following connection string in ShopBridgeAPI web.config.
	pls change datasource,userid and password to the database created in step I.
	
	<connectionStrings>
		<add name="EFDbContext" connectionString="Data Source=LAPTOP-L4CRCC5J;Initial Catalog=ShopBridge;User ID=sa;Password=Password@123 providerName="System.Data.SqlClient"/>
	</connectionStrings>
	
5. Run the ShopBridgeAPI project. This will automatically start the frontend solution developed in MVC5 + angular js solution that will consume WebApi.
6. Attached are screen shots that shows application in action