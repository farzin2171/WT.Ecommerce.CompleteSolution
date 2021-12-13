using System.Reflection;


namespace WT.Libraries.Logging
{
    public class ProductInformation
    {
        public ProductInformation()
        {
            var assembly = Assembly.GetEntryAssembly();

            var version = assembly.GetName().Version.ToString();
            var informationVersion = assembly.GetCustomAttribute<AssemblyInformationalVersionAttribute>()?.InformationalVersion;
            var productName = assembly.GetCustomAttribute<AssemblyProductAttribute>().Product;

            Version = version;
            InformationalVersion = informationVersion;
            Name = productName;
        }

        public string Version { get; set; }
        public string InformationalVersion { get; set; }
        public string Name { get; set; }
    }
}
