using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Brushes = System.Drawing.Brushes;
using Color = System.Drawing.Color;
using Pen = System.Drawing.Pen;
using Rectangle = System.Drawing.Rectangle;

namespace MP3Decoding
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        string fileName;
        Byte[] mp3ByteArr;
        FrameHeader frameHeader;

        public MainWindow()
        {
            InitializeComponent();
        }





        private void Btn_Browse_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.InitialDirectory = @"E:\Music";

            if(openFileDialog.ShowDialog() == true)
            {
                fileName = openFileDialog.FileName;
                txtBx_MP3File.Text = fileName;

                Read_MP3_file(fileName);
                
            }
        }




        private void Read_MP3_file(string mp3File)
        {
            mp3ByteArr = File.ReadAllBytes(mp3File);

            if(mp3ByteArr.Count() > 0)
            {
                Find_MP3_Header(mp3ByteArr);
            }
        }





        private void Find_MP3_Header(Byte[] byteArr)
        {
            int headerByte1 = 0xFF;
            int headerByte2A = 0xFF;
            int headerByte2B = 0xFB;
            bool foundHeader = false;

            for (int i = 0; i < byteArr.Length; i++)
            {
                if(foundHeader == true)
                {
                    //return;
                }

                //Look for the first byte with value FF and second byte with value FF or FB
                if(byteArr[i] == headerByte1 && (byteArr[i + 1] == headerByte2A || byteArr[i + 1] == headerByte2B))
                {
                    FrameHeader frameHeader = new FrameHeader(byteArr[i], byteArr[i + 1], byteArr[i + 2], byteArr[i + 3]);

                    if(frameHeader.IsStereo == false)
                    {
                        SideInformation sideInformation = new SideInformation(frameHeader.IsStereo, byteArr, (i + 4), (i + 20));
                    }
                    else
                    {
                        SideInformation sideInformation = new SideInformation(frameHeader.IsStereo, byteArr, (i + 4), (i + 35));
                    }

                    if (frameHeader.IsValidFrame == true)
                    {
                        //Look for the first byte with value FF and second byte with value FF or FB on the next frame header
                        int nextHeaderByte1 = byteArr[i + frameHeader.FrameLength];
                        int nextHeaderByte2 = byteArr[i + frameHeader.FrameLength + 1];
                        if (nextHeaderByte1 == headerByte1 && (nextHeaderByte2 == headerByte2A || nextHeaderByte2 == headerByte2B))
                        {
                            foundHeader = true;
                            Console.WriteLine("Byte Location: " + i);
                            Console.WriteLine();
                            PopulateGuiHeaderInfo(frameHeader);
                        }
                    }
                }
            }
        }




        private void PopulateGuiHeaderInfo(FrameHeader frameHeader)
        {
            txtBx_FrameHeader.Text = frameHeader.FrameHeaderBytes;
            txtBx_MpegVersion.Text = frameHeader.MpegVersion.ToString();
            txtBx_MpegLayer.Text = frameHeader.MpegLayer.ToString();
            txtBx_Bitrate.Text = frameHeader.BitRate.ToString();
            txtBx_SampleRate.Text = frameHeader.SampleRate.ToString();
            txtBx_FrameLength.Text = frameHeader.FrameLength.ToString();

            if(frameHeader.IsStereo == true)
            {
                chkBx_Stereo.IsChecked = true;
                chkBx_Mono.IsChecked = false;
            }
            else
            {
                chkBx_Stereo.IsChecked = false;
                chkBx_Mono.IsChecked = true;
            }

            if(frameHeader.IsCopyright == true)
            {
                chkBx_Copyright.IsChecked = true;
            }

            if(frameHeader.IsOriginal == true)
            {
                chkBx_OriginalMedia.IsChecked = true;
            }

            if(frameHeader.IsPadded == true)
            {
                chkBx_IsPadded.IsChecked = true;
            }
        }



    }
}
