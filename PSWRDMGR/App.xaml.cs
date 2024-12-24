using PSWRDMGR.Login;
using System;
using System.Security;
using System.Windows;
using PSWRDMGR.Properties;

namespace PSWRDMGR
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private void LoadTheme()
        {
            var themeName = Settings.Default.SelectedTheme;
            if (Enum.TryParse(themeName, out TheRThemes.ThemesController.ThemeTypes theme))
            {
                TheRThemes.ThemesController.SetTheme(theme);
            }
        }

        private void SaveTheme()
        {
            Settings.Default.SelectedTheme = TheRThemes.ThemesController.CurrentTheme.ToString();
            Settings.Default.Save();
        }

        public enum Theme
        {
            Light, ColourfulLight,
            Dark, ColourfulDark
        }

        private void SetThemeDictionary(ResourceDictionary value)
        {
            Resources.MergedDictionaries[0] = value;
        }

        private void ChangeTheme(Uri uri)
        {
            SetThemeDictionary(new ResourceDictionary() { Source = uri });
        }
        public void SetTheme(Theme theme)
        {
            string themeName = null;
            switch (theme)
            {
                case Theme.Dark: themeName = "DarkTheme"; break;
                case Theme.Light: themeName = "LightTheme"; break;
                case Theme.ColourfulDark: themeName = "ColourfulDarkTheme"; break;
                case Theme.ColourfulLight: themeName = "ColourfulLightTheme"; break;
            }

            try
            {
                if (!string.IsNullOrEmpty(themeName))
                    ChangeTheme(new Uri($"Themes/{themeName}.xaml", UriKind.Relative));
            }
            catch { }
        }

        private LoginWindow LoginWindow { get; set; }

        private void Application_Startup(object sender, StartupEventArgs e)
        {
            LoginWindow = new LoginWindow();
            LoginWindow.Login.TryLoginCallback = TryLogin;
            LoginWindow.Show();
        }

        public void TryLogin(SecureString pWrd)
        {
            // пароль це ім'я користувача
            if (pWrd.GetUnsecure() == Environment.UserName)
            {
                MainWindow mw = new MainWindow();
                mw.Show();
                MainWindow = mw;
                LoginWindow.Close();
                MainWindow = mw;
            }
            else
            {
                MessageBox.Show("Невірний пароль :(");
            }
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            LoadTheme();
        }

        protected override void OnExit(ExitEventArgs e)
        {
            base.OnExit(e);
            SaveTheme();
        }
    }
}
