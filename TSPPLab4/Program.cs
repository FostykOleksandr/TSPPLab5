using System;
using System.Collections.Generic;

namespace Lab4
{
    // 1 Factory Method
    public abstract class Report
    {
        public abstract void Generate();
    }

    public class PdfReport : Report
    {
        public override void Generate() => Console.WriteLine("Генерація звіту у форматі PDF...");
    }

    public class HtmlReport : Report
    {
        public override void Generate() => Console.WriteLine("Генерація звіту у форматі HTML...");
    }

    public class CsvReport : Report
    {
        public override void Generate() => Console.WriteLine("Генерація звіту у форматі CSV...");
    }

    public abstract class ReportFactory
    {
        public abstract Report CreateReport();
    }

    public class PdfReportFactory : ReportFactory
    {
        public override Report CreateReport() => new PdfReport();
    }

    public class HtmlReportFactory : ReportFactory
    {
        public override Report CreateReport() => new HtmlReport();
    }

    public class CsvReportFactory : ReportFactory
    {
        public override Report CreateReport() => new CsvReport();
    }

    // 2 Composite 

    public interface IMenuComponent
    {
        void Display();
    }

    public class MenuItem : IMenuComponent
    {
        private string _name;

        public MenuItem(string name) { _name = name; }

        public void Display() => Console.WriteLine($"  Елемент меню: {_name}");
    }

    public class MenuCategory : IMenuComponent
    {
        private List<IMenuComponent> _children = new List<IMenuComponent>();
        private string _name;

        public MenuCategory(string name) { _name = name; }

        public void Add(IMenuComponent component) => _children.Add(component);

        public void Display()
        {
            Console.WriteLine($"Категорія меню: {_name}");
            foreach (var child in _children)
            {
                child.Display();
            }
        }
    }

    // 3 Strategy 
    public interface IDiscountStrategy
    {
        void ApplyDiscount(double amount);
    }

    public class SeasonalDiscount : IDiscountStrategy
    {
        public void ApplyDiscount(double amount) =>
            Console.WriteLine($"Застосовано сезонну знижку (10%). Кінцева сума: {amount * 0.9} грн.");
    }

    public class LoyaltyDiscount : IDiscountStrategy
    {
        public void ApplyDiscount(double amount) =>
            Console.WriteLine($"Застосовано знижку постійного клієнта (15%). Кінцева сума: {amount * 0.85} грн.");
    }

    public class PromotionalDiscount : IDiscountStrategy
    {
        public void ApplyDiscount(double amount) =>
            Console.WriteLine($"Застосовано акційну знижку (-50 грн.). Кінцева сума: {(amount > 300 ? amount - 50 : amount)} грн.");
    }

    public class DiscountContext
    {
        private IDiscountStrategy _strategy;

        public void SetStrategy(IDiscountStrategy strategy) => _strategy = strategy;

        public void ExecuteDiscount(double amount) => _strategy.ApplyDiscount(amount);
    }

    // Тестування
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("1. ТЕСТУВАННЯ ПАТЕРНУ: FACTORY METHOD");
            ReportFactory factory;

            factory = new PdfReportFactory();
            Report pdf = factory.CreateReport();
            pdf.Generate();

            factory = new HtmlReportFactory();
            Report html = factory.CreateReport();
            html.Generate();

            factory = new CsvReportFactory();
            Report csv = factory.CreateReport();
            csv.Generate();

            Console.WriteLine("\n2. ТЕСТУВАННЯ ПАТЕРНУ: COMPOSITE");
            MenuCategory mainMenu = new MenuCategory("Головне меню");
            MenuCategory userSection = new MenuCategory("Особистий кабінет");

            mainMenu.Add(new MenuItem("Головна сторінка"));

            userSection.Add(new MenuItem("Профіль користувача"));
            userSection.Add(new MenuItem("Налаштування аккаунту"));

            mainMenu.Add(userSection);
            mainMenu.Add(new MenuItem("Контакти"));

            mainMenu.Display();

            Console.WriteLine("\n3. ТЕСТУВАННЯ ПАТЕРНУ: STRATEGY");
            DiscountContext context = new DiscountContext();
            double orderAmount = 500.0;

            Console.WriteLine($"Початкова сума замовлення: {orderAmount} грн.");

            context.SetStrategy(new SeasonalDiscount());
            context.ExecuteDiscount(orderAmount);

            context.SetStrategy(new LoyaltyDiscount());
            context.ExecuteDiscount(orderAmount);

            context.SetStrategy(new PromotionalDiscount());
            context.ExecuteDiscount(orderAmount);
        }
    }
}