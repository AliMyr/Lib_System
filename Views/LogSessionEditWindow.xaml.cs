using System;
using System.Windows;
using Lib_System.Models;
using Lib_System.Services.Interfaces;

namespace Lib_System.Views
{
    public partial class LogSessionEditWindow : Window
    {
        private readonly ILogSessionService _svc;
        public LogSession Session { get; private set; }

        public LogSessionEditWindow(ILogSessionService svc, LogSession session = null)
        {
            InitializeComponent();
            _svc = svc;
            Session = session != null
                ? new LogSession
                {
                    Id = session.Id,
                    UserId = session.UserId,
                    Token = session.Token,
                    CreatedAt = session.CreatedAt,
                    ExpiresAt = session.ExpiresAt
                }
                : new LogSession { CreatedAt = DateTime.Now, ExpiresAt = DateTime.Now };
            UserBox.Text = Session.UserId?.ToString() ?? "";
            TokenBox.Text = Session.Token;
            CreatedBox.Text = Session.CreatedAt.ToString("yyyy-MM-dd HH:mm:ss");
            ExpiresBox.Text = Session.ExpiresAt.ToString("yyyy-MM-dd HH:mm:ss");
        }

        private void Ok_Click(object sender, RoutedEventArgs e)
        {
            Session.UserId = int.TryParse(UserBox.Text.Trim(), out var u) ? u : (int?)null;
            Session.Token = TokenBox.Text.Trim();
            Session.CreatedAt = DateTime.ParseExact(CreatedBox.Text.Trim(), "yyyy-MM-dd HH:mm:ss", null);
            Session.ExpiresAt = DateTime.ParseExact(ExpiresBox.Text.Trim(), "yyyy-MM-dd HH:mm:ss", null);
            if (Session.Id == 0) Session.Id = _svc.CreateSession(Session);
            else _svc.UpdateSession(Session);
            DialogResult = true;
        }

        private void Cancel_Click(object sender, RoutedEventArgs e) => DialogResult = false;
    }
}
