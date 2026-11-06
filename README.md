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

screenshot/<img width="1366" height="729" alt="image" src="https://github.com/user-attachments/assets/58a51b42-c36a-4e97-a2e4-46350b6be134" />

