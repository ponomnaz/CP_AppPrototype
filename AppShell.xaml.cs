namespace CP_AppPrototype
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();
            Routing.RegisterRoute(nameof(SelectProfilesPage.UserProfilesView), typeof(SelectProfilesPage.UserProfilesView));
            Routing.RegisterRoute(nameof(SelectProfilesPage.UserView), typeof(SelectProfilesPage.UserView));
            Routing.RegisterRoute(nameof(Game.GameView), typeof(Game.GameView));
        }
    }
}
