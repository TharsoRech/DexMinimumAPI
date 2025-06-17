namespace DexMinimumApi.Configuration;

    public sealed class AppSettings
    {
        private static readonly Lazy<AppSettings> _instance =
            new Lazy<AppSettings>(() => new AppSettings());

        public static AppSettings Instance
        {
            get { return _instance.Value; }
        }

        public string SqlConnection { get; private set; }

        // Private constructor to prevent instantiation from outside
        private AppSettings() { }

        // Method to initialize the settings
        public void Load(IConfiguration configuration)
        {
            SqlConnection = configuration.GetConnectionString("SqlConnection");                                                                  // Load other settings as needed
        }
    }
