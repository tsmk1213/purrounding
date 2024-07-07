using Microsoft.Win32;
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
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace audio_button
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        //재생할 오디오 파일의 인덱스 초기 인덱스 값 설정
        private int currentFileIndex = -1;
        //재생할 오디오 파일 배열(현재는 2개만 있음)
        private readonly string[] audioFiles = new string[]
        {
        "C:\\Users\\kshn0\\Desktop\\audio button\\audio button\\mp3_file\\amalgam-217007.mp3",
        "C:\\Users\\kshn0\\Desktop\\audio button\\audio button\\mp3_file\\Bonjour Bonjour  - Danza della Tarantella.mp3"
        };

        //빌드 시 불러오는 함수들
        public MainWindow()
        {
            InitializeComponent();
            PositionClickableAreas();
            SizeChanged += MainWindow_SizeChanged;
        }
        
        //오디오 파일 재생 중지했을 때
        private void MediaMain_MediaEnded(object sender, RoutedEventArgs e)
        {
            mediaMain.Stop();
        }

        //파일 불러오기 실패했을 때
        private void MediaMain_MediaFailed(object sender, ExceptionRoutedEventArgs e)
        {
            MessageBox.Show("동영상 재생 실패 : " + e.ErrorException.Message.ToString());
        }

        //오디오 파일 선택 & 재생
        private void PlayAudio(int fileIndex)
        {
            if (fileIndex < 0 || fileIndex >= audioFiles.Length)
            {
                return;
            }

            currentFileIndex = fileIndex;
            mediaMain.Source = new Uri(audioFiles[fileIndex], UriKind.Absolute);
            mediaMain.Play();
        }

        //재생 버튼 눌렀을 때
        private void Music_Start(object sender, MouseButtonEventArgs e)
        {
            PlayAudio(0);
        }

        //오디오 이미지 파일의 누를 수 있는 경계 설정
        private void PositionClickableAreas()
        {
            double canvasWidth = MainCanvas.ActualWidth;
            double canvasHeight = MainCanvas.ActualHeight;

            double imageWidth = MainImage.Width;
            double imageHeight = MainImage.Height;

            Canvas.SetLeft(MainImage, (canvasWidth - imageWidth) / 2);
            Canvas.SetTop(MainImage, (canvasHeight - imageHeight) / 2);

            //재생 버튼
            Canvas.SetLeft(ClickableArea1, (canvasWidth - imageWidth) / 2 - 5);
            Canvas.SetTop(ClickableArea1, (canvasHeight - imageHeight) / 2 + 37);

            //일시정지 버튼
            Canvas.SetLeft(ClickableArea2, (canvasWidth - imageWidth) / 2 + 12);
            Canvas.SetTop(ClickableArea2, (canvasHeight - imageHeight) / 2 + 37);

            //중지 버튼
            Canvas.SetLeft(ClickableArea3, (canvasWidth - imageWidth) / 2 + 30);
            Canvas.SetTop(ClickableArea3, (canvasHeight - imageHeight) / 2 + 37);

            //다음 곡 재생 버튼
            Canvas.SetLeft(ClickableArea4, (canvasWidth - imageWidth) / 2 + 47);
            Canvas.SetTop(ClickableArea4, (canvasHeight - imageHeight) / 2 + 37);
        }

        //재생 버튼 클릭 시
        private void ClickableArea1_Start(object sender, MouseButtonEventArgs e)
        {
            if (mediaMain.Source == null)
            {
                PlayAudio(0);
            }
            else
            {
                mediaMain.Play();
            }
        }
        //일시정지 버튼 클릭 시
        private void ClickableArea2_Pause(object sender, MouseButtonEventArgs e)
        {
            if (mediaMain.Source == null)
            {
                return;
            }

            mediaMain.Pause();
        }
        //중지 버튼 클릭 시
        private void ClickableArea3_Stop(object sender, MouseButtonEventArgs e)
        {
            if (mediaMain.Source == null)
            {
                return;
            }

            mediaMain.Stop();
        }
        //다음 곡 재생 버튼 클릭 시
        private void ClickableArea4_Skip(object sender, MouseButtonEventArgs e)
        {
            if (currentFileIndex == -1)
            {
                return;
            }

            int nextFileIndex = (currentFileIndex + 1) % audioFiles.Length;
            PlayAudio(nextFileIndex);
        }

        //설정한 경계에 따라 이미지 이동 시키기
        private void MainWindow_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            PositionClickableAreas();
        }
    }
}