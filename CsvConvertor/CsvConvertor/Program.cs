using CsvConvertor;
using System.Text;
using System.Text.Json;

//var fileName = "products.json";
//var text = File.ReadAllText(fileName);
//IEnumerable<Product> products = JsonSerializer.Deserialize<List<Product>>(text) ;
//using StreamWriter sw = new("products.csv", false, Encoding.UTF8);
//sw.WriteLine("ProductId;Name;Price;MinStock;VendingMachineId;Description;QuantityAvailable;SalesTrend");
//foreach (var product in products)
//{
//    sw.WriteLine($"{product.id};{product.name};{product.price};{product.min_stock};{product.vending_machine_id};{product.description};{product.quantity_available};{product.sales_trend}");
//}

//var files = Directory.GetFiles("users");
//List<User> users = new();
//foreach (var file in files)
//{
//    var text = File.ReadAllText(file);
//    users.Add(JsonSerializer.Deserialize<User>(text));
//}
//using StreamWriter sw = new("users.csv", false, Encoding.UTF8);
//sw.WriteLine("Email;FullName;IsManager;IsEngineer;Phone;Id;Role;Image");
//foreach (var user in users)
//{
//    if (user.phone[0] == '8')
//    {
//        var phone = user.phone.ToCharArray();
//        phone[0] = '7';
//        user.phone = new String(phone);
//    }
//    user.phone = user.phone.Replace("+", "");
//    user.phone = user.phone.Replace(" ", "");
//    user.phone = user.phone.Replace("(", "");
//    user.phone = user.phone.Replace(")", "");
//    user.phone = user.phone.Replace("-", "");
//    sw.WriteLine($"{user.email};{user.full_name};{user.is_manager};{user.is_engineer};{user.phone};{user.id};{user.role};\"{user.image}\"");
//}

var passwords = Console.ReadLine().Split(" ");

foreach (var password in passwords)
{
    Console.WriteLine(BCrypt.Net.BCrypt.EnhancedHashPassword(password.Trim(), 11));
}
