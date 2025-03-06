using Microsoft.Maui.Controls;
using System.Diagnostics;

namespace CP_AppPrototype.Game;

public partial class GameView : ContentPage
{
    private int _activeMoveCount = 0;
         
    public GameView()
	{
        InitializeComponent();
    }

    private void OnPanUpdated(object sender, PanUpdatedEventArgs e)
    {
        var curframe = sender as Frame;
        var shapeModel = curframe?.BindingContext as ShapeModel;
        if (shapeModel != null)
        {
            switch (e.StatusType)
            {
                case GestureStatus.Started:
                    Debug.WriteLine("GestureStatus.Started");
                    _activeMoveCount++;
                    if (_activeMoveCount <= 2)
                    {
                        shapeModel.MoveStarted();
                    }
                    break;
                case GestureStatus.Running:
                    shapeModel.MoveRunning(e.TotalX / Content.Width, e.TotalY / Content.Height);
                    break;

                case GestureStatus.Completed:
                    Debug.WriteLine("GestureStatus.Completed");
                    var frames = cp.FindByName<AbsoluteLayout>("layout").Children.OfType<Frame>().ToList();
                    shapeModel.MoveEnded(Content.Width, Content.Height, curframe, frames);
                    _activeMoveCount--;
                    break;
            }
        }
    }
}