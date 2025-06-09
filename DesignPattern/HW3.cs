using System;
using System.Collections.Generic;

namespace DesignPatterns.Homework
{
    // This homework is based on the Observer Pattern
    // Students will implement a weather monitoring system using the Observer pattern

    #region Observer Pattern Interfaces

    // The subject interface that all weather stations must implement
    public interface IWeatherStation
    {
        // Methods to manage observers
        void RegisterObserver(IWeatherObserver observer);
        void RemoveObserver(IWeatherObserver observer);
        void NotifyObservers();

        // Weather data properties
        float Temperature { get; }
        float Humidity { get; }
        float Pressure { get; }
    }

    // The observer interface that all display devices must implement
    public interface IWeatherObserver
    {
        void Update(float temperature, float humidity, float pressure);
    }

    #endregion

    #region Weather Station Implementation

    // Concrete implementation of a weather station
    public class WeatherStation : IWeatherStation
    {
        // List to store all registered observers
        private List<IWeatherObserver> _observers;

        // Weather data
        private float _temperature;
        private float _humidity;
        private float _pressure;

        // Constructor
        public WeatherStation()
        {
            _observers = new List<IWeatherObserver>();
        }

        // Methods to register and remove observers
        public void RegisterObserver(IWeatherObserver observer)
        {
            _observers.Add(observer);
            Console.WriteLine("Observer registered.");
        }

        public void RemoveObserver(IWeatherObserver observer)
        {
            _observers.Remove(observer);
            Console.WriteLine("Observer removed.");
        }

        // Method to notify all observers when weather data changes
        public void NotifyObservers()
        {
            Console.WriteLine("Notifying observers...");
            foreach (var observer in _observers)
            {
                observer.Update(_temperature, _humidity, _pressure);
            }
        }

        // Properties to access weather data
        public float Temperature => _temperature;
        public float Humidity => _humidity;
        public float Pressure => _pressure;

        // Method to update weather data and notify observers
        public void SetMeasurements(float temperature, float humidity, float pressure)
        {
            Console.WriteLine("\n--- Weather Station: Weather measurements updated ---");

            // Update weather data
            _temperature = temperature;
            _humidity = humidity;
            _pressure = pressure;

            Console.WriteLine($"Temperature: {_temperature}°C");
            Console.WriteLine($"Humidity: {_humidity}%");
            Console.WriteLine($"Pressure: {_pressure} hPa");

            // Notify observers of the change
            NotifyObservers();
        }
    }

    #endregion

    #region Observer Implementations

    // CurrentConditionsDisplay
    public class CurrentConditionsDisplay : IWeatherObserver
    {
        private float _temperature;
        private float _humidity;
        private float _pressure;

        public CurrentConditionsDisplay(IWeatherStation station)
        {
            station.RegisterObserver(this);
        }

        public void Update(float temperature, float humidity, float pressure)
        {
            _temperature = temperature;
            _humidity = humidity;
            _pressure = pressure;
        }

        public void Display()
        {
            Console.WriteLine($"Current Conditions: {_temperature}°C, {_humidity}% humidity, {_pressure} hPa");
        }
    }

    // StatisticsDisplay
    public class StatisticsDisplay : IWeatherObserver
    {
        private float _minTemp = float.MaxValue;
        private float _maxTemp = float.MinValue;
        private float _sumTemp = 0;
        private int _numReadings = 0;

        public StatisticsDisplay(IWeatherStation station)
        {
            station.RegisterObserver(this);
        }

        public void Update(float temperature, float humidity, float pressure)
        {
            if (temperature < _minTemp) _minTemp = temperature;
            if (temperature > _maxTemp) _maxTemp = temperature;
            _sumTemp += temperature;
            _numReadings++;
        }

        public void Display()
        {
            float avg = _numReadings > 0 ? _sumTemp / _numReadings : 0;
            Console.WriteLine($"Statistics: Min Temp = {_minTemp}°C, Max Temp = {_maxTemp}°C, Avg Temp = {avg:F1}°C");
        }
    }

    // ForecastDisplay
    public class ForecastDisplay : IWeatherObserver
    {
        private float _lastPressure = 0;
        private float _currentPressure = 0;
        private string _forecast = "No forecast";

        public ForecastDisplay(IWeatherStation station)
        {
            station.RegisterObserver(this);
        }

        public void Update(float temperature, float humidity, float pressure)
        {
            _lastPressure = _currentPressure;
            _currentPressure = pressure;
            if (_lastPressure == 0)
            {
                _forecast = "No forecast";
            }
            else if (_currentPressure > _lastPressure)
            {
                _forecast = "Improving weather";
            }
            else if (_currentPressure < _lastPressure)
            {
                _forecast = "Cooler, rainy weather";
            }
            else
            {
                _forecast = "Same weather";
            }
        }

        public void Display()
        {
            Console.WriteLine($"Forecast: {_forecast}");
        }
    }

    #endregion

    #region Application

    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Observer Pattern Homework - Weather Station\n");

            try
            {
                // Create the weather station (subject)
                WeatherStation weatherStation = new WeatherStation();

                // Create display devices (observers)
                Console.WriteLine("Creating display devices...");
                CurrentConditionsDisplay currentDisplay = new CurrentConditionsDisplay(weatherStation);
                StatisticsDisplay statisticsDisplay = new StatisticsDisplay(weatherStation);
                ForecastDisplay forecastDisplay = new ForecastDisplay(weatherStation);

                // Simulate weather changes
                Console.WriteLine("\nSimulating weather changes...");

                // Initial weather
                weatherStation.SetMeasurements(25.2f, 65.3f, 1013.1f);

                // Display information from all displays
                Console.WriteLine("\n--- Displaying Information ---");
                currentDisplay.Display();
                statisticsDisplay.Display();
                forecastDisplay.Display();

                // Weather change 1
                weatherStation.SetMeasurements(28.5f, 70.2f, 1012.5f);

                // Display updated information
                Console.WriteLine("\n--- Displaying Updated Information ---");
                currentDisplay.Display();
                statisticsDisplay.Display();
                forecastDisplay.Display();

                // Weather change 2
                weatherStation.SetMeasurements(22.1f, 90.7f, 1009.2f);

                // Display updated information again
                Console.WriteLine("\n--- Displaying Final Information ---");
                currentDisplay.Display();
                statisticsDisplay.Display();
                forecastDisplay.Display();

                // Test removing an observer
                Console.WriteLine("\nRemoving CurrentConditionsDisplay...");
                weatherStation.RemoveObserver(currentDisplay);

                // Weather change after removing an observer
                weatherStation.SetMeasurements(24.5f, 80.1f, 1010.3f);

                // Display information without the removed observer
                Console.WriteLine("\n--- Displaying Information After Removal ---");
                statisticsDisplay.Display();
                forecastDisplay.Display();

                Console.WriteLine("\nObserver Pattern demonstration complete.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
            }

            Console.WriteLine("\nPress any key to exit...");
            Console.ReadKey();
        }
    }

    #endregion
}
