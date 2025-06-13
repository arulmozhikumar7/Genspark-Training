// Expense Tracker Full Demo
using System.Net.Http.Headers;
using System.Net.Http.Json;
using Newtonsoft.Json;

class Program
{
    static HttpClient client = new HttpClient();
    static string baseUrl = "http://localhost:5169";

    static string adminToken = "";
    static string userToken = "";
    static string refreshToken = "";

    static string categoryId = "";
    static string budgetId = "";
    static string expenseId = "";
    static string receiptId = "";

    static async Task Main(string[] args)
    {
        Console.WriteLine("=== EXPENSE TRACKER FULL API DEMO ===\n");

        await RegisterAdmin();
        await AdminLogin();

        await RegisterUser();
        await UserLogin();

        await CreateCategory();
        await GetCategories();
        await UpdateCategory();

        await AddBudget();
        await GetBudgets();
        await UpdateBudget();

        await AddMultipleExpenses();
        await GetExpenses();
        await GetFilteredExpenses();
        await GetExpenseById();
        await UpdateExpense();

        await UploadReceipt();
        await GetReceiptsByExpense();


        await ExportCSV();
        await RefreshUserToken();
        await Logout();

        Console.WriteLine("\n=== DEMO COMPLETE ===");
    }

    static async Task RegisterAdmin()
    {
        var body = new { userName = "Admin", email = "admin@example.com", password = "Admin1234" };
        await Post("/api/v1/User/register-admin", body, null, "Register Admin");
    }

    static async Task AdminLogin()
    {
        adminToken = await Login("admin@example.com", "Admin1234", "Admin Login");
    }

    static async Task RegisterUser()
    {
        var body = new { userName = "User", email = "user@example.com", password = "User1234" };
        await Post("/api/v1/User/register", body, null, "Register User");
    }

    static async Task UserLogin()
    {
        userToken = await Login("user@example.com", "User1234", "User Login");
    }

    static async Task CreateCategory()
    {
        var body = new { name = "Shopping" };
        var response = await Post("/api/v1/Category", body, adminToken, "Create Category");
        dynamic json = JsonConvert.DeserializeObject(response);
        categoryId = json?.data?.id;
    }

    static async Task GetCategories()
    {
        await Get("/api/v1/Category", userToken, "Get Categories");
    }

    static async Task UpdateCategory()
    {
        var body = new { name = "Shopping Updated" };
        await Put($"/api/v1/Category/{categoryId}", body, adminToken, "Update Category");
    }

    static async Task AddBudget()
    {
        var body = new
        {
            name = "Monthly Limit",
            categoryId = categoryId,
            limitAmount = 1000,
            startDate = DateTime.UtcNow,
            endDate = DateTime.UtcNow.AddMonths(1)
        };
        await Post("/api/v1/Budget", body, userToken, "Add Budget");
    }

    static async Task GetBudgets()
    {
        var response = await GetWithResponse("/api/v1/Budget", userToken);
        Console.WriteLine("Get Budgets: OK\n" + response);
        dynamic json = JsonConvert.DeserializeObject(response);
        budgetId = json?.data?[0]?.id;
        Console.WriteLine($"Extracted Budget ID: {budgetId}");
    }

    static async Task UpdateBudget()
    {
        var body = new
        {
            id = budgetId,
            name = "Updated Budget",
            categoryId = categoryId,
            limitAmount = 1500,
            startDate = DateTime.UtcNow,
            endDate = DateTime.UtcNow.AddMonths(1)
        };
        await Put($"/api/v1/Budget/{budgetId}", body, userToken, "Update Budget");
    }
    static async Task AddMultipleExpenses()
    {
        for (int i = 1; i <= 5; i++)
        {
            var body = new
            {
                categoryId = categoryId,
                amount = 100 * i,
                description = $"Test Expense {i}",
                expenseDate = DateTime.UtcNow
            };
            await Post("/api/v1/Expense", body, userToken, $"Add Expense {i}");
        }
    }

    static async Task GetExpenses()
    {
        await Get("/api/v1/Expense", userToken, "Get Expenses");
    }

    static async Task GetFilteredExpenses()
    {
        await Get($"/api/v1/Expense/filtered?MinAmount=200&MaxAmount=400", userToken, "Filtered Expenses");
    }

    static async Task GetExpenseById()
    {
        var response = await GetWithResponse("/api/v1/Expense", userToken);
        dynamic json = JsonConvert.DeserializeObject(response);
        expenseId = json?.data?.items?[0]?.id;
        await Get($"/api/v1/Expense/{expenseId}", userToken, "Get Expense by ID");
    }

    static async Task UpdateExpense()
    {
        var body = new { amount = 555.55, description = "Updated Expense", expenseDate = DateTime.UtcNow };
        await Put($"/api/v1/Expense/{expenseId}", body, userToken, "Update Expense");
    }

    static async Task UploadReceipt()
    {
        var filePath = "receipt.png";
        if (!File.Exists(filePath)) { Console.WriteLine("Receipt file not found."); return; }

        var content = new MultipartFormDataContent();
        var fileContent = new ByteArrayContent(await File.ReadAllBytesAsync(filePath));
        fileContent.Headers.ContentType = MediaTypeHeaderValue.Parse("image/png");
        content.Add(fileContent, "file", "receipt.png");

        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", userToken);
        var response = await client.PostAsync($"{baseUrl}/api/v1/Receipt/upload/{expenseId}", content);
        var json = await response.Content.ReadAsStringAsync();
        Console.WriteLine($"Upload Receipt: {response.StatusCode}\n{json}");
    }

    static async Task GetReceiptsByExpense()
    {
        await Get($"/api/v1/Receipt/expense/{expenseId}", userToken, "Get Receipts by Expense");
    }

    static async Task ExportCSV()
    {
        var response = await client.GetAsync($"{baseUrl}/api/v1/Expense/export/csv");
        var content = await response.Content.ReadAsStringAsync();
        File.WriteAllText("expenses.csv", content);
        Console.WriteLine("CSV Exported to expenses.csv");
    }

    static async Task RefreshUserToken()
    {
        var body = new { token = userToken, refreshToken = refreshToken };
        var response = await client.PostAsJsonAsync($"{baseUrl}/api/v1/Auth/refresh", body);
        Console.WriteLine($"Refresh Token: {response.StatusCode}");
    }

    static async Task Logout()
    {
        var body = new { token = userToken, refreshToken = refreshToken };
        var response = await client.PostAsJsonAsync($"{baseUrl}/api/v1/Auth/logout", body);
        Console.WriteLine($"Logout: {response.StatusCode}");
    }

    // Shared Helpers
    static async Task<string> Login(string email, string password, string label)
    {
        var body = new { email, password };
        var response = await client.PostAsJsonAsync($"{baseUrl}/api/v1/Auth/login", body);
        var content = await response.Content.ReadAsStringAsync();
        dynamic data = JsonConvert.DeserializeObject(content);

        string token = data?.data?.token;
        refreshToken = data?.data?.refreshToken;

        Console.WriteLine($"{label}: {response.StatusCode}");
        return token;
    }

    static async Task<string> Post(string url, object body, string token, string label)
    {
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        var response = await client.PostAsJsonAsync($"{baseUrl}{url}", body);
        var content = await response.Content.ReadAsStringAsync();
        Console.WriteLine($"{label}: {response.StatusCode}");
        return content;
    }

    static async Task Get(string url, string token, string label)
    {
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        var response = await client.GetAsync($"{baseUrl}{url}");
        var content = await response.Content.ReadAsStringAsync();
        Console.WriteLine($"{label}: {response.StatusCode}\n{content}\n");
    }

    static async Task<string> GetWithResponse(string url, string token)
    {
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        var response = await client.GetAsync($"{baseUrl}{url}");
        return await response.Content.ReadAsStringAsync();
    }

    static async Task Put(string url, object body, string token, string label)
    {
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        var response = await client.PutAsJsonAsync($"{baseUrl}{url}", body);
        Console.WriteLine($"{label}: {response.StatusCode}");
    }

    static async Task Delete(string url, string token, string label)
    {
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        var response = await client.DeleteAsync($"{baseUrl}{url}");
        Console.WriteLine($"{label}: {response.StatusCode}");
    }
}
