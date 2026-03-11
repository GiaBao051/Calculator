using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using Calculator.Commands;
using Calculator.Services;

namespace Calculator.ViewModels;

// CalculatorViewModel là c?u n?i gi?a View (XAML) và Service (logic).
//
// Quy t?c MVVM:
//   - View KHÔNG bi?t ViewModel làm gì bên trong.
//   - ViewModel KHÔNG bi?t View trông nh? th? nào.
//   - Hai bên nói chuy?n v?i nhau qua DataBinding và Command.
public class CalculatorViewModel : INotifyPropertyChanged
{
    // ====== Service ======

    // CalculatorService x? lý toàn b? logic tính toán.
    // ViewModel ch? g?i vào Service và c?p nh?t l?i property ?? UI t? c?p nh?t.
    private readonly CalculatorService _service = new();

    // ====== Properties binding ra View ======

    // Dòng k?t qu? l?n (s? ?ang nh?p ho?c k?t qu? phép tính).
    private string _displayText = "0";
    public string DisplayText
    {
        get => _displayText;
        private set { _displayText = value; OnPropertyChanged(); }
    }

    // Dòng bi?u th?c nh? phía trên (ví d?: "12 +" ho?c "12 + 3 =").
    private string _expressionText = string.Empty;
    public string ExpressionText
    {
        get => _expressionText;
        private set { _expressionText = value; OnPropertyChanged(); }
    }

    // ====== Commands g?n vào t?ng nút b?m ======

    // Nút s? (0–9): nh?n tham s? là chu?i ch? s?.
    public ICommand NumberCommand { get; }

    // Nút d?u ch?m th?p phân '.'.
    public ICommand DecimalCommand { get; }

    // Nút toán t? (+, -, *, /): nh?n tham s? là ký hi?u toán t?.
    public ICommand OperatorCommand { get; }

    // Nút '=' ?? tính k?t qu?.
    public ICommand EqualCommand { get; }

    // Nút C: reset toàn b?.
    public ICommand ResetCommand { get; }

    // Nút CE: ch? xóa s? ?ang nh?p.
    public ICommand ClearEntryCommand { get; }

    // Nút ?: xóa 1 ký t? cu?i.
    public ICommand BackspaceCommand { get; }

    // Nút +/?: ??i d?u s? hi?n t?i.
    public ICommand ToggleSignCommand { get; }

    public CalculatorViewModel()
    {
        // M?i Command nh?n m?t Action (hàm s? ch?y khi nút ???c b?m).
        // RelayCommand<string> cho phép nh?n tham s? t? XAML qua CommandParameter.
        NumberCommand = new RelayCommand<string>(OnNumberPressed);
        DecimalCommand = new RelayCommand(OnDecimalPressed);
        OperatorCommand = new RelayCommand<string>(OnOperatorPressed);
        EqualCommand = new RelayCommand(OnEqualPressed);
        ResetCommand = new RelayCommand(OnResetPressed);
        ClearEntryCommand = new RelayCommand(OnClearEntryPressed);
        BackspaceCommand = new RelayCommand(OnBackspacePressed);
        ToggleSignCommand = new RelayCommand(OnToggleSignPressed);
    }

    // ====== X? lý t?ng nút b?m ======
    // M?i hàm ch? làm 2 vi?c:
    //   1. G?i Service x? lý logic.
    //   2. C?p nh?t l?i property ?? View t? c?p nh?t qua DataBinding.

    private void OnNumberPressed(string digit)
    {
        _service.InputNumber(digit);
        SyncDisplay();
    }

    private void OnDecimalPressed()
    {
        _service.InputDecimalPoint();
        SyncDisplay();
    }

    private void OnOperatorPressed(string op)
    {
        _service.InputOperator(op);
        SyncDisplay();
    }

    private void OnEqualPressed()
    {
        _service.Calculate();
        SyncDisplay();
    }

    private void OnResetPressed()
    {
        _service.Reset();
        SyncDisplay();
    }

    private void OnClearEntryPressed()
    {
        _service.ClearEntry();
        SyncDisplay();
    }

    private void OnBackspacePressed()
    {
        _service.Backspace();
        SyncDisplay();
    }

    private void OnToggleSignPressed()
    {
        _service.ToggleSign();
        SyncDisplay();
    }

    // ??ng b? d? li?u t? Service vào property c?a ViewModel.
    // View s? t? c?p nh?t vì INotifyPropertyChanged.
    private void SyncDisplay()
    {
        DisplayText = _service.DisplayText;
        ExpressionText = _service.ExpressionText;
    }

    // ====== INotifyPropertyChanged ======
    // C? ch? thông báo cho View bi?t khi property thay ??i.
    // [CallerMemberName] t? ?i?n tên property ?ang g?i, không c?n vi?t th? công.

    public event PropertyChangedEventHandler? PropertyChanged;

    protected void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
