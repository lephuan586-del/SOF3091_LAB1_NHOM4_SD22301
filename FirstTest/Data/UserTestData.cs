using System.Text.Json;

namespace FirstTests.Data;

public class UserTestData
{
    public string Username { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public bool ThanhCong { get; set; }
    public string GhiChu { get; set; } = string.Empty;

    public static List<UserTestData> DocTuFileJson(string? duongDanFile = null)
    {
        string filePath = duongDanFile ?? Path.Combine(AppContext.BaseDirectory, "Data", "users.json");

        if (!File.Exists(filePath))
        {
            throw new FileNotFoundException(
                $"Khong tim thay tap tin du lieu test tai duong dan: {filePath}. Vui long kiem tra cau hinh CopyToOutputDirectory trong file csproj.");
        }

        string jsonContent = File.ReadAllText(filePath);
        var danhSach = JsonSerializer.Deserialize<List<UserTestData>>(jsonContent, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = false
        });

        if (danhSach == null || danhSach.Count == 0)
        {
            throw new InvalidOperationException(
                $"Khong the deserialize hoac tap tin du lieu trong tai duong dan: {filePath}");
        }

        return danhSach;
    }
}
