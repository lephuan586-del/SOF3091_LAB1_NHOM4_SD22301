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
