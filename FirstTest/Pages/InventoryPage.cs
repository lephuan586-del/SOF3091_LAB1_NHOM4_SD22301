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
    /// Kiem tra trang danh sach san pham da hien thi thanh cong hay chua.
    /// Ham dung WebDriverWait voi timeout ngan (~3 giay) va FindElements (so nhieu),
    /// duoc boc try/catch de tra ve false thay vi nem WebDriverTimeoutException.
    /// Ly do: Trong cac ca test dang nhap that bai (ca am tinh hoac ca bien),
    /// nguoi dung van o trang login, trang inventory khong bao gio xuat hien.
    /// Neu dung wait mac dinh (10s) hoac de nem exception, ca test DDT se bi cham
    /// va bao loi Error ngoai y muon thay vi tra ve ket qua false de Assert.That dung ky vong.
    /// </summary>
    public bool DaHienThi()
    {
        if (!driver.Url.Contains("inventory.html"))
        {
            return false;
        }

        try
        {
            var shortWait = new WebDriverWait(driver, TimeSpan.FromSeconds(3));
            return shortWait.Until(d =>
            {
                var elements = d.FindElements(vungChuaSanPham);
                return elements.Count > 0 && elements[0].Displayed;
            });
        }
        catch (WebDriverTimeoutException)
        {
            return false;
        }
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
