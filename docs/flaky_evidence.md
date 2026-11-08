# Bằng chứng kiểm tra độ ổn định và Flaky Test (Lab 3)

## 1. Code nguyên văn của Test A1 và Test A2

```csharp
[Test]
[Category("Demo")]
public void PartA_KhongWait_Rot()
{
    // 1. Điều hướng đến trang Dynamic Loading 2
    driver.Navigate().GoToUrl("https://the-internet.herokuapp.com/dynamic_loading/2");

    // 2. Bấm nút Start để kích hoạt tải dữ liệu bằng JavaScript
    driver.FindElement(By.CssSelector("#start button")).Click();

    // 3. KHÔNG CHỜ: Tìm ngay lập tức phần tử #finish khi DOM chưa kịp render -> Gây lỗi NoSuchElementException
    var finishElement = driver.FindElement(By.Id("finish"));

    // 4. Kiểm tra nội dung văn bản
    Assert.That(finishElement.Text, Does.Contain("Hello World!"));
}

[Test]
public void PartA_CoWait_Pass()
{
    // 1. Điều hướng đến trang Dynamic Loading 2
    driver.Navigate().GoToUrl("https://the-internet.herokuapp.com/dynamic_loading/2");

    // 2. Bấm nút Start để kích hoạt tải dữ liệu bằng JavaScript
    driver.FindElement(By.CssSelector("#start button")).Click();

    // 3. CÓ CHỜ: Dùng WebDriverWait poll liên tục đến khi #finish xuất hiện trong DOM (timeout 10 giây)
    var finishElement = wait.Until(d => d.FindElement(By.Id("finish")));

    // 4. Kiểm tra nội dung văn bản
    Assert.That(finishElement.Text, Does.Contain("Hello World!"));
}
```

---

## 2. Thông báo lỗi nguyên văn của Test A1 (PartA_KhongWait_Rot)

```text
  Failed PartA_KhongWait_Rot [4 s]
  Error Message:
   OpenQA.Selenium.NoSuchElementException : no such element: Unable to locate element: {"method":"css selector","selector":"#finish"}
  (Session info: chrome=156.0.8078.80); For documentation on this error, please visit: https://www.selenium.dev/documentation/webdriver/troubleshooting/errors#nosuchelementexception
  Stack Trace:
     at OpenQA.Selenium.WebDriver.UnpackAndThrowOnError(Response errorResponse, String commandToExecute)
   at OpenQA.Selenium.WebDriver.ExecuteAsync(String driverCommandToExecute, Dictionary`2 parameters)
   at OpenQA.Selenium.WebDriver.Execute(String driverCommandToExecute, Dictionary`2 parameters)
   at OpenQA.Selenium.WebDriver.FindElement(String mechanism, String value)
   at OpenQA.Selenium.By.<.ctor>b__6_0(ISearchContext context)
   at OpenQA.Selenium.By.FindElement(ISearchContext context)
   at OpenQA.Selenium.WebDriver.FindElement(By by)
   at FirstTests.WaitTests.PartA_KhongWait_Rot() in C:\Users\Admin\Downloads\SOF3091_LAB1_NHOM4_SD22301\FirstTest\WaitTests.cs:line 20
   at System.Reflection.MethodBaseInvoker.InterpretedInvoke_Method(Object obj, IntPtr* args)
   at System.Reflection.MethodBaseInvoker.InvokeWithNoArgs(Object obj, BindingFlags invokeAttr)

  Standard Output Messages:
 Đã chụp ảnh lỗi và lưu tại: C:\Users\Admin\Downloads\SOF3091_LAB1_NHOM4_SD22301\screenshots\PartA_KhongWait_Rot.png
```

---

## 3. Tần suất rớt đo được

- **Test A1 (`PartA_KhongWait_Rot` - Không Wait):**
  - Chạy lặp 10 lần liên tiếp: **10/10 lần rớt** (tỷ lệ rớt: **100%**).
  - Nguyên nhân: Phần tử `#finish` do JavaScript tạo ra sau khoảng 5.000 ms (5 giây), trong khi mã nguồn tìm kiếm ngay sau thao tác click mà không có cơ chế chờ (Implicit Wait = 0, không có Explicit Wait).

- **Toàn bộ bộ test không thuộc Demo (`Category!=Demo` - bao gồm A2 CoWait):**
  - Chạy lặp 10 lần liên tiếp (mỗi lần 10 test: 1 test Lab 1, 3 test Lab 2, 6 test Lab 3): **10/10 lần thành công 100% xanh** (tỷ lệ thành công: **100%**).

---

## 4. Điều kiện chạy kiểm thử thực tế

- **Hệ điều hành:** Microsoft Windows NT 10.0.19045.0 (Windows 10 Pro 64-bit)
- **Trình duyệt Google Chrome:** 156.0.8078.80
- **ChromeDriver:** Được Selenium Manager tự động tải và đồng bộ theo phiên bản Chrome 156.0.8078.80
- **Thư viện Selenium:** `Selenium.WebDriver` 4.52.0 & `Selenium.Support` 4.52.0
- **Test Framework:** `NUnit` 4.3.2, `NUnit3TestAdapter` 5.0.0, `Microsoft.NET.Test.Sdk` 17.14.0
- **Nền tảng .NET:** .NET SDK 10.0.402, Target Framework: `net10.0`
- **Chế độ hiển thị (Headless):** Không bật chế độ headless (mở trình duyệt Chrome giao diện đồ họa thông thường với cờ `--start-maximized`)
- **Mạng:** Kết nối Internet thông thường, độ trễ đến `the-internet.herokuapp.com` ổn định.
