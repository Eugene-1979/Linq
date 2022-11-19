using System.Drawing;
using System.Text;

namespace Linq
    {
    internal class Program
        {

static Random rand = new Random();

/*количество людей в списке*/
        static int PeopleCount = 10;

        /*количество друзей у каждого человека*/
        static int Frend = 2;
        static void Main(string[] args)
            {


           

            

            /*Создаем людей*/
            List<People> listPeople = CreatePeople(rand, PeopleCount);

            /* Создаем  друзей, при этом дружба не повторяется*/
            Frendly(listPeople, Frend, rand);


            /* Віводим people по крайним координатам*/


            Console.WriteLine($@"south-{listPeople.MinBy(p => p.Location.Y)}
north-{listPeople.MaxBy(p => p.Location.Y)}
west-{listPeople.MinBy(p => p.Location.X)}
east-{listPeople.MaxBy(p => p.Location.X)} ");

            Console.WriteLine(new String('-', 80));

            /* --------------------------------------------------------------------*/

            /* находим макс и мин между людьми*/
            /*Max*/
            var max = listPeople.
             AsParallel().
             Select(
             /*преобразовіваем каждого человека к расстоянию побегая циклом*/
             q => listPeople.Max(
             /* находим макс из макс*/
             lp => Distantion(q, lp))).Max();


             /*Min*/
            var min = listPeople.
            AsParallel().
            Select(
            /*преобразовіваем каждого человека к расстоянию побегая циклом*/
            q => listPeople.
            Select(lp => Distantion(q, lp)).
            /* Сортируем и берем 2 єлемент,
             т.к 1й минимальній это 0 -пересечение самого с собой */
            OrderBy(p => p).
            ElementAt(1)
            ).Min();

            Console.WriteLine($"Max {max} Min {min}");



            /*   ---------------------------------------------------------------------------------------*/
            /*   найди 2 человека, чье слово "about" имеет больше всего одинаковых слов*/

            /* сделал список (слово-имя)-Count*/  /*groupby-по 2 ключам*/
            var enumerable = listPeople.SelectMany(q => q.About.Split(" ").Select(w => new { word = w, name = q.Name })).
            GroupBy(q => q, w => w, (q, w) => new { words = q.word, names = q.name, count = w.Count() });


           /* фильтруем-сортируем-находим 2Х*/
            var enumerable1 = enumerable.Distinct().OrderBy(q => q.count).TakeLast(2);


            Console.WriteLine(new String('-',80));

            foreach (var item in enumerable1)
                {
                Console.WriteLine($"слово-{item.words}  у человека-{item.names}  встретилось-{item.count}");
                }

            Console.WriteLine(new String('-', 80));







           /* ------------------------------------------------------------------------*/

            /*           найти людей с одинаковыми друзьями(сравните по имени друга)*/


            var parallelQuery = listPeople.
            AsParallel().
            Select(p => new { Name = p.Freands.Select(f => f.Name).OrderBy(q => q).Aggregate((q, w) => q + ',' + w), peopl = p.Name });


            Console.WriteLine(@"Выводим список ,где колекция друзей преобразовались 
            в отсортированній string"+Environment.NewLine);
            foreach (var item in parallelQuery)
                {

                Console.WriteLine(item.Name + "--- " + item.peopl);

                }
            Console.WriteLine("группируем по друзьям и находим у кого совпали друзья");
            var parallelQuery1 = parallelQuery.
            GroupBy(q => q.Name).
            Where(q => q.Count() > 1);

            foreach (var item in parallelQuery1)
                {
                Console.WriteLine(item.Key);
                foreach (var item1 in item)
                    {
                    Console.WriteLine("\t"+item1.peopl);
                    }
                }








            }

            /*функция вічисления дистанции по теор пифагора*/
        static Func<People, People, int> Distantion = (p1, p2) =>
        (int)Math.Sqrt(Math.Pow((p1.Location.X - p2.Location.X), 2) +
        Math.Pow((p1.Location.Y - p2.Location.Y), 2));


        /*генератор друзей*/
        static void Frendly(List<People> peoples, int count, Random rnd)
            {

            foreach (var item in peoples)
                {
                People temp = peoples[rnd.Next(peoples.Count)];

                for (int i = 0; i < count; i++)
                    {
                    while (item.Equals(temp) ||
                    item.Freands.Contains(temp)
                    )
                        {
                        temp = peoples[rnd.Next(peoples.Count)];

                        }
                    item.Freands.Add(temp);
                    }
                }
            }

           /* генератор людей*/
        static List<People> CreatePeople(Random rnd, int count)
            {

            List<People> listPeople = new List<People>();

            for (int i = 0; i < count; i++)
                {

                People item = new People(
                $"Name{i}",
                new Point(rnd.Next(-180, 180), rnd.Next(-180, 180)),
                GenerateAbout(3, 1000, rnd));
                listPeople.Add(item);


                }
            return listPeople;

            }
      /*  создание текста About на text слов*/
        static string GenerateAbout(int words, int text, Random rnd)
            {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < text; i++)
                {
                for (int j = 0; j < words; j++)
                    {

                    sb.Append((char)rnd.Next(97, 122));

                    }
                sb.Append(' ');
                }
            return sb.ToString();


            }
        }
    }