using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CP_AppPrototype.SelectProfilesPage;

namespace CP_AppPrototype.MainMenu
{
    /// <summary>
    /// ViewModel for the Main Menu page.
    /// Handles navigation and application exit functionality.
    /// </summary>
    public partial class MainMenuViewModel : ObservableObject
    {
        /// <summary>
        /// Closes the application. 
        /// Uses different methods based on the runtime platform.
        /// </summary>
        [RelayCommand]
        void Exit()
        {
            if (Device.RuntimePlatform == Device.Android)
            {
                Application.Current.Quit();
            }
            else if (Device.RuntimePlatform == Device.WinUI)
            {
                Application.Current.CloseWindow(Application.Current.MainPage.Window);
            }
        }

        /// <summary>
        /// Navigates to the User Profiles selection page.
        /// </summary>
        [RelayCommand]
        void SelectProfile()
        {
            Shell.Current.GoToAsync(nameof(UserProfilesView));
        }

    }
}
