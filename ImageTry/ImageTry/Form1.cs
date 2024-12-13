using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;
using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Ocl;
using Emgu.CV.Structure;

namespace ImageTry
{
    public partial class Form1 : Form
    {
        private CancellationTokenSource cts = new CancellationTokenSource();

        public Form1()
        {
            InitializeComponent();
        }

        // 添加文件
        private void btnAddFile_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Filter = "图像文件 (*.jpg; *.png; *.bmp)|*.jpg;*.png;*.bmp",
                Multiselect = true
            };

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                var itemsCopy = new HashSet<string>(lstFileList1.Items.Cast<string>());

                foreach (string file in openFileDialog.FileNames)
                {
                    if (!itemsCopy.Contains(file))
                    {
                        lstFileList1.Items.Add(file + " [待处理]");
                    }
                }
            }
        }

        // 移除文件
        private void btnRemoveFile_Click(object sender, EventArgs e)
        {
            if (lstFileList1.SelectedItem != null)
            {
                lstFileList1.Items.Remove(lstFileList1.SelectedItem);
            }
            else
            {
                MessageBox.Show("请选择要删除的文件！");
            }
        }

        // 获取处理选项
        private string GetSelectedProcessingOption()
        {
            return cmbProcessing.SelectedItem?.ToString() ?? "灰度";
        }

        // 开始处理
        private async void btnStart_Click(object sender, EventArgs e)
        {
            if (lstFileList1.Items.Count == 0)
            {
                MessageBox.Show("请先添加文件！");
                return;
            }

            if (lstFileList1.SelectedItem == null)
            {
                MessageBox.Show("请先选择文件！");
                return;
            }

            cts = new CancellationTokenSource();
            string processingOption = GetSelectedProcessingOption();

            lstFileList1.BeginUpdate();
            //var tempFileList = lstFileList.Items.Cast<object>().ToList();
            var tempFileList = lstFileList1.SelectedItems.Cast<object>().ToList();

            foreach (object item in tempFileList)
            {
                string filePath = item.ToString().Split(' ')[0];
                UpdateFileStatus(filePath, "[处理中]");

                try
                {
                    await Task.Run(() => ProcessImage(filePath, processingOption, cts.Token));
                    UpdateFileStatus(filePath, "[处理完毕]");
                }
                catch (OperationCanceledException)
                {
                    UpdateFileStatus(filePath, "[已取消]");
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"处理出错：{ex.Message}");
                    UpdateFileStatus(filePath, "[处理失败]");
                }
            }
            lstFileList1.EndUpdate();
        }

        // 更新文件状态
        private void UpdateFileStatus(string filePath, string status)
        {
            Invoke((MethodInvoker)(() =>
            {
                for (int i = 0; i < lstFileList1.Items.Count; i++)
                {
                    if (lstFileList1.Items[i].ToString().StartsWith(filePath))
                    {
                        lstFileList1.Items[i] = filePath + " " + status;
                        break;
                    }
                }
            }));
        }

        // 图像处理方法
        private void ProcessImage(string filePath, string processingOption, CancellationToken token)
        {
            if (token.IsCancellationRequested)
                throw new OperationCanceledException();

            using (Mat image = CvInvoke.Imread(filePath, ImreadModes.Color))
            {
                if (image.IsEmpty)
                {
                    throw new Exception("无法加载图像。");
                }

                Mat processedImage = new Mat();


                switch (processingOption)
                {
                    case "灰度":
                        CvInvoke.CvtColor(image, processedImage, ColorConversion.Bgr2Gray);
                        break;
                    case "放大":
                        CvInvoke.Resize(image, processedImage, new Size(image.Width * 2, image.Height * 2), interpolation: Inter.Linear);
                        break;
                    case "缩小":
                        CvInvoke.Resize(image, processedImage, new Size(image.Width / 2, image.Height / 2), interpolation: Inter.Area);
                        break;
                    case "顺时针旋转90°":
                        CvInvoke.Rotate(image, processedImage, RotateFlags.Rotate90Clockwise);
                        break;
                    case "逆时针旋转90°":
                        CvInvoke.Rotate(image, processedImage, RotateFlags.Rotate90CounterClockwise);
                        break;
                    case "图像反转":
                        CvInvoke.BitwiseNot(image, processedImage); // 使用 BitwiseNot 进行反转
                        break;
                    case "模糊":
                        CvInvoke.GaussianBlur(image, processedImage, new Size(35, 35), 5);
                        break;
                    case "边缘提取":
                        Mat grayImage = new Mat();
                        CvInvoke.CvtColor(image, grayImage, ColorConversion.Bgr2Gray);
                        double threshold1 = 100; // 较低的阈值
                        double threshold2 = 200; // 较高的阈值
                        CvInvoke.Canny(grayImage, processedImage, threshold1, threshold2);
                        break;
                    default:
                        throw new Exception("未知的处理选项。");
                }

                string resultPath = Path.Combine(Path.GetDirectoryName(filePath), "Processed_" + Path.GetFileName(filePath));
                processedImage.Save(resultPath);
            }
        }

        // 取消处理
        private void btnCancel_Click(object sender, EventArgs e)
        {
            cts.Cancel();
            MessageBox.Show("所有处理任务已取消！");
        }

        // 查看处理结果
        private void btnViewResult_Click(object sender, EventArgs e)
        {
            if (lstFileList1.SelectedItem != null)
            {
                try
                {
                    // 获取原始文件路径
                    string selectedItem = lstFileList1.SelectedItem.ToString();
                    string filePath = selectedItem.Contains(" ")
                        ? selectedItem.Substring(0, selectedItem.LastIndexOf(" "))
                        : selectedItem;

                    // 检查原始文件是否存在
                    if (!File.Exists(filePath))
                    {
                        MessageBox.Show($"原始文件未找到：{filePath}");
                        return;
                    }

                    // 构建处理后文件路径
                    string resultPath = Path.Combine(Path.GetDirectoryName(filePath), "Processed_" + Path.GetFileName(filePath));

                    // 检查处理后文件是否存在
                    if (!File.Exists(resultPath))
                    {
                        MessageBox.Show($"处理结果未找到：{resultPath}");
                        return;
                    }

                    // 打开原始文件和处理后的文件
                    Process.Start(new ProcessStartInfo(filePath) { UseShellExecute = true });
                    Process.Start(new ProcessStartInfo(resultPath) { UseShellExecute = true });
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"查看文件时出错：{ex.Message}");
                }
            }
            else
            {
                MessageBox.Show("请先选择文件！");
            }
        }

        // 打开文件
        private void OpenFile(string path)
        {
            try
            {
                Process.Start(new ProcessStartInfo(path) { UseShellExecute = true });
            }
            catch (Exception ex)
            {
                MessageBox.Show($"无法打开文件：{ex.Message}");
            }
        }
    }
        
}
