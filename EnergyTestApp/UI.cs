namespace EnergyTestApp
{
    using System.Windows;

    internal static class UI
    {
        internal static void DoEvents()
        {
            Application.Current?.Dispatcher?.Invoke(() => { }, System.Windows.Threading.DispatcherPriority.Background);
        }
    }
}
