using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;

namespace Threading
{
	public class MultiThreading
	{
    public static int GlobalCount;
    static List<int> globalList = new List<int>();
    static int primeNumber = 2;
	  static void RandomOddNumbers()
    {
      while (globalList.Count < 1000000){
        Random rng = new Random();
        int randOddNum = rng.Next(1000000); // number between 0 and 1000000
        if(randOddNum % 2 == 0){
         randOddNum++;
         globalList.Add(randOddNum);
        }else{
          globalList.Add(randOddNum);
        }
      }
    }
    static bool isPrime(int number){
      if(number == 2 || number == 3){
        return true;
      }
      else if(number % 2 == 0 || number <= 1||number != 2){
        return false;
      }else{
        var numberSqrt = (int)Math.Floor(Math.Sqrt(number));
        for (int i = 3; i <= numberSqrt; i += 2){
          if (number % i == 0)
          return false;
        }
        return true;
      }
    }
    static void NegativePrimes()
    {
      while (globalList.Count < 1000000){
        while(!isPrime(primeNumber)){
          primeNumber++;
        }
      }
      if (globalList.Count == 250000)
      {
        Thread thread3 = new Thread(EvenNumbers);
        thread3.Start();
      }
    }
    static void EvenNumbers(){
      int primeNo = 2;
      if(primeNo % 2 == 0){
        globalList.Add(primeNo*-1);
      }
      
    }
		public static void Main(string[] args)
		{
      Thread thread1 = new Thread(new ThreadStart(RandomOddNumbers));
      Thread thread2 = new Thread(new ThreadStart(NegativePrimes));
      thread1.Start();
      thread2.Start();
      thread1.Join();
      thread2.Join();
      globalList.Sort();
      int oddCount = globalList.FindAll(x => x % 2 != 0).Count;
      int evenCount = globalList.FindAll(x => x % 2 == 0).Count;
      Console.WriteLine("Odd number count: " + oddCount);
      Console.WriteLine("Even number count: " + evenCount);
		}
	}
}
