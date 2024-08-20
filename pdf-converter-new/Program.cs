//FileRecordOrCreate();
//FileRead();

static void FileRecordOrCreate()
{
    Console.WriteLine("Введите имя файла: ");
    string fileName = Console.ReadLine();

    // Приглашение пользователя ввести текст
    Console.WriteLine("Введите текст: ");
    // Считывание введенной пользователем строки
    string? str = Console.ReadLine();

    //определяем базовую директорию проекта
    string baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
    string directoryPath = Path.Combine(baseDirectory, "files");

    // Проверяем существование папки и создаем ее, если она не существует
    if (!Directory.Exists(directoryPath))
    {
        Directory.CreateDirectory(directoryPath);
    }

    string filePath = Path.Combine(directoryPath, fileName);

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

static void FileRead()
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