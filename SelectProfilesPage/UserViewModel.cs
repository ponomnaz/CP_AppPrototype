using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CP_AppPrototype.Game;

namespace CP_AppPrototype.SelectProfilesPage
{
    /// <summary>
    /// ViewModel for handling user profile interactions such as saving, deleting, and validating profile data.
    /// Implements IQueryAttributable to handle query parameters for loading user data.
    /// </summary>
    public partial class UserViewModel : ObservableObject, IQueryAttributable
    {
        private User _user; // The user model associated with the profile

        [ObservableProperty]
        private bool notValidName = false; // Flag to track if the name is invalid

        [ObservableProperty]
        private string nameWarning = ""; // Warning message for invalid name input

        public string Name
        {
            get => _user.Name;
            set
            {
                if (_user.Name != value)
                {
                    _user.Name = value;
                    OnPropertyChanged(); // Notify property change for binding
                }
            }
        }

        public DateTime BirthDay
        {
            get => _user.BirthDay;
            set
            {
                if (_user.BirthDay != value)
                {
                    _user.BirthDay = value;
                    OnPropertyChanged();
                }
            }
        }

        public Gender Gender
        {
            get => _user.Gender;
            set
            {
                if (_user.Gender != value)
                {
                    _user.Gender = value;
                    OnPropertyChanged();
                }
            }
        }

        public DateTime ChangeDate
        {
            get => _user.ChangeDate;
            set
            {
                if (_user.ChangeDate != value)
                {
                    _user.ChangeDate = value;
                    OnPropertyChanged();
                }
            }
        }

        public string Identifier => _user.FileName;

        public UserViewModel()
        {
            _user = new User(); // Initializes with a new User instance
        }

        public UserViewModel(User user)
        {
            _user = user; // Initializes with an existing User instance
        }

        /// <summary>
        /// Command to save the user profile. Validates the name and updates the profile if valid.
        /// </summary>
        [RelayCommand]
        void Save()
        {
            // Validate the name input
            if (Name.Contains('|'))
            {
                notValidName = true;
                nameWarning = "Name must not contain |";
                OnPropertyChanged(nameof(NotValidName));
                OnPropertyChanged(nameof(NameWarning));
                return;
            }
            else if (string.IsNullOrWhiteSpace(Name))
            {
                notValidName = true;
                nameWarning = "Enter your name";
                OnPropertyChanged(nameof(NotValidName));
                OnPropertyChanged(nameof(NameWarning));
                return;
            }
            else if (Name.Contains(' '))
            {
                notValidName = true;
                nameWarning = "Name must not contain spaces";
                OnPropertyChanged(nameof(NotValidName));
                OnPropertyChanged(nameof(NameWarning));
                return;
            }
            else
            {
                notValidName = false;
                OnPropertyChanged(nameof(NotValidName));
            }

            // Update change date and save the profile
            _user.ChangeDate = DateTime.Now;
            _user.Save();
            Shell.Current.GoToAsync($"..?saved={_user.FileName}"); // Navigate with saved profile identifier
        }

        /// <summary>
        /// Command to delete the user profile.
        /// </summary>
        [RelayCommand]
        void Delete()
        {
            _user.Delete(); // Deletes the user file
            Shell.Current.GoToAsync($"..?deleted={_user.FileName}"); // Navigate with deleted profile identifier
        }

        /// <summary>
        /// Applies query attributes to load user data when navigating to this page.
        /// </summary>
        /// <param name="query">A dictionary of query parameters</param>
        void IQueryAttributable.ApplyQueryAttributes(IDictionary<string, object> query)
        {
            if (query.ContainsKey("load"))
            {
                _user = User.Load(query["load"].ToString()); // Load user from query parameter
                RefreshProperties();
            }
        }

        /// <summary>
        /// Reloads the current user data from storage.
        /// </summary>
        public void Reload()
        {
            _user = User.Load(_user.FileName); // Reload user profile from storage
            RefreshProperties();
        }

        /// <summary>
        /// Refreshes the properties to notify the UI about changes.
        /// </summary>
        private void RefreshProperties()
        {
            OnPropertyChanged(nameof(Name));
            OnPropertyChanged(nameof(BirthDay));
            OnPropertyChanged(nameof(Gender));
            OnPropertyChanged(nameof(ChangeDate));
        }

        /// <summary>
        /// Command to navigate to the game page.
        /// </summary>
        [RelayCommand]
        void Play()
        {
            Shell.Current.GoToAsync(nameof(GameView)); // Navigates to the game view
        }
    }
}
