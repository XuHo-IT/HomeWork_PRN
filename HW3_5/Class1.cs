namespace HW3_5
{

    public interface IWeatherStation
    {
        void RegisterObserver(IWeatherObserver observer);
        void RemoveObserver(IWeatherObserver observer);
        void NotifyObservers();

        float Temperature { get; }
        float Humidity { get; }
        float Pressure { get; }
    }

    public interface IWeatherObserver
    {
        void Update(float temperature, float humidity, float pressure);
    }


    public class WeatherStation : IWeatherStation
    {
        private List<IWeatherObserver> _observers;
        private float _temperature;
        private float _humidity;
        private float _pressure;

        public WeatherStation()
        {
            _observers = new List<IWeatherObserver>();
        }

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

        public void NotifyObservers()
        {
            Console.WriteLine("Notifying observers...");
            foreach (var observer in _observers)
            {
                observer.Update(_temperature, _humidity, _pressure);
            }
        }

        public float Temperature => _temperature;
        public float Humidity => _humidity;
        public float Pressure => _pressure;

        public void SetMeasurements(float temperature, float humidity, float pressure)
        {
            Console.WriteLine("\n--- Weather Station: Weather measurements updated ---");
            _temperature = temperature;
            _humidity = humidity;
            _pressure = pressure;

            Console.WriteLine($"Temperature: {_temperature}°C");
            Console.WriteLine($"Humidity: {_humidity}%");
            Console.WriteLine($"Pressure: {_pressure} hPa");

            NotifyObservers();
        }
    }


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

    public class StatisticsDisplay : IWeatherObserver
    {
        private List<float> _temperatures = new List<float>();

        public StatisticsDisplay(IWeatherStation station)
        {
            station.RegisterObserver(this);
        }

        public void Update(float temperature, float humidity, float pressure)
        {
            _temperatures.Add(temperature);
        }

        public void Display()
        {
            float max = float.MinValue;
            float min = float.MaxValue;
            float sum = 0;

            foreach (var temp in _temperatures)
            {
                if (temp > max) max = temp;
                if (temp < min) min = temp;
                sum += temp;
            }

            float avg = _temperatures.Count > 0 ? sum / _temperatures.Count : 0;
            Console.WriteLine($"Statistics: Max Temp: {max}°C, Min Temp: {min}°C, Avg Temp: {avg:F2}°C");
        }
    }

    public class ForecastDisplay : IWeatherObserver
    {
        private float _lastPressure = 1013.1f;
        private float _currentPressure;

        public ForecastDisplay(IWeatherStation station)
        {
            _currentPressure = _lastPressure;
            station.RegisterObserver(this);
        }

        public void Update(float temperature, float humidity, float pressure)
        {
            _lastPressure = _currentPressure;
            _currentPressure = pressure;
        }

        public void Display()
        {
            string forecast;
            if (_currentPressure > _lastPressure)
            {
                forecast = "Improving weather on the way!";
            }
            else if (_currentPressure < _lastPressure)
            {
                forecast = "Cooler, rainy weather coming.";
            }
            else
            {
                forecast = "More of the same.";
            }

            Console.WriteLine($"Forecast: {forecast}");
        }
    }


    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Observer Pattern Homework - Weather Station\n");

            try
            {
                WeatherStation weatherStation = new WeatherStation();

                Console.WriteLine("Creating display devices...");
                CurrentConditionsDisplay currentDisplay = new CurrentConditionsDisplay(weatherStation);
                StatisticsDisplay statisticsDisplay = new StatisticsDisplay(weatherStation);
                ForecastDisplay forecastDisplay = new ForecastDisplay(weatherStation);

                Console.WriteLine("\nSimulating weather changes...");

                weatherStation.SetMeasurements(25.2f, 65.3f, 1013.1f);
                Console.WriteLine("\n--- Displaying Information ---");
                currentDisplay.Display();
                statisticsDisplay.Display();
                forecastDisplay.Display();

                weatherStation.SetMeasurements(28.5f, 70.2f, 1012.5f);
                Console.WriteLine("\n--- Displaying Updated Information ---");
                currentDisplay.Display();
                statisticsDisplay.Display();
                forecastDisplay.Display();

                weatherStation.SetMeasurements(22.1f, 90.7f, 1009.2f);
                Console.WriteLine("\n--- Displaying Final Information ---");
                currentDisplay.Display();
                statisticsDisplay.Display();
                forecastDisplay.Display();

                Console.WriteLine("\nRemoving CurrentConditionsDisplay...");
                weatherStation.RemoveObserver(currentDisplay);

                weatherStation.SetMeasurements(24.5f, 80.1f, 1010.3f);
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
}


