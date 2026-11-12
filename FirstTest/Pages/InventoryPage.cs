using System.Globalization;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace FirstTests.Pages;

public record ThongTinSanPham(string Ten, string Gia);

public class InventoryPage : BasePage
{
    private readonly By vungChuaSanPham = By.CssSelector("[data-test='inventory-container']");
    private readonly By danhSachItem = By.CssSelector("[data-test='inventory-item']");
    private readonly By tenItem = By.CssSelector("[data-test='inventory-item-name']");
    private readonly By giaItem = By.CssSelector("[data-test='inventory-item-price']");
    private readonly By hopChonSapXep = By.CssSelector("[data-test='product-sort-container']");
    private readonly By danhSachTheGia = By.CssSelector(".inventory_item_price");

    public InventoryPage(IWebDriver driver) : base(driver)
    {
    }

    /// <summary>
    /// Kiểm tra trang danh sách sản phẩm đã hiển thị thành công.
    /// </summary>
    public bool DaHienThi()
    {
        return driver.Url.Contains("inventory.html") && CoHienThi(vungChuaSanPham);
    }

    /// <summary>
    /// Chọn phương thức sắp xếp danh mục sản phẩm theo giá trị value và trả về chính InventoryPage.
    /// </summary>
    public InventoryPage SapXep(string value)
    {
        var phanTuChon = Tim(hopChonSapXep);
        var select = new SelectElement(phanTuChon);
        select.SelectByValue(value);
        return this;
    }

    /// <summary>
    /// Lấy danh sách số tiền của tất cả sản phẩm, chuẩn hóa bỏ dấu $ và chuyển thành decimal.
    /// </summary>
    public List<decimal> LayDanhSachGia()
    {
        var phanTuGia = TimTatCa(danhSachTheGia);
        return phanTuGia
            .Select(el => decimal.Parse(el.Text.Replace("$", string.Empty).Trim(), CultureInfo.InvariantCulture))
            .ToList();
    }

    /// <summary>
    /// Lấy danh sách thông tin chi tiết (tên, giá) của toàn bộ sản phẩm đang hiển thị.
    /// </summary>
    public List<ThongTinSanPham> LayDanhSachSanPham()
    {
        var cacPhanTu = TimTatCa(danhSachItem);
        var ketQua = new List<ThongTinSanPham>();

        foreach (var item in cacPhanTu)
        {
            string ten = item.FindElement(tenItem).Text;
            string gia = item.FindElement(giaItem).Text;
            ketQua.Add(new ThongTinSanPham(ten, gia));
        }

        return ketQua;
    }
}
