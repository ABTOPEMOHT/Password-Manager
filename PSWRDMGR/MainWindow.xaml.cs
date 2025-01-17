﻿using TheRThemes;
using PSWRDMGR.ViewModels;
using PSWRDMGR.Views;
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
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using static PSWRDMGR.App;
using static TheRThemes.ThemesController;

namespace PSWRDMGR
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainViewModel ViewModel { get; set; }
        public MainWindow()
        {
            InitializeComponent();
            ViewModel = new MainViewModel();
            this.DataContext = ViewModel;
            ViewModel.ScrollIntoView = ScrollIntoViewThingy;
            ViewModel.ShowContentPanelCallback = this.ShowContentPanel;
            ViewModel.HideContentPanelCallback = this.HideContentPanel;
            HideContentPanel();
            LoadSettings();
        }

        public void SetViewModelVariables()
        {
        }

        public void ScrollIntoViewThingy()
        {
            lBox.ScrollIntoView(lBox.SelectedItem);
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            ViewModel.KeyDown(e.Key, true);
        }

        private void Window_KeyUp(object sender, KeyEventArgs e)
        {
            ViewModel.KeyDown(e.Key, false);

        }

        public void AnimateContentPanelWidth(double oldWidth, double newWidth)
        {
            DoubleAnimation da = new DoubleAnimation(oldWidth, newWidth, TimeSpan.FromSeconds(0.2))
            {
                AccelerationRatio = 0,
                DecelerationRatio = 1,
            };
            AccountPanel.BeginAnimation(WidthProperty, da);
        }

        public void ShowContentPanel()
        {
            AnimateContentPanelWidth(0, 450);
        }

        public void HideContentPanel()
        {
            AnimateContentPanelWidth(450, 0);
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            //e.Cancel = true;
            if (ViewModel.AutosaveWhenClosing)
                ViewModel.SaveAccounts();
            else
            {
                MessageBoxResult results = 
                    MessageBox.Show(
                        "Зберегти всі акаунти в списку за замовчуванням?",
                        "Зберегти акаунти в місце за замовчуванням?",
                        MessageBoxButton.YesNoCancel, MessageBoxImage.Warning);
                if (results == MessageBoxResult.Yes)
                {
                    ViewModel.SaveAccounts();
                }
                else if (results == MessageBoxResult.Cancel)
                {
                    e.Cancel = true;
                    return;
                }
            }
            ViewModel.CloseAllWindows();
            SaveSettings();
        }

        public void SaveSettings()
        {
            if (WindowState == WindowState.Maximized)
            {
                Properties.Settings.Default.Top = RestoreBounds.Top;
                Properties.Settings.Default.Left = RestoreBounds.Left;
                Properties.Settings.Default.Height = RestoreBounds.Height;
                Properties.Settings.Default.Width = RestoreBounds.Width;
                Properties.Settings.Default.Maximized = true;
            }
            else
            {
                Properties.Settings.Default.Top = this.Top;
                Properties.Settings.Default.Left = this.Left;
                Properties.Settings.Default.Height = this.Height;
                Properties.Settings.Default.Width = this.Width;
                Properties.Settings.Default.Maximized = false;
            }
            Properties.Settings.Default.Save();
        }

        public void LoadSettings()
        {
            if (Properties.Settings.Default.Height > 0 && Properties.Settings.Default.Width > 0)
            {
                this.Top = Properties.Settings.Default.Top;
                this.Left = Properties.Settings.Default.Left;
                this.Height = Properties.Settings.Default.Height;
                this.Width = Properties.Settings.Default.Width;
            }
            else
            {
                this.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                this.Height = 720;
                this.Width = 1280;
            }

            if (Properties.Settings.Default.Maximized)
            {
                WindowState = WindowState.Maximized;
            }
        }

        private void ChangeTheme(object sender, RoutedEventArgs e)
        {
            switch (int.Parse(((MenuItem)sender).Uid))
            {
                case 0: SetTheme(ThemeTypes.Light); break;
                case 2: SetTheme(ThemeTypes.Dark); break;
            }
            e.Handled = true;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            if (this.Width < 800 || this.Height < 600)
            {
                this.Width = 1880;
                this.Height = 720;
            }
        }
    }
}