using System;
using System.ComponentModel;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApplication1
{
    public partial class BackgroundWorkerTestForm : Form
    {
        private BackgroundWorker backgroundWorker;

        public BackgroundWorkerTestForm()
        {
            InitializeComponent();

            Disposed += OnDisposed;
        }

        private void OnDisposed(object sender, EventArgs eventArgs)
        {
            var threadId = Thread.CurrentThread.ManagedThreadId;
            Console.WriteLine("{0}: disposed", threadId);

            if (backgroundWorker != null)
            {
                backgroundWorker.CancelAsync();
                backgroundWorker.Dispose();
            }

            Disposed -= OnDisposed;
        }

        private void BackgroundWorkerOnDoWork(object sender, DoWorkEventArgs doWorkEventArgs)
        {
            var threadId = Thread.CurrentThread.ManagedThreadId;
            var i = 0;
            while (!backgroundWorker.CancellationPending && i++ < 4)
            {
                Console.WriteLine("{0}: before wait {1}", threadId, i);
                Thread.Sleep(5 * 1000);
                Console.WriteLine("{0}: after wait", threadId);
            }

            if (backgroundWorker.CancellationPending)
            {
                Console.WriteLine("{0}: Cancellation requested", threadId);
            }

            Console.WriteLine("{0}: finished", threadId);
        }

        private void BackgroundWorkerOnRunWorkerCompleted(
            object sender,
            RunWorkerCompletedEventArgs runWorkerCompletedEventArgs)
        {
            var threadId = Thread.CurrentThread.ManagedThreadId;
            Console.WriteLine("{0}: completed", threadId);
        }

        private async void button1_Click(object sender, EventArgs e)
        {
            var threadId = Thread.CurrentThread.ManagedThreadId;

            if (backgroundWorker == null)
            {
                Console.WriteLine("{0}: constructing", threadId);

                backgroundWorker = new BackgroundWorker
                {
                    WorkerSupportsCancellation = true
                };
                backgroundWorker.DoWork += BackgroundWorkerOnDoWork;
                backgroundWorker.RunWorkerCompleted += BackgroundWorkerOnRunWorkerCompleted;
            }

            var waitCount = 0;
            while (backgroundWorker.IsBusy && waitCount++ < 10)
            {
                Console.WriteLine("{0}: previous worker is busy. cancellation request sent.", threadId);
                backgroundWorker.CancelAsync();
                await Task.Delay(1 * 1000);
            }

            if (backgroundWorker.IsBusy)
            {
                Console.WriteLine("{0}: cannot cancel previous worker. new worker not started", threadId);
                return;
            }

            backgroundWorker.RunWorkerAsync();
            Console.WriteLine("{0}: worker started", threadId);
        }
    }
}
