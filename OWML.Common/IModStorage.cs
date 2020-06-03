namespace OWML.Common
{
    public interface IModStorage
    {
        /// <summary>Deserialize JSON file to given type.</summary>
        /// <typeparam name="T">The type to deserialize the file to.</typeparam>
        /// <param name="filename">The name of the file. The folder that the mod is in is automatically added to the beginning.</param>
        T Load<T>(string filename);

        /// <summary>Serialize object to JSON file.</summary>
        /// <typeparam name="T">The type to serialize.</typeparam>
        /// <param name="filename">The name of the output file. The folder that the mod is in is automatically added to the beginning.</param>
        void Save<T>(T obj, string filename);
    }
}
