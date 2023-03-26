using EcomerceConsole.Data.Context;
using EcomerceConsole.Data.Model;
using Microsoft.EntityFrameworkCore;

internal class Program
{
    private static async Task Main(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<ShoppingContext>();
        optionsBuilder.UseSqlServer("Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=Ecommerce;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False");


        using (var context = new ShoppingContext(optionsBuilder.Options))
        {
            // Register a new user
            var newUser = new User { Name = "Marat", Password = "marat111" };
            await context.Users.AddAsync(newUser);
            await context.SaveChangesAsync();

            // Log in as the new user
            var loggedInUser = await context.Users.FirstOrDefaultAsync(u => u.Name == "Marat" && u.Password == "marat111");
            if (loggedInUser == null)
            {
                Console.WriteLine("Invalid username or password.");
                return;
            }

            // Add some products as an admin
            var adminUser = await context.Users.FirstOrDefaultAsync(u => u.IsAdmin);
            if (adminUser != null)
            {
                var category = new Category { Name = "Electronics" };
                await context.Categories.AddAsync(category);
                await context.SaveChangesAsync();

                var product1 = new Product { Name = "iPhone 13", Description = "The latest iPhone", Price = 999.99M, Category = category};
                var product2 = new Product { Name = "Samsung Galaxy S21", Description = "The latest Samsung phone", Price = 899.99M, Category = category};
                await context.Products.AddRangeAsync(product1, product2);
                await context.SaveChangesAsync();
            }

            // Add some items to the cart
            var cart = new ShoppingCart { UserId = loggedInUser.Id };
            var iphone13 = await context.Products.FirstOrDefaultAsync(p => p.Name == "iPhone 13");
            if (iphone13 != null)
            {
                var cartItem = new CartItem { Product = iphone13, Quantity = 2 };
                cart.Items.Add(cartItem);
            }

            // Purchase the items in the cart
            if (cart.Items.Any())
            {
                await context.ShoppingCarts.AddAsync(cart);
                await context.SaveChangesAsync();

                Console.WriteLine($"Successfully purchased {cart.Items.Count} items for a total of {cart.TotalPrice:C}.");
            }
            else
            {
                Console.WriteLine("Cart is empty.");
            }
        }
    }
}