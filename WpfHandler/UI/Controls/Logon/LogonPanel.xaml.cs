﻿//Copyright 2019 Volodymyr Podshyvalov
//
//Licensed under the Apache License, Version 2.0 (the "License");
//you may not use this file except in compliance with the License.
//You may obtain a copy of the License at
//
//    http://www.apache.org/licenses/LICENSE-2.0
//
//Unless required by applicable law or agreed to in writing, software
//distributed under the License is distributed on an "AS IS" BASIS,
//WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//See the License for the specific language governing permissions and
//limitations under the License.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WpfHandler.UI.Controls.Logon
{
    /// <summary>
    /// Interaction logic for LogonPanel.xaml
    /// </summary>
    public partial class LogonPanel : UserControl
    {
        /// <summary>
        /// Property that bridging control's property between XAML and code.
        /// </summary>
        public static readonly RoutedEvent LoginClickEvent = EventManager.RegisterRoutedEvent("LoginCallback",
            RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(LogonPanel));

        /// <summary>
        /// Property that bridging control's property between XAML and code.
        /// </summary>
        public static readonly RoutedEvent SingUpClickEvent = EventManager.RegisterRoutedEvent("SingUpCallback",
            RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(LogonPanel));

        /// <summary>
        /// Method that will has been calling during click on button.
        /// </summary>
        public event RoutedEventHandler SingUpCallback
        {
            add => base.AddHandler(SingUpClickEvent, value); 
            remove => base.RemoveHandler(SingUpClickEvent, value); 
        }

        /// <summary>
        /// Method that will has been calling during click on button.
        /// </summary>
        public event RoutedEventHandler LoginCallback
        {
            add => base.AddHandler(LoginClickEvent, value);
            remove => base.RemoveHandler(LoginClickEvent, value);
        }

        /// <summary>
        /// Margine of internal form.
        /// </summary>
        public Thickness LogonFormMargin
        {
            get
            {
                return new Thickness(0, 0, 0, ActualHeight / 2 - logonPanel_FormBlock.ActualHeight / 2);
            }
        }
        
        /// <summary>
        /// Entered login value.
        /// </summary>
        public string Login
        {
            get
            {
                return loginField.Text;
            }
            set
            {
                loginField.Text = value;
            }
        }

        /// <summary>
        /// Entered password value.
        /// </summary>
        public string Password
        {
            get
            {
                return passwordField.Text;
            }
            set
            {
                passwordField.Text = value;
            }
        }

        /// <summary>
        /// Error message tha twould displayed at UI's label.
        /// </summary>
        public string ErrorMessage
        {
            set
            {
                // Hide message if null.
                if(value == null)
                {
                    errorMessage.Visibility = Visibility.Collapsed;
                    return;
                }

                // Show message.
                errorMessage.Visibility = Visibility.Visible;
                errorMessage.Content = value;
            }
        }

        #region Constructor\destructor
        /// <summary>
        /// Default constructor.
        /// </summary>
        public LogonPanel()
        {
            InitializeComponent();
            DataContext = this;

            SizeChanged += MainWindow_SizeChanged;
        }

        /// <summary>
        /// Releasing an unmanaged memory.
        /// </summary>
        ~LogonPanel()
        {
            // Unsubscribe from events.
            SizeChanged -= MainWindow_SizeChanged;
        }
        #endregion

        /// <summary>
        /// Clear all entered data from forms.
        /// </summary>
        public void Clear()
        {
            Login = "";
            Password = "";
            ErrorMessage = null;
        }

        #region Callbacks
        /// <summary>
        /// Callback that will has been calling when widow size will be changed.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MainWindow_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            // Update size of control panel.
            BindingOperations.GetBindingExpression(logonPanel_FormBlock, MarginProperty).UpdateTarget();
        }

        /// <summary>
        /// Callback that will caling when panel will loaded.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void LogonPanel_Loaded(object sender, RoutedEventArgs e)
        {
            // Call recomputing of size.
            MainWindow_SizeChanged(sender, null);
        }
        #endregion
    }
}
