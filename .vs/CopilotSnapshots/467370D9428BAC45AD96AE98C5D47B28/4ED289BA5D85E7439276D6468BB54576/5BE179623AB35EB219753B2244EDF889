# 🧮 Calculator - Đồ án WPF (.NET 8)

## 1) Giới thiệu
Đây là đồ án xây dựng ứng dụng **máy tính cơ bản** trên nền tảng **WPF** bằng **C# / .NET 8**.

Ứng dụng tập trung vào các phép toán số học thông dụng, giao diện trực quan, thao tác nhanh và quản lý trạng thái rõ ràng trong mã nguồn.

---

## 2) Mục tiêu đồ án
- Xây dựng một ứng dụng desktop đơn giản nhưng đầy đủ nghiệp vụ máy tính cơ bản.
- Áp dụng mô hình xử lý sự kiện trong WPF (`Click` event routing).
- Quản lý trạng thái nhập liệu và trạng thái phép tính một cách tường minh.
- Viết mã dễ đọc, dễ mở rộng và có chú thích rõ ràng theo từng phần.

---

## 3) Công nghệ sử dụng
- **Ngôn ngữ**: C# 12
- **Framework**: .NET 8 (`net8.0-windows`)
- **UI Framework**: WPF
- **IDE khuyến nghị**: Visual Studio 2022 trở lên

Thông tin cấu hình chính trong `Calculator/Calculator.csproj`:
- `UseWPF = true`
- `TargetFramework = net8.0-windows`
- `Nullable = enable`

---

## 4) Chức năng chính
Ứng dụng hỗ trợ các thao tác:
- Nhập số `0-9`
- Nhập số thập phân (`.`)
- Các toán tử: `+`, `-`, `*`, `/`
- `=` để tính kết quả
- `C` để reset toàn bộ máy tính
- `CE` để xóa giá trị đang nhập
- `⌫` (Backspace) để xóa 1 ký tự cuối
- `+/−` để đổi dấu số hiện tại
- Hiển thị biểu thức và kết quả riêng biệt
- Bắt lỗi chia cho 0 và hiển thị thông báo an toàn

---

## 5) Cấu trúc dự án
```text
Calculator/
├─ Calculator.sln
├─ README.md
└─ Calculator/
   ├─ Calculator.csproj
   ├─ App.xaml
   ├─ App.xaml.cs
   ├─ MainWindow.xaml          # Giao diện máy tính
   ├─ MainWindow.xaml.cs       # Logic xử lý nghiệp vụ
   └─ AssemblyInfo.cs
```

---

## 6) Thiết kế giao diện
File `MainWindow.xaml` gồm các phần chính:
- **`Window.Resources`**: định nghĩa style cho nút (`BaseButtonStyle`, `NumberButtonStyle`, `OperatorButtonStyle`, `EqualButtonStyle`)
- **Vùng hiển thị**:
  - `ExpressionTextBlock`: hiển thị biểu thức đang tính
  - `ResultTextBlock`: hiển thị số hiện tại/kết quả
- **5 hàng phím chức năng** theo dạng `UniformGrid`

Thiết kế theo tông màu tối, dễ nhìn, thao tác bằng chuột trực tiếp.

---

## 7) Thiết kế logic xử lý
File `MainWindow.xaml.cs` tổ chức logic theo từng nhóm:

### 7.1 Biến trạng thái
- `_firstOperand`: toán hạng thứ nhất
- `_currentOperator`: toán tử hiện tại
- `_isWaitingForNewInput`: đang chờ nhập toán hạng mới hay không
- `_isShowingResult`: đang hiển thị kết quả sau `=`

### 7.2 Luồng xử lý nút bấm
- Mọi nút đều đi qua `OnCalculatorButtonClick(...)`
- Dựa vào `Tag` của button để route sang hàm phù hợp:
  - `AppendNumber`, `AppendDecimalPoint`
  - `SetOperator`, `CalculateAndShowResult`
  - `ResetCalculator`, `ClearEntry`, `HandleBackspace`, `ToggleSign`

### 7.3 Tính toán lõi
- `CalculateResult(left, right, op)` xử lý nghiệp vụ phép tính
- Có kiểm tra chia cho 0 (`DivideByZeroException`)

### 7.4 Xử lý lỗi và định dạng
- `IsDisplayError()` kiểm tra trạng thái hiển thị có hợp lệ không
- `ShowError(...)` đưa app về trạng thái an toàn
- `FormatNumber(...)` và `GetCurrentDisplayValue()` dùng `InvariantCulture`

---

## 8) Cách cài đặt và chạy

### Cách 1: Dùng Visual Studio
1. Mở file `Calculator.sln`
2. Chọn cấu hình `Debug` hoặc `Release`
3. Nhấn `F5` để chạy

### Cách 2: Dùng .NET CLI
Tại thư mục gốc dự án:

```bash
dotnet restore
dotnet build
dotnet run --project Calculator/Calculator.csproj
```

---

## 9) Kiểm thử thủ công (Manual test)
Có thể kiểm tra nhanh bằng các kịch bản sau:

1. **Cộng cơ bản**: `12 + 3 =` → `15`
2. **Phép tính chuỗi**: `2 + 3 + 4 =` → `9`
3. **Số thập phân**: `1.5 + 2.25 =` → `3.75`
4. **Đổi dấu**: nhập `8`, bấm `+/−` → `-8`
5. **Backspace**: nhập `123`, bấm `⌫` → `12`
6. **CE**: sau khi nhập số, bấm `CE` → về `0` nhưng vẫn giữ toán tử trước đó
7. **C**: bấm `C` → reset toàn bộ trạng thái
8. **Chia cho 0**: `9 / 0 =` → hiển thị lỗi `Không thể chia cho 0`

---

## 10) Điểm nổi bật của đồ án
- Mã xử lý chia theo từng chức năng rõ ràng
- Trạng thái ứng dụng được quản lý minh bạch
- UI và logic tách biệt hợp lý theo chuẩn WPF code-behind cho ứng dụng nhỏ
- Có xử lý ngoại lệ để tránh crash khi thao tác sai

---

## 11) Hướng phát triển
- Thêm các phép toán nâng cao: `%`, `x²`, `√x`, `1/x`
- Hỗ trợ bàn phím vật lý (key binding)
- Lưu lịch sử phép tính
- Tách logic tính toán thành lớp riêng (`CalculatorEngine`) + unit test
- Áp dụng MVVM để mở rộng quy mô tốt hơn

---

## 12) Tác giả
Đồ án thực hiện bởi: Trần Dương Gia Bảo

---

