<Query Kind="Statements">
  <Connection>
    <ID>03ce6cdf-6790-4860-82a2-e12332704224</ID>
    <NamingServiceVersion>2</NamingServiceVersion>
    <Persist>true</Persist>
    <Server>(local)</Server>
    <AllowDateOnlyTimeOnly>true</AllowDateOnlyTimeOnly>
    <DeferDatabasePopulation>true</DeferDatabasePopulation>
    <Database>GroceryList</Database>
  </Connection>
</Query>

//Create a product list which indicates what products are purchased by our customers and how many times that product was purchased. 
//Order the list by most popular product then by alphabetic description.
var Query1 = from x in Products
						
					orderby x.OrderLists.Count descending
				 
					select new 
					{
					
						Products = x.Description,
						TimesPurchased = x.OrderLists.Count()
					};
					Query1.Dump("Query 1");
//---------------------------------------------------------------------------------		
//We want a mailing list for a Valued Customers flyer that is being sent out. List the customer addresses for customers who have shopped at each store. List by the store. 
//Include the store location as well as the customer's address. Do NOT include the customer name in the results.
var Query2 = from x in Stores

orderby x.Location
select new 
{
	Locations = x.Location,
	Client = (from y in x.Orders
				
			select new 
			
			{
				Address = y.Customer.Address,
				City = y.Customer.City,
				Province = y.Customer.Province
			}).Distinct()
};
Query2.Dump("Query 2");					