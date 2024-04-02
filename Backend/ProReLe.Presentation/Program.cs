using System;
using Microsoft.EntityFrameworkCore;
using ProReLe.Application.Interfaces.Queries;
using ProReLe.Application.Interfaces.Services;
using ProReLe.Application.Queries;
using ProReLe.Application.Services;
using ProReLe.Data.Persistence;
using ProReLe.Data.Persistence.OuW;
using ProReLe.Domain.Entities;
using ProReLe.Domain.Interfaces.OuW;

namespace ProReLe.Presentation
{
    public class Program
    {
        #region Constants
        private const int MENU_DELAY_MILLISECONDS = 1500;
        #endregion

        #region Dependencies
        private static IUnitOfWork _unitOfWork {get;set;} = null!;
        private static IProductQuery _productQuery {get;set;} = null!;
        private static IProductService _productService {get;set;} = null!;
        private static IClientQuery _clientQuery {get;set;} = null!;
        private static IClientService _clientService {get;set;} = null!;
        private static ISaleQuery _saleQuery {get;set;} = null!;
        private static ISaleService _saleService {get;set;} = null!;
        #endregion

        public static void Main(string[] args)
        {
            LoadDependencies();

            bool repeat;
            do
            {
                repeat = ShowMainMenu();
                ClearConsole();
            } while (repeat);

            Console.WriteLine("Application finished. Press any key to close the program!");
            Console.Read();
        }

        private static bool ShowMainMenu()
        {
            var menuText = 
                "ProReLe (Product, Registration and Sale)" + Environment.NewLine + 
                "[1] - Product." + Environment.NewLine +
                "[2] - Client." + Environment.NewLine +
                "[3] - Sale." + Environment.NewLine +
                "[4] - Exit." + Environment.NewLine;

            Console.WriteLine(menuText);

            int menuOption = ReadChoiceNumber(4);
            switch(menuOption)
            {
                case 1:
                    Console.WriteLine("[Selecting] option 'PRODUCT'!");
                    Thread.Sleep(MENU_DELAY_MILLISECONDS);
                    ClearConsole();

                    bool repeatProductMenu;
                    do
                    {
                        ClearConsole();
                        repeatProductMenu = ShowProductMenu();
                    } while(repeatProductMenu);
                    break;
                case 2:
                    Console.WriteLine("[Selecting] option 'CLIENT'!");
                    Thread.Sleep(MENU_DELAY_MILLISECONDS);
                    ClearConsole();

                    bool repeatClientMenu;
                    do
                    {
                        ClearConsole();
                        repeatClientMenu = ShowClientMenu();
                    } while(repeatClientMenu);
                    break;
                case 3:
                    Console.WriteLine("[Selecting] option 'SALE'!");
                    Thread.Sleep(MENU_DELAY_MILLISECONDS);
                    ClearConsole();

                    bool repeatSaleMenu;
                    do
                    {
                        ClearConsole();
                        repeatSaleMenu = ShowSaleMenu();
                    } while(repeatSaleMenu);
                    break;
                case 4:
                    return false;
            }

            return true;
        }

        #region Product
        private static bool ShowProductMenu()
        {
            var menuText = 
                "ProReLe (Product, Registration and Sale) > PRODUCT MENU" + Environment.NewLine + 
                "[1] - Get." + Environment.NewLine +
                "[2] - Include." + Environment.NewLine +
                "[3] - Edit." + Environment.NewLine +
                "[4] - Delete." + Environment.NewLine +
                "[5] - Return." + Environment.NewLine;

            Console.WriteLine(menuText);
            
            int menuOption = ReadChoiceNumber(5);
            switch(menuOption)
            {
                case 1:
                    Console.WriteLine("[Selecting] option 'GET PRODUCT'!");
                    Thread.Sleep(MENU_DELAY_MILLISECONDS);
                    ClearConsole();

                    ShowGetProductMenu();
                    break;
                case 2:
                    Console.WriteLine("[Selecting] option 'INCLUDE PRODUCT'!");
                    Thread.Sleep(MENU_DELAY_MILLISECONDS);
                    ClearConsole();
                    
                    ShowIncludeProductMenu();
                    break;
                case 3:
                    Console.WriteLine("[Selecting] option 'EDIT PRODUCT'!");
                    Thread.Sleep(MENU_DELAY_MILLISECONDS);
                    ClearConsole();

                    ShowEditProductMenu();
                    break;
                case 4:
                    Console.WriteLine("[Selecting] option 'DELETE PRODUCT'!");
                    Thread.Sleep(MENU_DELAY_MILLISECONDS);
                    ClearConsole();

                    ShowDeleteProductMenu();
                    break;
                case 5:
                    return false;
            }

            return true;
        }

        private static void ShowGetProductMenu()
        {
            var menuText = 
                "ProReLe (Product, Registration and Sale) > PRODUCT MENU > GET" + Environment.NewLine + 
                "[1] - Get By Description." + Environment.NewLine +
                "[2] - Get Ordered By Price." + Environment.NewLine +
                "[3] - Get Ordered By Amount." + Environment.NewLine +
                "[4] - Return." + Environment.NewLine;

            Console.WriteLine(menuText);
            int menuOption = ReadChoiceNumber(4);

            switch(menuOption)
            {
                case 1:
                    Console.Write("Enter the product description:");
                    string description = Console.ReadLine() ?? string.Empty;

                    var productsByDescription = _productQuery.GetByDescription(description);
                    WriteProductList(productsByDescription);
                    Console.Read();
                    break;
                case 2:
                    var productsByPrice = _productQuery.GetAllOrderedByPrice();
                    WriteProductList(productsByPrice);
                    Console.Read();
                    break;
                case 3:
                    var productsByAmount = _productQuery.GetAllOrderedByAmount();
                    WriteProductList(productsByAmount);
                    Console.Read();
                    break;
                case 4:
                    break;
            }
        }

        private static void ShowIncludeProductMenu()
        {
            Console.WriteLine("ProReLe (Product, Registration and Sale) > PRODUCT MENU > INCLUDE" + Environment.NewLine);

            Console.Write("Enter the product description: ");
            string descricao = Console.ReadLine() ?? string.Empty;

            Console.Write("Enter the price of the product: ");
            decimal price = ReadDecimalInput();

            Console.Write("Enter the quantity of the product in stock: ");
            int aumont = ReadIntegerInput();

            var product = new Product(descricao, price, aumont);
            var result = _productService.Include(product);
            Console.WriteLine((result.Success ? "[Success]" : "[Error]") + " - " + result.Message);
            Console.Read();
        }

        private static void ShowEditProductMenu()
        {
            Console.WriteLine("ProReLe (Product, Registration and Sale) > PRODUCT MENU > EDIT" + Environment.NewLine);

            Console.Write("Enter the product id: ");
            int id = ReadIntegerInput();

            var product = _unitOfWork.ProductRepository.GetById(id);
            if (product is null)
            {
                Console.WriteLine("Product not found!");
                Console.Read();
                return;
            }

            WriteProductList(new List<Product>() { product });

            Console.Write("Enter the new product description: ");
            product.Description = Console.ReadLine() ?? string.Empty;

            Console.Write("Enter the new price of the product: ");
            product.Price = ReadDecimalInput();

            Console.Write("Enter the new quantity of the product in stock: ");
            product.Amount = ReadIntegerInput();

            var result = _productService.Update(product);
            Console.WriteLine((result.Success ? "[Success]" : "[Error]") + " - " + result.Message);
            Console.Read();
        }

        private static void ShowDeleteProductMenu()
        {
            Console.WriteLine("ProReLe (Product, Registration and Sale) > PRODUCT MENU > DELETE" + Environment.NewLine);

            Console.Write("Enter the product id: ");
            int id = ReadIntegerInput();

            var product = _unitOfWork.ProductRepository.GetById(id);
            if (product is null)
            {
                Console.WriteLine("Product not found!");
                Console.Read();
                return;
            }

            WriteProductList(new List<Product>() { product });

            var result = _productService.Delete(product);
            Console.WriteLine((result.Success ? "[Success]" : "[Error]") + " - " + result.Message);
            Console.Read();
        }

        private static void WriteProductList(IEnumerable<Product> products)
        {
            Console.WriteLine("|     ID     |          DESCRIPTION          |     PRICE     |     AMOUNT     |");
            foreach(var product in products)
            {
                var id = product.Id.ToString();
                while(id.Length < 10)
                {
                    id += " ";
                }

                var description = product.Description;
                while (description.Length < 29)
                {
                    description += " ";
                }

                var price = product.Price.ToString();
                while (price.Length < 13)
                {
                    price += " ";
                }

                var amount = product.Amount.ToString();
                while (amount.Length < 14)
                {
                    amount += " ";
                }

                Console.WriteLine($"| {id} | {description} | {price} | {amount} |");
            }
        } 
        #endregion

        #region Client
        private static bool ShowClientMenu()
        {
            var menuText = 
                "ProReLe (Product, Registration and Sale) > CLIENT MENU" + Environment.NewLine + 
                "[1] - Get." + Environment.NewLine +
                "[2] - Include." + Environment.NewLine +
                "[3] - Edit." + Environment.NewLine +
                "[4] - Delete." + Environment.NewLine +
                "[5] - Return." + Environment.NewLine;

            Console.WriteLine(menuText);
            
            int menuOption = ReadChoiceNumber(5);
            switch(menuOption)
            {
                case 1:
                    Console.WriteLine("[Selecting] option 'GET CLIENT'!");
                    Thread.Sleep(MENU_DELAY_MILLISECONDS);
                    ClearConsole();

                    ShowGetClientMenu();
                    break;
                case 2:
                    Console.WriteLine("[Selecting] option 'INCLUDE CLIENT'!");
                    Thread.Sleep(MENU_DELAY_MILLISECONDS);
                    ClearConsole();
                    
                    ShowIncludeClientMenu();
                    break;
                case 3:
                    Console.WriteLine("[Selecting] option 'EDIT CLIENT'!");
                    Thread.Sleep(MENU_DELAY_MILLISECONDS);
                    ClearConsole();

                    ShowEditClientMenu();
                    break;
                case 4:
                    Console.WriteLine("[Selecting] option 'DELETE CLIENT'!");
                    Thread.Sleep(MENU_DELAY_MILLISECONDS);
                    ClearConsole();

                    ShowDeleteClientMenu();
                    break;
                case 5:
                    return false;
            }

            return true;
        }

        private static void ShowGetClientMenu()
        {
            var menuText = 
                "ProReLe (Product, Registration and Sale) > CLIENT MENU > GET" + Environment.NewLine + 
                "[1] - Get By CPF." + Environment.NewLine +
                "[2] - Get By Name." + Environment.NewLine +
                "[3] - Get All." + Environment.NewLine +
                "[4] - Return." + Environment.NewLine;

            Console.WriteLine(menuText);
            int menuOption = ReadChoiceNumber(4);

            switch(menuOption)
            {
                case 1:
                    Console.Write("Enter the client CPF:");
                    string cpf = Console.ReadLine() ?? string.Empty;

                    var clientByCpf = _clientQuery.GetByCpf(cpf);
                    if (clientByCpf is null)
                    {
                        Console.WriteLine("Client not found!");
                        Console.Read();
                        return;
                    }

                    WriteClientList(new List<Client>() { clientByCpf } );
                    Console.Read();
                    break;
                case 2:
                    Console.Write("Enter the client name:");
                    string name = Console.ReadLine() ?? string.Empty;

                    var clientsByName = _clientQuery.GetByName(name);
                    WriteClientList(clientsByName);
                    Console.Read();
                    break;
                case 3:
                    var clients = _clientQuery.GetAll();
                    WriteClientList(clients);
                    Console.Read();
                    break;
                case 4:
                    break;
            }
        }

        private static void ShowIncludeClientMenu()
        {
            Console.WriteLine("ProReLe (Product, Registration and Sale) > CLIENT MENU > INCLUDE" + Environment.NewLine);

            Console.Write("Enter the client name: ");
            string name = Console.ReadLine() ?? string.Empty;

            Console.Write("Enter the client CPF: ");
            string cpf = Console.ReadLine() ?? string.Empty;

            var client = new Client(name, cpf);
            var result = _clientService.Include(client);
            Console.WriteLine((result.Success ? "[Success]" : "[Error]") + " - " + result.Message);
            Console.Read();
        }

        private static void ShowEditClientMenu()
        {
            Console.WriteLine("ProReLe (Product, Registration and Sale) > CLIENT MENU > EDIT" + Environment.NewLine);

            Console.Write("Enter the client id: ");
            int id = ReadIntegerInput();

            var client = _unitOfWork.ClientRepository.GetById(id);
            if (client is null)
            {
                Console.WriteLine("Client not found!");
                Console.Read();
                return;
            }

            WriteClientList(new List<Client>() { client });

            Console.Write("Enter the new client name: ");
            client.Name = Console.ReadLine() ?? string.Empty;

            var result = _clientService.Update(client);
            Console.WriteLine((result.Success ? "[Success]" : "[Error]") + " - " + result.Message);
            Console.Read();
        }

        private static void ShowDeleteClientMenu()
        {
            Console.WriteLine("ProReLe (Product, Registration and Sale) > CLIENT MENU > DELETE" + Environment.NewLine);

            Console.Write("Enter the client id: ");
            int id = ReadIntegerInput();

            var client = _unitOfWork.ClientRepository.GetById(id);
            if (client is null)
            {
                Console.WriteLine("Client not found!");
                Console.Read();
                return;
            }

            WriteClientList(new List<Client>() { client });

            var result = _clientService.Delete(client);
            Console.WriteLine((result.Success ? "[Success]" : "[Error]") + " - " + result.Message);
            Console.Read();
        }

        private static void WriteClientList(IEnumerable<Client> clients)
        {
            Console.WriteLine("|     ID     |     CPF     |          NAME          |");
            foreach(var client in clients)
            {
                var id = client.Id.ToString();
                while(id.Length < 10)
                {
                    id += " ";
                }

                var cpf = client.Cpf;
                while (cpf.Length < 11)
                {
                    cpf += " ";
                }

                var name = client.Name;
                while (name.Length < 22)
                {
                    name += " ";
                }

                Console.WriteLine($"| {id} | {cpf} | {name} |");
            }
        }
        #endregion
        
        #region Sale
        private static bool ShowSaleMenu()
        {
            var menuText = 
                "ProReLe (Product, Registration and Sale) > SALE MENU" + Environment.NewLine + 
                "[1] - Get." + Environment.NewLine +
                "[2] - Include." + Environment.NewLine +
                "[3] - Edit." + Environment.NewLine +
                "[4] - Delete." + Environment.NewLine +
                "[5] - Return." + Environment.NewLine;

            Console.WriteLine(menuText);
            
            int menuOption = ReadChoiceNumber(5);
            switch(menuOption)
            {
                case 1:
                    Console.WriteLine("[Selecting] option 'GET SALE'!");
                    Thread.Sleep(MENU_DELAY_MILLISECONDS);
                    ClearConsole();

                    ShowGetSaleMenu();
                    break;
                case 2:
                    Console.WriteLine("[Selecting] option 'INCLUDE SALE'!");
                    Thread.Sleep(MENU_DELAY_MILLISECONDS);
                    ClearConsole();
                    
                    ShowIncludeSaleMenu();
                    break;
                case 3:
                    Console.WriteLine("[Selecting] option 'EDIT SALE'!");
                    Thread.Sleep(MENU_DELAY_MILLISECONDS);
                    ClearConsole();

                    ShowEditSaleMenu();
                    break;
                case 4:
                    Console.WriteLine("[Selecting] option 'DELETE SALE'!");
                    Thread.Sleep(MENU_DELAY_MILLISECONDS);
                    ClearConsole();

                    ShowDeleteSaleMenu();
                    break;
                case 5:
                    return false;
            }

            return true;
        }

        private static void ShowGetSaleMenu()
        {
            var menuText = 
                "ProReLe (Product, Registration and Sale) > PRODUCT MENU > GET" + Environment.NewLine + 
                "[1] - Get By Product." + Environment.NewLine +
                "[2] - Get By Client." + Environment.NewLine +
                "[3] - Get By Date." + Environment.NewLine +
                "[4] - Get All." + Environment.NewLine +
                "[5] - Return." + Environment.NewLine;

            Console.WriteLine(menuText);
            int menuOption = ReadChoiceNumber(5);

            switch(menuOption)
            {
                case 1:
                    Console.Write("Enter the product id:");
                    int productId = ReadIntegerInput();

                    var salesByProduct = _saleQuery.GetByProduct(productId);
                    WriteSaleList(salesByProduct);
                    Console.Read();
                    break;
                case 2:
                    Console.Write("Enter the client id:");
                    int clientId = ReadIntegerInput();

                    var salesByClient = _saleQuery.GetByClient(clientId);
                    WriteSaleList(salesByClient);
                    Console.Read();
                    break;
                case 3:
                    Console.Write("Enter the date in the format day/month/year:");
                    DateTimeOffset date = ReadDateTimeInput();

                    var salesByDate = _saleQuery.GetByDate(date);
                    WriteSaleList(salesByDate);
                    Console.Read();
                    break;
                case 4:
                    var sales = _saleQuery.GetAll();
                    WriteSaleList(sales);
                    Console.Read();
                    break;
                case 5:
                    break;
            }
        }

        private static void ShowIncludeSaleMenu()
        {
            Console.WriteLine("ProReLe (Product, Registration and Sale) > SALE MENU > INCLUDE" + Environment.NewLine);

            Console.Write("Enter the product id: ");
            int productId = ReadIntegerInput();

            Console.Write("Enter the client id: ");
            int clientId = ReadIntegerInput();

            Console.Write("Enter the quantity of the product sold: ");
            int aumont = ReadIntegerInput();

            Console.Write("Enter the discount: ");
            decimal discount = ReadDecimalInput();

            var sale = new Sale(productId, clientId, aumont, discount);
            var result = _saleService.Include(sale);
            Console.WriteLine((result.Success ? "[Success]" : "[Error]") + " - " + result.Message);
            Console.Read();
        }

        private static void ShowEditSaleMenu()
        {
            Console.WriteLine("ProReLe (Product, Registration and Sale) > SALE MENU > EDIT" + Environment.NewLine);

            Console.Write("Enter the sale id: ");
            int id = ReadIntegerInput();

            var sale = _unitOfWork.SaleRepository.GetById(id);
            if (sale is null)
            {
                Console.WriteLine("Sale not found!");
                Console.Read();
                return;
            }

            WriteSaleList(new List<Sale>() { sale });

            Console.Write("Enter the new discount: ");
            sale.Discount = ReadDecimalInput();

            var result = _saleService.Update(sale);
            Console.WriteLine((result.Success ? "[Success]" : "[Error]") + " - " + result.Message);
            Console.Read();
        }

        private static void ShowDeleteSaleMenu()
        {
            Console.WriteLine("ProReLe (Product, Registration and Sale) > SALE MENU > DELETE" + Environment.NewLine);

            Console.Write("Enter the sale id: ");
            int id = ReadIntegerInput();

            var sale = _unitOfWork.SaleRepository.GetById(id);
            if (sale is null)
            {
                Console.WriteLine("Sale not found!");
                Console.Read();
                return;
            }

            WriteSaleList(new List<Sale>() { sale });

            var result = _saleService.Delete(sale);
            Console.WriteLine((result.Success ? "[Success]" : "[Error]") + " - " + result.Message);
            Console.Read();
        }

        private static void WriteSaleList(IEnumerable<Sale> sales)
        {
            Console.WriteLine("|     ID     |          PRODUCT          |          CLIENT          |     AMOUNT     |     INITIAL PRICE     |     DISCOUNT     |     FINAL PRICE     |       DATE       |");
            foreach(var sale in sales)
            {                
                var id = sale.Id.ToString();
                while(id.Length < 10)
                {
                    id += " ";
                }

                string product = sale.Product?.Description ?? string.Empty;
                while (product.Length < 25)
                {
                    product += " ";
                }

                string client = sale.Client?.Name ?? string.Empty;
                while (client.Length < 24)
                {
                    client += " ";
                }

                var amount = sale.Amount.ToString();
                while (amount.Length < 14)
                {
                    amount += " ";
                }

                var initialPrice = sale.InitialPrice.ToString();
                while (initialPrice.Length < 21)
                {
                    initialPrice += " ";
                }

                var discount = sale.Discount.ToString();
                while (discount.Length < 16)
                {
                    discount += " ";
                }

                var finalPrice = sale.FinalPrice.ToString();
                while (finalPrice.Length < 19)
                {
                    finalPrice += " ";
                }

                var date = sale.Date.ToString("dd/MM/yyyy HH:mm");
                while (date.Length < 12)
                {
                    date += " ";
                }               

                Console.WriteLine($"| {id} | {product} | {client} | {amount} | {initialPrice} | {discount} | {finalPrice} | {date} |");
            }
        } 
        #endregion
        
        #region Helper Methods
        private static void LoadDependencies()
        {
            _unitOfWork = new UnitOfWork(new ProReLeContext(new DbContextOptions<ProReLeContext>()));   
            _productQuery = new ProductQuery(_unitOfWork);
            _productService = new ProductService(_unitOfWork);
            _clientQuery = new ClientQuery(_unitOfWork);
            _clientService = new ClientService(_unitOfWork);
            _saleQuery = new SaleQuery(_unitOfWork);
            _saleService = new SaleService(_unitOfWork);
        }
        
        private static void ClearConsole()
        {
            try
            {
                Console.Clear();
            }
            catch (IOException)
            {
                // Not block the flow!
            }
        }

        private static int ReadChoiceNumber(int maxOption)
        {
            bool success;
            int choiceResult;
            do
            {
                Console.Write($"Enter the number corresponding to your choice [between 1 and {maxOption}]: ");
                success = Int32.TryParse(Console.ReadLine(), out choiceResult) && choiceResult >= 1 && choiceResult <= maxOption;
            } while (!success);
            
            return choiceResult;
        }

         private static int ReadIntegerInput()
        {
            bool success;
            int inputResult;
            do
            {
                success = int.TryParse(Console.ReadLine(), out inputResult);
                if (!success)
                {
                    Console.Write("Error, enter the value again: ");
                }
            } while (!success);

            return inputResult;
        }

        private static decimal ReadDecimalInput()
        {
            bool success;
            decimal inputResult;
            do
            {
                success = Decimal.TryParse(Console.ReadLine(), out inputResult);
                if (!success)
                {
                    Console.Write("Error, enter the value again: ");
                }
            } while (!success);

            return inputResult;
        }
        
        private static DateTime ReadDateTimeInput()
        {
            int day = 0, month = 0, year = 0;
            DateTime? inputResult = null;

            bool success;
            do
            {
                try
                {
                    var dateInputed = Console.ReadLine() ?? string.Empty;
                    
                    var dateSplit = dateInputed.Split("/");
                    day = int.Parse(dateSplit[0]);
                    month = int.Parse(dateSplit[1]);
                    year = int.Parse(dateSplit[2]);

                    inputResult = new DateTime(year, month, day);
                    success = true;
                }
                catch(Exception)
                {
                    success = false;
                    Console.Write("Error! Enter again: ");
                }
            } while (!success);
            
            return inputResult!.Value;
        }
        #endregion
    }
}