using System;
using System.Windows;
using Lib_System.Models;
using Lib_System.Services.Interfaces;

namespace Lib_System.Views
{
    public partial class LogAuditEditWindow : Window
    {
        private readonly ILogAuditService _svc;
        public LogAudit Audit { get; private set; }

        public LogAuditEditWindow(ILogAuditService svc, LogAudit audit = null)
        {
            InitializeComponent();
            _svc = svc;
            Audit = audit != null
                ? new LogAudit { Id = audit.Id, UserId = audit.UserId, Action = audit.Action, CreatedAt = audit.CreatedAt }
                : new LogAudit { CreatedAt = DateTime.Now };
            UserBox.Text = Audit.UserId?.ToString() ?? "";
            ActionBox.Text = Audit.Action;
            CreatedBox.Text = Audit.CreatedAt.ToString("yyyy-MM-dd HH:mm:ss");
        }

        private void Ok_Click(object sender, RoutedEventArgs e)
        {
            Audit.UserId = int.TryParse(UserBox.Text.Trim(), out var u) ? u : (int?)null;
            Audit.Action = ActionBox.Text.Trim();
            Audit.CreatedAt = DateTime.ParseExact(CreatedBox.Text.Trim(), "yyyy-MM-dd HH:mm:ss", null);
            if (Audit.Id == 0) Audit.Id = _svc.CreateAudit(Audit);
            else _svc.UpdateAudit(Audit);
            DialogResult = true;
        }

        private void Cancel_Click(object sender, RoutedEventArgs e) => DialogResult = false;
    }
}
