using System;

//    {X}= x1,x2,…,xn, представляющих собой реализацию равномерно распределенной на интервале(0,1)
//    случайной величины ξ,  или - в статистических терминах - повторную выборку из равномерно 
//    распределенной на(0,1) генеральной совокупности значений величины ξ.

// M == {Expected value}
// D == {Variance}
// σ == {Standard deviation}

//имеет равномерное распределение в интервале(a, b), то
// M == (a+b) /2                            || 0.5     
// D == sqr(b-a) /12                        || 1 /12
// σ == (b-a) /sqrt(12)                     || 1 /sqrt(12)


namespace SaimmodOne
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
        }
    }
}
