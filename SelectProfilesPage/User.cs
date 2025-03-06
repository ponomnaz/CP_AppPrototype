namespace CP_AppPrototype.SelectProfilesPage
{
    /// <summary>
    /// Represents a user profile that can be stored and retrieved from local storage.
    /// </summary>
    public class User
    {
        /// <summary>Gets or sets the filename where the user's data is stored.</summary>
        public string FileName { get; set; }

        /// <summary>Gets or sets the name of the user.</summary>
        public string Name { get; set; }

        /// <summary>Gets or sets the birth date of the user.</summary>
        public DateTime BirthDay { get; set; }

        /// <summary>Gets or sets the gender of the user.</summary>
        public Gender Gender { get; set; }

        /// <summary>Gets or sets the last modification date of the user profile.</summary>
        public DateTime ChangeDate { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="User"/> class with default values.
        /// </summary>
        public User()
        {
            FileName = $"{Path.GetRandomFileName()}.notes.txt";
            Name = string.Empty;
            BirthDay = DateTime.Now;
            Gender = Gender.Male;
            ChangeDate = DateTime.Now;
        }

        /// <summary>
        /// Saves the user data to a file in the application's storage directory.
        /// </summary>
        public void Save()
        {
            try
            {
                string filePath = Path.Combine(FileSystem.AppDataDirectory, FileName);
                string fileContent = string.Join("|", Name, BirthDay.ToString("o"), Gender);
                File.WriteAllText(filePath, fileContent);
                ChangeDate = File.GetLastWriteTime(filePath);
            }
            catch (Exception ex)
            {
                throw new IOException("Failed to save user data.", ex);
            }
        }

        /// <summary>
        /// Deletes the user's saved file from storage.
        /// </summary>
        public void Delete()
        {
            try
            {
                string filePath = Path.Combine(FileSystem.AppDataDirectory, FileName);
                if (File.Exists(filePath))
                {
                    File.Delete(filePath);
                }
            }
            catch (Exception ex)
            {
                throw new IOException("Failed to delete user data.", ex);
            }
        }

        /// <summary>
        /// Loads a user profile from a file.
        /// </summary>
        /// <param name="fileName">The name of the file to load.</param>
        /// <returns>The loaded <see cref="User"/> object.</returns>
        /// <exception cref="FileNotFoundException">Thrown if the file does not exist.</exception>
        /// <exception cref="FormatException">Thrown if the file format is invalid.</exception>
        public static User Load(string fileName)
        {
            string filePath = Path.Combine(FileSystem.AppDataDirectory, fileName);

            if (!File.Exists(filePath))
                throw new FileNotFoundException("Unable to find file on local storage.", filePath);

            string fileContent = File.ReadAllText(filePath);
            string[] parts = fileContent.Split('|');

            if (parts.Length != 3)
                throw new FormatException("File format is incorrect. Unable to load data.");

            if (!DateTime.TryParse(parts[1].Trim(), out DateTime parsedDate))
                throw new FormatException("Invalid birth date format in user file.");

            return new User
            {
                FileName = Path.GetFileName(filePath),
                Name = parts[0].Trim(),
                BirthDay = parsedDate,
                Gender = Enum.Parse<Gender>(parts[2].Trim()),
                ChangeDate = File.GetLastWriteTime(filePath)
            };
        }

        /// <summary>
        /// Loads all user profiles stored in the application's data directory.
        /// </summary>
        /// <returns>A sorted collection of <see cref="User"/> objects, ordered by last modified date.</returns>
        public static IEnumerable<User> LoadAll()
        {
            string appDataPath = FileSystem.AppDataDirectory;

            return Directory
                .EnumerateFiles(appDataPath, "*.notes.txt")
                .Select(fileName => Load(Path.GetFileName(fileName)))
                .OrderByDescending(user => user.ChangeDate);
        }
    }
}
