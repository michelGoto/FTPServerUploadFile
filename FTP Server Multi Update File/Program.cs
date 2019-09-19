
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace FTP_Server_Multi_Update_File
{
    class Program
    {
        static void Main(string[] args)
        {
            //App.Config　から情報をとる
            var user = ConfigurationManager.AppSettings["UserName"].Trim();
            var pass = ConfigurationManager.AppSettings["Password"].Trim();
            var server = ConfigurationManager.AppSettings["ServerSendTo"].Trim();

            //引数を取り入れる
            StringBuilder Adress = new StringBuilder();// "\"");
            foreach (string str in args)
            {
                Adress.Append($"{str} ");
              // Console.WriteLine(str);
            }
        //   Adress.Append("\"");
            var UpdateFile = Adress.ToString();//.Replace('"', ' ');
          //  Console.WriteLine(UpdateFile);
          //  Console.ReadKey();
            var file = UpdateFile;

            foreach (string adress in file.Split(','))
            {
                UpdateFile = adress;
                var path = Path.GetFileName(UpdateFile);

                //fileの有無を確認
                if (File.Exists(@UpdateFile))
                    Upload(@UpdateFile, user, pass, server, path);              
            }
        }

        public static void Upload(string fileFrom, string userName, string password, string server, string FileName)
        {
            try
            {
                //FTP接続先指定
                var request = FtpWebRequest.Create($"{server}/{FileName}");
                //FTPに接続して何をするかを決めるー　この場合ファイル送信
                request.Method = WebRequestMethods.Ftp.AppendFile;  //request.Method = WebRequestMethods.Ftp.UploadFile;
                //ユーザー名/パスワード
                request.Credentials = new NetworkCredential(userName, password);
                //FTPに接続する
                var requesStream = request.GetRequestStream();

                //送信したいファイルのコピー的なもの
                var conteudo = File.ReadAllBytes(fileFrom);
                request.ContentLength = conteudo.Length;

                //送信URLに書き込み
                requesStream.Write(conteudo, 0, conteudo.Length);
                requesStream.Close();

                var response = request.GetResponse();
                response.Close();

                // Console.ReadKey();
            }
            catch
            {
                Console.WriteLine("UserName / Password / Server Url invalid.");
                Console.ReadKey();
                return;
            }
        }

    }
}
