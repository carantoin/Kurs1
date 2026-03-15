using Kurs11.Data;
using Kurs11.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Collections.Generic;
using System.Linq;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading.Tasks;

namespace KursWa.Services
{
    public class DatabaseService
    {
        private readonly AppDbContext _context;

        public DatabaseService()
        {
            _context = new AppDbContext();
            _context.Database.EnsureCreated();
        }

        public async Task<List<Supplier>> GetSuppliersAsync() => await _context.Suppliers.ToListAsync();
        public async Task AddSupplierAsync(Supplier supplier) { _context.Suppliers.Add(supplier); await _context.SaveChangesAsync(); }
        public async Task UpdateSupplierAsync(Supplier supplier) { _context.Suppliers.Update(supplier); await _context.SaveChangesAsync(); }
        public async Task DeleteSupplierAsync(Supplier supplier) { _context.Suppliers.Remove(supplier); await _context.SaveChangesAsync(); }

        public async Task<List<Customer>> GetCustomersAsync() => await _context.Customers.ToListAsync();
        public async Task AddCustomerAsync(Customer customer) { _context.Customers.Add(customer); await _context.SaveChangesAsync(); }
        public async Task UpdateCustomerAsync(Customer customer) { _context.Customers.Update(customer); await _context.SaveChangesAsync(); }
        public async Task DeleteCustomerAsync(Customer customer) { _context.Customers.Remove(customer); await _context.SaveChangesAsync(); }

        public async Task<List<Product>> GetProductsAsync() => await _context.Products.Include(p => p.Supplier).ToListAsync();
        public async Task AddProductAsync(Product product) { _context.Products.Add(product); await _context.SaveChangesAsync(); }
        public async Task UpdateProductAsync(Product product) { _context.Products.Update(product); await _context.SaveChangesAsync(); }
        public async Task DeleteProductAsync(Product product) { _context.Products.Remove(product); await _context.SaveChangesAsync(); }

        public async Task<List<Sale>> GetSalesAsync() => await _context.Sales
            .Include(s => s.Product)
            .Include(s => s.Supplier)
            .Include(s => s.Customer)
            .ToListAsync();

        public async Task AddSaleAsync(Sale sale)
        {
            var product = await _context.Products.FindAsync(sale.ProductId);
            if (product != null && product.Quantity >= sale.Quantity)
            {
                product.Quantity -= sale.Quantity;
                sale.TotalAmount = sale.Quantity * product.SalePrice;

                _context.Sales.Add(sale);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<List<Product>> GetAvailableProducts() => await _context.Products
            .Where(p => p.Quantity > 0)
            .Include(p => p.Supplier)
            .ToListAsync();
    }
}