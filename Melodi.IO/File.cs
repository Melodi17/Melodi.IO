using System;
using System.Linq;

namespace Melodi.IO
{
    public class File
    {
        private readonly string _path;
        /// <summary>
        /// Path to file
        /// </summary>
        public string Path => _path;
        /// <summary>
        /// Name of file
        /// </summary>
        public string Name => System.IO.Path.GetFileName(Path);
        /// <summary>
        /// Name of file without extension
        /// </summary>
        public string DisplayName => System.IO.Path.GetFileNameWithoutExtension(Path);
        /// <summary>
        /// Extension of file
        /// </summary>
        public string Extension => System.IO.Path.GetExtension(Path);
        /// <summary>
        /// Directory that file is in
        /// </summary>
        public Directory Parent => new(System.IO.Path.GetDirectoryName(Path));
        /// <summary>
        /// Text content of file
        /// </summary>
        public string Text
        {
            get => System.IO.File.ReadAllText(Path);
            set => System.IO.File.WriteAllText(Path, value);
        }
        /// <summary>
        /// Text lines content of file
        /// </summary>
        public string[] Lines
        {
            get => System.IO.File.ReadAllLines(Path);
            set => System.IO.File.WriteAllLines(Path, value);
        }
        /// <summary>
        /// Byte content of file
        /// </summary>
        public byte[] Bytes
        {
            get => System.IO.File.ReadAllBytes(Path);
            set => System.IO.File.WriteAllBytes(Path, value);
        }
        /// <summary>
        /// Create a new file or open an existing from <paramref name="path"/>
        /// </summary>
        /// <param name="path">File path to check</param>
        public File(string path)
        {
            _path = path;
            if (!System.IO.File.Exists(Path))
                System.IO.File.Create(Path);
        }
        /// <summary>
        /// Create a new file or open an existing file in <paramref name="parent"/> from <paramref name="path"/>
        /// </summary>
        /// <param name="parent">Parent directory to check</param>
        /// <param name="path">File path to check (relative to <paramref name="parent"/>)</param>
        public File(Directory parent, string path) 
            : this(System.IO.Path.Combine(parent.Path, path)) { }
        /// <summary>
        /// Delete the file
        /// </summary>
        public void Delete()
        {
            System.IO.File.Delete(Path);
        }
        /// <summary>
        /// Copy the file to <paramref name="destPath"/>
        /// </summary>
        /// <param name="destPath">Path to copy file to</param>
        /// <returns>The copied file</returns>
        public File Copy(string destPath)
        {
            System.IO.File.Copy(Path, destPath);
            return new(destPath);
        }
        /// <summary>
        /// Move the file to <paramref name="destPath"/>
        /// </summary>
        /// <param name="destPath">Path to move file to</param>
        /// <returns>The copied file</returns>
        public File Move(string destPath)
        {
            System.IO.File.Move(Path, destPath);
            return new(destPath);
        }
        /// <summary>
        /// Check if file exists at <paramref name="path"/>
        /// </summary>
        /// <param name="path">File path to check</param>
        /// <returns>Whether file was found</returns>
        public static bool Exists(string path)
        {
            return System.IO.File.Exists(path);
        }
        
        public static implicit operator string(File f) => f.Path;
        public static implicit operator File(string path) => new(path);
        public static File operator +(Directory left, File right)
        {
            return new(System.IO.Path.Join(left.Path, right.Path));
        }
    }

    public class Directory
    {
        private string _path;
        /// <summary>
        /// Path to directory
        /// </summary>
        public string Path => _path;
        /// <summary>
        /// Name of directory
        /// </summary>
        public string Name => System.IO.Path.GetFileName(Path);
        /// <summary>
        /// Directory that directory is in
        /// </summary>
        public Directory Parent => new(System.IO.Path.GetDirectoryName(Path));
        /// <summary>
        /// Files in directory
        /// </summary>
        public File[] ChildFiles => System.IO.Directory.GetFiles(Path).Select(x => new File(x)).ToArray();
        /// <summary>
        /// Directories in directory
        /// </summary>
        public Directory[] ChildDirectories => System.IO.Directory.GetDirectories(Path).Select(x => new Directory(x)).ToArray();
        /// <summary>
        /// Create a new directory or open an existing from <paramref name="path"/>
        /// </summary>
        /// <param name="path">Directory path to check</param>
        public Directory(string path)
        {
            _path = path;
            if (!System.IO.Directory.Exists(Path))
                System.IO.Directory.CreateDirectory(Path);
        }
        /// <summary>
        /// Delete the directory
        /// </summary>
        public void Delete()
        {
            DeleteDirectory(Path);
        }
        /// <summary>
        /// Move the directory to <paramref name="destPath"/>
        /// </summary>
        /// <param name="destPath">Path to move directory to</param>
        /// <returns>The copied directory</returns>
        public Directory Move(string destPath)
        {
            System.IO.Directory.Move(Path, destPath);
            return new(destPath);
        }
        /// <summary>
        /// Check if directory exists at <paramref name="path"/>
        /// </summary>
        /// <param name="path">Directory path to check</param>
        /// <returns>Whether directory was found</returns>
        public static bool Exists(string path)
        {
            return System.IO.Directory.Exists(path);
        }
        /// <summary>
        /// Helper delete method for deleting all children and self
        /// </summary>
        /// <param name="path">Path of directory to delete</param>
        private static void DeleteDirectory(string path)
        {
            foreach (string directory in System.IO.Directory.GetDirectories(path))
            {
                DeleteDirectory(directory);
            }

            try
            {
                System.IO.Directory.Delete(path, true);
            }
            catch (System.IO.IOException)
            {
                System.IO.Directory.Delete(path, true);
            }
            catch (UnauthorizedAccessException)
            {
                System.IO.Directory.Delete(path, true);
            }
        }
        public static Directory Current => new(Environment.CurrentDirectory);
        public static implicit operator string(Directory d) => d.Path;
        public static implicit operator Directory(string path) => new(path);
        public static Directory operator +(Directory left, Directory right)
        {
            return new(System.IO.Path.Join(left.Path, right.Path));
        }
        public static File operator +(Directory left, string right)
        {
            return new(System.IO.Path.Join(left.Path, right));
        }
    }
}