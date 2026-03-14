using System;
using System.Globalization;

namespace Calculator.Services;

// Lop CalculatorService chiu trach nhiem xu ly toan bo logic nghiep vu cua may tinh.
// Tac biet hoan toan khoi User Interface. Cung cap kha nang test va tai su dung cao.
public class CalculatorService
{
    // ==========================================
    // 1. BIEN TRANG THAI NHO (STATE VARIABLES)
    // ==========================================

    // Toan hang thu nhat dang cho (VD: 12 trong phep tinh 12 + 3).
    private decimal? _firstOperand;

    // Toan tu hien tai (VD: +, -, *, /).
    private string? _currentOperator;

    // True neu he thong dang cho ky tu moi (sau khi bam phep toan hoac dau bang).
    private bool _isWaitingForNewInput = true;

    // True neu dong hien thi chinh dang la ket qua cua phep tinh truoc do.
    private bool _isShowingResult;

    // ==========================================
    // 2. CAC THUOC TINH GIAO TIEP VOI VIEWMODEL
    // ==========================================

    // Dong van ban hien thi lo lon (ket qua hoac dang nhap dieu).
    public string DisplayText { get; private set; } = "0";

    // Dong van ban nho phia tren the hien bieu thuc dang tinh.
    public string ExpressionText { get; private set; } = string.Empty;

    // ==========================================
    // 3. XU LY THAO TAC NHAP LIEU CA CO BAN
    // ==========================================

    // Xu ly viec an cac phim tu 0 - 9.
    public void InputNumber(string digit)
    {
        if (IsError()) Reset();

        // Neu da co ket qua cua phep toan truoc roi ma chua bam phep tinh ke tiep, 
        // thi tu dong lam moi hoan toan khi nhap so moi.
        if (_isShowingResult && _currentOperator is null)
        {
            ExpressionText = string.Empty;
            _firstOperand = null;
            _isShowingResult = false;
        }

        if (_isWaitingForNewInput || DisplayText == "0")
            DisplayText = digit;
        else
            DisplayText += digit;

        _isWaitingForNewInput = false;
    }

    // Xu ly phim nhap ky tu cham '.' phan thap phan. Dam bao chi co 1 dau cham ton tai.
    public void InputDecimalPoint()
    {
        if (IsError()) Reset();

        if (_isWaitingForNewInput)
        {
            DisplayText = "0.";
            _isWaitingForNewInput = false;
            _isShowingResult = false;
            return;
        }

        if (!DisplayText.Contains('.'))
            DisplayText += ".";
    }

    // ==========================================
    // 4. XU LY PHEP TOAN & TINH TOAN LOGIC
    // ==========================================

    // Duoc goi khi nhan vao cac phep toan bo tro nhu '+, -, *, /'.
    public void InputOperator(string op)
    {
        if (IsError()) { Reset(); return; }

        try
        {
            decimal current = ParseDisplay();

            // Ho tro tinh chuoi lien tiep. (Vi du: nhap 1 + 2 + thi khi nhan dau + lan 2 se tinh ra 3 truoc).
            if (_firstOperand.HasValue && _currentOperator is not null && !_isWaitingForNewInput)
            {
                decimal chainedResult = Compute(_firstOperand.Value, current, _currentOperator);
                _firstOperand = chainedResult;
                DisplayText = Format(chainedResult);
            }
            else
            {
                _firstOperand = current;
            }

            _currentOperator = op;
            _isWaitingForNewInput = true;
            _isShowingResult = false;
            ExpressionText = $"{Format(_firstOperand.Value)} {ToSymbol(op)}";
        }
        catch (DivideByZeroException) { SetError("Khong the chia cho 0"); }
        catch { SetError("Loi phep tinh"); }
    }

    // Duoc goi khi nhan nut dau '=' de thuc hien phep tinh va ket thuc block hien tai.
    public void Calculate()
    {
        if (_firstOperand is null || _currentOperator is null || _isWaitingForNewInput || IsError())
            return;

        decimal second = ParseDisplay();

        try
        {
            decimal result = Compute(_firstOperand.Value, second, _currentOperator);
            ExpressionText = $"{Format(_firstOperand.Value)} {ToSymbol(_currentOperator)} {Format(second)} =";
            DisplayText = Format(result);

            _firstOperand = result;
            _currentOperator = null;
            _isWaitingForNewInput = true;
            _isShowingResult = true;
        }
        catch (DivideByZeroException) { SetError("Khong the chia cho 0"); }
        catch { SetError("Loi phep tinh"); }
    }

    // ==========================================
    // 5. XU LY THAO TAC DIEU KHIEN CHUC NANG
    // ==========================================

    // Xoa sach tat ca lich su hien tai, ke ve so 0.
    public void Reset()
    {
        _firstOperand = null;
        _currentOperator = null;
        _isWaitingForNewInput = true;
        _isShowingResult = false;
        DisplayText = "0";
        ExpressionText = string.Empty;
    }

    // Xoa ky tu hoac so vua nhap tren man hinh, KHONG bo phep tinh hay toan hang truoc.
    public void ClearEntry()
    {
        if (IsError()) { Reset(); return; }

        DisplayText = "0";
        _isWaitingForNewInput = true;
        _isShowingResult = false;
    }

    // Xoa dung mot ky tu cuoi cùng ra khoi man hinh hien thi. Tu dong handle dau '-'.

    public void Backspace()
    {
        if (_isWaitingForNewInput || IsError()) return;

        if (DisplayText.Length <= 1)
        {
            DisplayText = "0";
            _isWaitingForNewInput = true;
            return;
        }

        DisplayText = DisplayText[..^1]; // Loai bo 1 ky tu tu cuoi chuoi

        if (DisplayText == "-" || DisplayText.Length == 0)
        {
            DisplayText = "0";
            _isWaitingForNewInput = true;
        }
    }

    // Dao vi so am va so duong. Bo qua phan dao dau neu la so 0.
    public void ToggleSign()
    {
        if (IsError()) { Reset(); return; }

        decimal current = ParseDisplay();
        if (current == 0) return;

        DisplayText = Format(current * -1);
        _isWaitingForNewInput = false;
    }

    // ==========================================
    // 6. HAM TIEN ICH NOI BO (INTERNAL HELPERS)
    // ==========================================

    // Chua logic Toan hoc chinh.
    private static decimal Compute(decimal left, decimal right, string op)
    {
        return op switch
        {
            "+" => left + right,
            "-" => left - right,
            "*" => left * right,
            "/" when right == 0 => throw new DivideByZeroException(),
            "/" => left / right,
            _ => throw new InvalidOperationException("Toan tu khong hop le")
        };
    }

    // Dam bao Parse string xuong kieu Decimal tuong dong voi moi van hoa
    private decimal ParseDisplay()
    {
        if (!decimal.TryParse(DisplayText, NumberStyles.Number, CultureInfo.InvariantCulture, out decimal value))
            throw new FormatException("Gia tri hien thi khong hop le");

        return value;
    }

    // Dam bao ghi Decimal vao he thong String chinh xac.
    private static string Format(decimal number) =>
        number.ToString(CultureInfo.InvariantCulture);

    // Bien doi cac tu ky hieu thong thuong sang de nhin tren UI hon (nhan va chia).
    private static string ToSymbol(string op) => op switch
    {
        "/" => "÷",
        "*" => "×",
        _ => op
    };

    // Kiem tra thu tren do dang bao loi kieu chu (Vi du 'Cannot Divide By Zero')
    private bool IsError() =>
        !decimal.TryParse(DisplayText, NumberStyles.Number, CultureInfo.InvariantCulture, out _);

    // Kich hoat hien loi va Clear tat ca System
    private void SetError(string message)
    {
        ExpressionText = string.Empty;
        DisplayText = message;
        _firstOperand = null;
        _currentOperator = null;
        _isWaitingForNewInput = true;
        _isShowingResult = true;
    }
}
