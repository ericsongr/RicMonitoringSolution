using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RicAuthJwtServer.Data.Exception
{
    public class RepositoryException : System.Exception
    {
        private const string ErrorInUpdatingRecord = "Error has occured while updating a record";
        private const string ErrorInAddingNewRecord = "Error has occured while adding a new record";
        private const string ErrorInDeletingRecord = "Error has occured while deleting a record";
        private const string ErrorUnableToFetchRecordFromSource = "Unable to fetch record from data source";
        private const string DefaultError = "Repository error";
        private const string ErrorInPersistingARecord = "Error has occured while saving/persisting a record";
        private const string RecordAlreadyExist = "Record already exist";
        private string _message;

        public RepositoryException() : base(DefaultError)
        {
        }

        public RepositoryException(System.Exception innerException) : base(DefaultError, innerException)
        {
        }

        public RepositoryException(string message, System.Exception innerException) : base(message, innerException)
        {
        }

        public override string Message
        {
            get { return !string.IsNullOrEmpty(_message) ? _message : base.Message; }
        }

        public RepositoryException ErrorInCreate()
        {
            _message = ErrorInAddingNewRecord;
            return this;
        }

        public RepositoryException ErrorInUpdate()
        {
            _message = ErrorInUpdatingRecord;
            return this;
        }

        public RepositoryException ErrorInDelete()
        {
            _message = ErrorInDeletingRecord;
            return this;
        }

        public RepositoryException ErrorUnableToFetchRecord()
        {
            _message = ErrorUnableToFetchRecordFromSource;
            return this;
        }

        public RepositoryException ErrorInSaving()
        {
            _message = ErrorInPersistingARecord;
            return this;
        }

        public RepositoryException ErrorRecordAlreadyExist()
        {
            _message = RecordAlreadyExist;
            return this;
        }
        
    }
}
