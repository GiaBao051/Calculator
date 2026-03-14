# 🧮 Calculator WPF (.NET 8)

Ứng dụng máy tính desktop viết bằng **C# WPF** theo phong cách **Windows Calculator Standard (mức cơ bản + level 1)**.

## 1. Giới thiệu
Dự án được xây dựng nhằm:
- Luyện tập lập trình giao diện desktop với WPF.
- Tổ chức mã theo hướng dễ đọc, dễ bảo trì.
- Xử lý đầy đủ các thao tác phổ biến của máy tính chuẩn.

Phiên bản hiện tại ưu tiên cách làm **thực tế, dễ hiểu** cho sinh viên, với logic chính đặt trong `MainWindow.xaml.cs` (code-behind).

----

## 2. Công nghệ sử dụng
- **Ngôn ngữ:** C# 12
- **Nền tảng:** .NET 8 (`net8.0-windows`)
- **UI Framework:** WPF
- **IDE khuyến nghị:** Visual Studio 2022+

Thông tin chính trong `Calculator/Calculator.csproj`:
- `UseWPF = true`
- `TargetFramework = net8.0-windows`
- `Nullable = enable`

---

## 3. Chức năng đã hoàn thành (Level 1)
### Nhóm nhập liệu
- Nhập số `0-9`
- Nhập dấu thập phân `.` (chặn nhập nhiều dấu chấm)
- Đổi dấu `+/−`

### Nhóm phép toán nhị phân
- Cộng `+`
- Trừ `-`
- Nhân `*`
- Chia `/`
- Bằng `=`

### Nhóm thao tác hệ thống
- `CE` (xóa giá trị đang nhập)
- `C` (xóa toàn bộ)
- `⌫` Backspace (xóa 1 ký tự cuối)

### Nhóm phép toán mở rộng
- `%`
- `1/x`
- `x²`
- `√x`

### Nhóm hiển thị và an toàn
- Hiển thị biểu thức (`ExpressionTextBlock`) và kết quả (`ResultTextBlock`) riêng.
- Bắt lỗi chia cho 0.
- Bắt lỗi căn bậc hai số âm.
- Đưa ứng dụng về trạng thái an toàn khi lỗi.

---

## 4. Cấu trúc thư mục
```text
Calculator/
├─ Calculator.sln
├─ README.md
└─ Calculator/
   ├─ Calculator.csproj
   ├─ App.xaml
   ├─ App.xaml.cs
   ├─ MainWindow.xaml          # Giao diện máy tính
   ├─ MainWindow.xaml.cs       # Logic xử lý chính (code-behind)
   ├─ AssemblyInfo.cs
   ├─ Commands/                # Thành phần command (định hướng mở rộng)
   ├─ Services/                # Thành phần service (định hướng mở rộng)
   └─ ViewModels/              # Thành phần ViewModel (định hướng mở rộng)
```

> Ghi chú: Bản chạy chính hiện tại dùng luồng code-behind trong `MainWindow.xaml.cs`.

---

## 5. Thiết kế giao diện
`MainWindow.xaml` gồm:
- **`Window.Resources`**: định nghĩa style cho các nhóm nút.
- **Vùng hiển thị**:
  - `ExpressionTextBlock`: biểu thức đang thao tác.
  - `ResultTextBlock`: kết quả/số đang nhập.
- **Các hàng phím** dùng `UniformGrid` để chia layout đều như máy tính chuẩn.

Giao diện tông tối, dễ nhìn, thao tác trực tiếp bằng chuột.

---

## 6. Thiết kế logic xử lý
Logic trong `MainWindow.xaml.cs` được tách theo hàm rõ ràng:
- `AppendNumber`
- `AppendDecimalPoint`
- `SetOperator`
- `CalculateAndShowResult`
- `ClearEntry`
- `ResetCalculator`
- `HandleBackspace`
- `ToggleSign`
- `ApplyPercent`
- `ApplyUnaryOperation`

Điều phối tập trung tại `OnCalculatorButtonClick`, dựa trên `Tag` của từng nút để gọi đúng hàm xử lý.

---

## 7. Cách chạy dự án
### Cách 1: Visual Studio
1. Mở `Calculator.sln`
2. Chọn `Debug` hoặc `Release`
3. Nhấn `F5`

### Cách 2: .NET CLI
Tại thư mục gốc dự án:

```bash
dotnet restore
dotnet build
dotnet run --project Calculator/Calculator.csproj
```

---

## 8. Kịch bản kiểm thử nhanh
1. `12 + 3 =` → `15`
2. `2 + 3 + 4 =` → `9`
3. `1.5 + 2.25 =` → `3.75`
4. `8` rồi `+/−` → `-8`
5. `123` rồi `⌫` → `12`
6. `200 + 10 % =` → `220`
7. `9 / 0 =` → báo lỗi chia cho 0
8. `-9` rồi `√x` → báo lỗi căn số âm
9. `CE` chỉ xóa số hiện tại, không mất toán tử đang chờ
10. `C` reset toàn bộ về trạng thái ban đầu

---

## 9. Lỗi thường gặp đã xử lý
- Chia cho 0 (`/` hoặc `1/x`).
- Căn bậc hai số âm.
- Nhập nhiều dấu `.` trong cùng một số.
- Backspace khi chỉ còn 1 ký tự.
- Đổi dấu khi màn hình là `0`.

---

## 10. Hướng phát triển tiếp theo
- Hỗ trợ bàn phím vật lý (phím số, Enter, Backspace...).
- Thêm bộ nhớ: `MC`, `MR`, `M+`, `M-`.
- Lưu lịch sử phép tính.
- Tách logic hoàn toàn theo MVVM khi nâng cấp phiên bản.
- Viết unit test cho lớp xử lý tính toán.

---

## 11. Tác giả
- **Trần Dương Gia Bảo**

