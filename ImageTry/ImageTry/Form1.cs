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

        // ����ļ�
        private void btnAddFile_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Filter = "ͼ���ļ� (*.jpg; *.png; *.bmp)|*.jpg;*.png;*.bmp",
                Multiselect = true
            };

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                var itemsCopy = new HashSet<string>(lstFileList1.Items.Cast<string>());

                foreach (string file in openFileDialog.FileNames)
                {
                    if (!itemsCopy.Contains(file))
                    {
                        lstFileList1.Items.Add(file + " [������]");
                    }
                }
            }
        }

        // �Ƴ��ļ�
        private void btnRemoveFile_Click(object sender, EventArgs e)
        {
            if (lstFileList1.SelectedItem != null)
            {
                lstFileList1.Items.Remove(lstFileList1.SelectedItem);
            }
            else
            {
                MessageBox.Show("��ѡ��Ҫɾ�����ļ���");
            }
        }

        // ��ȡ����ѡ��
        private string GetSelectedProcessingOption()
        {
            return cmbProcessing.SelectedItem?.ToString() ?? "�Ҷ�";
        }

        // ��ʼ����
        private async void btnStart_Click(object sender, EventArgs e)
        {
            if (lstFileList1.Items.Count == 0)
            {
                MessageBox.Show("��������ļ���");
                return;
            }

            if (lstFileList1.SelectedItem == null)
            {
                MessageBox.Show("����ѡ���ļ���");
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
                UpdateFileStatus(filePath, "[������]");

                try
                {
                    await Task.Run(() => ProcessImage(filePath, processingOption, cts.Token));
                    UpdateFileStatus(filePath, "[�������]");
                }
                catch (OperationCanceledException)
                {
                    UpdateFileStatus(filePath, "[��ȡ��]");
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"�������{ex.Message}");
                    UpdateFileStatus(filePath, "[����ʧ��]");
                }
            }
            lstFileList1.EndUpdate();
        }

        // �����ļ�״̬
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

        // ͼ������
        private void ProcessImage(string filePath, string processingOption, CancellationToken token)
        {
            if (token.IsCancellationRequested)
                throw new OperationCanceledException();

            using (Mat image = CvInvoke.Imread(filePath, ImreadModes.Color))
            {
                if (image.IsEmpty)
                {
                    throw new Exception("�޷�����ͼ��");
                }

                Mat processedImage = new Mat();


                switch (processingOption)
                {
                    case "�Ҷ�":
                        CvInvoke.CvtColor(image, processedImage, ColorConversion.Bgr2Gray);
                        break;
                    case "�Ŵ�":
                        CvInvoke.Resize(image, processedImage, new Size(image.Width * 2, image.Height * 2), interpolation: Inter.Linear);
                        break;
                    case "��С":
                        CvInvoke.Resize(image, processedImage, new Size(image.Width / 2, image.Height / 2), interpolation: Inter.Area);
                        break;
                    case "˳ʱ����ת90��":
                        CvInvoke.Rotate(image, processedImage, RotateFlags.Rotate90Clockwise);
                        break;
                    case "��ʱ����ת90��":
                        CvInvoke.Rotate(image, processedImage, RotateFlags.Rotate90CounterClockwise);
                        break;
                    case "ͼ��ת":
                        CvInvoke.BitwiseNot(image, processedImage); // ʹ�� BitwiseNot ���з�ת
                        break;
                    case "ģ��":
                        CvInvoke.GaussianBlur(image, processedImage, new Size(35, 35), 5);
                        break;
                    case "��Ե��ȡ":
                        Mat grayImage = new Mat();
                        CvInvoke.CvtColor(image, grayImage, ColorConversion.Bgr2Gray);
                        double threshold1 = 100; // �ϵ͵���ֵ
                        double threshold2 = 200; // �ϸߵ���ֵ
                        CvInvoke.Canny(grayImage, processedImage, threshold1, threshold2);
                        break;
                    default:
                        throw new Exception("δ֪�Ĵ���ѡ�");
                }

                string resultPath = Path.Combine(Path.GetDirectoryName(filePath), "Processed_" + Path.GetFileName(filePath));
                processedImage.Save(resultPath);
            }
        }

        // ȡ������
        private void btnCancel_Click(object sender, EventArgs e)
        {
            cts.Cancel();
            MessageBox.Show("���д���������ȡ����");
        }

        // �鿴������
        private void btnViewResult_Click(object sender, EventArgs e)
        {
            if (lstFileList1.SelectedItem != null)
            {
                try
                {
                    // ��ȡԭʼ�ļ�·��
                    string selectedItem = lstFileList1.SelectedItem.ToString();
                    string filePath = selectedItem.Contains(" ")
                        ? selectedItem.Substring(0, selectedItem.LastIndexOf(" "))
                        : selectedItem;

                    // ���ԭʼ�ļ��Ƿ����
                    if (!File.Exists(filePath))
                    {
                        MessageBox.Show($"ԭʼ�ļ�δ�ҵ���{filePath}");
                        return;
                    }

                    // ����������ļ�·��
                    string resultPath = Path.Combine(Path.GetDirectoryName(filePath), "Processed_" + Path.GetFileName(filePath));

                    // ��鴦����ļ��Ƿ����
                    if (!File.Exists(resultPath))
                    {
                        MessageBox.Show($"������δ�ҵ���{resultPath}");
                        return;
                    }

                    // ��ԭʼ�ļ��ʹ������ļ�
                    Process.Start(new ProcessStartInfo(filePath) { UseShellExecute = true });
                    Process.Start(new ProcessStartInfo(resultPath) { UseShellExecute = true });
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"�鿴�ļ�ʱ����{ex.Message}");
                }
            }
            else
            {
                MessageBox.Show("����ѡ���ļ���");
            }
        }

        // ���ļ�
        private void OpenFile(string path)
        {
            try
            {
                Process.Start(new ProcessStartInfo(path) { UseShellExecute = true });
            }
            catch (Exception ex)
            {
                MessageBox.Show($"�޷����ļ���{ex.Message}");
            }
        }
    }
        
}
