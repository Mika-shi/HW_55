using System.Text;

namespace HW_55;

class Program
{
    private const int MillisecondsPerMinute = 100;
    private static int _totalMinutes = 0;

    static void Main(string[] args)
    {
        Console.OutputEncoding = Encoding.UTF8;

        Console.WriteLine("Основной поток запущен.");
        Console.WriteLine("Основной поток создаёт отдельный поток для утра Васи.");
        Console.WriteLine();

        Thread vasyaThread = new Thread(MorningRoutine);
        vasyaThread.Start();
        vasyaThread.Join();

        Console.WriteLine();
        Console.WriteLine("Все завершено.");
    }

    static void MorningRoutine()
    {
        Console.WriteLine("Вася проснулся в 07:30.");
        Console.WriteLine();

        LieInBed();
        PutKettleOnStove();
        HeatDinner();
        FillBath();
        TakeBath();
        HaveBreakfast();
        GetDressed();
        GoToWork();

        Console.WriteLine();
        Console.WriteLine($"Всего времени затрачено: {_totalMinutes} минут.");

        int availableMinutes = 90;

        if (_totalMinutes > availableMinutes)
        {
            Console.WriteLine($"Вася опоздал на {_totalMinutes - availableMinutes} минут.");
        }
        else
        {
            Console.WriteLine($"Вася успел. В запасе осталось {availableMinutes - _totalMinutes} минут.");
        }
    }

    static void LieInBed()
    {
        DoAction("Просыпается и лежит в кровати", 15);
    }

    static void PutKettleOnStove()
    {
        DoAction("Ставит чайник на плиту", 5);
    }

    static void HeatDinner()
    {
        DoAction("Подогревает вчерашний ужин", 5);
    }

    static void FillBath()
    {
        DoAction("Набирает ванну", 10);
    }

    static void TakeBath()
    {
        DoAction("Принимает ванну", 15);
    }

    static void HaveBreakfast()
    {
        DoAction("Завтракает", 10);
    }

    static void GetDressed()
    {
        DoAction("Одевается", 5);
    }

    static void GoToWork()
    {
        DoAction("Едет на работу", 55);
    }

    static void DoAction(string actionName, int minutes)
    {
        Console.WriteLine($"{actionName}. Время: {minutes} минут.");

        int delay = minutes * MillisecondsPerMinute;

        if (delay > 5000)
        {
            delay = 5000;
        }

        Thread.Sleep(delay);

        _totalMinutes += minutes;
    }
}