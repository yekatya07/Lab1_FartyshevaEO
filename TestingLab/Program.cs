using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestingLab
{
    internal class Program
    {
        public static (string, float[], float[], float[]) GoTriangle(string first, string second, string third)
        {
            try
            {
                float f = float.Parse(first);
                float s = float.Parse(second);
                float t = float.Parse(third); // конвертируем длины треугольников

                if (f <= 0 || s <= 0 || t <= 0)
                {
                    float[] xy1 = new float[] { -2, -2 };
                    float[] xy2 = new float[] { -2, -2 };
                    float[] xy3 = new float[] { -2, -2 };
                    string msg = $" ({xy1[0]}; {xy1[1]}), ({xy2[0]}; {xy2[1]}), ({xy3[0]}; {xy3[1]})";
                    Log.Information(msg);
                    return ("", xy1, xy2, xy3);
                }

                if(f + s >= t && f + t >= s && s + t >= f) // если конвертируется успешно, проверяем, является ли треугольиком: сумма длины двух сторон в каждом случае должна
                    // быть больше, чем длина третьей стороны
                { //случай, при котором у нас условие выполняется корректно, фигура является треугольником
                    string type = "";


                    if (f == t && s == f && s == t) type = "Равносторонний"; // определение типа треугольника по условиям
                    else if (f == s || s == t || f == t) type = "Равнобедренный";
                    else type = "Разносторонний";

                    float[] xy1 = new float[] { 0, 0 }; // первая вершина треугольника лежит в точке (0; 0)
                    float[] xy2 = new float[] {t, 0 }; // следующая по оси ординат находится в точке, а по абциссе отходит от начала координат на длину третьей стороны

                    float cosA = (s * s + t * t - f * f) / (2 * s * t); // для получения третьей координаты по формуле через длины вычислим косинус угла, лежащего между первой и второй сторонами
                    float sinA = (float)Math.Sqrt(1 - cosA * cosA); // по формуле вычислим синус
                    float height = s * sinA; // также по формуле находим высоту по формуле синуса и прилежащей длины

                    float temp = (t * t - s * s + f * f) / (2 * f); // находим координаты третьей вершины
                    float[] xy3 = new float[] {temp, (float)Math.Sqrt(height * height - temp * temp)};

                    if (Math.Max(xy1[0], Math.Max(xy1[1], Math.Max(xy2[0], Math.Max(xy2[1], Math.Max(xy3[0], xy3[1]))))) > 80) // если значение координат выходит за пределы
                    { // определённого значения, масштабируем
                        float scale = 100 / Math.Max(xy2[0], Math.Max(xy3[0], xy3[1])); // условно, 100 процентов делим на максимальную координату
                        xy1[0] *= scale;
                        xy1[1] *= scale; // домножаем, чтобы масштабировать
                        xy2[0] *= scale;
                        xy2[1] *= scale;
                        xy3[0] *= scale;
                        xy3[1] *= scale;
                    }

                    string msg = $"{type} ({xy1[0]}; {xy1[1]}), ({xy2[0]}; {xy2[1]}), ({xy3[0]}; {xy3[1]})";
                    Log.Information(msg);
                    return (type, xy1, xy2, xy3); // возвращаем значения, логируем
                }
                else
                {
                    float[] xy1 = new float[] { -1, -1 };
                    float[] xy2 = new float[] { -1, -1 };
                    float[] xy3 = new float[] { -1, -1 };
                    string msg = $" Не треугольник ({xy1[0]}; {xy1[1]}), ({xy2[0]}; {xy2[1]}), ({xy3[0]}; {xy3[1]})";
                    Log.Information(msg);
                    return ("Не треугольник", xy1, xy2, xy3);
                }
            }
            catch (Exception ex) // случай, при котором данные не могут быть корректно конвертированы
            {
                float[] xy1 = new float[] { -2, -2 };
                float[] xy2 = new float[] { -2, -2 };
                float[] xy3 = new float[] { -2, -2 };
                string msg = $" ({xy1[0]}; {xy1[1]}), ({xy2[0]}; {xy2[1]}), ({xy3[0]}; {xy3[1]})";
                Log.Error(ex, msg);
                return ("", xy1, xy2, xy3);
            }
        }


        static void Main(string[] args)
        {
            Console.WriteLine("Введите стороны треугольника");
            while (true)
            {
                Log.Logger = new LoggerConfiguration()
                .WriteTo.Console()
                .WriteTo.File("C:\\Users\\Ekaterina\\source\\repos\\TestingLab\\TestingLab\\file.txt")
               .CreateLogger(); // инициализация логгера

                string first = Console.ReadLine();
                string second = Console.ReadLine();
                string third = Console.ReadLine(); // ввод данных
                string type;
                float[] f = new float[2]; // переменные для хранения координат
                float[] s = new float[2];
                float[] t = new float[2];

                (type, f, s, t) = GoTriangle(first, second, third); // переход в метод, в котором происходят операции над треугольником
                // по завершению выполнения метода возвращается тип треугольника, кординаты трёх его вершин, информация логируется
                Log.CloseAndFlush(); // закрывается логгер
            }
        }
    }
}

