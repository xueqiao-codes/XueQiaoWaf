using lib.configuration.Sources;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lib.configuration
{
    /// <summary>
    /// A configuration manager that loads, saves settings from
    /// sources. Create configuration objects of
    /// type <typeparam name="T"></typeparam>.
    /// </summary>
    /// <typeparam name="T">Strongly typed Configuration object</typeparam>
    public class ConfigurationManager<T> where T : class, new()
    {
        private readonly HashSet<string> propertiesAlwaysSerialized = new HashSet<string>();

        private readonly HashSet<string> propertiesNeverSerialized = new HashSet<string>();

        private IConfigurationSource<T> confSource;

        /// <summary>
        /// The object to hold configuration content. This object never be null.
        /// </summary>
        public T Config { get; private set; }

        /// <summary>
        /// The exception when load configuration.
        /// </summary>
        public Exception LoadConfigException { get; private set; }

        /// <summary>
        /// Create a ConfigurationManager loading its information
        /// from a XML configuration file parsed by
        /// <see cref="XmlFileConfigurationSource{T}"/> source.
        /// Convenience method when only using XML file for storage.
        /// </summary>
        /// <param name="configFileName">Filename to load configuration.</param>
        public ConfigurationManager(string configFileName)
            : this(new XmlFileConfigurationSource<T>(configFileName))
        {
        }

        /// <summary>
        /// Create a ConfigurationManager, loading its information
        /// from an ordered list of sources.
        /// 
        /// If no source is marked as a PrimarySource, the last
        /// source in the sequence will be assumed to be primary.
        /// </summary>
        /// <exception cref="ArgumentNullException">
        /// paramater `source` is null.
        /// </exception>
        /// <param name="sources"></param>
        public ConfigurationManager(IConfigurationSource<T> source)
        {
            if (source == null) throw new ArgumentNullException("source");

            confSource = source;
            T _loadedConfig = null;
            
            try
            {
                _loadedConfig = source.Config;
            }
            catch (Exception e)
            {
                LoadConfigException = e; 
            }

            if (_loadedConfig == null)
                Config = new T();
            else
                Config = _loadedConfig;
        }

        /// <summary>
        /// Tell the configuration manager to always serialize this property.
        /// If the property is currently marked as "never serialize", that
        /// status will be removed.
        /// </summary>
        /// <param name="propName"></param>
        /// <returns></returns>
        public ConfigurationManager<T> AlwaysSerializeProp(string propName)
        {
            propertiesAlwaysSerialized.Add(propName);
            propertiesNeverSerialized.Remove(propName);
            return this;
        }

        /// <summary>
        /// Tell the configuration manager to never serialize this property.
        /// If the property is currently marked as "always serialize", that
        /// status will be removed.
        /// </summary>
        /// <param name="propName"></param>
        /// <returns></returns>
        public ConfigurationManager<T> NeverSerializeProp(string propName)
        {
            propertiesNeverSerialized.Add(propName);
            propertiesAlwaysSerialized.Remove(propName);
            return this;
        }

        /// <summary>
        /// Save current <seealso cref="Config"/>
        /// </summary>
        /// <param name="saveException">Exception when save config</param>
        public ConfigurationManager<T> SaveConfig(out Exception saveException)
        {
            return SaveConfig(this.Config, out saveException);
        }

        /// <summary>
        /// Save given config. And update current <see cref="Config"/> as given config value
        /// </summary>
        /// <param name="config">Given value to update current <see cref="Config"/></param>
        /// <param name="saveException">Exception when save config</param>
        /// <returns></returns>
        public ConfigurationManager<T> SaveConfig(T config, out Exception saveException)
        {
            if (config == null) { throw new ArgumentNullException("config"); }

            var _conf = config;
            saveException = null;
            var saveProps = typeof(T).GetProperties().Select(p => p.Name)
                     .Concat(propertiesAlwaysSerialized)
                     .Except(propertiesNeverSerialized);
            try
            {
                confSource.PartialSave(_conf, saveProps);

                // Update current config when save success
                this.Config = _conf;
            }
            catch (Exception e)
            {
                saveException = e;
            }
            return this;
        }
    }
}
