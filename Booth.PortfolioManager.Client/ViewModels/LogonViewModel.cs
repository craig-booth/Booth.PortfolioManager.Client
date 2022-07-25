using System;

using Booth.WpfControls;
using Booth.PortfolioManager.RestApi.Client;
using Booth.PortfolioManager.Client.Utilities;

namespace Booth.PortfolioManager.Client.ViewModels
{
    class LogonViewModel : PopupWindow
    {
        private readonly RestClient _RestClient;
        public string UserName { get; set; }
        public string Password { get; set; }

        private bool _LogonFailed;
        public bool LogonFailed
        {
            get => _LogonFailed;
            set
            {
                if (_LogonFailed != value)
                {
                    _LogonFailed = value;
                    OnPropertyChanged();
                }
            }
        }
        private string _ErrorMessage;
        public string ErrorMessage
        {
            get => _ErrorMessage;
            set
            {
                if (_ErrorMessage != value)
                {
                    _ErrorMessage = value;
                    OnPropertyChanged();
                }
            }
        }

        public LogonViewModel(RestClient restClient)
        {
            _RestClient = restClient;
            LogonCommand = new RelayCommand(Logon);
            CancelCommand = new RelayCommand(Cancel);

            Commands.Add(new DialogCommand("Logon", LogonCommand) { IsDefault = true });
            Commands.Add(new DialogCommand("Cancel", CancelCommand) { IsCancel = true });

            LogonFailed = false;
            ErrorMessage = "";
        }

        public RelayCommand LogonCommand { get; private set; }
        private async void Logon()
        {
            try
            {
                LogonFailed = false;
                ErrorMessage = "";

                await _RestClient.Authenticate(UserName, Password);

                _RestClient.SetPortfolio(new Guid("5D5DE669-726C-4C5D-BB2E-6520C924DB90"));
                Close();
                OnLoggedOn(EventArgs.Empty);
            }
            catch (Exception e)
            {
                ErrorMessage = e.Message;
                LogonFailed = true;
            }
        }

        public RelayCommand CancelCommand { get; private set; }
        private void Cancel()
        {
            Close();
        }

        public event EventHandler LoggedOn;

        private void OnLoggedOn(EventArgs e)
        {
            LoggedOn?.Invoke(this, e);
        }
    }
}
