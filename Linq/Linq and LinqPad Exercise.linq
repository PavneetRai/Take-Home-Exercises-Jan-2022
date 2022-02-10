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
//-------------------------------------------------------------------------------
//Create a Daily Sales per Store request for a specified month.
//Order stores by city by location. For Sales, show order date, number of orders, total sales without GST tax and total GST tax.
var Query3 = from x in Stores
    
    orderby x.City,x.Location
    
    select new 
    {
        City = x.City,
        Locations = x.Location,
        Sales =from y in x.Orders
                    
                where y.OrderDate >= new DateTime(2017,12,1) && y.OrderDate <= new DateTime(2017,12,30)
                group y by y.OrderDate into gTemp
                select new 
                
                {
                    date = gTemp.Key,
                    numberoforders = gTemp.Count(),
                    productsales = gTemp.Sum(st => st.SubTotal),
                    gst = gTemp.Sum(st => st.GST)
					
                }
    };
    Query3.Dump("Query 3");
	
//-------------------------------------------------------------------------------
//Print out all product items on a requested order (use Order #33). Group by Category and order by Product Description. 
//You do not need to format money as this would be done at the presentation level. Use the QtyPicked in your calculations. 
//Hint: You will need to using type casting (decimal). Use of the ternary operator will help.


var Query4 = from x in OrderLists
where x.OrderID == 33
group x by x.Product.Category.Description into categoryItems
	orderby categoryItems.Key 
	select new 
	{
		Category = categoryItems.Key,
		
		OrderProducts = from y in categoryItems											
							select new 
						{
							Product = y.Product.Description,							
							Price = y.Price,
							PickedQty = y.QtyPicked,
							Discount = y.Discount,
							Subtotal =  (decimal)y.QtyPicked * (y.Price - y.Discount),
							Tax = !y.Product.Taxable ? 0.00m : (decimal)y.QtyPicked * (y.Price - y.Discount) * 0.05m,
							ExtendedPrice = !y.Product.Taxable ? (decimal)y.QtyPicked * (y.Price - y.Discount)
									: (decimal)y.QtyPicked * (y.Price - y.Discount) * 1.05m	
							
						}
						
	};
	Query4.Dump("Query 4");
//-------------------------------------------------------------------------------
//Generate a report on store orders and sales. Group this report by store. Show the total orders, the average order size (number of items per order) and average pre-tax revenue.

var Query5 = from x in Stores
orderby x.Location
select new 
	{
  		Location = x.Location,
  		Orders = x.Orders.Count(),
 		AvgSize = (from s in x.Orders
  			select s.OrderID).Average(),
  		AvgRevenue = x.Orders.Average(st => st.SubTotal)  
	};
Query5.Dump("Query 5");
//-------------------------------------------------------------------------------
//List all the products a customer (use Customer #1) has purchased and the number of times the product was purchased. Order by number of times purchased then description.
var Query6 = from x in Customers
where x.CustomerID == 1
select new
{
	Customer = x.LastName + ", " + x.FirstName,
	OrdersCount = x.Orders.Count(),
	Items = from y in x.Orders
	from z in y.OrderLists
	group z by z.Product into gTemp
	orderby gTemp.Count() descending , gTemp.Key.Description 
	select new
	{
		Description = gTemp.Key.Description,
		timesbought = gTemp.Count() 
	}
};

Query6.Dump("Query 6");

