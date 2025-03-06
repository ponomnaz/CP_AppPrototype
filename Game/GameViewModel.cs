using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;

namespace CP_AppPrototype.Game
{
    public partial class GameViewModel : ObservableObject
    {
        public ObservableCollection<ShapeModel> Shapes { get; set; }

        private readonly Random _random = new();


        private bool _timerRunning;
        private int matches = 0;
        private int curShapesNum = 0;

        [ObservableProperty]
        private string strMatches;

        [ObservableProperty]
        private string strCurShapesNum;

        [ObservableProperty]
        private bool gameIsOver;

        [ObservableProperty]
        private string strEnd;

        private const int _initialShapeCount = 5;

        private const int _shapeSize = 100;

        private const int targetMatches = 20;

        private const int maxShapesNum = 10;

        private const int _creationInterval = 2200;

        public GameViewModel()
        {
            UpdateUI();
            Shapes = new ObservableCollection<ShapeModel>();
            CreateRandomShapes(_initialShapeCount, _shapeSize);
            StartShapeCreationTimer();
        }

        /// <summary>
        /// Starts a timer to create shapes periodically.
        /// </summary>
        private void StartShapeCreationTimer()
        {
            if (gameIsOver || _timerRunning) return;

            _timerRunning = true;

            Device.StartTimer(TimeSpan.FromMilliseconds(_creationInterval), () =>
            {
                if (!_timerRunning)
                    return false;

                CreateRandomShape(_shapeSize);
                return true;
            });

        }

        /// <summary>
        /// Creates a specified number of random shapes and adds them to the collection.
        /// </summary>
        private void CreateRandomShape(int size)
        {
            Color sColor = Color.FromRgb(_random.Next(256), _random.Next(256), _random.Next(256));
            Shapes.Add(new ShapeModel(_random.NextDouble(), _random.NextDouble(), size, size, sColor, this));
            Shapes.Add(new ShapeModel(_random.NextDouble(), _random.NextDouble(), size, size, sColor, this));
            curShapesNum++;
            UpdateUI();
            CheckForEndGame();
        }

        /// <summary>
        /// Stops the shape creation timer.
        /// </summary>
        public void StopShapeCreationTimer()
        {
            _timerRunning = false;
        }

        /// <summary>
        /// Initializes the game with a set number of shapes.
        /// </summary>
        private void CreateRandomShapes(int number, int size)
        {
            for (int i = 0; i < number; i++)
            {
                CreateRandomShape(size);
            }
        }

        /// <summary>
        /// Updates the UI text properties.
        /// </summary>
        private void UpdateUI()
        {
            strMatches = $"Matches: {matches} / {targetMatches}";
            strCurShapesNum = $"Shapes on map: {curShapesNum} / {maxShapesNum}";

            OnPropertyChanged(nameof(StrMatches));
            OnPropertyChanged(nameof(StrCurShapesNum));
        }

        /// <summary>
        /// Increments the match count and checks if the game has ended.
        /// </summary>
        public void Match()
        {
            matches++;
            curShapesNum--;

            UpdateUI();
            CheckForEndGame();
        }

        /// <summary>
        /// Checks if the game is over due to max shape count or target matches.
        /// </summary>
        private void CheckForEndGame()
        {
            if (curShapesNum > maxShapesNum)
            {
                EndGame(false);
            }
            else if (matches >= targetMatches)
            {
                EndGame(true);
            }
        }


        private void EndGame (bool win)
        {
            StrEnd = win ? "You are a winner!" : "Game Over";
            StopShapeCreationTimer();
            GameIsOver = true;
            OnPropertyChanged(nameof(StrEnd));
        }

        /// <summary>
        /// Resets the game state and starts a new game.
        /// </summary>
        [RelayCommand]
        private void StartNewGame()
        {

            matches = 0;
            curShapesNum = 0;
            GameIsOver = false;
            UpdateUI();
            Shapes.Clear();
            CreateRandomShapes(_initialShapeCount, _shapeSize);
            StartShapeCreationTimer();
        }

        /// <summary>
        /// Exits the application.
        /// </summary>
        [RelayCommand]
        private void ExitApp()
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
    }

    public partial class ShapeModel : ObservableObject
    {
        [ObservableProperty]
        private Color sColor;

        [ObservableProperty]
        private Rect sRect;

        private double _initialX;
        private double _initialY;
        private double _initialWidth;
        private double _initialHeight;
        private bool _canMove = false;
        public bool IsExplosing = false;

        private GameViewModel _VM;

        public Frame AssociatedFrame { get; set; }

        public ShapeModel(double x, double y, double width, double height, Color color, GameViewModel VM)
        {
            _initialX = x;
            _initialY = y;
            _initialWidth = width;
            _initialHeight = height;
            sColor = color;
            sRect = new Rect(x, y, width, height);
            _VM = VM;
        }

        /// <summary>
        /// Called when movement starts.
        /// </summary>
        public void MoveStarted()
        {
            _canMove = true;
        }

        /// <summary>
        /// Handles the movement update while dragging.
        /// </summary>
        public void MoveRunning(double xTranslation, double yTranslation)
        {
            if (_canMove)
            {
                double x = Math.Clamp(sRect.X + xTranslation, -_initialWidth, _initialWidth);
                double y = Math.Clamp(sRect.Y + yTranslation, -_initialHeight, _initialHeight);

                sRect = new Rect(x, y, _initialWidth, _initialHeight);
                OnPropertyChanged(nameof(SRect));
            }
        }

        /// <summary>
        /// Called when movement ends; checks for collisions.
        /// </summary>
        public void MoveEnded(double winWidth, double winHeight, Frame curFrame, List<Frame> frames)
        {
            CheckForOverlaps(winWidth, winHeight, curFrame, frames);

            if (IsExplosing)
            {
                _canMove = false;
                return;
            }

            sRect = new Rect(_initialX, _initialY, _initialWidth, _initialHeight);
            OnPropertyChanged(nameof(SRect));
        }

        /// <summary>
        /// Checks for shape collisions and triggers explosions if colors match.
        /// </summary>
        private void CheckForOverlaps(double winWidth, double winHeight, Frame curFrame, List<Frame> frames)
        {
            var normRect1 = new Rect(sRect.X, sRect.Y, sRect.Width / winWidth, sRect.Height / winHeight);
            
            foreach (var frame in frames)
            {
                if (frame == curFrame) continue;

                var otherShape = frame.BindingContext as ShapeModel;
                if (otherShape != null && otherShape.CanBeMatched() && sColor == otherShape.sColor)
                {
                    var normRect2 = new Rect(otherShape.sRect.X, otherShape.sRect.Y,
                                            otherShape.sRect.Width / winWidth, otherShape.sRect.Height / winHeight);

                    if (normRect1.IntersectsWith(normRect2))
                    {
                        IsExplosing = true;
                        otherShape.IsExplosing = true;
                        ExplosionAnimation(curFrame);
                        ExplosionAnimation(frame);
                        _VM.Match();
                    }
                }
            }
        }

        /// <summary>
        /// Determines whether this shape can be matched with another.
        /// </summary>
        public bool CanBeMatched()
        {
            return _canMove;
        }

        /// <summary>
        /// Triggers an explosion animation for the matched shape.
        /// </summary>
        public void ExplosionAnimation(Frame frame)
        {
            if (frame == null) return;

            double duration = 500;
            double scaleFactor = 1.5;

            var scaleAnimation = new Animation(s => frame.Scale = s, 1, scaleFactor);
            var fadeAnimation = new Animation(f => frame.Opacity = f, 1, 0);

            var explodeAnimation = new Animation
            {
                { 0, 0.5, scaleAnimation },
                { 0.5, 1, fadeAnimation }
            };

            explodeAnimation.Commit(frame, "ExplosionAnimation", 16, (uint)duration, Easing.Linear, (v, c) =>
            {
                frame.Scale = 1;
                frame.Opacity = 1;

                if (frame.BindingContext is ShapeModel shapeToRemove && _VM.Shapes.Contains(shapeToRemove))
                {
                    _VM.Shapes.Remove(shapeToRemove);
                }
            });
        }
    }
}
