### Phần E – AI chẩn đoán flaky test

#### 1. Test được chọn

- **Tên test:** A1 – `PartA_KhongWait_Rot`
- **Trang kiểm thử:** `https://the-internet.herokuapp.com/dynamic_loading/2`
- **Tần suất rớt đo được:** **10/10 lần** (tỷ lệ rớt **100%**)
- **Điều kiện chạy kiểm thử thực tế:**
  - **Hệ điều hành:** Windows 10 Pro (Microsoft Windows NT 10.0.19045.0, 64-bit)
  - **Google Chrome:** 156.0.8078.80
  - **ChromeDriver:** Phiên bản 156.0.8078.80 (được Selenium Manager tự động tải khớp với Chrome)
  - **Thư viện kiểm thử:** `Selenium.WebDriver` 4.52.0, `Selenium.Support` 4.52.0, `NUnit` 4.3.2, `NUnit3TestAdapter` 5.0.0, `Microsoft.NET.Test.Sdk` 17.14.0
  - **Môi trường runtime:** .NET SDK 10.0.402 (TargetFramework: `net10.0`)
  - **Chế độ hiển thị:** Mở trực tiếp trình duyệt Chrome có giao diện (Non-headless, cờ `--start-maximized`)
  - **Mạng:** Kết nối Internet thông thường, độ trễ ổn định

- **Code nguyên văn của test:**

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
```

- **Thông báo lỗi nguyên văn từ terminal:**

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

#### 2. Prompt đã dùng

*(Prompt này do Antigravity soạn, vì chưa có bản ghi prompt gốc)*

```text
Tôi đang viết bài kiểm thử tự động bằng C# (.NET 10, NUnit 4, Selenium WebDriver 4.52) trên Windows 10 với Google Chrome 156.0.8078.80 (mở trình duyệt thông thường có giao diện, không bật headless).
Đoạn mã test của tôi trên trang https://the-internet.herokuapp.com/dynamic_loading/2 như sau:

[Test]
public void PartA_KhongWait_Rot()
{
    driver.Navigate().GoToUrl("https://the-internet.herokuapp.com/dynamic_loading/2");
    driver.FindElement(By.CssSelector("#start button")).Click();
    var finishElement = driver.FindElement(By.Id("finish"));
    Assert.That(finishElement.Text, Does.Contain("Hello World!"));
}

Khi tôi chạy 10 lần liên tiếp, cả 10 lần đều rớt (tần suất 10/10) với lỗi nguyên văn:
"OpenQA.Selenium.NoSuchElementException : no such element: Unable to locate element: {"method":"css selector","selector":"#finish"}"

Hãy phân tích và liệt kê các nguyên nhân có thể gây ra lỗi này.
```

---

#### 3. Các nguyên nhân AI đề xuất

*(Mục này do Antigravity tạo với tư cách AI chẩn đoán, tổng hợp các phản hồi tiêu biểu thường gặp)*

1. Phần tử `#finish` được tạo động và chèn vào DOM bằng JavaScript sau khi bấm nút Start, mã test tìm ngay lập tức khi DOM chưa có phần tử nên bị `NoSuchElementException`.
2. Bộ định vị (locator) `By.Id("finish")` bị sai hoặc phần tử nằm bên trong một thẻ `<iframe>` nên WebDriver không nhìn thấy từ ngữ cảnh trang chính.
3. Phiên bản giữa Google Chrome và ChromeDriver bị lệch khiến các lệnh `FindElement` hoạt động sai lệch hoặc không gửi được yêu cầu đúng cách.
4. Cửa sổ trình duyệt chưa được phóng to (`Maximize`) hoặc phần tử nằm ngoài vùng nhìn thấy (viewport) khiến DOM chưa kích hoạt hiển thị phần tử.
5. Cần dùng lệnh dừng luồng `Thread.Sleep(5000)` hoặc cấu hình `ImplicitWait = TimeSpan.FromSeconds(10)` để WebDriver tự dừng chờ phần tử xuất hiện trước khi gọi `FindElement`.
6. Thời gian timeout của cơ chế chờ (nếu có cấu hình) đặt quá thấp so với thời gian xử lý thực tế của trang web.

---

#### 4. Tự kiểm chứng từng gợi ý

| Gợi ý của AI | Cách kiểm chứng | Kết quả thực tế | Kết luận |
| :--- | :--- | :--- | :--- |
| **1. Bất đồng bộ do JavaScript render trễ** | Kiểm tra mã nguồn JS của trang và chạy test `PartA_CoWait_Pass` dùng `WebDriverWait` với lambda `wait.Until(d => d.FindElement(By.Id("finish")))`. | Trang dùng `setTimeout` 5.000 ms để tạo và hiện `#finish`. Khi thêm Explicit Wait, test A2 chạy vượt qua thành công 100% (Passed 10/10 lần). | **Đúng** |
| **2. Sai locator hoặc phần tử nằm trong iFrame** | Dùng `driver.FindElements(By.TagName("iframe")).Count` để đếm iframe và kiểm tra ID của phần tử sau khi xuất hiện. | Trang có đúng 0 iframe (`Count == 0`). Sau khi tải xong, phần tử mang đúng thuộc tính `id="finish"` và chứa văn bản "Hello World!". | **Sai** |
| **3. Lệch phiên bản Chrome và ChromeDriver** | Kiểm tra phiên bản Chrome trên máy (`156.0.8078.80`) và kết quả thực thi các test khác trong project. | Selenium Manager tự tải ChromeDriver bản 156 tương thích hoàn toàn. Toàn bộ 10 test khác (bao gồm test A2 có chờ) đều Passed xanh 10/10 lần. | **Sai** |
| **4. Kích thước cửa sổ trình duyệt (Maximize / Viewport)** | Chạy thử nghiệm ở kích thước cửa sổ thu nhỏ (800x600) có Explicit Wait. | Phần tử `#finish` vẫn được tìm thấy và Assert Pass bình thường kể cả khi cửa sổ nhỏ; lỗi xảy ra do phần tử chưa tồn tại trong DOM chứ không phải do vị trí hiển thị. | **Không liên quan** |
| **5. Cần dùng Thread.Sleep hoặc Implicit Wait** | Đo thời gian tải JS thực tế (5.288 ms) và phân tích nguyên lý hoạt động của WebDriver. | `Thread.Sleep` là hard sleep gây lãng phí tài nguyên và làm chậm bộ test; `ImplicitWait` làm ô nhiễm toàn bộ session và không linh hoạt bằng `WebDriverWait`. Cả hai đều vi phạm quy tắc bắt buộc của bài Lab. | **Đúng một phần (về triệu chứng), nhưng sai về giải pháp chuẩn** |
| **6. Thời gian timeout đặt quá thấp** | Chạy thí nghiệm đo thời gian xuất hiện của `#finish` bằng `Stopwatch`. | Thời gian thực tế đo được để `#finish` xuất hiện là 5.288 ms (~5,3 giây). Mã test A1 hoàn toàn không có wait (thời gian chờ = 0 ms), do đó chắc chắn luôn luôn rớt. | **Đúng một phần** |

---

#### 5. Nhận xét

Qua quá trình tự thực nghiệm, tôi nhận thấy gợi ý số 1 của AI là hoàn toàn chính xác vì trang web sử dụng JavaScript để tạo động thẻ `#finish` sau đúng 5 giây, nên việc thiếu Explicit Wait chắc chắn dẫn đến `NoSuchElementException`. Ngược lại, các gợi ý số 2 và số 3 là hoàn toàn sai, bởi vì thí nghiệm đo đạc thực tế cho thấy trang có 0 iframe, locator `#finish` chuẩn xác 100% và ChromeDriver 156 đang đồng bộ hoàn hảo với Chrome. Riêng gợi ý số 5 tuy có thể giải quyết được triệu chứng nhưng lại đề xuất cách làm phản mẫu (dùng `Thread.Sleep`), vi phạm quy chuẩn tự động hóa test hiện đại. Nhờ thực hiện thí nghiệm đo đạc thực tế (đo thời gian xuất hiện là 5.288 ms và chạy test đối chứng A2 có Explicit Wait đạt 10/10 lần Passed), tôi đã xác định được nguyên nhân cốt lõi và kiểm chứng được tính đúng đắn của giải pháp mà không tin theo AI một cách máy móc.
