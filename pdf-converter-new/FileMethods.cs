using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Properties;
using iText.Layout.Element;
using iText.Html2pdf;
using iText.StyledXmlParser.Css.Media;
using iText.IO.Image;
using iText.Kernel.Pdf.Canvas.Draw;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Drawing;
using System.Drawing.Printing;

namespace FileMethods
{
    public class FileMethods
    {
        public static void FileRecordOrCreate()
        {
            Console.WriteLine("Введите имя файла: ");
            string fileName = Console.ReadLine();

            // Приглашение пользователя ввести текст
            Console.WriteLine("Введите текст: ");
            // Считывание введенной пользователем строки
            string? str = Console.ReadLine();


            string filePath = FilePath(fileName);
            //Console.WriteLine(filePath);

            // Создание или открытие файла "info.txt" для записи
            using (FileStream stream = new FileStream(filePath, FileMode.OpenOrCreate))
            {

                // Преобразование строки в массив байтов с использованием кодировки UTF-8
                byte[] textArray = System.Text.Encoding.UTF8.GetBytes(str);
                // Запись массива байтов в файловый поток
                stream.Write(textArray, 0, textArray.Length);
            }
        }

        public static void FileRead()
        {
            Console.WriteLine("Введите имя файла: ");
            string fileName = Console.ReadLine();

            //определяем базовую директорию проекта
            string baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
            string directoryPath = Path.Combine(baseDirectory, "files");
            string filePath = Path.Combine(directoryPath, fileName);

            // Открытие файла для чтения
            using (FileStream stream = File.OpenRead(filePath))
            {
                // Создание массива байтов для хранения данных из файла
                byte[] textArray = new byte[stream.Length];
                // Чтение данных из файла в массив байтов
                stream.Read(textArray, 0, textArray.Length);

                // Преобразование массива байтов в строку с использованием кодировки по умолчанию
                string textFromFile = System.Text.Encoding.Default.GetString(textArray);
                // Вывод содержимого файла на консоль
                Console.WriteLine(textFromFile);
            }
        }

        public static string FilePath(string fileName)
        {
            //определяем базовую директорию проекта
            string baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
            string directoryPath = Path.Combine(baseDirectory, "files");

            // Проверяем существование папки и создаем ее, если она не существует
            if (!Directory.Exists(directoryPath))
            {
                Directory.CreateDirectory(directoryPath);
            }

            string filePath = Path.Combine(directoryPath, fileName);
            return filePath;
        }

        public static async Task CreateFileByURL(string url)
        {
            Console.WriteLine("Введите имя файла: ");
            string fileName = Console.ReadLine();
            string filePath = FilePath(fileName + ".pdf");

            Console.WriteLine("Полное имя файла: {0}", filePath);

            // Загрузка HTML с веб-страницы
            using (HttpClient client = new HttpClient())
            {
                try
                {
                    Console.WriteLine("Запрос HTML страницы");
                    string html = await client.GetStringAsync(url);

                    // Создание PDF
                    PdfDocument pdf = new PdfDocument(new PdfWriter(filePath));
                    //определяем свойства документа
                    ConverterProperties properties = new ConverterProperties();
                    pdf.SetDefaultPageSize(new iText.Kernel.Geom.PageSize(1920, 1080)); // A4 размер в пунктах (595x842)
                    properties.SetMediaDeviceDescription(new MediaDeviceDescription(MediaType.SCREEN));
                    //properties.GetCssApplierFactory();
                    HtmlConverter.ConvertToPdf(html, pdf, properties);

                    pdf.Close();

                    Console.WriteLine("PDF файл успешно создан.");
                }
                catch (HttpRequestException e)
                {
                    Console.WriteLine($"Ошибка при выполнении HTTP-запроса: {e.Message}");
                }
            }
        }
    }
}