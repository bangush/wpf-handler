﻿using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

using System.Collections.Generic;
using System.Windows.Controls;
using WpfHandler.UI.AutoLayout.Configuration;
using WpfHandler.UI.AutoLayout.Controls;
using WpfHandler.UI.AutoLayout.Options;
using WpfHandler.UI.AutoLayout;

namespace ExamplePanelDescriptor
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        /// <summary>
        /// Instance of the current active main window.
        /// </summary>
        public static MainWindow Active { get; protected set; }

        /// <summary>
        /// Brush that applied for the window on the lunch time.
        /// </summary>
        public Brush DefaultBrush { get; protected set; }

        public MainWindow()
        {
            // Set widows as active.
            Active = this;

            InitializeComponent();

            // Buferizing the current background as default.
            DefaultBrush = Background;

            // Instiniating the descriptor.
            var descriptor = new ExampleDescriptor();

            // Binding the descriptor to the auto layout view from the XAML.
            alView.OnLayout(descriptor);
        }
    }

    /// <summary>
    /// Class descriptor.
    /// </summary>
    [Foreground("WhiteSmoke")]
    public class ExampleDescriptor : UIDescriptor
    {
        // The enum that will be used as the source for the toggle element.
        public enum SwitcherState
        {
            On,
            Off
        }

        // Prperty that will attached to the UI like the string field.
        [Order(100)]
        public string stringField = "sample text";

        // Prperty that will attached to the UI like the int field.
        [Order(200)]
        public int IntProperty { get; set; } = 0;

        // Adding the space from the previous element.
        [Space(30)]
        // The member that will attached to the GUI.
        [Content("Highlight")]
        // Moving control panel to the center.
        [HorizontalAlign(HorizontalAlignment.Center)]
        public SwitcherState state = SwitcherState.Off;

        [Space(30)]
        // Defining the default title for header.
        // Adding `pi_loc` as the localization key for that header.
        [Header("Default Personal info", null, "pi_loc")]
        public string firstName = "";
        public string lastName = "";

        // Set the end of the group started by previous header.
        // In case of skip the End the next header and its relative
        // elements will be added as subgroup of the previous header.
        [EndGroup]
        [Header("Advanced", null, "adv_lov")]
        public string city;
        public string country;

        [EndGroup]
        [Header("Buttons")]
        [BeginHorizontalGroup]
        [HorizontalAlign(HorizontalAlignment.Center)]
        [Background("Orange")]
        [Foreground("Black")]
        [FontWeight(FontWeightAttribute.WeightType.Bold)]
        // Source for the button.
        public Action sampleActionHandler = delegate() { MessageBox.Show("Simple action activated."); };

        // Source for button handled by routed event.
        [HorizontalAlign(HorizontalAlignment.Center)]
        public RoutedEventHandler routedEventHandler  = 
            delegate (object sednder, RoutedEventArgs args) 
            { MessageBox.Show("Routed event handled."); };

        // Callback that occurs when the UI is ready.
        public override void OnLoaded()
        {
            // Receiving the IGUIField generated from the state member.
            var stateField = GetField("state");

            // Subscribing on the state value changes.
            stateField.ValueChanged += delegate (IGUIField objw, object[] _)
            {
                SwitcherState state = (SwitcherState)objw.Value;

                switch (state)
                {
                    case SwitcherState.On:
                        MainWindow.Active.Background = Brushes.DarkSlateBlue;
                        break;
                    case SwitcherState.Off:
                        MainWindow.Active.Background = MainWindow.Active.DefaultBrush;
                        break;
                }
            };
        }
    }
}
