using System.ComponentModel;

namespace WPF_Demo;

public class MainViewModel : INotifyPropertyChanged
{
    public event PropertyChangedEventHandler? PropertyChanged;


    private bool _isShimming = false;

    public bool IsShimming
    {
        get { return _isShimming; }
        set
        {
            if (_isShimming == value) return;
            _isShimming = value;
            OnPropertyChanged(nameof(IsShimming));
        }
    }


    protected void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
