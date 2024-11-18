using System;

class Interpolation
{
    // Задана функція
    static double Function(double x) => 2 * Math.Pow(x, 7) - 2 * Math.Pow(x, 5) + 3 * Math.Pow(x, 3) - 1;

    // Похідна від заданої функції
    static double DerivativeFunction(double x) => 14 * Math.Pow(x, 6) - 10 * Math.Pow(x, 4) + 9 * Math.Pow(x, 2);

    // Генерація п'яти вузлів для знаходження інтерполяційного поліному Ньютона
    static double[] GenerateNodes(int n, double a, double b)
    {
        int nodesCount = n;
        double[] nodes = new double[nodesCount];
        for (int i = 0; i < nodesCount; i++)
        {
            nodes[i] = a + i * (b - a) / (nodesCount - 1);
        }
        return nodes;
    }

    // Розділені різниці для знаходження інтерполяційного поліному Ньютона
    static double[] DividedDifferences(double[] x, double[] y)
    {
        int n = x.Length;
        double[] divDiff = new double[n];
        Array.Copy(y, divDiff, n);

        for (int j = 1; j < n; j++)
        {
            for (int i = n - 1; i >= j; i--)
            {
                divDiff[i] = (divDiff[i] - divDiff[i - 1]) / (x[i] - x[i - j]);
            }
        }
        return divDiff;
    }

    // Побудова інтерполяційного поліному Ньютона
    static string NewtonPolynomial(double[] x, double[] divDiff)
    {
        string polynomial = $"{divDiff[0]:0.###}";
        string product = "";

        for (int i = 1; i < x.Length; i++)
        {
            product += $"(x - {x[i - 1]:0.###})";
            polynomial += $" + {divDiff[i]:0.###} * {product}";
        }

        return polynomial;
    }

    // Побудова квадратичного сплайну
    static (double a, double b, double c)[] BuildQuadraticSpline(double[] x)
    {
        int n = x.Length - 1; 
        var splines = new (double a, double b, double c)[n];

        // Коефіцієнти для другого інтервалу [x[1], x[2]] = [3, 5]
        splines[0].a = Function(x[0]);
        splines[0].b = DerivativeFunction(x[0]);
        splines[0].c = (Function(x[1]) - splines[0].a - 2 * splines[0].b) / 4;

        // Коефіцієнти для другого інтервалу [x[1], x[2]] = [5, 7]
        splines[1].a = Function(x[1]);
        splines[1].b = splines[0].b + 4 * splines[0].c;
        splines[1].c = (Function(x[2]) - splines[1].a - 2 * splines[1].b) / 4;

        return splines;
    }

    static void PrintQuadraticSpline((double a, double b, double c)[] splines, double[] x)
    {
        for (int i = 0; i < splines.Length; i++)
        {
            Console.WriteLine($"- На інтервалі [{x[i]}, {x[i + 1]}] сплайн S{i + 1}(x) :");
            Console.WriteLine($"S{i + 1}(x) = {splines[i].a} + {splines[i].b} * (x - {x[i]}) + {splines[i].c} * (x - {x[i]})^2\n");
        }
    }

    // Побудова лінійного сплайну
    static void LinearSpline()
    {
        double start = 3.0;
        double end = 7.0;
        double step = 0.5;

        int n = (int)((end - start) / step) + 1;
        double[] x = new double[n];
        double[] y = new double[n];

        for (int i = 0; i < n; i++)
        {
            x[i] = start + i * step;
            y[i] = Function(x[i]);
        }

        Console.WriteLine("Лінійні інтерполяційні функції на кожному інтервалі: ");
        for (int i = 0; i < n - 1; i++)
        {
            double a = y[i];
            double b = (y[i + 1] - y[i]) / (x[i + 1] - x[i]);
            Console.WriteLine($"На інтервалі [{x[i]}, {x[i + 1]}]: S(x) = {a} + {b} * (x - {x[i]})");
        }
    }

    static void Main()
    {
        Console.OutputEncoding = System.Text.Encoding.UTF8;

        double a = 3, b = 7;
        int n = 5;
        double[] x = GenerateNodes(n, a, b);
        double[] y = new double[x.Length];
        for (int i = 0; i < x.Length; i++)
        {
            y[i] = Function(x[i]);
        }

        double[] divDiff = DividedDifferences(x, y);

        Console.WriteLine("f(x) = 2*x^7 - 2*x^5 + 3*x^3 - 1 ");
        Console.WriteLine("Інтерполяційний поліном Ньютона за 5 вузлами: ");
        Console.WriteLine(NewtonPolynomial(x, divDiff));

        Console.WriteLine("\n-----------------------------------------------------------------------------------------------------\n");

        double[] xspline = [3, 5, 7];
        
        var splines = BuildQuadraticSpline(xspline);

        Console.WriteLine("Квадратичний сплайн за точками x=3, 5, 7:");
        PrintQuadraticSpline(splines, xspline);

        Console.WriteLine("\n-----------------------------------------------------------------------------------------------------\n");

        LinearSpline();
    }
}