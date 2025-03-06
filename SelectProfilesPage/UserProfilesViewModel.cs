using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;

namespace CP_AppPrototype.SelectProfilesPage
{
    public partial class UserProfilesViewModel : ObservableObject, IQueryAttributable
    {
        // Observable collection to hold user profiles
        public ObservableCollection<UserViewModel> Profiles { get; set; }

        // Constructor initializes the profiles collection by loading all saved users
        public UserProfilesViewModel()
        {
            Profiles = new ObservableCollection<UserViewModel>(User.LoadAll().Select(n => new UserViewModel(n)));
        }

        // Command to create a new user profile
        [RelayCommand]
        void CreateProfile()
        {
            Shell.Current.GoToAsync(nameof(UserView));
        }

        // Command to select an existing profile and navigate to the UserView
        [RelayCommand]
        void SelectProfile(UserViewModel user)
        {
            if (user != null)
                Shell.Current.GoToAsync($"{nameof(UserView)}?load={user.Identifier}");
        }

        // Handles query parameters passed during navigation (such as saving or deleting a profile)
        void IQueryAttributable.ApplyQueryAttributes(IDictionary<string, object> query)
        {
            if (query.ContainsKey("deleted"))
            {
                string userId = query["deleted"].ToString();

                // Use LINQ to find the profile and remove it from the collection
                var matchedUser = Profiles.FirstOrDefault(n => n.Identifier == userId);

                if (matchedUser != null)
                    Profiles.Remove(matchedUser);
            }
            else if (query.ContainsKey("saved"))
            {
                string userId = query["saved"].ToString();

                // Find the existing user by identifier
                var matchedUser = Profiles.FirstOrDefault(n => n.Identifier == userId);

                if (matchedUser != null)
                {
                    // If the user already exists, reload and move it to the top of the collection
                    matchedUser.Reload();
                    Profiles.Move(Profiles.IndexOf(matchedUser), 0);
                }
                else
                {
                    // If the user is new, add it to the collection
                    try
                    {
                        var newUser = new UserViewModel(User.Load(userId));
                        Profiles.Insert(0, newUser);
                    }
                    catch (Exception ex)
                    {
                        // Handle errors, such as if the user can't be loaded
                        // You can log the exception or show an error message
                        System.Diagnostics.Debug.WriteLine($"Error loading user: {ex.Message}");
                    }
                }
            }
        }
    }
}
