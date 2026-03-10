using System;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;

namespace Calculator;

public partial class MainWindow : Window
{
    // _firstOperand: toán hạng thứ nhất (ví dụ trong phép 12 + 3 thì đây là 12)
    private decimal? _firstOperand;

    // _currentOperator: toán tử hiện tại (+, -, *, /)
    private string? _currentOperator;

    // _isWaitingForNewInput:
    // true  = đang chờ người dùng nhập số mới (thường sau khi bấm toán tử)
    // false = đang trong quá trình nhập số hiện tại
    private bool _isWaitingForNewInput = true;

    // _isShowingResult: đánh dấu màn hình đang hiển thị kết quả sau '='
    private bool _isShowingResult;

    public MainWindow()
    {
        InitializeComponent();
        ResetCalculator();
    }

    // Event tổng: chỉ làm nhiệm vụ điều hướng (route) hành động.
    // Không đặt logic tính toán phức tạp ở đây để dễ bảo trì.
    private void OnCalculatorButtonClick(object sender, RoutedEventArgs e)
    {
        if (sender is not Button button || button.Tag is not string action)
        {
            return;
        }

        if (action.Length == 1 && char.IsDigit(action[0]))
        {
            AppendNumber(action);
            return;
        }

        switch (action)
        {
            case ".":
                AppendDecimalPoint();
                break;
            case "+":
            case "-":
            case "*":
            case "/":
                SetOperator(action);
                break;
            case "=":
                CalculateAndShowResult();
                break;
            case "C":
                ResetCalculator();
                break;
            case "CE":
                ClearEntry();
                break;
            case "Backspace":
                HandleBackspace();
                break;
            case "ToggleSign":
                ToggleSign();
                break;
        }
    }

    // Thêm số vào màn hình hiện tại.
    // Ví dụ: đang là "12" bấm "3" => "123"
    // Nếu đang ở trạng thái chờ nhập mới thì ghi đè luôn số cũ.
    private void AppendNumber(string numberText)
    {
        if (IsDisplayError())
        {
            ResetCalculator();
        }

        // Nếu vừa bấm '=' và chưa chọn toán tử mới,
        // nhập số mới sẽ bắt đầu một phép tính mới.
        if (_isShowingResult && _currentOperator is null)
        {
            ExpressionTextBlock.Text = string.Empty;
            _firstOperand = null;
            _isShowingResult = false;
        }

        if (_isWaitingForNewInput || ResultTextBlock.Text == "0")
        {
            ResultTextBlock.Text = numberText;
        }
        else
        {
            ResultTextBlock.Text += numberText;
        }

        _isWaitingForNewInput = false;
    }

    // Thêm dấu thập phân '.' nếu chưa có.
    // Nếu đang chờ nhập mới thì khởi tạo thành "0.".
    private void AppendDecimalPoint()
    {
        if (IsDisplayError())
        {
            ResetCalculator();
        }

        if (_isWaitingForNewInput)
        {
            ResultTextBlock.Text = "0.";
            _isWaitingForNewInput = false;
            _isShowingResult = false;
            return;
        }

        if (!ResultTextBlock.Text.Contains('.'))
        {
            ResultTextBlock.Text += ".";
        }
    }

    // Lưu toán tử và chuẩn bị cho toán hạng tiếp theo.
    // Có hỗ trợ phép tính chuỗi: 2 + 3 + 4 => bấm '+' lần 2 sẽ tính 2+3 trước.
    private void SetOperator(string selectedOperator)
    {
        if (IsDisplayError())
        {
            ResetCalculator();
            return;
        }

        try
        {
            decimal currentValue = GetCurrentDisplayValue();

            if (_firstOperand.HasValue && _currentOperator is not null && !_isWaitingForNewInput)
            {
                decimal chainedResult = CalculateResult(_firstOperand.Value, currentValue, _currentOperator);
                _firstOperand = chainedResult;
                ResultTextBlock.Text = FormatNumber(chainedResult);
            }
            else
            {
                _firstOperand = currentValue;
            }

            _currentOperator = selectedOperator;
            _isWaitingForNewInput = true;
            _isShowingResult = false;
            ExpressionTextBlock.Text = $"{FormatNumber(_firstOperand.Value)} {GetOperatorSymbol(selectedOperator)}";
        }
        catch (DivideByZeroException)
        {
            ShowError("Không thể chia cho 0");
        }
        catch
        {
            ShowError("Lỗi phép tính");
        }
    }

    // Xử lý khi bấm '=': lấy toán hạng 2, tính kết quả, hiển thị biểu thức + kết quả.
    private void CalculateAndShowResult()
    {
        if (_firstOperand is null || _currentOperator is null || _isWaitingForNewInput || IsDisplayError())
        {
            return;
        }

        decimal secondOperand = GetCurrentDisplayValue();

        try
        {
            decimal result = CalculateResult(_firstOperand.Value, secondOperand, _currentOperator);
            ExpressionTextBlock.Text =
                $"{FormatNumber(_firstOperand.Value)} {GetOperatorSymbol(_currentOperator)} {FormatNumber(secondOperand)} =";

            ResultTextBlock.Text = FormatNumber(result);
            _firstOperand = result;
            _currentOperator = null;
            _isWaitingForNewInput = true;
            _isShowingResult = true;
        }
        catch (DivideByZeroException)
        {
            ShowError("Không thể chia cho 0");
        }
        catch
        {
            ShowError("Lỗi phép tính");
        }
    }

    // Hàm lõi tính toán: chỉ nhận input và trả output, không phụ thuộc UI.
    // Đây là điểm tốt để sau này tách sang class CalculatorEngine.
    private decimal CalculateResult(decimal leftOperand, decimal rightOperand, string mathOperator)
    {
        return mathOperator switch
        {
            "+" => leftOperand + rightOperand,
            "-" => leftOperand - rightOperand,
            "*" => leftOperand * rightOperand,
            "/" when rightOperand == 0 => throw new DivideByZeroException(),
            "/" => leftOperand / rightOperand,
            _ => throw new InvalidOperationException("Toán tử không hợp lệ")
        };
    }

    // Đọc giá trị hiện tại đang hiển thị trên màn hình kết quả.
    // Dùng InvariantCulture để thống nhất dấu '.' cho số thập phân.
    private decimal GetCurrentDisplayValue()
    {
        if (!decimal.TryParse(ResultTextBlock.Text, NumberStyles.Number, CultureInfo.InvariantCulture, out decimal value))
        {
            throw new FormatException("Giá trị hiển thị không hợp lệ");
        }

        return value;
    }

    // C: reset toàn bộ trạng thái máy tính.
    private void ResetCalculator()
    {
        _firstOperand = null;
        _currentOperator = null;
        _isWaitingForNewInput = true;
        _isShowingResult = false;

        ExpressionTextBlock.Text = string.Empty;
        ResultTextBlock.Text = "0";
    }

    // CE: chỉ xóa giá trị đang nhập, không xóa toán tử/toán hạng đã lưu.
    private void ClearEntry()
    {
        if (IsDisplayError())
        {
            ResetCalculator();
            return;
        }

        ResultTextBlock.Text = "0";
        _isWaitingForNewInput = true;
        _isShowingResult = false;
    }

    // Xóa 1 ký tự cuối cùng của số đang nhập.
    private void HandleBackspace()
    {
        if (_isWaitingForNewInput || IsDisplayError())
        {
            return;
        }

        if (ResultTextBlock.Text.Length <= 1)
        {
            ResultTextBlock.Text = "0";
            _isWaitingForNewInput = true;
            return;
        }

        ResultTextBlock.Text = ResultTextBlock.Text[..^1];

        if (ResultTextBlock.Text == "-" || ResultTextBlock.Text.Length == 0)
        {
            ResultTextBlock.Text = "0";
            _isWaitingForNewInput = true;
        }
    }

    // Đổi dấu số hiện tại (+/-).
    private void ToggleSign()
    {
        if (IsDisplayError())
        {
            ResetCalculator();
            return;
        }

        decimal currentValue = GetCurrentDisplayValue();

        if (currentValue == 0)
        {
            return;
        }

        ResultTextBlock.Text = FormatNumber(currentValue * -1);
        _isWaitingForNewInput = false;
    }

    // Đổi ký hiệu toán tử để hiển thị đẹp hơn trên UI.
    private static string GetOperatorSymbol(string mathOperator)
    {
        return mathOperator switch
        {
            "/" => "÷",
            "*" => "×",
            _ => mathOperator
        };
    }

    // Chuẩn hóa format số khi hiển thị.
    private static string FormatNumber(decimal number)
    {
        return number.ToString(CultureInfo.InvariantCulture);
    }

    // Kiểm tra màn hình đang hiển thị lỗi (không parse được về số).
    private bool IsDisplayError()
    {
        return !decimal.TryParse(ResultTextBlock.Text, NumberStyles.Number, CultureInfo.InvariantCulture, out _);
    }

    // Hiển thị lỗi và đưa app về trạng thái an toàn.
    private void ShowError(string errorMessage)
    {
        ExpressionTextBlock.Text = string.Empty;
        ResultTextBlock.Text = errorMessage;

        _firstOperand = null;
        _currentOperator = null;
        _isWaitingForNewInput = true;
        _isShowingResult = true;
    }
}