# Задания
## Система логирования
### Обзор
Был создан класс `Logger` для записи сообщений системы в консоль программы и в файл логов. В качестве контрольного примера разработано простое консольное приложение вызывающее методы класса для записи некоторой информации
### Выполненные требования
1. Класс `Logger` может использовать в качестве своего ядра/движка для записи логов сторонний класс, достаточно, чтобы класс ядра соответствовал интерфейсу:
```c#
public  interface  ILoggerEngine
{
void  Debug(string  source, string  text);
void  Info(string  source, string  text);
void  Warning(string  source, string  text);
void  Error(string  source, string  text);
void  Shutdown();
}
```

Ядро класса `Logger` можно легко заменить другим. В работе показан пример использования в качестве ядра сторонних библиотек NLog и log4net . Для настройки данных библиотек можно использовать конфиг-файл `Assignment/App.config`

2. Класс предоставляет методы записи логов разной серьёзности, а также исключений. Для уровней серьёзности введено перечисление:
```c#
public  enum  LogMessageSeverity
{
Debug  =  0,
Info  =  1,
Warning  =  2,
Error  =  3
}
```

3. При записи исключений можно указать, является ли оно критичным, а при в сообщение исключения записывается стек вызовов функций.
public void LogException(string source, Exception exception, bool isCritical)
```c#
{
    var stack = new System.Diagnostics.StackTrace();
    if (isCritical)
    {
        this.engine.Error(source ?? "Unknown", $"[CRITICAL EXCEPTION]\n{stack}{exception}");
    }
    else
    {
        this.engine.Error(source ?? "Unknown", $"[EXCEPTION]\n{stack}{exception}");
    }
}
```
4. Для объекта класса можно указать уровень записи логов. Например при уровне записи Info записываются только логи с серьёзностью Info и выше. По заявленным требованиям сообщения с серьёзностью Debug не записываются в релизной сборке программы:
```c#
        public Logger(ILoggerEngine engine, LogMessageSeverity logLevel = LogMessageSeverity.Debug)
        {
            this.engine = engine;
            this.logLevel = logLevel;
            bool isRelease = false;
#if RELEASE
            isRelease = true;
#endif
            if (isRelease && logLevel == LogMessageSeverity.Debug)
            {
                this.logLevel = LogMessageSeverity.Info;
            }
        }
```

5. Для каждой записи логов указывается источник - класс в котором была вызвана функция записи лога. Источник записывается в отдельной колонке:
```
2024-07-03 10:48:27,551|INFO |SomeClass| Doing something, info message
```

6. Класс предоставляет краткие формы для записи сообщений и исключений:
```c#
public void Debug(string message)
{
    if (this.logLevel == LogMessageSeverity.Debug)
    {
        ClassSource();
        LogMessage(this.source ?? "Unknown", message, LogMessageSeverity.Debug);
    }

}
public void Info(string message)
{
    if (this.logLevel <= LogMessageSeverity.Info)
    {
        ClassSource();
        LogMessage(this.source ?? "Unknown", message, LogMessageSeverity.Info);
    }
}
public void Warning(string message)
{
    if (this.logLevel <= LogMessageSeverity.Warning)
    {
        ClassSource();
        LogMessage(this.source ?? "Unknown", message, LogMessageSeverity.Warning);
    }
}
public void Error(string message)
{
    ClassSource();
    LogMessage(this.source ?? "Unknown", message, LogMessageSeverity.Error);
}
public void Exception(Exception exception, bool isCritical = false)
{
    ClassSource();
    var stack = new System.Diagnostics.StackTrace();
    if (isCritical)
    {
        this.engine.Error(this.source ?? "Unknown", $"[CRITICAL EXCEPTION]\n{stack}{exception}");
    }
    else
    {
        this.engine.Error(this.source ?? "Unknown", $"[EXCEPTION]\n{stack}{exception}");
    }
}
```

### Вспомогательные классы

Для обёртки библиотек и приведения их к интерфейсу `ILoggerEngine` были созданы вспомогательные классы `NLogEngine` и `Log4NetEngine` -  см. файл `Assignment/Logger/Engines.cs`

Для создания объектов класса `Logger` создана фабрика для создания этих объектов - `LoggerFactory`. Предусматривается, что каждый класс, нуждающийся в логировании, использует собственный объект класса `Logger`, фабрика позволяет упростить создание этих объектов:
```c#
public class LoggerFactory
{
    private ILoggerEngine? engine;
    private LogMessageSeverity logLevel = LogMessageSeverity.Debug;

    public LoggerFactory(ILoggerEngine? engine = null, LogMessageSeverity? logLevel = null)
    {
        this.engine = engine;
        this.logLevel = logLevel ?? LogMessageSeverity.Debug;
    }

    public Logger Create()
    {
        if (engine == null)
        {
            engine = new Log4NetEngine();
        }
        return new Logger(engine, logLevel);
    }
    public LoggerFactory WithLogLevel(LogMessageSeverity logLevel)
    {
        this.logLevel = logLevel;
        return this;
    }
    public LoggerFactory WithEngine(ILoggerEngine engine)
    {
        this.engine = engine;
        return this;
    }
}
```

### Контрольный пример
В качестве контрольного примера была сделана небольшая программа, вызывающая методы объекта класса `Logger`, где были использованы разные ядра - NLog и log4net.
В программе создаётся объект ядра, затем через фабрику создается объект класса `Logger` и производится вызов записи всех типов серьёзности, а также исключения. Результат можем видеть в консольном окне, сперва логи с помощью NLog, затем - log4net:

```
NLog Engine/Core:
2024-07-03 11:13:35.2651|DEBUG|Program| NLog debug message
2024-07-03 11:13:35.2734|INFO|Program| NLog info message
2024-07-03 11:13:35.2734|WARN|Program| NLog warning message
2024-07-03 11:13:35.2734|ERROR|Program| NLog error message
2024-07-03 11:13:35.2734|ERROR|Program| [EXCEPTION]
   at Logger.Logger.Exception(Exception exception, Boolean isCritical)
   at Assignment.Program.Main(String[] args)
System.DivideByZeroException: Attempted to divide by zero.
   at Assignment.Program.Main(String[] args) in D:\work\c#-test\DirectumAssignment\Assignment\Program.cs:line 69

Log4Net Engine/Core:
2024-07-03 11:13:35,333|DEBUG|SomeClass| Doing something, debug message
2024-07-03 11:13:35,335|INFO |SomeClass| Doing something, info message
2024-07-03 11:13:35,336|WARN |SomeClass| Doing something, warning message
2024-07-03 11:13:35,336|ERROR|SomeClass| Doing something, error message
2024-07-03 11:13:35,336|ERROR|SomeClass| [CRITICAL EXCEPTION]
   at Logger.Logger.Exception(Exception exception, Boolean isCritical)
   at Assignment.Program.SomeClass.DoSomethingException()
   at Assignment.Program.Main(String[] args)
System.Exception: Test Exception Message
   at Assignment.Program.SomeClass.DoSomethingException() in D:\work\c#-test\DirectumAssignment\Assignment\Program.cs:line 42
```
В релизной сборке нет сообщений уровня Debug:
```
NLog Engine/Core:
2024-07-03 11:14:54.1788|INFO|Program| NLog info message
2024-07-03 11:14:54.1874|WARN|Program| NLog warning message
2024-07-03 11:14:54.1874|ERROR|Program| NLog error message
2024-07-03 11:14:54.1874|ERROR|Program| [EXCEPTION]
   at Logger.Logger.Exception(Exception exception, Boolean isCritical)
   at Assignment.Program.Main(String[] args)
System.DivideByZeroException: Attempted to divide by zero.
   at Assignment.Program.Main(String[] args) in D:\work\c#-test\DirectumAssignment\Assignment\Program.cs:line 67

Log4Net Engine/Core:
2024-07-03 11:14:54,257|INFO |SomeClass| Doing something, info message
2024-07-03 11:14:54,260|WARN |SomeClass| Doing something, warning message
2024-07-03 11:14:54,260|ERROR|SomeClass| Doing something, error message
2024-07-03 11:14:54,260|ERROR|SomeClass| [CRITICAL EXCEPTION]
   at Logger.Logger.Exception(Exception exception, Boolean isCritical)
   at Assignment.Program.SomeClass.DoSomethingException()
   at Assignment.Program.Main(String[] args)
System.Exception: Test Exception Message
   at Assignment.Program.SomeClass.DoSomethingException() in D:\work\c#-test\DirectumAssignment\Assignment\Program.cs:line 42
```

Эти логи записываются в соответствующие файлы логов. Работу программы можно посмотреть в [Github Actions](https://github.com/YaroslavVotincev/DirectumAssignment/actions)

## Анализ кода
### Обзор
```c#
static void Func1(ref KeyValuePair<int, string>[] a, int key, string value)
{
  Array.Resize(ref a, a.Length + 1);
      
  var keyValuePair = new KeyValuePair<int, string>(key, value);
  a[a.Length - 1] = keyValuePair;

  for (int i = 0; i < a.Length; i++)
  {
    for (int j = a.Length - 1; j > 0; j--)
    {
      if (a[j - 1].Key > a[j].Key)
      {
        KeyValuePair<int, string> x;
        x = a[j - 1];
        a[j - 1] = a[j];
        a[j] = x;
      }
    }
  }
}
```
Предоставленная функция берет на вход ссылку на массив из пар(число, строка), одно число и одну строку.
Алгоритм создаёт новую пару из переданных  значений числа и строки и добавляет эту пару в конец массива.
Последнее действие - сортировка массива по значению ключа пар сортировкой "пузырьком".

### Недостатки
1. Отсутствие проверки на null. Можно заметить, что функция принимает на вход ссылочный тип массива. В алгоритме отсутствует проверка ссылки на null. Если в функцию передано такое значение, то возникнет исключение.
2. Неэффективный алгоритм сортировки. Используется пузырьковый алгоритм сортировки, который является неэффективным алгоритмом со сложностью O(n^2^). Использование такого алгоритма будет сильно тормозить производительность программы при больших размерах входного массива

### Func1Lists
Если допускается изменение сигнатуры функции, то моим решением было бы изменение входного параметра массива на объект типа List. Этот тип предоставляет эффективный встроенный метод сортировки коллекции без создания новой.
```c#
public static void Func1Lists(ref List<KeyValuePair<int, string>> list, int key, string value)
{
    if (list == null)
    {
        list = new List<KeyValuePair<int, string>>([new KeyValuePair<int, string>(key, value)]);
        return;
    }
    list.Add(new KeyValuePair<int, string>(key, value));
    list.Sort((x, y) => x.Key.CompareTo(y.Key));
}
``` 

### Func1Linq
Если менять сигнатуру функции нельзя, то возможно решение с использованием стандартной библиотеки LINQ. Такое решение лаконично и понятно с первого взгляда. Однако такое решение не самое эффективное, библиотека LINQ, помимо алгоритмов которая она реализует, приносит оверхед, в частности методы создают новые массивы и коллекции, вместо изменения "на месте". 
```c#
public static void Func1Linq(ref KeyValuePair<int, string>[] a, int key, string value)
{
    if (a == null)
    {
        a = [new KeyValuePair<int, string>(key, value)];
        return;
    }
    a = a.Append(new KeyValuePair<int, string>(key, value)).OrderBy(x => x.Key).ToArray();
}
```

### Func1Insertion
Эта функция реализует алгоритм сортировки "вставками", вместо пузырькового алгоритма в оригинальном решении. В худшем случае алгоритм имеет сложность O(n^2^), однако работает лучше в случаях, когда массив уже частично сортирован.
```c#
public static void Func1Insertion(ref KeyValuePair<int, string>[] a, int key, string value)
{
    if (a == null)
    {
        a = [new KeyValuePair<int, string>(key, value)];
        return;
    }
    Array.Resize(ref a, a.Length + 1);
    a[^1] = new KeyValuePair<int, string>(key, value);
    for (int i = 1; i < a.Length; i++)
    {
        var current = a[i];
        int j = i - 1;
        for (; j >= 0 && a[j].Key > current.Key; j--)
            a[j + 1] = a[j];
        a[j + 1] = current;
    }
}
```

### Func1Quicksort
В случае, если разрешено создавать новые методы, то последнее решение - сортировка алгоритмом Quick Sort. Алгоритм имеет среднюю временную сложность O(n*log⁡ n), а также работает "на месте", не создавая нового массива и не выделяя дополнительной памяти на это. В этом решении вводится 2 новых вспомогательных функции Swap и QuickSort.
```c#
public static void Func1QuickSort(ref KeyValuePair<int, string>[] a, int key, string value)
{
    if (a == null)
    {
        a = [new KeyValuePair<int, string>(key, value)];
        return;
    }

    Array.Resize(ref a, a.Length + 1);
    a[^1] = new KeyValuePair<int, string>(key, value);

    QuickSort(ref a, 0, a.Length - 1);
}

public static void QuickSort(ref KeyValuePair<int, string>[] array, int leftIndex, int rightIndex)
{
    var i = leftIndex;
    var j = rightIndex;
    var pivot = array[leftIndex + (rightIndex - leftIndex) / 2];

    while (i <= j)
    {
        while (array[i].Key < pivot.Key)
            i++;

        while (array[j].Key > pivot.Key)
            j--;

        if (i <= j)
        {
            Swap(ref array[i], ref array[j]);
            i++;
            j--;
        }
    }

    if (leftIndex < j)
        QuickSort(ref array, leftIndex, j);

    if (i < rightIndex)
        QuickSort(ref array, i, rightIndex);
}
private static void Swap(ref KeyValuePair<int, string> a, ref KeyValuePair<int, string> b)
{
    (b, a) = (a, b);
}
```

### Контрольный пример 
В качестве проверки алгоритмов были созданы модульные тесты. Тест предоставляет заготовленный массив с перемешанными парами, а также эталонный, отсортированный массив с одним вставленным элементом. Каждый алгоритм проверяется на правильность сортировки и вставки элемента при сравнении с эталонным массивом. 
Прохождение тестов можно посмотреть в [Github Actions](https://github.com/YaroslavVotincev/DirectumAssignment/actions) 