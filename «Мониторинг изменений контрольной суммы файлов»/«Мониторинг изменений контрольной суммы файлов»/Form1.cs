using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Security.Cryptography;

namespace _Мониторинг_изменений_контрольной_суммы_файлов_
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
                if (openFileDialog1.ShowDialog() == DialogResult.Cancel)
                    return;
                // получаем выбранный файл
                string file_path = openFileDialog1.FileName;
                textBox1.Text = file_path;

        }

        private void button2_Click(object sender, EventArgs e)
        {
            textBox2.Text = ComputeMD5Checksum(openFileDialog1.FileName);

            string strings = File.ReadAllText("MD5.txt");
            strings = strings.Replace("\r\n", string.Empty);
            string[] path_and_hash = strings.Split(new char[] { '@' });
            if (path_and_hash.Length > 1)
            {  
                List<string> for_write = path_and_hash.ToList();
                for_write.RemoveAt(for_write.Count - 1);

                for (int i = 0; i < for_write.Count - 1; i += 2)
                {
                    string _path = path_and_hash[i + 1];
                    string _hash = path_and_hash[i];
                    if (openFileDialog1.FileName == _path && textBox2.Text == _hash)
                    {
                        break;
                    }
                    else
                    {
                        if (openFileDialog1.FileName == _path && textBox2.Text != _hash)
                        {
                            path_and_hash[i] = textBox2.Text;
                            write_in_file(path_and_hash);
                            break;
                        }
                        else
                        {
                            if (i == for_write.Count - 2)
                            {
                                File.AppendAllText("MD5.txt", "\r\n" + textBox2.Text + "@" + openFileDialog1.FileName + "@");
                                break;
                            }
                        }
                    }
                }
            }
            else
            {
                File.AppendAllText("MD5.txt", "\r\n" + textBox2.Text + "@" + openFileDialog1.FileName + "@");
            }
            
        }

        private void write_in_file(string[] ar)
        {
            for (int i = 0; i < ar.Length - 1 ; i+=2)
            {
                File.WriteAllText("MD5.txt", "\r\n" + ar[i] + "@" + ar[i + 1] + "@");
            }
        }

        public string ComputeMD5Checksum(string path)
        {
            using (FileStream fs = System.IO.File.OpenRead(path))
            {
                MD5 md5 = new MD5CryptoServiceProvider();
                byte[] fileData = new byte[fs.Length];
                fs.Read(fileData, 0, (int)fs.Length);
                byte[] checkSum = md5.ComputeHash(fileData);
                string result = BitConverter.ToString(checkSum).Replace("-", String.Empty);
                return result;
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (textBox2.Text == string.Empty || openFileDialog1.FileName == string.Empty)
            {
                MessageBox.Show("Введите путь и хэш код!");
            }
            else
            {
                string strings = File.ReadAllText("MD5.txt");
                strings = strings.Replace("\r\n", string.Empty);
                string[] path_and_hash = strings.Split(new char[] { '@' });

                for (int i = 0; i < path_and_hash.Length - 1; i += 2)
                {
                    if (openFileDialog1.FileName == path_and_hash[i + 1] && textBox2.Text == path_and_hash[i])
                    {
                        MessageBox.Show("Всё gut!");
                        break;
                    }
                    else
                    {
                        if (openFileDialog1.FileName == path_and_hash[i + 1] && textBox2.Text != path_and_hash[i])
                        {
                            MessageBox.Show("Хеш-код не совпадает");
                            break;
                        }
                        else
                        {
                            if (i == path_and_hash.Length - 2)
                            {
                                MessageBox.Show("Такого файла нет в базе");
                                break;
                            }
                        }
                    }

                }
            }
        }
    }
}
