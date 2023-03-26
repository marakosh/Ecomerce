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
    public class CartItemService
    {
        private readonly ShoppingContext _context;

        public CartItemService(ShoppingContext context)
        {
            _context = context;
        }

        public async Task<CartItem> CreateCartItemAsync(CartItem cartItem)
        {
            _context.CartItems.Add(cartItem);
            await _context.SaveChangesAsync();
            return cartItem;
        }

        public async Task<CartItem> GetCartItemByIdAsync(int cartItemId)
        {
            return await _context.CartItems.FindAsync(cartItemId);
        }

        public async Task<List<CartItem>> GetAllCartItemsAsync()
        {
            return await _context.CartItems.ToListAsync();
        }

        public async Task<CartItem> UpdateCartItemAsync(CartItem cartItem)
        {
            _context.Entry(cartItem).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return cartItem;
        }

        public async Task DeleteCartItemAsync(CartItem cartItem)
        {
            _context.CartItems.Remove(cartItem);
            await _context.SaveChangesAsync();
        }
    }
}
