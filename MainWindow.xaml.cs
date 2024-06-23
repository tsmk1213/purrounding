using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Threading;

namespace purrounding_test
{
    public partial class MainWindow : Window
    {
        private GifBitmapDecoder? gifDecoder;
        private int frameIndex = 0;
        private DispatcherTimer? frameTimer;
        private DispatcherTimer? moveTimer;
        private Random random = new Random();
        private bool moveRight = true;  // 초기 이동 방향 설정
        private DateTime lastDirectionChangeTime;
        private bool isPaused = false;
        private DispatcherTimer? pauseTimer;

        public MainWindow()
        {
            InitializeComponent();
            var screenWidth = SystemParameters.PrimaryScreenWidth;
            var screenHeight = SystemParameters.PrimaryScreenHeight;

            Rectangle.Width = screenWidth;
            Rectangle.Height = screenHeight;

            LoadGif(@"C:\Users\water\OneDrive\바탕 화면\2024 IT프로젝트_23bit\purrounding_test\reverseCat.gif");
            StartMovingImage();
        }

        private void LoadGif(string filePath)
        {
            gifDecoder = new GifBitmapDecoder(new Uri(filePath), BitmapCreateOptions.PreservePixelFormat, BitmapCacheOption.Default);
            frameTimer = new DispatcherTimer();
            frameTimer.Interval = TimeSpan.FromMilliseconds(100);  // 프레임 간격 설정
            frameTimer.Tick += FrameTimer_Tick;
            frameTimer.Start();
        }

        private void FrameTimer_Tick(object sender, EventArgs e)
        {
            if (gifDecoder != null)
            {
                AnimatedImage.Source = gifDecoder.Frames[frameIndex];
                frameIndex = (frameIndex + 1) % gifDecoder.Frames.Count;
            }
        }

        private void StartMovingImage()
        {
            moveTimer = new DispatcherTimer();
            moveTimer.Interval = TimeSpan.FromMilliseconds(100);  // 이동 간격 설정 (0.1초마다)
            moveTimer.Tick += MoveTimer_Tick;
            lastDirectionChangeTime = DateTime.Now;
            moveTimer.Start();
        }

        private void MoveTimer_Tick(object sender, EventArgs e)
        {
            // if (isPaused) return;  // 멈춘 상태에서는 이동하지 않음

            double moveDistance = 1;  // 이동 거리 설정 (0.1초마다 1만큼 이동, 즉 1초에 10만큼 이동)
            double newLeft = AnimatedImage.Margin.Left + (moveRight ? moveDistance : -moveDistance);

            // 화면 경계를 넘어가지 않도록 제한
            if (newLeft < 0)
            {
                newLeft = 0;
                ChangeDirection();
            }
            else if (newLeft + AnimatedImage.Width > Rectangle.Width)
            {
                newLeft = Rectangle.Width - AnimatedImage.Width;
                ChangeDirection();
            }

            // 최소 10초가 지난 후에만 방향을 랜덤하게 변경
            if ((DateTime.Now - lastDirectionChangeTime).TotalSeconds >= 10)
            {
                if (random.NextDouble() < 0.1)  // 10% 확률로 최소 10초 동안 멈춤
                {
                    //PauseMovement(10000);  // 10초 동안 멈춤
                    //lastDirectionChangeTime = DateTime.Now;
                }
                else if (random.NextDouble() < 0.1)  // 10% 확률로 방향 변경
                {
                    ChangeDirection();
                }
            }

            // 새로운 위치 설정
            AnimatedImage.Margin = new Thickness(newLeft, AnimatedImage.Margin.Top, 0, 0);
        }

        private void ChangeDirection()
        {
            moveRight = !moveRight;
            lastDirectionChangeTime = DateTime.Now;
            PauseMovement(1000);  // 방향이 바뀔 때 1초 동안 멈춤

            // GIF 방향 뒤집기
            var scaleTransform = new ScaleTransform
            {
                ScaleX = moveRight ? 1 : -1,
                ScaleY = 1
            };
            AnimatedImage.RenderTransformOrigin = new Point(0.5, 0.5);  // 중심을 기준으로 뒤집기
            AnimatedImage.RenderTransform = scaleTransform;
        }

        private void PauseMovement(int milliseconds)
        {
            isPaused = true;
            pauseTimer = new DispatcherTimer();
            pauseTimer.Interval = TimeSpan.FromMilliseconds(milliseconds);
            pauseTimer.Tick += (s, e) =>
            {
                isPaused = false;
                pauseTimer.Stop();
            };
            pauseTimer.Start();
        }
    }
}
