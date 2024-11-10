using System;

namespace ExampleNamespace
{
    class ExampleClass
    {
        public void MethodA()
        {
            Console.WriteLine("ExampleClass MethodA");
        }
    }

    class ExampleClassDuplicate
    {
        public void MethodA()
        {
            Console.WriteLine("ExampleClass MethodA");
        }
    }

    class AnotherClass
    {
        public void MethodB()
        {
            Console.WriteLine("AnotherClass MethodB");
        }
    }

    class Animal
    {
        public void Eat()
        {
            Console.WriteLine("Eating...");
        }

        public void Sleep()
        {
            Console.WriteLine("Sleeping...");
        }
    }

    class Dog
    {
        public void Eat()
        {
            Console.WriteLine("Eating...");
        }

        public void Sleep()
        {
            Console.WriteLine("Sleeping...");
        }

        public void Bark()
        {
            Console.WriteLine("Barking...");
        }
    }

    class Cat
    {
        public void Eat()
        {
            Console.WriteLine("Eating...");
        }

        public void Sleep()
        {
            Console.WriteLine("Sleeping...");
        }

        public void Meow()
        {
            Console.WriteLine("Meowing...");
        }
    }
}