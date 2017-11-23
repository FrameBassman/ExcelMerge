using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExcelMerge
{
    using System.IO;

    public class Program
    {
        private List<Car> cars1;
        private List<Car> cars2;
        private List<Car> cars3;

        static void Main(string[] args)
        {
            Program program = new Program();
            program.Run();
        }

        private void Run()
        {
            cars1 = Parse(@"docs/1.txt");
            //cars2 = Parse(@"docs/2.txt");
            cars3 = Parse(@"docs/3.txt");

            foreach (Car car1 in cars1)
            {
                foreach (Car car2 in cars3)
                {
                    if (car1.Vin == car2.Vin && car1.Pts == car2.Pts)
                    {
                        car1.IsRepeated = "Both";
                        car2.IsRepeated = "Both";

                        string car1RN = car1.Number;
                        string car2RN = car2.Number;

                        car1.RepeatedNumber = car2RN;
                        car2.RepeatedNumber = car1RN;
                    }
                    else
                    {
                        if (car1.Vin == car2.Vin)
                        {
                            car1.IsRepeated = "Vin";
                            car2.IsRepeated = "Vin";
                            car1.RepeatedNumber = car2.Number;
                            car2.RepeatedNumber = car1.Number;
                        }

                        if (car1.Pts == car2.Pts)
                        {
                            car1.IsRepeated = "Pts";
                            car2.IsRepeated = "Pts";
                            car1.RepeatedNumber = car2.Number;
                            car2.RepeatedNumber = car1.Number;
                        }
                    }

                    
                }
            }

            Print(cars1, @"docs/1-result.txt");
            Print(cars3, @"docs/3-result.txt");
            cars1.Where(c => c.IsRepeated != "").Count();

        }

        private List<Car> Parse(string path)
        {
            string line = "";
            string[] buffer;

            List<Car> result = new List<Car>();
            
            using (StreamReader sr = new StreamReader(path))
            {
                while (!sr.EndOfStream)
                {
                    line = sr.ReadLine();
                    buffer = line.Split('\t');

                    var bufferCar = new Car();

                    bufferCar.Number = buffer[0].Trim(' ');
                    bufferCar.Model = buffer[1].Trim(' ');
                    bufferCar.Vin = ReplaceCyrilic(buffer[2].Trim(' '));
                    bufferCar.Pts = ReplaceLatin(buffer[3].Trim(' ').Replace(" ", ""));

                    result.Add(bufferCar);
                }
            }

            return result;
        }

        private void Print(List<Car> cars, string path)
        {
            using (StreamWriter sr = new StreamWriter(path))
            {
                foreach (var car in cars)
                {
                    sr.WriteLine(
                        car.Number + '\t' +
                        car.Model + '\t' +
                        car.Vin + '\t' +
                        car.Pts + '\t' +
                        car.IsRepeated + '\t' +
                        car.RepeatedNumber);
                }
                int result = cars.Where(c => !string.IsNullOrEmpty(c.IsRepeated)).Count();
                sr.WriteLine("Совпадений: {0}", result);
            }
        }

        private static string ReplaceLatin(string s)
        {
            return s.Replace('A', 'А').Replace('B', 'В').Replace('C', 'С').Replace('E', 'Е').Replace('H', 'Н')
                .Replace('K', 'К').Replace('M', 'М').Replace('O', 'О').Replace('P', 'Р').Replace('T', 'Т').Replace('X', 'Х');
        }

        private static string ReplaceCyrilic(string s)
        {
            return s.Replace('А', 'A').Replace('В', 'B').Replace('Е', 'E').Replace('К', 'K').Replace('М', 'M')
                .Replace('О', 'O').Replace('Р', 'P').Replace('С', 'C').Replace('Т', 'T').Replace('Х', 'X');
        }

        public int Execute(MergeSettings settings)
        {
            cars1 = Parse(settings.FirstFilePath);
            cars2 = Parse(settings.SecondFilePath);

            foreach (Car car1 in cars1)
            {
                foreach (Car car2 in cars2)
                {
                    if (car1.Vin == car2.Vin && car1.Pts == car2.Pts)
                    {
                        car1.IsRepeated = "Both";
                        car2.IsRepeated = "Both";

                        string car1RN = car1.Number;
                        string car2RN = car2.Number;

                        car1.RepeatedNumber = car2RN;
                        car2.RepeatedNumber = car1RN;
                    }
                    else
                    {
                        if (car1.Vin == car2.Vin)
                        {
                            car1.IsRepeated = "Vin";
                            car2.IsRepeated = "Vin";
                            car1.RepeatedNumber = car2.Number;
                            car2.RepeatedNumber = car1.Number;
                        }

                        if (car1.Pts == car2.Pts)
                        {
                            car1.IsRepeated = "Pts";
                            car2.IsRepeated = "Pts";
                            car1.RepeatedNumber = car2.Number;
                            car2.RepeatedNumber = car1.Number;
                        }
                    }
                }
            }

            Print(cars1, settings.FirstResultFile);
            Print(cars2, settings.SecondResultFile);
            return cars1.Count(c => !string.IsNullOrEmpty(c.IsRepeated));
        }
    }

    public class MergeSettings
    {
        public MergeSettings(string firstFilePath, string secondFilePath, string firstResultFile, string secondResultFile)
        {
            this.FirstFilePath = firstFilePath;
            this.SecondFilePath = secondFilePath;
            this.FirstResultFile = firstResultFile;
            this.SecondResultFile = secondResultFile;
        }

        public string SecondResultFile { get; set; }

        public string FirstResultFile { get; set; }

        public string SecondFilePath { get; set; }

        public string FirstFilePath { get; set; }
    }
}
