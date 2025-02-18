using Lexxika.Documents.Models;
using System.Collections.Concurrent;

namespace Lexxika.Documents
{
    public sealed class InMemoryStorage
    {
        private static readonly InMemoryStorage instance = new InMemoryStorage();
        public static List<TranslationDocument> Storage;

        static InMemoryStorage()
        {
        }
        
        private InMemoryStorage()
        {
            Initialize();
        }

        public static InMemoryStorage Instance
        {
            get
            {
                return instance;
            }
        }

        public static void Initialize()
        {
            // create in memory table
            Storage = new List<TranslationDocument>();
            
            #region Create Initial Dummy Data
            var newDocument = new TranslationDocument
            {
                Id = Guid.NewGuid().ToString(),
                Title = "Urgent German Translation",
                TranslationText = "Some translation text ",
                User = "user1"
            };
            Storage.Add(newDocument);
            
            newDocument = new TranslationDocument
            {
                Id = Guid.NewGuid().ToString(),
                Title = "French Medical",
                TranslationText = "Other translation text",
                User = "user2"
            };
            Storage.Add(newDocument);

            newDocument = new TranslationDocument
            {
                Id = Guid.NewGuid().ToString(),
                Title = "Russian challenge",
                TranslationText = "More translation text ",
                User = "admin"
            };
            Storage.Add(newDocument);
            #endregion
        }
    }
}
