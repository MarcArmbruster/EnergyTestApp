namespace EnergyTestApp
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Diagnostics;
    using System.Windows;
    using WpfCommandAggregator.Core;

    public class VmMain : BaseVm
    {
        private readonly TestMode Mode = TestMode.Smart;

        public bool IsRunning 
        {
            get => this.GetPropertyValue<bool>(); 
            set => this.SetPropertyValue<bool>(value); 
        }

        public string Info
        {
            get => this.GetPropertyValue<string>();
            set => this.SetPropertyValue<string>(value);
        }

        public ObservableCollection<DataItem> Data { get; } = new ObservableCollection<DataItem>();
        public ObservableCollectionExt<DataItem> SmartData { get; } = new ObservableCollectionExt<DataItem>();

        protected override void InitCommands()
        {
            this.CmdAgg.AddOrSetCommand("RunCommand", p1 => this.Run(), p2 => !this.IsRunning);
        }

        private void Run()
        {
            this.IsRunning = true;
            Stopwatch watch = new();
            watch.Start();
            Debug.WriteLine($"STARTED: {DateTime.Now}");

            List<DataItem> items = GetTestData();

            int outerLoopSize = 1000;
            for (int i = 0; i < outerLoopSize; i++)
            {
                this.ClearDataCollection();
                this.LogOuterLoopState(outerLoopSize, i);

                // Default collection
                if (this.Mode == TestMode.Default)
                {
                    foreach (var item in items)
                    {
                        this.Data.Add(item);
                    }
                }

                // optimized collection
                if (this.Mode == TestMode.Smart)
                {
                    this.SmartData.AddRange(items);
                }
            }

            this.LogEndOfTestRun(watch);

            // Quit application to end energy consumption measurement of Windwos E3 component.
            Application.Current.Shutdown(0);
        }

        private void ClearDataCollection()
        {
            // Clear collection after each loop
            this.Data.Clear();
            this.SmartData.Clear();
        }

        private void LogEndOfTestRun(Stopwatch watch)
        {
            // Logging (UI & Debug Output Trace)
            watch.Stop();
            this.Info += $"\telapsed milli seconds: {watch.Elapsed.TotalMilliseconds:N0}";
            Debug.WriteLine($"ENDED: {DateTime.Now}");
            Debug.WriteLine($"ELAPSED TIME (ms): {watch.Elapsed.TotalMilliseconds}");
        }

        private void LogOuterLoopState(int outerLoop, int i)
        {
            // Logging (UI & Debug Output Trace)
            this.Info = $"processing {i + 1}/{outerLoop}";
            UI.DoEvents();
            Debug.WriteLine($"Loop: {i + 1}");
        }

        private static List<DataItem> GetTestData()
        {
            // Prepare data
            var items = new List<DataItem>();
            for (int r = 0; r < 1000; r++)
            {
                items.Add(new DataItem() { Id = r + 1, Name = $"Item {r + 1}", Description = $"Description text for {r + 1}" });
            }

            return items;
        }
    }
}