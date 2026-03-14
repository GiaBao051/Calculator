using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using Calculator.Commands;
using Calculator.Services;

namespace Calculator.ViewModels;

// Doi tuong CalculatorViewModel la tang trung gian de the hien du lieu cho UI.
// Theo chuan MVVM: DataBinding giao tiep tu dong ma khong can Code-Behind biet cac dieu khien ben trong.
public class CalculatorViewModel : INotifyPropertyChanged
{
    // ==========================================
    // 1. DICH VU (SERVICE)
    // ==========================================

    // Chua toan bo logic. Tang nay chi can goi va dong bo len property.
    private readonly CalculatorService _service = new();

    // ==========================================
    // 2. THUOC TINH RANG BUOC GIAO DIEN (PROPERTIES)
    // ==========================================

    // Dong thong bao ket qua (Chu so lon, dau ra ket qua).
    private string _displayText = "0";
    public string DisplayText
    {
        get => _displayText;
        private set { _displayText = value; OnPropertyChanged(); }
    }

    // Dong thong bao bieu thuc hien tai (Chu so nho o tren, bieu thuc '12 +').
    private string _expressionText = string.Empty;
    public string ExpressionText
    {
        get => _expressionText;
        private set { _expressionText = value; OnPropertyChanged(); }
    }

    // ==========================================
    // 3. DANH SACH LENH THUC THI (COMMANDS)
    // ==========================================

    public ICommand NumberCommand { get; }
    public ICommand DecimalCommand { get; }
    public ICommand OperatorCommand { get; }
    public ICommand EqualCommand { get; }
    public ICommand ResetCommand { get; }
    public ICommand ClearEntryCommand { get; }
    public ICommand BackspaceCommand { get; }
    public ICommand ToggleSignCommand { get; }

    public CalculatorViewModel()
    {
        // Khoi tao cac lenh mapping voi cac ham dieu kien.
        // Nhom yeu cau tham so dau vao.
        NumberCommand = new RelayCommand<string>(OnNumberPressed);
        OperatorCommand = new RelayCommand<string>(OnOperatorPressed);

        // Nhom khong yeu cau tham so dau vao.
        DecimalCommand = new RelayCommand(OnDecimalPressed);
        EqualCommand = new RelayCommand(OnEqualPressed);
        ResetCommand = new RelayCommand(OnResetPressed);
        ClearEntryCommand = new RelayCommand(OnClearEntryPressed);
        BackspaceCommand = new RelayCommand(OnBackspacePressed);
        ToggleSignCommand = new RelayCommand(OnToggleSignPressed);
    }

    // ==========================================
    // 4. HAM XU LY DIEU KHIEN (ACTION HANDLERS)
    // ==========================================

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

    // ==========================================
    // 5. CA CAP NHAT VA DONG BO (SYNCHRONIZATION)
    // ==========================================

    // Dong bo lai toan bo du lieu that tren UI theo xu ly cua Service.
    private void SyncDisplay()
    {
        DisplayText = _service.DisplayText;
        ExpressionText = _service.ExpressionText;
    }

    // ==========================================
    // 6. TINH NANG THONG BAO THAY DOI EVENT PROPERTY
    // ==========================================

    public event PropertyChangedEventHandler? PropertyChanged;

    // CallerMemberName tu dong nhan ten thuoc tinh thay doi.
    protected void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
