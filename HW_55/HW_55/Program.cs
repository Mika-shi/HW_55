using System;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace HW_55;

class Program
{
    private const int MillisecondsPerMinute = 100;

    static async Task Main(string[] args)
    {
        Console.OutputEncoding = Encoding.UTF8;

        RunFirstTask();

        Console.WriteLine();
        Console.WriteLine("==================================");
        Console.WriteLine();

        RunSecondTask();

        Console.WriteLine();
        Console.WriteLine("==================================");
        Console.WriteLine();

        await RunThirdTaskAsync();
    }

    static void RunFirstTask()
    {
        Console.WriteLine("Задание 1. Последовательное утро через Thread.");
        Console.WriteLine();

        Thread vasyaThread = new Thread(MorningRoutineWithThread);
        vasyaThread.Start();
        vasyaThread.Join();
    }

    static void MorningRoutineWithThread()
    {
        Console.WriteLine("Вася проснулся в 07:30.");
        Console.WriteLine();

        int totalMinutes = 0;

        totalMinutes += LieInBed();
        totalMinutes += PutKettleOnStove();
        totalMinutes += HeatDinner();
        totalMinutes += FillBath();
        totalMinutes += TakeBath();
        totalMinutes += HaveBreakfast();
        totalMinutes += GetDressed();
        totalMinutes += GoToWork();

        PrintResult(totalMinutes);
    }

    static void RunSecondTask()
    {
        Console.WriteLine("Задание 2. Оптимизированное утро через TPL.");
        Console.WriteLine();
        Console.WriteLine("Вася проснулся в 07:30.");
        Console.WriteLine();

        Task<int> lieInBedTask = Task.Run(LieInBed);
        Task<int> kettleTask = Task.Run(PutKettleOnStove);
        Task<int> dinnerTask = Task.Run(HeatDinner);
        Task<int> fillBathTask = Task.Run(FillBath);

        Task.WaitAll(lieInBedTask, kettleTask, dinnerTask, fillBathTask);

        int firstBlockMinutes = Math.Max(
            lieInBedTask.Result,
            Math.Max(kettleTask.Result, Math.Max(dinnerTask.Result, fillBathTask.Result))
        );

        Console.WriteLine();
        Console.WriteLine($"Первый параллельный блок занял: {firstBlockMinutes} минут.");
        Console.WriteLine();

        Task<int> takeBathTask = Task.Run(TakeBath);
        Task<int> breakfastTask = Task.Run(HaveBreakfast);

        Task.WaitAll(takeBathTask, breakfastTask);

        int secondBlockMinutes = Math.Max(takeBathTask.Result, breakfastTask.Result);

        Console.WriteLine();
        Console.WriteLine($"Второй параллельный блок занял: {secondBlockMinutes} минут.");
        Console.WriteLine();

        int dressedMinutes = GetDressed();
        int workMinutes = GoToWork();

        int optimizedTotalMinutes = firstBlockMinutes + secondBlockMinutes + dressedMinutes + workMinutes;

        PrintOptimizedResult("Фактическое время с параллельными задачами", optimizedTotalMinutes);
    }

    static async Task RunThirdTaskAsync()
    {
        Console.WriteLine("Задание 3. Асинхронное утро через async - await.");
        Console.WriteLine();
        Console.WriteLine("Вася проснулся в 07:30.");
        Console.WriteLine();

        Task<int> lieInBedTask = LieInBedAsync();
        Task<int> kettleTask = PutKettleOnStoveAsync();
        Task<int> dinnerTask = HeatDinnerAsync();
        Task<int> fillBathTask = FillBathAsync();

        int[] firstBlockResults = await Task.WhenAll(
            lieInBedTask,
            kettleTask,
            dinnerTask,
            fillBathTask
        );

        int firstBlockMinutes = firstBlockResults.Max();

        Console.WriteLine();
        Console.WriteLine($"Первый асинхронный блок занял: {firstBlockMinutes} минут.");
        Console.WriteLine();

        Task<int> takeBathTask = TakeBathAsync();
        Task<int> breakfastTask = HaveBreakfastAsync();

        int[] secondBlockResults = await Task.WhenAll(
            takeBathTask,
            breakfastTask
        );

        int secondBlockMinutes = secondBlockResults.Max();

        Console.WriteLine();
        Console.WriteLine($"Второй асинхронный блок занял: {secondBlockMinutes} минут.");
        Console.WriteLine();

        int dressedMinutes = await GetDressedAsync();
        int workMinutes = await GoToWorkAsync();

        int optimizedTotalMinutes = firstBlockMinutes + secondBlockMinutes + dressedMinutes + workMinutes;

        PrintOptimizedResult("Фактическое время с асинхронными задачами", optimizedTotalMinutes);
    }

    static int LieInBed()
    {
        return DoAction("Просыпается и лежит в кровати", 15);
    }

    static int PutKettleOnStove()
    {
        return DoAction("Ставит чайник на плиту", 5);
    }

    static int HeatDinner()
    {
        return DoAction("Подогревает вчерашний ужин", 5);
    }

    static int FillBath()
    {
        return DoAction("Набирает ванну", 10);
    }

    static int TakeBath()
    {
        return DoAction("Принимает ванну", 15);
    }

    static int HaveBreakfast()
    {
        return DoAction("Завтракает", 10);
    }

    static int GetDressed()
    {
        return DoAction("Одевается", 5);
    }

    static int GoToWork()
    {
        return DoAction("Едет на работу", 55);
    }

    static async Task<int> LieInBedAsync()
    {
        return await DoActionAsync("Просыпается и лежит в кровати", 15);
    }

    static async Task<int> PutKettleOnStoveAsync()
    {
        return await DoActionAsync("Ставит чайник на плиту", 5);
    }

    static async Task<int> HeatDinnerAsync()
    {
        return await DoActionAsync("Подогревает вчерашний ужин", 5);
    }

    static async Task<int> FillBathAsync()
    {
        return await DoActionAsync("Набирает ванну", 10);
    }

    static async Task<int> TakeBathAsync()
    {
        return await DoActionAsync("Принимает ванну", 15);
    }

    static async Task<int> HaveBreakfastAsync()
    {
        return await DoActionAsync("Завтракает", 10);
    }

    static async Task<int> GetDressedAsync()
    {
        return await DoActionAsync("Одевается", 5);
    }

    static async Task<int> GoToWorkAsync()
    {
        return await DoActionAsync("Едет на работу", 55);
    }

    static int DoAction(string actionName, int minutes)
    {
        Console.WriteLine($"Начал: {actionName}. Плановое время: {minutes} минут.");

        int delay = minutes * MillisecondsPerMinute;

        if (delay > 5000)
        {
            delay = 5000;
        }

        Thread.Sleep(delay);

        Console.WriteLine($"Закончил: {actionName}. Затрачено: {minutes} минут.");

        return minutes;
    }

    static async Task<int> DoActionAsync(string actionName, int minutes)
    {
        Console.WriteLine($"Начал: {actionName}. Плановое время: {minutes} минут.");

        int delay = minutes * MillisecondsPerMinute;

        if (delay > 5000)
        {
            delay = 5000;
        }

        await Task.Delay(delay);

        Console.WriteLine($"Закончил: {actionName}. Затрачено: {minutes} минут.");

        return minutes;
    }

    static void PrintResult(int totalMinutes)
    {
        Console.WriteLine();
        Console.WriteLine($"Всего времени затрачено: {totalMinutes} минут.");

        int availableMinutes = 90;

        if (totalMinutes > availableMinutes)
        {
            Console.WriteLine($"Вася опоздал на {totalMinutes - availableMinutes} минут.");
        }
        else
        {
            Console.WriteLine($"Вася успел. В запасе осталось {availableMinutes - totalMinutes} минут.");
        }
    }

    static void PrintOptimizedResult(string title, int totalMinutes)
    {
        Console.WriteLine();
        Console.WriteLine($"{title}: {totalMinutes} минут.");

        int availableMinutes = 90;

        if (totalMinutes > availableMinutes)
        {
            Console.WriteLine($"Вася опоздал на {totalMinutes - availableMinutes} минут.");
        }
        else
        {
            Console.WriteLine($"Вася успел. В запасе осталось {availableMinutes - totalMinutes} минут.");
        }
    }
}