using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Threading.Tasks;
using WoL.Models;

namespace WoL.Data
{
    public interface IHostService
    {
        Task Add(Host host);
        Task Delete(int id);
        Task<Host> Find(int id);
        Task<List<Host>> GetAll();

        public class DuplicateEntryException : ArgumentException
        {
            public string Field { get; }
            public string Value { get; }

            public DuplicateEntryException()
            {
            }

            public DuplicateEntryException(string message) : base(message)
            {
            }

            public DuplicateEntryException(string message, Exception innerException) : base(message, innerException)
            {
            }

            protected DuplicateEntryException(SerializationInfo info, StreamingContext context) : base(info, context)
            {
            }

            private static string CreateMessage(string field, string value) =>
                $"A host entry with {field} '{value}' does already exist.";

            private static string CreateMessage(string field) =>
                $"A host entry with the same {field} does already exist.";

            public DuplicateEntryException(string field, string value, string paramName, Exception innerException) : base(CreateMessage(field, value), paramName, innerException)
            {
                Field = field;
                Value = value;
            }

            public DuplicateEntryException(string field, string paramName, Exception innerException) : base(CreateMessage(field), paramName, innerException)
            {
                Field = field;
            }
        }
    }
}
