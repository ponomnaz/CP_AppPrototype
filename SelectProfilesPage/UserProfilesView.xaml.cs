namespace CP_AppPrototype.SelectProfilesPage;

public partial class UserProfilesView : ContentPage
{
	public UserProfilesView()
	{
		InitializeComponent();
	}

    private void ContentPage_NavigatedTo(object sender, NavigatedToEventArgs e)
    {
        profilesCollection.SelectedItem = null;
    }
}