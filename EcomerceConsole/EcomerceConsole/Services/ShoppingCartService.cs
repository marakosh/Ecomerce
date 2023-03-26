using EcomerceConsole.Data.Context;
using EcomerceConsole.Data.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EcomerceConsole.Services
{
    public class ShoppingCartService
    {
        private readonly ShoppingContext _context;

        public ShoppingCartService(ShoppingContext context)
        {
            _context = context;
        }

        public async Task<ShoppingCart> CreateShoppingCartAsync(ShoppingCart shoppingCart)
        {
            _context.ShoppingCarts.Add(shoppingCart);
            await _context.SaveChangesAsync();
            return shoppingCart;
        }

        public async Task<ShoppingCart> GetShoppingCartByIdAsync(int shoppingCartId)
        {
            return await _context.ShoppingCarts
                .Include(sc => sc.Items)
                .ThenInclude(ci => ci.Product)
                .FirstOrDefaultAsync(sc => sc.Id == shoppingCartId);
        }

        public async Task<List<ShoppingCart>> GetAllShoppingCartsAsync()
        {
            return await _context.ShoppingCarts.ToListAsync();
        }

        public async Task<ShoppingCart> UpdateShoppingCartAsync(ShoppingCart shoppingCart)
        {
            _context.Entry(shoppingCart).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return shoppingCart;
        }

        public async Task DeleteShoppingCartAsync(ShoppingCart shoppingCart)
        {
            _context.ShoppingCarts.Remove(shoppingCart);
            await _context.SaveChangesAsync();
        }

        public async Task<CartItem> AddItemToCartAsync(int shoppingCartId, int productId, int quantity)
        {
            var shoppingCart = await GetShoppingCartByIdAsync(shoppingCartId);

            if (shoppingCart == null)
            {
                throw new Exception($"Shopping cart with ID {shoppingCartId} not found.");
            }

            var product = await _context.Products.FindAsync(productId);

            if (product == null)
            {
                throw new Exception($"Product with ID {productId} not found.");
            }

            var existingCartItem = shoppingCart.Items.FirstOrDefault(ci => ci.ProductId == productId);

            if (existingCartItem != null)
            {
                existingCartItem.Quantity += quantity;
                await UpdateCartItemAsync(existingCartItem);
                return existingCartItem;
            }
            else
            {
                var cartItem = new CartItem
                {
                    ProductId = productId,
                    Quantity = quantity,
                    ShoppingCartId = shoppingCartId
                };

                _context.CartItems.Add(cartItem);
                await _context.SaveChangesAsync();
                shoppingCart.Items.Add(cartItem);
                return cartItem;
            }
        }

        public async Task RemoveItemFromCartAsync(int cartItemId)
        {
            var cartItem = await GetShoppingCartByIdAsync(cartItemId);

            if (cartItem != null)
            {
                _context.CartItems.Remove(cartItem);
                await _context.SaveChangesAsync();
            }
        }

        public async Task UpdateCartItemAsync(CartItem cartItem)
        {
            _context.Entry(cartItem).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }
    }
}
