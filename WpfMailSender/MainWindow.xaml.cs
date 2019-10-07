using MailSender.Interfaces;
using MailSender.Services;
using MailSender;
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
using MailSender.Models;
using System.IO;

namespace WpfMailSender
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        byte[] file;
        public MainWindow()
        {
            InitializeComponent();
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                IMailService service = new SendGridService();
                var model = new MailModel
                {
                    From = tbFrom.Text,
                    ToStr = tbTo.Text,
                    Title = tbTitle.Text,
                    Body = new TextRange(rtbBody.Document.ContentStart, rtbBody.Document.ContentEnd).Text,
                };
                if (file !=null)
                {
                    model.Attachments.Add(new MailModel.Attachment
                    {
                        Name = lFile.Content.ToString(),
                        Content = file
                    });
                }
                MessageBox.Show(await service.Send(model));
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            var formDialog = new System.Windows.Forms.OpenFileDialog();
            var result = formDialog.ShowDialog();
            switch (result)
            {
                case System.Windows.Forms.DialogResult.OK:
                    var FullFileName = formDialog.FileName;
                    lFile.Content = FullFileName.Split('\\').LastOrDefault();
                    file = File.ReadAllBytes(FullFileName);
                    break;
                case System.Windows.Forms.DialogResult.Cancel:
                default:
                    lFile.Content = "...";
                    break;
            }
        }
    }
}
