﻿using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;
using System.Windows.Input;
using WuHu.BL.Impl;

namespace WuHu.Terminal.ViewModels
{
    internal class LoginVm : BaseVm
    {
        private string _username;
        private readonly Action _onLoginAction;

        public ICommand LoginCommand { get; private set; }

        public LoginVm()
        {
            LoginCommand = new RelayCommand(Login);
        }

        public LoginVm(Action onLogin)
        {
            _onLoginAction = onLogin;
            LoginCommand = new RelayCommand(Login);
        }


        public string Username
        {
            get { return _username; }
            set
            {
                if (value != _username)
                {
                    _username = value;
                    OnPropertyChanged(this);
                }
            }
        }

        private void Login(object param)
        {
            var pwBox = param as PasswordBox;
            // don't save password in memory, just send it to the Manager right away
            if (pwBox == null) return;

            var success = Manager.Login(Username, pwBox.Password);
            pwBox.Password = null;

            if (success)
            {
                OnAuthenticatedChanged(this);
                _onLoginAction?.Invoke();
            }
        }
    }
}