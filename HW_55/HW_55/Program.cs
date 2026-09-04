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
        Console.WriteLine("================ > - < ==================");
        Console.WriteLine();

        RunSecondTask();

        Console.WriteLine();
        Console.WriteLine("================ > - < ==================");
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

        PrintResult("Всего времени затрачено", totalMinutes);
    }

    static void RunSecondTask()
    {
        Console.WriteLine("Задание 2. Оптимизированное утро через TPL.");
        Console.WriteLine();
        Console.WriteLine("Вася проснулся в 07:30.");
        Console.WriteLine("Вася запускает автоматические процессы и возвращается в кровать.");
        Console.WriteLine();

        Task<int> kettleTask = Task.Run(PutKettleOnStove);
        Task<int> dinnerTask = Task.Run(HeatDinner);
        Task<int> fillBathTask = Task.Run(FillBath);

        int lieInBedMinutes = LieInBed();

        Task.WaitAll(kettleTask, dinnerTask, fillBathTask);

        int firstBlockMinutes = Math.Max(
            lieInBedMinutes,
            Math.Max(kettleTask.Result, Math.Max(dinnerTask.Result, fillBathTask.Result))
        );

        Console.WriteLine();
        Console.WriteLine($"Первый параллельный блок занял: {firstBlockMinutes} минут.");
        Console.WriteLine();

        int takeBathMinutes = TakeBath();
        int dressedMinutes = GetDressed();

        Console.WriteLine();
        Console.WriteLine("Вася опаздывает, поэтому пропускает завтрак.");
        Console.WriteLine();

        int workMinutes = GoToWork();

        int optimizedTotalMinutes = firstBlockMinutes + takeBathMinutes + dressedMinutes + workMinutes;

        PrintResult("Фактическое время с параллельными задачами без завтрака", optimizedTotalMinutes);

        Console.WriteLine("Итог: Вася успел на работу, но не успел позавтракать.");
    }

    static async Task RunThirdTaskAsync()
    {
        Console.WriteLine("Задание 3. Асинхронное утро через async/await.");
        Console.WriteLine();
        Console.WriteLine("Вася проснулся в 07:30.");
        Console.WriteLine("Теперь Вася запускает общие действия асинхронно и дожидается самого долгого.");
        Console.WriteLine();

        Task<int> kettleTask = PutKettleOnStoveAsync();
        Task<int> dinnerTask = HeatDinnerAsync();
        Task<int> fillBathTask = FillBathAsync();

        Console.WriteLine("Пока чайник кипит, ужин греется, а ванна набирается, Вася лежит в кровати.");
        Console.WriteLine();

        Task<int> lieInBedTask = LieInBedAsync();

        int[] firstBlockResults = await Task.WhenAll(
            kettleTask,
            dinnerTask,
            fillBathTask,
            lieInBedTask
        );

        int firstBlockMinutes = firstBlockResults.Max();

        Console.WriteLine();
        Console.WriteLine($"Первый асинхронный блок занял: {firstBlockMinutes} минут.");
        Console.WriteLine();

        int takeBathMinutes = await TakeBathAsync();
        int dressedMinutes = await GetDressedAsync();

        Console.WriteLine();
        Console.WriteLine("Вася берёт завтрак с собой и завтракает по дороге на работу.");
        Console.WriteLine();

        Task<int> breakfastTask = HaveBreakfastAsync();
        Task<int> workTask = GoToWorkAsync();

        int[] lastBlockResults = await Task.WhenAll(
            breakfastTask,
            workTask
        );

        int lastBlockMinutes = lastBlockResults.Max();

        Console.WriteLine();
        Console.WriteLine($"Последний асинхронный блок занял: {lastBlockMinutes} минут.");
        Console.WriteLine();

        int optimizedTotalMinutes = firstBlockMinutes + takeBathMinutes + dressedMinutes + lastBlockMinutes;
        
        PrintResult("Фактическое время с асинхронными задачами", optimizedTotalMinutes);

        Console.WriteLine("Итог: Вася успел на работу и успел сделать все дела.");
    }

    static int LieInBed()
    {
        return DoAction("Просыпается и лежит в кровати", 15);
    }

    static int PutKettleOnStove()
    {
        return DoAction("Чайник кипит после того, как Вася поставил его на плиту", 5);
    }

    static int HeatDinner()
    {
        return DoAction("Вчерашний ужин подогревается", 5);
    }

    static int FillBath()
    {
        return DoAction("Ванна набирается", 10);
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
        return await DoActionAsync("Чайник кипит после того, как Вася поставил его на плиту", 5);
    }

    static async Task<int> HeatDinnerAsync()
    {
        return await DoActionAsync("Вчерашний ужин подогревается", 5);
    }

    static async Task<int> FillBathAsync()
    {
        return await DoActionAsync("Ванна набирается", 10);
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

    static void PrintResult(string title, int totalMinutes)
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