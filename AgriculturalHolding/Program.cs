using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgriculturalHolding
{
    //для оперативности чтения и анализа классы и интерфейсы по новым файлам не разносились:)
    public static class MyRandom
    {
        public static Random Rand { get; set; }

        static MyRandom()
        {
            Rand = new Random();
        }
    }


    public class Subsidy
    {
        public decimal GetSubsidy()
        {
            return MyRandom.Rand.Next(500000, 1000000);
        }
    }


    interface IAbout // Нарушение I в SOLID (в данном интерфейсе - несколько "контрактов", 
                     //логичней разбить на 2 самостоятельных интерфейса)
    {
        void AboutHolding();
        void AboutFarm();
    }

    public class Holding : Farm, IAbout
    {
        public string HoldingName { get; set; }
        public int PositionInCompetitionTable { get; set; }
        /*private readonly*/ Subsidy _subsidy; // O из SOLID? Поле открыто, возможно изменение
        public Holding()
        {

        }
        // нарушение D по SOLID. Не используется абстракция (интерфейс, IoC)
        public Holding(string holdingName, int posistionInCompetition, Subsidy subsidy)
        {
            HoldingName = holdingName;
            PositionInCompetitionTable = posistionInCompetition;
            _subsidy = subsidy;
        }
        public List<Farm> Farms { get; set; }

        //                               По причине нарушения принципа I из SOLID,
        //                               метод AboutHolding() искусственным образом,
        //                               чтобы сработал "контракт" имплементированного интерфейса
        //                               был определен в классе Farm, вынужденное,
        //                               переопределение методов без имеющейся на то необходимости
        public override void AboutHolding()
        {
            Console.WriteLine($"This is an agricultural holding {HoldingName}. " +
                $"The holding consists of {Farms.Count} sucessful farms. It's position in the competition table of the region is {PositionInCompetitionTable}! " +
                $"But...the secret of the holding is that because it got {_subsidy.GetSubsidy()} $" +
                $" subsidy last year:)");
        }

        public override void AboutFarm()
        {
            Console.WriteLine();
        }

        public new void Motto() // Аналогично. L. Функционал метод базового класса перекрыт реализацией наследника
        {
            Console.WriteLine(@"It's motto is: ""Washed cows - half of a battle:)""");
        }        
        

    }

    public class Farm : IAbout // "I" некорректное проектирование интерфейса провоцирует к дублежу ненужных методов
    {
        public string FarmName { get; set; }

        public Holding Holding { get; set; }

        public List<AnimalsAndPlants> AnimalsAndPlants { get; set; }
        public virtual void AboutFarm()
        {
            Console.WriteLine();
        }

        public virtual void AboutHolding()
        {
            Console.WriteLine();
        }
        public void Motto()
        {
            Console.WriteLine("Slow progress is steady progress:)");
        }
    }

    public class AnimalsAndPlants //Нарушение S в SOLID (данный класс логично разбить на три самостоятельных класса) 
    {
        public string AnimalName { get; set; }
        public int AnimalQuantity { get; set; }
        public string PlantName { get; set; }
        public int PlantQuantity { get; set; }
        public string ProductName { get; set; }
        public int ProductQuantity { get; set; }        

        public void AboutAnimalsPlantsProducts()
        {
            Console.WriteLine();
        }        

    }
    class Program
    {
        static void Main(string[] args)
        {            
            var holdingHornsAndHoves = new Holding(@"""Horns and Hoves""", 1, new Subsidy());
            holdingHornsAndHoves.Farms = new List<Farm>() { new Farm { FarmName = "Super Farm" }, new Farm { FarmName = "Super-Super Farm" } };
            holdingHornsAndHoves.AboutHolding();
            holdingHornsAndHoves.Motto();            
            Console.ReadLine();
        }
    }

}
