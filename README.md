# Lab 1 - Nhập môn kiểm thử tự động & Thiết lập môi trường

- Sinh viên: Nguyễn Quốc Tuấn - Mã SV: TB02181
- Môn: SOF3091

## Cách chạy project

1. Cài .NET SDK và Google Chrome.
2. Clone repo:
   ```
   git clone https://github.com/darkgrial/Lab1-SOF3091-TB02181.git
   ```
3. Chạy test:
   ```
   cd Lab1-SOF3091-TB02181/FirstTest
   dotnet test --logger "console;verbosity=detailed"
   ```
4. Chrome sẽ tự mở trang example.com, test đọc tiêu đề và kiểm tra bằng Assert, kết quả hiển thị **Passed**.

Ảnh minh chứng nằm trong thư mục `screenshots/`.

## Phần D - Trải nghiệm dùng AI

### Tình huống 1: Code bị 4 lỗi gạch đỏ

**Bối cảnh:** Mình dùng Windows, Visual Studio, project NUnit. Sau khi dán code test Selenium vào file `UnitTest1.cs`, các chữ `OpenQA`, `IWebDriver`, `ChromeDriver` bị gạch đỏ và báo 4 lỗi.

**Prompt đã dùng:** "Khúc này sao lỗi nhiều vậy?" (kèm ảnh chụp màn hình code).

**Câu trả lời của AI:** Cả 4 lỗi cùng một nguyên nhân là project chưa có gói `Selenium.WebDriver`. AI hướng dẫn cài gói qua NuGet, sau đó Rebuild Solution.

**Cách mình kiểm chứng:** Sau khi cài, mình mở file `.csproj` thấy dòng `Selenium.WebDriver Version="4.52.0"`, ô lỗi báo "No issues found" và test chạy ra kết quả Passed.

### Tình huống 2: Phím tắt chạy test không nhận

**Bối cảnh:** Mình bấm `Ctrl+R, A` để chạy test thì thanh trạng thái báo "The key combination (Ctrl+R, Left Windows) is not a command".

**Prompt đã dùng:** "Sao lại không được rồi?" (kèm ảnh chụp màn hình).

**Câu trả lời của AI:** Đây chỉ là lỗi phím tắt, không phải lỗi code; AI hướng dẫn chạy bằng chuột: menu Test, Test Explorer, Run All Tests.

**Cách mình kiểm chứng:** Mình chạy theo cách bấm chuột, Chrome tự mở trang example.com và Test Explorer hiện dấu xanh Passed.

### Nhận xét

AI giúp mình tìm ra nguyên nhân nhanh, chỉ cần gửi ảnh chụp màn hình là AI nhận ra cả 4 lỗi cùng do thiếu một gói thư viện và hướng dẫn từng bước bằng chuột nên mình dễ làm theo. Tuy nhiên AI cũng có chỗ chưa chính xác với máy của mình: ban đầu AI đưa code mẫu dùng `namespace FirstTests` trong khi project của mình tên `FirstTest`, và AI hướng dẫn theo VS Code (C# Dev Kit) trong khi mình đang dùng Visual Studio. Visual Studio của mình cũng tạo file `.slnx` thay vì `.sln` như AI nói. Vì vậy mình phải tự đối chiếu với màn hình thực tế rồi mới làm theo, không làm theo máy móc. Qua bài này mình rút ra là nên gửi ảnh chụp hoặc thông báo lỗi nguyên văn kèm ngữ cảnh để AI trả lời đúng hơn, và luôn kiểm chứng lại kết quả sau khi sửa.

# Lab 2 Selenium WebDriver cơ bản — Tương tác với trình duyệt và phần tử web

- Sinh viên: Lê Phú An - Mã SV: TB02149
- Môn: SOF3091

## 2 Yêu cầu 
Phần A - Bảng locator

| Phần tử | Locator 1 | Số khớp | Locator 2 | Số khớp | Chọn | Lý do |
| :--- | :--- | :---: | :--- | :---: | :--- | :--- |
| **Ô Username** | `#user-name` | 1 | `[data-test="username"]` | 1 | `#user-name` | Thuộc tính `id` là định danh duy nhất và có thứ tự ưu tiên cao nhất, giúp Selenium định vị nhanh nhất và không bị ảnh hưởng khi giao diện thay đổi style hay cấu trúc. |
| **Ô Password** | `#password` | 1 | `[data-test="password"]` | 1 | `#password` | Sử dụng `id` duy nhất trên toàn trang, mang lại độ ổn định tối đa và được ưu tiên hàng đầu trong các chiến lược định vị của Selenium. |
| **Nút Login** | `#login-button` | 1 | `[data-test="login-button"]` | 1 | `#login-button` | Phần tử có sẵn `id` định danh độc nhất, rõ ràng về mặt ngữ nghĩa và bền vững nhất khi thực thi kịch bản kiểm thử. |
| **Tên sản phẩm "Sauce Labs Backpack"** | `#item_4_title_link` | 1 | `//div[@data-test='inventory-item-name' and text()='Sauce Labs Backpack']` | 1 | `#item_4_title_link` | Phần tử có sẵn `id` duy nhất giúp truy vấn trực tiếp và đạt hiệu năng cao nhất theo đúng thứ tự ưu tiên; XPath ở Locator 2 cũng rất tốt khi cần định danh nghiệp vụ bằng text. |
| **Giá của sản phẩm "Sauce Labs Backpack"** | `.inventory_item:has(#item_4_title_link) .inventory_item_price` | 1 | `//div[text()='Sauce Labs Backpack']/ancestor::div[@data-test='inventory-item']//div[@data-test='inventory-item-price']` | 1 | `//div[text()='Sauce Labs Backpack']/ancestor::div[@data-test='inventory-item']//div[@data-test='inventory-item-price']` | Giá không có `id` hay `data-test` độc nhất; XPath tương đối này liên kết trực tiếp giá với tên sản phẩm trong cùng thẻ hàng, đảm bảo tính độc nhất 1 of 1 và hoạt động ổn định kể cả khi trạng thái giỏ hàng thay đổi. |

## Phần D — Dùng AI để tối ưu locator

**Phần tử chọn:** giá của sản phẩm Sauce Labs Backpack

**AI sử dụng:** Claude (Anthropic)

**outerHTML đã gửi cho AI:**
```html
<div class="inventory_item" data-test="inventory-item">
  <div class="inventory_item_img">...</div>
  <div class="inventory_item_description">
    <div class="inventory_item_label">
      <a href="#" id="item_4_title_link" data-test="item-4-title-link">
        <div class="inventory_item_name " data-test="inventory-item-name">Sauce Labs Backpack</div>
      </a>
      <div class="inventory_item_desc" data-test="inventory-item-desc">carry.allTheThings() with the sleek, streamlined Sly Pack...</div>
    </div>
    <div class="pricebar">
      <div class="inventory_item_price" data-test="inventory-item-price">$29.99</div>
      <button class="btn btn_primary btn_small btn_inventory " id="add-to-cart-sauce-labs-backpack" data-test="add-to-cart-sauce-labs-backpack">Add to cart</button>
    </div>
  </div>
</div>
```

**Prompt đã dùng:**
```
Tôi đang viết Selenium WebDriver bằng C# trên trang saucedemo.com.
Đây là outerHTML của khung sản phẩm trên trang inventory:

<outerHTML ở trên>

Tôi muốn lấy GIÁ của sản phẩm "Sauce Labs Backpack".
Hãy đề xuất locator bền vững nhất cho phần tử giá này.
Yêu cầu:
- Không dùng XPath tuyệt đối (bắt đầu bằng /html/body/...)
- Locator phải mô tả danh tính của phần tử, không mô tả đường đi
- Ưu tiên id, data-test, rồi CSS Selector, sau cùng mới XPath tương đối
- Locator chỉ được khớp đúng 1 phần tử trên trang
Hãy giải thích ngắn gọn vì sao locator đó bền.
```

**Câu trả lời của AI:**
```
Thẻ giá không có id riêng, và data-test="inventory-item-price" lặp lại ở cả 6 sản phẩm
nên không dùng một mình được (sẽ khớp 6 phần tử). Cần gắn giá với danh tính sản phẩm.

Đề xuất 1 (XPath tương đối, nên chọn):
//div[@data-test='inventory-item'][.//div[@data-test='inventory-item-name' and normalize-space()='Sauce Labs Backpack']]//div[@data-test='inventory-item-price']

Lý do bền: tìm khung sản phẩm theo TÊN rồi mới lấy giá bên trong, nên không phụ thuộc
thứ tự sản phẩm hay vị trí trong DOM. Dùng data-test thay cho class nên ít bị vỡ khi đổi giao diện.

Đề xuất 2 (CSS Selector):
.pricebar:has(#add-to-cart-sauce-labs-backpack) [data-test='inventory-item-price']

Lý do: id của nút Add to cart chứa tên sản phẩm (sauce-labs-backpack) nên là duy nhất;
chọn thanh giá chứa nút đó rồi lấy giá. Lưu ý :has() chỉ chạy tốt trên trình duyệt hiện đại như Chrome.

Không nên dùng: .inventory_item_price hoặc [data-test='inventory-item-price'] một mình
(khớp 6 phần tử), và div:nth-child(1) (phụ thuộc vị trí).
```

**Kiểm chứng bằng Ctrl+F:** cả 2 locator đều cho kết quả `1 of 1`

Test Passed

Screenshot/<img width="1366" height="728" alt="image" src="https://github.com/user-attachments/assets/2a0ebcd6-b8d4-4f42-9f43-f026ecfeb57a" />


**Nhận xét của tôi:**
Cả hai locator AI đề xuất khi kiểm tra bằng Ctrl+F đều ra `1 of 1`, không dùng XPath tuyệt đối và mô tả danh tính theo tên sản phẩm nên không phụ thuộc thứ tự danh sách. AI đúng khi nhận ra `data-test="inventory-item-price"` lặp lại ở 6 sản phẩm nên không thể dùng một mình. Locator CSS dùng `:has()` gọn hơn nhưng phụ thuộc trình duyệt hỗ trợ, nên tôi vẫn ưu tiên XPath ở đề xuất 1 cho ổn định. Dù AI đúng, tôi vẫn phải tự kiểm chứng vì AI không nhìn thấy trang thật.

# Lab 3 - Xử lý các tình huống thực tế phức tạp

- Sinh viên: Nguyễn Quốc Tuấn - Mã SV: TB02181
- Môn: SOF3091

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


# Lab 4 - OOP và Page Object Model (POM)

- Sinh viên: Lê Phú An - Mã SV: TB02149
- Môn: SOF3091

## Cấu trúc thư mục Pages/ và Tests/

```
FirstTest/
├── Pages/
│   ├── BasePage.cs               # Lớp cơ sở trừu tượng: quản lý driver, wait (10s), các hàm tìm kiếm có/không Explicit Wait
│   ├── DynamicLoadingPage.cs     # Đại diện trang Dynamic Loading 2 (The Internet)
│   ├── ExamplePage.cs            # Đại diện trang example.com
│   ├── IframePage.cs             # Đại diện trang iFrame TinyMCE: tự động chuyển ngữ cảnh trong try/finally
│   ├── InventoryPage.cs          # Đại diện trang kho hàng Swag Labs: kiểm tra hiển thị, sắp xếp, lấy danh sách giá/sản phẩm
│   ├── JavaScriptAlertsPage.cs   # Đại diện trang JavaScript Alerts: tương tác alert/confirm/prompt và lấy kết quả
│   └── LoginPage.cs              # Đại diện trang đăng nhập Swag Labs: xử lý luồng đăng nhập thành công và không thành công
└── Tests/
    ├── AlertIframeTests.cs       # Kiểm thử tương tác Alerts và iFrame (Lab 3) qua Page Object
    ├── BaseTest.cs               # Lớp cơ sở kiểm thử: khởi tạo ChromeDriver, ImplicitWait = 0, tự động chụp ảnh khi Test Failed
    ├── DemoFailTests.cs          # Ca kiểm thử cố ý rớt (Category "Demo") để thẩm định chức năng chụp ảnh khi thất bại
    ├── LoginTests.cs             # Ca kiểm thử đăng nhập thuận/nghịch và thu thập dữ liệu sản phẩm (Lab 2) qua Page Object
    ├── SortTests.cs              # Ca kiểm thử sắp xếp giá tăng dần dùng method chaining (Lab 3) qua Page Object
    ├── UnitTest1.cs              # Ca kiểm thử mở trang example.com và đọc tiêu đề (Lab 1) qua Page Object
    └── WaitTests.cs              # Ca kiểm thử đối chứng cơ chế chờ Explicit Wait (Lab 3): PartA_KhongWait_Rot và PartA_CoWait_Pass
```

## Cách chạy test

- **Chạy toàn bộ các ca kiểm thử chính thức (loại trừ nhóm Demo):**
  ```powershell
  dotnet test --filter "Category!=Demo" --logger "console;verbosity=normal"
  ```

- **Chạy các ca kiểm thử minh họa lỗi và chụp màn hình (nhóm Demo):**
  ```powershell
  dotnet test --filter "Category=Demo" --logger "console;verbosity=normal"
  ```

## Ảnh minh chứng

- Kết quả kiểm thử TRƯỚC refactor: `screenshots/Lab4_TRUOC_refactor.png`
- Kết quả kiểm thử SAU refactor: `screenshots/Lab4_SAU_refactor.png`

## Phần E – Thẩm định AI

**Kết luận:** bản AI trả về đáp ứng cả 9 quy ước trong prompt. Tôi không tìm thấy chỗ vi phạm quy ước nào. Tuy nhiên có 3 điểm AI tự quyết hoặc làm chưa tốt, tôi nêu ở phần sau.

**Các tiêu chí đã kiểm và cách kiểm:**

| # | Quy ước | Cách kiểm | Kết quả |
|---|---------|-----------|---------|
| 1 | BasePage abstract có driver, WebDriverWait 10 giây, hàm Tim; Page kế thừa và không khai báo lại | Đọc code; tìm `IWebDriver driver;` và `WebDriverWait` ngoài BasePage.cs | Đạt, không Page nào khai báo lại |
| 2 | Locator `private readonly By`, hành vi `public` | Đọc từng Page; tìm mọi dòng khai báo `By` trong Pages/ | Đạt, mọi locator đều private readonly |
| 3 | Tên hành vi theo việc người dùng làm | Đọc tên hàm: DangNhap, SapXep, LayDanhSachGia, LayThongBaoLoi, DaHienThi | Đạt |
| 4 | Chuyển trang trả về Page mới, ở lại trang trả về this | Đọc return của từng hàm | Đạt: DangNhap trả về `new InventoryPage(driver)`; SapXep, DangNhapKhongThanhCong, MoTrang trả về `this` |
| 5 | Không có Assert trong Page | Tìm "Assert" trong Pages/ | 0 kết quả |
| 6 | Không Page nào trả về IWebElement | Tìm "IWebElement" trong Pages/ | 2 lần xuất hiện, đều là hàm `protected` trong BasePage (Tim, TimTatCa); không có member public nào trả về IWebElement |
| 7 | File test không còn chữ "By" | Tìm "By" trong Tests/ (cả phân biệt và không phân biệt hoa thường) | 0 kết quả |
| 8 | Giữ nguyên Assert và hành vi; không Thread.Sleep | Đếm `Assert.That`: bản gốc 8, bản refactor 8, cùng nội dung; tìm Thread.Sleep và ImplicitWait | Đạt, không có Thread.Sleep |
| 9 | Namespace đúng thư mục | Đọc dòng namespace | Đạt: `FirstTests.Pages` và `FirstTests.Tests` |

**Chỗ AI làm chưa tốt / tự quyết ngoài prompt:**

1. **`ThongBaoLoiDaHienThi()` trả về `bool` nhưng không bao giờ trả `false`.** Hàm gọi `Tim(...)`, nên nếu thông báo lỗi không xuất hiện thì ném `WebDriverTimeoutException` sau 10 giây thay vì trả về `false`. Test vẫn rớt đúng (giống bản gốc), nhưng thông báo rớt là lỗi timeout chứ không phải kết quả Assert. Trong khi đó `InventoryPage.DaHienThi()` lại bắt exception và trả về `false`, nên hai hàm cùng kiểu `bool` mà xử lý không nhất quán.
2. **AI tự thêm `DangNhapKhongThanhCong` và lớp `SanPham`, không có trong prompt.** Quy ước 4 yêu cầu `DangNhap` trả về `InventoryPage`, nhưng test đăng nhập sai ở lại trang đăng nhập nên cần một hàm riêng trả về `this` để gọi `LayThongBaoLoi()`. Hướng giải quyết hợp lý nhưng là quyết định của AI, người dùng phải tự hiểu và xác nhận.
3. **AI tự bỏ `ImplicitWait = 5 giây` ở `Setup` của test gốc.** Prompt không yêu cầu việc này. Lý do hợp lý là không nên trộn Implicit và Explicit Wait, và `Tim`/`TimTatCa` đã có Explicit Wait thay thế. Nhưng đây vẫn là thay đổi cơ chế chờ so với bản gốc, nên cần chạy lại test để chắc kết quả không đổi.

**Chưa kiểm được ở bước này:** kết quả biên dịch và chạy `dotnet test` của riêng bản AI trả về. [ĐIỀN SAU KHI CHẠY]

### Prompt đã dùng

Hãy refactor file test Selenium C# (NUnit) dưới đây sang Page Object Model. Quy ước bắt buộc:
1. Có BasePage (abstract) chứa driver, WebDriverWait (10 giây) và hàm Tim(By) tìm phần tử kèm Explicit Wait. Mọi Page class kế thừa BasePage, không tự khai báo lại driver/wait.
2. Locator là private readonly By; hành vi nghiệp vụ là public.
3. Tên hành vi theo việc người dùng làm được (DangNhap, SapXep, LayDanhSachGia), không đặt theo thẻ HTML.
4. Hành vi chuyển trang trả về Page mới (return new TrangMoi(driver)); hành vi ở lại trang trả về this.
5. KHÔNG có Assert trong Page Object; Assert chỉ nằm trong file test.
6. Không Page Object nào trả về IWebElement.
7. File test sau refactor không được còn chữ "By".
8. Giữ nguyên hành vi và mọi Assert của test gốc. Không dùng Thread.Sleep.
9. Namespace: FirstTests.Pages cho Pages/, FirstTests.Tests cho Tests/.
Trả về đầy đủ: BasePage.cs, các Page class, và file test đã refactor.

Code gốc: [using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace FirstTests;

public class LoginTests
{
    private IWebDriver driver;

    [SetUp]
    public void Setup()
    {
        driver = new ChromeDriver();
        driver.Manage().Window.Maximize();
        driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(5);
        driver.Navigate().GoToUrl("https://www.saucedemo.com");
    }

    [TearDown]
    public void TearDown()
    {
        driver.Quit();
        driver.Dispose();
    }

    public void Login(string username, string password)
    {
        driver.FindElement(By.Id("user-name")).SendKeys(username);
        driver.FindElement(By.Id("password")).SendKeys(password);
        driver.FindElement(By.Id("login-button")).Click();
    }

    [Test]
    public void Login_ValidUser_GoesToInventoryPage()
    {
        Login("standard_user", "secret_sauce");
        Assert.That(driver.Url, Does.Contain("inventory.html"));
    }

    [Test]
    public void Login_LockedOutUser_ShowsErrorMessage()
    {
        Login("locked_out_user", "secret_sauce");

        var errorMessage = driver.FindElement(By.CssSelector("[data-test='error']"));

        Assert.That(errorMessage.Displayed, Is.True);
        Assert.That(errorMessage.Text, Is.EqualTo("Epic sadface: Sorry, this user has been locked out."));
        Assert.That(driver.Url, Does.Not.Contain("inventory.html"));
    }

    [Test]
    public void CollectProducts_PrintsAndAssertsDetails()
    {
        Login("standard_user", "secret_sauce");

        var productItems = driver.FindElements(By.CssSelector("[data-test='inventory-item']"));
        Assert.That(productItems.Count, Is.EqualTo(6), "Danh sách phải có đủ 6 sản phẩm.");

        var productNames = new List<string>();

        foreach (var item in productItems)
        {
            var nameElement = item.FindElement(By.CssSelector("[data-test='inventory-item-name']"));
            var priceElement = item.FindElement(By.CssSelector("[data-test='inventory-item-price']"));

            string name = nameElement.Text;
            string price = priceElement.Text;

            TestContext.Out.WriteLine($"Sản phẩm: {name} | Giá: {price}");

            Assert.That(string.IsNullOrWhiteSpace(name), Is.False, "Tên sản phẩm không được rỗng hoặc chỉ chứa khoảng trắng.");
            Assert.That(price, Does.StartWith("$"), $"Giá của sản phẩm '{name}' phải bắt đầu bằng '$'.");

            productNames.Add(name);
        }

        Assert.That(productNames, Does.Contain("Sauce Labs Backpack"), "Danh sách sản phẩm phải chứa 'Sauce Labs Backpack'.");
    }
}]

### Kết quả AI trả về

AI trả về 5 file: Pages/BasePage.cs, Pages/LoginPage.cs, Pages/InventoryPage.cs, Pages/SanPham.cs (lớp dữ liệu nhỏ chứa tên và giá), và Tests/LoginTests.cs đã refactor. Bên dưới là nguyên văn code AI trả về.

<details> <summary>Pages/BasePage.cs</summary>

using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace FirstTests.Pages;

public abstract class BasePage
{
    protected readonly IWebDriver driver;
    protected readonly WebDriverWait wait;

    protected BasePage(IWebDriver driver)
    {
        this.driver = driver;
        wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
    }

    // Tìm 1 phần tử, kèm Explicit Wait (chờ tối đa 10 giây)
    protected IWebElement Tim(By locator)
        => wait.Until(d => d.FindElement(locator));

    // Tìm nhiều phần tử, chờ đến khi có ít nhất 1 phần tử
    protected IReadOnlyCollection<IWebElement> TimTatCa(By locator)
        => wait.Until(d =>
        {
            var danhSach = d.FindElements(locator);
            return danhSach.Count > 0 ? danhSach : null;
        });

    public string LayUrlHienTai() => driver.Url;
}

</details> <details> <summary>Pages/LoginPage.cs</summary>

using OpenQA.Selenium;

namespace FirstTests.Pages;

public class LoginPage : BasePage
{
    private const string DuongDan = "https://www.saucedemo.com";

    private readonly By oTenDangNhap = By.Id("user-name");
    private readonly By oMatKhau = By.Id("password");
    private readonly By nutDangNhap = By.Id("login-button");
    private readonly By khungThongBaoLoi = By.CssSelector("[data-test='error']");

    public LoginPage(IWebDriver driver) : base(driver) { }

    public LoginPage MoTrang()
    {
        driver.Navigate().GoToUrl(DuongDan);
        return this;
    }

    // Đăng nhập thành công -> chuyển sang trang sản phẩm
    public InventoryPage DangNhap(string tk, string mk)
    {
        NhapThongTinVaBam(tk, mk);
        return new InventoryPage(driver);
    }

    // Đăng nhập thất bại -> ở lại trang đăng nhập
    public LoginPage DangNhapKhongThanhCong(string tk, string mk)
    {
        NhapThongTinVaBam(tk, mk);
        return this;
    }

    public bool ThongBaoLoiDaHienThi() => Tim(khungThongBaoLoi).Displayed;

    public string LayThongBaoLoi() => Tim(khungThongBaoLoi).Text;

    private void NhapThongTinVaBam(string tk, string mk)
    {
        Tim(oTenDangNhap).SendKeys(tk);
        Tim(oMatKhau).SendKeys(mk);
        Tim(nutDangNhap).Click();
    }
}

</details> <details> <summary>Pages/InventoryPage.cs</summary>

using System.Globalization;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace FirstTests.Pages;

public class InventoryPage : BasePage
{
    private readonly By moiSanPham = By.CssSelector("[data-test='inventory-item']");
    private readonly By tenSanPham = By.CssSelector("[data-test='inventory-item-name']");
    private readonly By giaSanPham = By.CssSelector("[data-test='inventory-item-price']");
    private readonly By oChonSapXep = By.CssSelector("[data-test='product-sort-container']");

    public InventoryPage(IWebDriver driver) : base(driver) { }

    public bool DaHienThi()
    {
        try
        {
            return TimTatCa(moiSanPham).Count > 0;
        }
        catch (WebDriverTimeoutException)
        {
            return false;
        }
    }

    public InventoryPage SapXep(string value)
    {
        new SelectElement(Tim(oChonSapXep)).SelectByValue(value);
        return this;
    }

    public List<decimal> LayDanhSachGia()
    {
        return TimTatCa(giaSanPham)
            .Select(e => decimal.Parse(e.Text.Replace("$", ""), CultureInfo.InvariantCulture))
            .ToList();
    }

    public List<SanPham> LayDanhSachSanPham()
    {
        var ketQua = new List<SanPham>();
        foreach (var o in TimTatCa(moiSanPham))
        {
            ketQua.Add(new SanPham(
                o.FindElement(tenSanPham).Text,
                o.FindElement(giaSanPham).Text));
        }
        return ketQua;
    }
}

</details> <details> <summary>Pages/SanPham.cs</summary>

namespace FirstTests.Pages;

// Dữ liệu 1 sản phẩm hiển thị trên trang (chỉ chứa chữ, không chứa phần tử web)
public record SanPham(string Ten, string Gia);

</details> <details> <summary>Tests/LoginTests.cs</summary>

using FirstTests.Pages;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace FirstTests.Tests;

public class LoginTests
{
    private IWebDriver driver;
    private LoginPage loginPage;

    [SetUp]
    public void Setup()
    {
        driver = new ChromeDriver();
        driver.Manage().Window.Maximize();
        loginPage = new LoginPage(driver).MoTrang();
    }

    [TearDown]
    public void TearDown()
    {
        driver.Quit();
        driver.Dispose();
    }

    [Test]
    public void Login_ValidUser_GoesToInventoryPage()
    {
        var inventoryPage = loginPage.DangNhap("standard_user", "secret_sauce");

        Assert.That(inventoryPage.LayUrlHienTai(), Does.Contain("inventory.html"));
    }

    [Test]
    public void Login_LockedOutUser_ShowsErrorMessage()
    {
        loginPage.DangNhapKhongThanhCong("locked_out_user", "secret_sauce");

        Assert.That(loginPage.ThongBaoLoiDaHienThi(), Is.True);
        Assert.That(loginPage.LayThongBaoLoi(), Is.EqualTo("Epic sadface: Sorry, this user has been locked out."));
        Assert.That(loginPage.LayUrlHienTai(), Does.Not.Contain("inventory.html"));
    }

    [Test]
    public void CollectProducts_PrintsAndAssertsDetails()
    {
        var inventoryPage = loginPage.DangNhap("standard_user", "secret_sauce");

        var sanPhams = inventoryPage.LayDanhSachSanPham();
        Assert.That(sanPhams.Count, Is.EqualTo(6), "Danh sách phải có đủ 6 sản phẩm.");

        var productNames = new List<string>();

        foreach (var sanPham in sanPhams)
        {
            TestContext.Out.WriteLine($"Sản phẩm: {sanPham.Ten} | Giá: {sanPham.Gia}");

            Assert.That(string.IsNullOrWhiteSpace(sanPham.Ten), Is.False, "Tên sản phẩm không được rỗng hoặc chỉ chứa khoảng trắng.");
            Assert.That(sanPham.Gia, Does.StartWith("$"), $"Giá của sản phẩm '{sanPham.Ten}' phải bắt đầu bằng '$'.");

            productNames.Add(sanPham.Ten);
        }

        Assert.That(productNames, Does.Contain("Sauce Labs Backpack"), "Danh sách sản phẩm phải chứa 'Sauce Labs Backpack'.");
    }
}



### Chỗ AI làm sai / tiêu chí đã kiểm

Kết luận: bản AI trả về đáp ứng cả 9 quy ước trong prompt. Tôi không tìm thấy chỗ vi phạm quy ước nào. Tuy nhiên có 3 điểm AI tự quyết hoặc làm chưa tốt, tôi nêu ở phần sau.

Các tiêu chí đã kiểm và cách kiểm:

#	Quy ước	Cách kiểm	Kết quả
1	BasePage abstract có driver, WebDriverWait 10 giây, hàm Tim; Page kế thừa và không khai báo lại	Đọc code; tìm IWebDriver driver; và WebDriverWait ngoài BasePage.cs	Đạt, không Page nào khai báo lại
2	Locator private readonly By, hành vi public	Đọc từng Page; tìm mọi dòng khai báo By trong Pages/	Đạt, mọi locator đều private readonly
3	Tên hành vi theo việc người dùng làm	Đọc tên hàm: DangNhap, SapXep, LayDanhSachGia, LayThongBaoLoi, DaHienThi	Đạt
4	Chuyển trang trả về Page mới, ở lại trang trả về this	Đọc return của từng hàm	Đạt: DangNhap trả về new InventoryPage(driver); SapXep, DangNhapKhongThanhCong, MoTrang trả về this
5	Không có Assert trong Page	Tìm "Assert" trong Pages/	0 kết quả
6	Không Page nào trả về IWebElement	Tìm "IWebElement" trong Pages/	2 lần xuất hiện, đều là hàm protected trong BasePage (Tim, TimTatCa); không có member public nào trả về IWebElement
7	File test không còn chữ "By"	Tìm "By" trong Tests/ (cả phân biệt và không phân biệt hoa thường)	0 kết quả
8	Giữ nguyên Assert và hành vi; không Thread.Sleep	Đếm Assert.That: bản gốc 8, bản refactor 8, cùng nội dung; tìm Thread.Sleep và ImplicitWait	Đạt, không có Thread.Sleep
9	Namespace đúng thư mục	Đọc dòng namespace	Đạt: FirstTests.Pages và FirstTests.Tests

Chỗ AI làm chưa tốt / tự quyết ngoài prompt:

ThongBaoLoiDaHienThi() trả về bool nhưng không bao giờ trả false. Hàm gọi Tim(...), nên nếu thông báo lỗi không xuất hiện thì ném WebDriverTimeoutException sau 10 giây thay vì trả về false. Test vẫn rớt đúng (giống bản gốc, FindElement cũng ném lỗi khi không thấy), nhưng thông báo rớt là lỗi timeout chứ không phải kết quả Assert. Trong khi đó InventoryPage.DaHienThi() lại bắt exception và trả về false. Hai hàm cùng kiểu bool mà cách xử lý không nhất quán.
AI tự thêm DangNhapKhongThanhCong và lớp SanPham, không có trong prompt. Quy ước 4 yêu cầu DangNhap trả về InventoryPage, nhưng test đăng nhập sai (locked_out_user) ở lại trang đăng nhập; nếu chỉ dùng DangNhap thì không gọi được LayThongBaoLoi() trên LoginPage. AI giải quyết bằng một hàm riêng trả về this. Hướng giải quyết hợp lý nhưng là quyết định của AI, người dùng phải tự hiểu và xác nhận.
AI tự bỏ ImplicitWait = 5 giây ở Setup của test gốc. Prompt không yêu cầu việc này. Lý do hợp lý là không nên trộn Implicit và Explicit Wait, và Tim/TimTatCa đã có Explicit Wait thay thế (TimTatCa chờ đến khi có ít nhất 1 phần tử, giống cách FindElements của bản gốc đã chờ nhờ Implicit Wait). Nhưng đây vẫn là một thay đổi cơ chế chờ so với bản gốc, nên cần chạy lại test để chắc kết quả không đổi.
