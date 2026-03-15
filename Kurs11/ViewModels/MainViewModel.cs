using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Kurs11.Models;
using KursWa.Services;
using System;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading.Tasks;

namespace Kurs11.ViewModels
{
    public partial class MainViewModel : ObservableObject
    {
        private readonly DatabaseService _dbService;

        [ObservableProperty]
        private ObservableCollection<Supplier> suppliers;

        [ObservableProperty]
        private ObservableCollection<Customer> customers;

        [ObservableProperty]
        private ObservableCollection<Product> products;

        [ObservableProperty]
        private ObservableCollection<Sale> sales;

        [ObservableProperty]
        private ObservableCollection<Product> availableProducts;

        [ObservableProperty]
        private Supplier selectedSupplier;

        [ObservableProperty]
        private Customer selectedCustomer;

        [ObservableProperty]
        private Product selectedProduct;

        [ObservableProperty]
        private Sale selectedSale;

        [ObservableProperty]
        private string supplierName;

        [ObservableProperty]
        private string supplierAddress;

        [ObservableProperty]
        private string supplierPhone;

        [ObservableProperty]
        private string customerName;

        [ObservableProperty]
        private string customerAddress;

        [ObservableProperty]
        private string customerPhone;

        [ObservableProperty]
        private string productName;

        [ObservableProperty]
        private string productUnit;

        [ObservableProperty]
        private int productQuantity;

        [ObservableProperty]
        private decimal purchasePrice;

        [ObservableProperty]
        private decimal salePrice;

        [ObservableProperty]
        private int saleQuantity;

        public MainViewModel()
        {
            _dbService = new DatabaseService();
            Suppliers = new ObservableCollection<Supplier>();
            Customers = new ObservableCollection<Customer>();
            Products = new ObservableCollection<Product>();
            Sales = new ObservableCollection<Sale>();
            AvailableProducts = new ObservableCollection<Product>();

            LoadDataAsync();
        }

        private async Task LoadDataAsync()
        {
            await LoadSuppliersAsync();
            await LoadCustomersAsync();
            await LoadProductsAsync();
            await LoadSalesAsync();
            await LoadAvailableProductsAsync();
        }

        [RelayCommand]
        private async Task AddSupplier()
        {
            if (!string.IsNullOrWhiteSpace(SupplierName))
            {
                var supplier = new Supplier
                {
                    Name = SupplierName,
                    Address = SupplierAddress,
                    Phone = SupplierPhone
                };

                await _dbService.AddSupplierAsync(supplier);
                await LoadSuppliersAsync();
                ClearSupplierFields();
            }
        }

        [RelayCommand]
        private async Task DeleteSupplier()
        {
            if (SelectedSupplier != null)
            {
                await _dbService.DeleteSupplierAsync(SelectedSupplier);
                await LoadSuppliersAsync();
            }
        }

        [RelayCommand]
        private async Task AddCustomer()
        {
            if (!string.IsNullOrWhiteSpace(CustomerName))
            {
                var customer = new Customer
                {
                    Name = CustomerName,
                    Address = CustomerAddress,
                    Phone = CustomerPhone
                };

                await _dbService.AddCustomerAsync(customer);
                await LoadCustomersAsync();
                ClearCustomerFields();
            }
        }

        [RelayCommand]
        private async Task DeleteCustomer()
        {
            if (SelectedCustomer != null)
            {
                await _dbService.DeleteCustomerAsync(SelectedCustomer);
                await LoadCustomersAsync();
            }
        }

        [RelayCommand]
        private async Task AddProduct()
        {
            if (!string.IsNullOrWhiteSpace(ProductName) && SelectedSupplier != null)
            {
                var product = new Product
                {
                    Name = ProductName,
                    Unit = ProductUnit,
                    Quantity = ProductQuantity,
                    PurchasePrice = PurchasePrice,
                    SalePrice = SalePrice,
                    SupplierId = SelectedSupplier.Id
                };

                await _dbService.AddProductAsync(product);
                await LoadProductsAsync();
                await LoadAvailableProductsAsync();
                ClearProductFields();
            }
        }

        [RelayCommand]
        private async Task DeleteProduct()
        {
            if (SelectedProduct != null)
            {
                await _dbService.DeleteProductAsync(SelectedProduct);
                await LoadProductsAsync();
                await LoadAvailableProductsAsync();
            }
        }

        [RelayCommand]
        private async Task AddSale()
        {
            if (SelectedProduct != null && SelectedCustomer != null && SaleQuantity > 0)
            {
                var sale = new Sale
                {
                    ProductId = SelectedProduct.Id,
                    SupplierId = SelectedProduct.SupplierId,
                    CustomerId = SelectedCustomer.Id,
                    Quantity = SaleQuantity
                };

                await _dbService.AddSaleAsync(sale);
                await LoadSalesAsync();
                await LoadProductsAsync();
                await LoadAvailableProductsAsync();
                SaleQuantity = 0;
            }
        }

        private async Task LoadSuppliersAsync()
        {
            var list = await _dbService.GetSuppliersAsync();
            Suppliers.Clear();
            foreach (var item in list)
            {
                Suppliers.Add(item);
            }
        }

        private async Task LoadCustomersAsync()
        {
            var list = await _dbService.GetCustomersAsync();
            Customers.Clear();
            foreach (var item in list)
            {
                Customers.Add(item);
            }
        }

        private async Task LoadProductsAsync()
        {
            var list = await _dbService.GetProductsAsync();
            Products.Clear();
            foreach (var item in list)
            {
                Products.Add(item);
            }
        }

        private async Task LoadSalesAsync()
        {
            var list = await _dbService.GetSalesAsync();
            Sales.Clear();
            foreach (var item in list)
            {
                Sales.Add(item);
            }
        }

        private async Task LoadAvailableProductsAsync()
        {
            var list = await _dbService.GetAvailableProducts();
            AvailableProducts.Clear();
            foreach (var item in list)
            {
                AvailableProducts.Add(item);
            }
        }

        private void ClearSupplierFields()
        {
            SupplierName = string.Empty;
            SupplierAddress = string.Empty;
            SupplierPhone = string.Empty;
        }

        private void ClearCustomerFields()
        {
            CustomerName = string.Empty;
            CustomerAddress = string.Empty;
            CustomerPhone = string.Empty;
        }

        private void ClearProductFields()
        {
            ProductName = string.Empty;
            ProductUnit = string.Empty;
            ProductQuantity = 0;
            PurchasePrice = 0;
            SalePrice = 0;
        }
    }
}