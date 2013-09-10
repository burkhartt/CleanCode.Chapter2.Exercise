using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace CleanCodeChapter2 {
    internal class Program {
        private static void Main(string[] args) {
            var carFactory = new SlowFactory();
            var car = carFactory.Create();

            var fastCarFactory = new FastFactory();
            var fastCar = fastCarFactory.Create();

            var cars = Cars(car, fastCar);

            var carString = LoadString(cars);
            Console.WriteLine("The drivers are: " + carString);

            for (var i = 0; i < cars.Length; i++) {
                var timer = new Stopwatch();
                timer.Start();
                if (cars[0].CarRacingNumber == 32) {
                    cars[i].Kablammo();
                }
                try {
                    if (cars[i].IsAlive)
                    {
                        cars[i].Drive();
                        Console.WriteLine("Car " + i + " (Driver " + cars[i].CarDriverNameString + ") completed a mile in " + timer.ElapsedMilliseconds + " seconds");
                    }
                    else
                    {
                        Console.WriteLine(cars[i].CarDriverNameString + " was killed :*-(");
                    }
                } catch (BadNewsBearsException bnbe) {
                    Console.WriteLine("An error occurred. Investigate!");
                }
                
                timer.Stop();
            }

            Console.ReadKey();
        }

        private static string LoadString(Car[] cars) {
            var str = "";
            var c = "";
            for (var i = 0; i < cars.Length; i++) {
                if (i < cars.Length - 1) {
                    c = ",";
                    str += cars[i].CarDriverNameString;
                } else {
                    str += cars[i].CarDriverNameString;
                    c = "";
                }
                str += c;
            }
            return str;
        }

        private static Car[] Cars(Car car, Car fastCar) {
            var rand = new Random();
            var fastCars = rand.Next(50);
            var slowCars = rand.Next(50);
            var groupingOfAutomobilesThatCanBeLoopedThroughLaterOn = new List<Car>();
            for (var i = 0; i < fastCars; i++) {
                groupingOfAutomobilesThatCanBeLoopedThroughLaterOn.Add(fastCar);
            }
            for (var i = 0; i < slowCars; i++) {
                groupingOfAutomobilesThatCanBeLoopedThroughLaterOn.Add(car);
            }

            return groupingOfAutomobilesThatCanBeLoopedThroughLaterOn.ToArray();
        }
    }

    internal class ResultRepository {}

    internal class FastFactory : ICarFactory {
        public Car Create() {
            var rand = new Random();
            return new Car(rand.Next(50), "Bob");
        }
    }

    internal class SlowFactory : ICarFactory {
        public Car Create() {
            var rand = new Random();
            return new Car(rand.Next(50), "Joe");
        }
    }

    internal interface ICarFactory {
        Car Create();
    }

    internal class Car {
        private readonly int mCyl;

        public Car(int cyl, string driverNameString) {
            mCyl = cyl;
            CarDriverNameString = driverNameString;
            IsAlive = true;
        }

        public int CarRacingNumber { get; set; }
        public bool IsAlive { get; set; }
        public string CarDriverNameString { get; set; }
        public int CarCylinders { get; set; }
        public bool CarIsTurnedOn { get; set; }
        public int CarMaxSpeed { get; set; }

        public void Drive() {
            var distance = 0.00;
            while (distance < 5280) {
                distance += 0.0001*mCyl;
                if (!Check()) {
                    throw new BadNewsBearsException("Definitely not good news bears!");
                }
            }
        }

        private bool Check() {
            var rand = new Random();
            var next = rand.Next(100);
            if (next == 51) {
                IsAlive = false;
            }
            return IsAlive;
        }

        public void Kablammo() {
            IsAlive = false;
        }
    }

    internal class BadNewsBearsException : Exception {
        private readonly string badNewsBears;

        public BadNewsBearsException(string badNewsBears) {
            this.badNewsBears = badNewsBears;
        }
    }
}